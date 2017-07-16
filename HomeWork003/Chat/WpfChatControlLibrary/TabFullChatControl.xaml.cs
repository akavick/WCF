using System.Collections.Generic;
using System.Windows.Controls;

namespace WpfChatControlLibrary
{
    public partial class TabFullChatControl
    {
        public TabControl TalksTabControl { get; }
        public FullChatControl MainChat { get; }
        public HashSet<ChatControl> PrivateTalks { get; }

        public TabFullChatControl()
        {
            InitializeComponent();
            TalksTabControl = _talksTabControl;
            PrivateTalks = new HashSet<ChatControl>();
            MainChat = _fullChatControl;
            MainChat.TabItem = _mainChatTab;
        }

    }
}
