public interface IMessage
{
    public MessageType Type { get; }
    public string Text { get; set; }
}

