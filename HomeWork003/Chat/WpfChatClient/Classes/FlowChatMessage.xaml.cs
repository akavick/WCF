using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WpfChatClient.Classes
{
    public partial class FlowChatMessage
    {
        private enum State
        {
            Collapsed,
            Medified,
            Expanded,
            Undefined
        }

        private State _currentState = State.Undefined;
        private bool _isLongMessage = false;
        private const double MediHeight = 200.0;
        private const int ToolTipImageWidth = 300;
        private const int ToolTipImageHeight = 100;
        private const double Dpi = 96.0;



        public FlowChatMessage(FlowDocument document, string name, DateTime time)
        {
            try
            {
                InitializeComponent();
                Initialize(document, name, time);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        private void CreateToolTip()
        {
            var renderTargetBitmap = new RenderTargetBitmap(ToolTipImageWidth, ToolTipImageHeight, Dpi, Dpi, PixelFormats.Default);
            renderTargetBitmap.Render(_flowDocumentScrollViewer);
            var pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            var bitmapImage = new BitmapImage();
            using (var stream = new MemoryStream())
            {
                pngImage.Save(stream);
                renderTargetBitmap = null;
                pngImage = null;
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.StreamSource = null;
                bitmapImage.Freeze();
            }

            ToolTip = new Image
            {
                Source = bitmapImage,
                Width = ToolTipImageWidth / 2.0,
                Height = ToolTipImageHeight / 2.0
            };
            ToolTipService.SetInitialShowDelay(this, 200);
        }


        private void Initialize(FlowDocument document, string name, DateTime time)
        {
            try
            {
                document.PagePadding = new Thickness(0.0);
                document.FontFamily = new FontFamily("Segoe Ui");
                document.FontSize = 15.0;
                _butMedify.IsEnabled = false;
                _flowDocumentScrollViewer.Document = document;
                _dateTime.Content = $"{time.ToLongDateString()} {time.ToLongTimeString()}";
                _who.Content = name;

                MouseEnter += (s, e) => _buttons.Visibility = Visibility.Visible;

                MouseLeave += (s, e) => _buttons.Visibility = Visibility.Collapsed;

                _butCollapse.Click += (s, e) =>
                {
                    _butCollapse.IsEnabled = false;
                    _butExpand.IsEnabled = true;
                    if (_isLongMessage)
                        _butMedify.IsEnabled = true;
                    _currentState = State.Collapsed;
                    _grid.Height = _info.Height + _buttons.Height;
                };

                _butExpand.Click += (s, e) =>
                {
                    _butCollapse.IsEnabled = true;
                    _butExpand.IsEnabled = false;
                    if (_isLongMessage)
                        _butMedify.IsEnabled = true;
                    _currentState = State.Expanded;
                    _grid.Height = double.NaN;
                };

                _butMedify.Click += async (s, e) =>
                {
                    _butCollapse.IsEnabled = true;
                    _butExpand.IsEnabled = true;
                    if (_isLongMessage)
                        _butMedify.IsEnabled = false;
                    _currentState = State.Medified;
                    Visibility = Visibility.Hidden;
                    _grid.Height = double.NaN;
                    await _grid.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);
                    if (_grid.ActualHeight > MediHeight)
                        _grid.Height = MediHeight;
                    Visibility = Visibility.Visible;
                };

                ToolTipOpening += (s, e) =>
                {
                    if (double.IsNaN(_grid.Height) || _grid.Height > _info.Height + _buttons.Height)
                        e.Handled = true;
                };

                Loaded += async (s, e) =>
                {
                    if (_currentState != State.Undefined)
                        return;
                    _currentState = State.Medified;

                    await _grid.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);
                    if (_grid.ActualHeight > MediHeight)
                    {
                        _grid.Height = MediHeight;
                        _isLongMessage = true;
                    }
                    else
                    {
                        _grid.Height = double.NaN;
                        _butExpand.IsEnabled = false;
                    }

                    ToolTip = new object();
                };

                ToolTipClosing += (s, e) =>
                {
                    ToolTip = null;
                    ToolTip = new object();
                };

                ToolTipOpening += (s, e) =>
                {
                    CreateToolTip();
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
