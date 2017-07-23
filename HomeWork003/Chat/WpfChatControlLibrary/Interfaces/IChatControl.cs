namespace WpfChatControlLibrary.Interfaces
{
    public interface IChatControl
    {
        IChatMessage GetWrittenMessage();
        IChatHistory GetHistory();
        void PushNewMessage(IChatMessage message);
    }
}
