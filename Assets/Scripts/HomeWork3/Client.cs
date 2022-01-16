using Newtonsoft.Json;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


public class Client : MonoBehaviour
{
    public event Action<string> MessageReceived;
    public event Action Connected;

    private const int MAX_CONNECTION = 10;

    public bool IsConnected => _isConnected;

    private int _port = 6000;
    private int _serverPort = 5805;

    private int _hostID;

    private int _reliableChannel;
    private int _connectionID;

    private bool _isConnected = false;
    private byte _error;

    public void Connect()
    {
        if (_isConnected)
            return;
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();

        _reliableChannel = cc.AddChannel(QosType.Reliable);

        HostTopology topology = new HostTopology(cc, MAX_CONNECTION);
        
        _hostID = NetworkTransport.AddHost(topology, _port);        
        _connectionID = NetworkTransport.Connect(_hostID, "127.0.0.1", _serverPort, 0, out _error);

        if ((NetworkError)_error == NetworkError.Ok)
            _isConnected = true;
        else
            Debug.Log((NetworkError)_error);
    }

    public void Disconnect()
    {
        if (!_isConnected) return;

        NetworkTransport.Disconnect(_hostID, _connectionID, out _error);
        _isConnected = false;
    }
        
    private void Update()
    {
        if (!_isConnected)
            return;

        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out _error);

        while (recData != NetworkEventType.Nothing)
        {
            switch (recData)
            {
                case NetworkEventType.ConnectEvent:
                    MessageReceived?.Invoke($"You have been connected to server.");
                    Connected?.Invoke();
                    Debug.Log($"You have been connected to server.");
                    break;

                case NetworkEventType.DataEvent:
                    string messageJson = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    var message = JsonConvert.DeserializeObject<IMessage>(messageJson);
                    MessageReceived?.Invoke(message.Text);
                    Debug.Log(message.Text);
                    break;

                case NetworkEventType.DisconnectEvent:
                    _isConnected = false;
                    MessageReceived?.Invoke($"You have been disconnected from server.");
                    Debug.Log($"You have been disconnected from server.");
                    break;

                case NetworkEventType.BroadcastEvent:
                    break;
            }

            recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out _error);
        }
    }

    public void SendMessage(IMessage message)
    {
        string messageJson = JsonConvert.SerializeObject(message);
        byte[] buffer = Encoding.Unicode.GetBytes(messageJson);
        if (buffer.Length == 0)
            return;

        NetworkTransport.Send(_hostID, _connectionID, _reliableChannel, buffer, messageJson.Length * sizeof(char), out _error);
        if ((NetworkError)_error != NetworkError.Ok) Debug.Log((NetworkError)_error);
    }
}
