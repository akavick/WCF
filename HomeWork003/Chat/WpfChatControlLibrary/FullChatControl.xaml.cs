﻿using System.Collections.Generic;
using System.Windows.Controls;

namespace WpfChatControlLibrary
{
    public partial class FullChatControl
    {
        public RichTextBox ChatRichTextBox { get; }
        public RichTextBox MessageRichTextBox { get; }
        public Button SendButton { get; }
        public ListBox ClientsListBox { get; }
        public TabItem TabItem { get; set; }
        public Label ClientsCountLabel { get; set; }
        public Label SomeLabel { get; set; }

        public FullChatControl()
        {
            InitializeComponent();
            ChatRichTextBox = _chatControl.ChatRichTextBox;
            MessageRichTextBox = _chatControl.MessageRichTextBox;
            SendButton = _chatControl.SendButton;
            ClientsListBox = _clientsListBox;
            ClientsCountLabel = _clientsCountLabel;
            SomeLabel = _label;
            TabItem = null;
        }
    }
}
