using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using WpfChatClient.Interfaces;

namespace WpfChatClient.Classes
{
    public partial class PanelChatControl : IChatControl
    {
        public TabItem TabItem { get; set; }

        public event Action<object> SendClick;

        public PanelChatControl()
        {
            InitializeComponent();
            TabItem = null;
            _sendMessageButton.Click += SendButton_Click;
        }

        public void DockMessage(FlowDocument doc)
        {
            var flowChatMessage = new FlowChatMessage{ Document = doc };
            _chatDockPanel.Children.Add(flowChatMessage);
        }










        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            //endClick?.Invoke(sender, e);
        }



        private TextRange GetMessageText()
            => GetTextFromRichTextBox(_messageRichTextBox);


        private TextRange GetTextFromRichTextBox(RichTextBox rtb)
            => new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
    }
}



//public partial class PanelChatControl
//{
//    public PanelChatControl()
//    {
//        InitializeComponent();
//        _sendMessageButton.Click += SendButton_Click;




//        _sliderFontSize.ValueChanged += _sliderFontSize_ValueChanged;
//        _checkboxBold.Checked += _checkbox_CheckedChanged;
//        _checkboxBold.Unchecked += _checkbox_CheckedChanged;
//        _checkboxItalic.Checked += _checkbox_CheckedChanged;
//        _checkboxItalic.Unchecked += _checkbox_CheckedChanged;
//        _checkboxUnderline.Checked += _checkbox_CheckedChanged;
//        _checkboxUnderline.Unchecked += _checkbox_CheckedChanged;
//        _messageRichTextBox.SelectionChanged += _richTextBox_SelectionChanged;
//        _colorPicker.SelectedColorChanged += _colorPicker_SelectedColorChanged;
//        _checkboxBold.IsChecked = _checkboxItalic.IsChecked = _checkboxUnderline.IsChecked = false;
//    }

//    public void DockMessage(FlowDocument doc)
//    {
//        var flowChatMessage = new FlowChatMessage { Doc = doc };
//        DockPanel.SetDock(flowChatMessage, Dock.Top);
//        _chatDockPanel.Children.Add(flowChatMessage);
//    }










//    private void SendButton_Click(object sender, RoutedEventArgs e)
//    {
//        byte[] arr = null;
//        var doc = _messageRichTextBox.Document;
//        _messageRichTextBox.Document = new FlowDocument();

//        FlowDocument document;
//        using (var stream = new MemoryStream())
//        {
//            if (doc != null)
//                XamlWriter.Save(doc, stream);
//            stream.Position = 0;

//            using (var reader = new BinaryReader(stream))
//            {
//                arr = reader.ReadBytes((int)stream.Length);
//            }
//        }

//        using (var stream = new MemoryStream(arr))
//        {
//            document = XamlReader.Load(stream) as FlowDocument;
//        }


//        if (document != null)
//            DockMessage(document);

//    }



//    private TextRange GetMessageText()
//        => GetTextFromRichTextBox(_messageRichTextBox);


//    private TextRange GetTextFromRichTextBox(RichTextBox rtb)
//        => new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);













//    private void _colorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
//    {
//        Color? c = _colorPicker.SelectedColor;
//        TextSelection text = _messageRichTextBox.Selection;
//        if (c == null || text.IsEmpty)
//            return;
//        SolidColorBrush brush = new SolidColorBrush((Color)c);
//        text.ApplyPropertyValue(ForegroundProperty, brush);
//    }

//    private void ButtonShow_Click(object sender, RoutedEventArgs e)
//    {
//        FlowDocument fd = _messageRichTextBox.Document;
//        using (FileStream fs = new FileStream(@"demo.xaml", FileMode.OpenOrCreate, FileAccess.Write))
//        {
//            TextRange textRange = new TextRange(fd.ContentStart, fd.ContentEnd);
//            textRange.Save(fs, DataFormats.Xaml);
//        }
//    }

//    private void _checkbox_CheckedChanged(object sender, RoutedEventArgs e)
//    {
//        TextSelection text = _messageRichTextBox.Selection;
//        if (text.IsEmpty)
//            return;
//        if ((CheckBox)sender == _checkboxBold)
//            text.ApplyPropertyValue(FontWeightProperty, (bool)_checkboxBold.IsChecked ?
//                FontWeights.Bold : FontWeights.Normal);
//        else if ((CheckBox)sender == _checkboxItalic)
//            text.ApplyPropertyValue(FontStyleProperty, (bool)_checkboxItalic.IsChecked ?
//                FontStyles.Italic : FontStyles.Normal);
//        else if ((CheckBox)sender == _checkboxUnderline)
//            text.ApplyPropertyValue(Inline.TextDecorationsProperty, (bool)_checkboxUnderline.IsChecked ?
//                TextDecorations.Underline : null);
//    }

//    private void _richTextBox_SelectionChanged(object sender, RoutedEventArgs e)
//    {
//        _checkboxBold.IsChecked = false;
//        _checkboxItalic.IsChecked = false;
//        _checkboxUnderline.IsChecked = false;
//    }

//    private void _sliderFontSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
//    {
//        TextSelection text = _messageRichTextBox.Selection;
//        if (!text.IsEmpty)
//            text.ApplyPropertyValue(FontSizeProperty, e.NewValue);
//        text.Select(text.Start, text.End);
//    }





//    /*
//        Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
//        if (ofd.ShowDialog() == false)
//            return;
//        try
//        {
//            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
//            image.Source = new BitmapImage(new Uri(ofd.FileName));
//            image.Width = 300;
//            BlockUIContainer buic = new BlockUIContainer(image);
//            richTextBox.Document.Blocks.Add(buic);
//        }
//        catch (Exception ex)
//        {
//            System.Windows.MessageBox.Show(ex.Message);
//        }
//    */
//}