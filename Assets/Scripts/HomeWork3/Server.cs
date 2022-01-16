using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


public class Server : MonoBehaviour
{
    private const int MAX_CONNECTION = 10;

    private int _port = 5805;

    private int _hostID;
    private int _reliableChannel;

    private bool _isStarted = false;
    private byte _error;

    private Dictionary<int, string> _connectinsDictionary = new Dictionary<int, string>();

    public void StartServer()
    {        
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        _reliableChannel = cc.AddChannel(QosType.Reliable);

        HostTopology topology = new HostTopology(cc, MAX_CONNECTION);        
        _hostID = NetworkTransport.AddHost(topology, _port);

        _isStarted = true;
    }

    void Update()
    {
        if (!_isStarted)
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
                case NetworkEventType.Nothing:
                    break;

                case NetworkEventType.ConnectEvent:
                    _connectinsDictionary.Add(connectionId, string.Empty);

                    var connectedMessage = new Message()
                    {
                        Type = MessageType.Message,
                        Text = $"Player {connectionId} has connected."
                    };
                    SendMessageToAll(connectedMessage);
                    Debug.Log(connectedMessage.Text);
                    break;

                case NetworkEventType.DataEvent:
                    string messageJson = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    var message = JsonConvert.DeserializeObject<IMessage>(messageJson);
                    ProcessDataEvent(connectionId, message);

                    break;

                case NetworkEventType.DisconnectEvent:
                    string playerName = GetPlayerName(connectionId);
                    _connectinsDictionary.Remove(connectionId);
                    var disconnectedMessage = new Message()
                    {
                        Type = MessageType.Message,
                        Text = $"Player {playerName} has disconnected."
                    };
                    SendMessageToAll(disconnectedMessage);
                    Debug.Log(disconnectedMessage.Text);
                    break;

                case NetworkEventType.BroadcastEvent:
                    break;

            }

            recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out _error);
        }
    }

    private void ProcessDataEvent(int connectionId, IMessage message)
    {
        switch (message.Type)
        {
            case MessageType.Unknown:
                break;
            case MessageType.Name:
                {
                    _connectinsDictionary[connectionId] = message.Text;
                    break;
                }
            case MessageType.Message:
                {
                    string playerName = GetPlayerName(connectionId);
                    var newMessage = new Message()
                    {
                        Type = MessageType.Message,
                        Text = $"Player {playerName}: {message.Text}"
                    };
                    SendMessageToAll(newMessage);
                    Debug.Log(newMessage.Text);
                    break;
                }
            default:
                break;
        }
    }

    public void ShutDownServer()
    {
        if (!_isStarted)
            return;

        NetworkTransport.RemoveHost(_hostID);
        NetworkTransport.Shutdown();
        _isStarted = false;
    }

    public void SendMessage(IMessage message, int connectionID)
    {
        string messageJson = JsonConvert.SerializeObject(message);
        byte[] buffer = Encoding.Unicode.GetBytes(messageJson);
        NetworkTransport.Send(_hostID, connectionID, _reliableChannel, buffer, messageJson.Length * sizeof(char), out _error);
        if ((NetworkError)_error != NetworkError.Ok)
            Debug.Log((NetworkError)_error);
    }

    public void SendMessageToAll(IMessage message)
    {
        foreach (var connectionId in _connectinsDictionary.Keys)
            SendMessage(message, connectionId);        
    }

    private string GetPlayerName(int connectionId)
    {
        if (!_connectinsDictionary.ContainsKey(connectionId))
            return string.Empty;

        if (_connectinsDictionary[connectionId] == string.Empty)
            return connectionId.ToString();

        return _connectinsDictionary[connectionId];
    }
}
