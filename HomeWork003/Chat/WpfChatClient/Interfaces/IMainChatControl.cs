using System.Windows.Controls;

namespace WpfChatClient.Interfaces
{
    public interface IMainChatControl : IChatControl
    {
        ListBox ClientsListBox { get; }
        Label ClientsCountLabel { get; set; }
    }
}