using System.Collections.Generic;
using System.Windows.Controls;

namespace WpfChatControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для TabFullChatControl.xaml
    /// </summary>
    public partial class TabFullChatControl : UserControl
    {
        public TabControl TalksTabControl { get; }
        public FullChatControl MainChat { get; }
        public Dictionary<ChatControl, TabItem> PrivateTalks { get; }

        public TabFullChatControl()
        {
            InitializeComponent();
            TalksTabControl = _talksTabControl;
            PrivateTalks = new Dictionary<ChatControl, TabItem>();
            MainChat = _fullChatControl;
        }

    }
}
