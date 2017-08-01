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
        private const double MediHeight = 200.0;
        private const int ToolTipImageWidth = 300;
        private const int ToolTipImageHeight = 100;
        private const double Dpi = 96.0;

        public FlowChatMessage(FlowDocument document, string name, DateTime time)
        {
            try
            {
                InitializeComponent();
                document.PagePadding = new Thickness(0.0);
                document.FontFamily = new FontFamily("Segoe Ui");
                document.FontSize = 15.0;
                _flowDocumentScrollViewer.Document = document;
                _dateTime.Content = $"{time.ToLongDateString()} {time.ToLongTimeString()}";
                _who.Content = name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            MouseEnter += (s, e) =>
            {
                try
                {
                    _buttons.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            };

            MouseLeave += (s, e) =>
            {
                try
                {
                    _buttons.Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            };

            _butCollapse.Click += (s, e) =>
            {
                try
                {
                    _butCollapse.IsEnabled = false;
                    _butExpand.IsEnabled = true;
                    _butMedify.IsEnabled = true;
                    _grid.Height = _info.Height + _buttons.Height;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            };

            _butExpand.Click += (s, e) =>
            {
                try
                {
                    _butCollapse.IsEnabled = true;
                    _butExpand.IsEnabled = false;
                    _butMedify.IsEnabled = true;
                    _grid.Height = double.NaN;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            };

            _butMedify.Click += async (s, e) =>
            {
                try
                {
                    _butCollapse.IsEnabled = true;
                    _butExpand.IsEnabled = true;
                    _butMedify.IsEnabled = false;
                    Visibility = Visibility.Hidden;
                    _grid.Height = double.NaN;
                    await _grid.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);
                    if (_grid.ActualHeight > MediHeight)
                        _grid.Height = MediHeight;
                    Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            };

            ToolTipOpening += (s, e) =>
            {
                try
                {
                    if (double.IsNaN(_grid.Height) || _grid.Height > _info.Height + _buttons.Height)
                        e.Handled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            };

            Loaded += async (s, e) =>
            {
                try
                {
                    await _grid.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);
                    _grid.Height = _grid.ActualHeight > MediHeight ? MediHeight : double.NaN;

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
                    ToolTipService.SetInitialShowDelay(this, 500);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            };
        }
    }
}
