﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
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
        private bool _isLongMessage;
        private const double MediHeight = 200.0;
        private const int ToolTipLength = 1024;
        private const int ToolTipTime = 200;

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
                    await await _grid.Dispatcher.InvokeAsync(async () => { await Task.Yield(); }, DispatcherPriority.Render);
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

                    await await _grid.Dispatcher.InvokeAsync(async () => { await Task.Yield(); }, DispatcherPriority.Render);
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

                    var text = new TextRange(document.ContentStart, document.ContentEnd).Text;
                    text = text.Substring(0, text.Length > ToolTipLength ? ToolTipLength : text.Length);
                    ToolTip = text;
                    ToolTipService.SetInitialShowDelay(this, ToolTipTime);
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
