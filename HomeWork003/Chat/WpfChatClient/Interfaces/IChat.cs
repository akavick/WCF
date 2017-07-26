using WpfChatClient.ChatServiceReference;

namespace WpfChatClient.Interfaces
{
    public interface IChat
    {
        string UserName { get; set; }
        IChatContract Server { get; set; }
    }
}