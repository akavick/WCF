namespace ChatLibrary.ControlInterfaces
{
    public interface IChatControl
    {
        IChatMessage GetWrittenMessage();
        IChatHistory GetHistory();
        void PushNewMessage(IChatMessage message);
    }
}
