using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIController : MonoBehaviour
{
    [SerializeField]
    private Button _buttonStartServer;
    [SerializeField]
    private Button _buttonShutDownServer;
    [SerializeField]
    private Button _buttonConnectClient;
    [SerializeField]
    private Button _buttonDisconnectClient;
    [SerializeField]
    private Button _buttonSendMessage;

    [SerializeField]
    private TMP_InputField _inputField;
    [SerializeField]
    private InputField _name;


    [SerializeField]
    private TextField _textField;

    [SerializeField]
    private Server _server;
    [SerializeField]
    private Client _client;

    private void Start()
    {
        _buttonStartServer.onClick.AddListener(() => StartServer());
        _buttonShutDownServer.onClick.AddListener(() => ShutDownServer());
        _buttonConnectClient.onClick.AddListener(() => Connect());
        _buttonDisconnectClient.onClick.AddListener(() => Disconnect());
        _buttonSendMessage.onClick.AddListener(() => SendMessage());
        _inputField.onEndEdit.AddListener((text) =>SendMessage());
        _client.MessageReceived += ReceiveMessage;
    }

    private void StartServer() =>    
        _server.StartServer();
    
    private void ShutDownServer() =>    
        _server.ShutDownServer();

    private void Connect()
    {
        _client.Connected += SendInitialMessage;
        _client.Connect();
    }
    private void SendInitialMessage()
    {
        var nameMessage = new Message()
        {
            Type = MessageType.Name,
            Text = _name.text
        };
        _client.SendMessage(nameMessage);
        _client.Connected -= SendInitialMessage;
    }

    private void Disconnect() =>    
        _client.Disconnect();    

    private void SendMessage()
    {
        var newMessage = new Message()
        {
            Type = MessageType.Message,
            Text = _inputField.text
        };
        _client.SendMessage(newMessage);
        _inputField.text = "";
    }

    public void ReceiveMessage(object message) =>    

        _textField.ReceiveMessage(message);
    
}
