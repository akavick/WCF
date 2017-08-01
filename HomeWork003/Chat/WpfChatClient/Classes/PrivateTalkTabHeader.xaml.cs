using System;
using System.Windows.Controls;

namespace WpfChatClient.Classes
{
    public partial class PrivateTalkTabHeader
    {
        public event Action CloseButtonPressed;
        public string TabName
        {
            get => _lblName.Content.ToString();
            set => _lblName.Content = value;
        }

        public PrivateTalkTabHeader()
        {
            InitializeComponent();
            _butClose.Click += (s, e) => CloseButtonPressed?.Invoke();
        }
    }
}
