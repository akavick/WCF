using System.Windows.Controls;

namespace WpfChatClient.Interfaces
{
    public interface IMainChatControl : IChatControl
    {
        ListBox ClientsListBox { get; }//todo: fixit
        Label ClientsCountLabel { get; set; }//todo: fixit
    }
}