using System.Windows.Controls;
using System.Windows.Documents;

namespace WpfChatClient.Classes
{
    public partial class FlowChatMessage
    {
        public FlowDocument Document
        {
            get => _flowDocumentScrollViewer.Document;
            set => _flowDocumentScrollViewer.Document = value;
        }

        public FlowChatMessage()
        {
            InitializeComponent();
        }
    }
}
