using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.System;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Controls
{
    [TemplatePart(Name = TextRender.CanvasElementName, Type = typeof(CanvasControl))]
    public sealed class TextEditor : Control, ITextEditor
    {
        public TextEditor()
        {
            this.DefaultStyleKey = typeof(TextEditor);
            // 初始化光标闪烁定时器
            _cursorTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _cursorTimer.Tick += CursorTimer_Tick;
            _cursorTimer.Start();
        }


        private CanvasTextLayout _textLayout;
        private CanvasControl? _canvas;
        private int _cursorPosition;
        private Vector2 _cursorPositionPixels;
        private bool _cursorVisible = true;
        private DispatcherTimer _cursorTimer;
        private float _scrollOffset;
        private string _source = string.Empty;

        public string Text { 
            get => _source; 
            set {
                _source = value;
                if (_textLayout is not null)
                {
                    Render();
                }
            }
        }
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _canvas = GetTemplateChild(TextRender.CanvasElementName) as CanvasControl;
            if (_canvas is not null)
            {
                _canvas.Draw += Canvas_Draw;
                _canvas.CreateResources += Canvas_CreateResources;
            }
        }

        private void Canvas_CreateResources(CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            Render();
        }

        private void Canvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (_textLayout is null)
            {
                return;
            }
            // 绘制文本
            args.DrawingSession.DrawTextLayout(_textLayout, 0, _scrollOffset, Colors.Black);

            // 绘制光标
            if (_cursorVisible)// && sender.FocusState != FocusState.Unfocused)
            {
                var cursorPosition = _textLayout.GetCaretPosition(_cursorPosition, false, out var cursorRegion);
                _cursorPositionPixels = new Vector2(cursorPosition.X, cursorPosition.Y + _scrollOffset);

                args.DrawingSession.DrawLine(
                    _cursorPositionPixels,
                    new Vector2(_cursorPositionPixels.X,
                    _cursorPositionPixels.Y + (float)cursorRegion.LayoutBounds.Height
                    ),
                    Colors.Black,
                    2);
            }
        }

        private void Render()
        {
            if (_canvas is null)
            {
                return;
            }
            // 创建或更新文本布局
            var format = new CanvasTextFormat()
            {
                FontSize = (float)FontSize,
                WordWrapping = CanvasWordWrapping.Wrap
            };
            _textLayout = new CanvasTextLayout(_canvas, Text, format,
                (float)_canvas.ActualWidth, (float)_canvas.ActualHeight);
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            base.OnKeyDown(e);
            var textChanged = false;

            switch (e.Key)
            {
                case VirtualKey.Back:
                    if (_cursorPosition > 0 && Text.Length > 0)
                    {
                        Text = Text.Remove(_cursorPosition - 1, 1);
                        _cursorPosition--;
                        textChanged = true;
                    }
                    break;

                case VirtualKey.Delete:
                    if (_cursorPosition < Text.Length)
                    {
                        Text = Text.Remove(_cursorPosition, 1);
                        textChanged = true;
                    }
                    break;

                case VirtualKey.Left:
                    if (_cursorPosition > 0)
                    {
                        _cursorPosition--;
                        textChanged = true;
                    }
                    break;

                case VirtualKey.Right:
                    if (_cursorPosition < Text.Length)
                    {
                        _cursorPosition++;
                        textChanged = true;
                    }
                    break;

                case VirtualKey.Up:
                    // 简化版 - 实际应计算上一行的位置
                    _cursorPosition = Math.Max(0, _cursorPosition - 20);
                    textChanged = true;
                    break;

                case VirtualKey.Down:
                    // 简化版 - 实际应计算下一行的位置
                    _cursorPosition = Math.Min(Text.Length, _cursorPosition + 20);
                    textChanged = true;
                    break;

                case VirtualKey.Home:
                    _cursorPosition = 0;
                    textChanged = true;
                    break;

                case VirtualKey.End:
                    _cursorPosition = Text.Length;
                    textChanged = true;
                    break;

                case VirtualKey.Enter:
                    Text = Text.Insert(_cursorPosition, "\n");
                    _cursorPosition++;
                    textChanged = true;
                    break;
            }

            // 处理常规文本输入
            if (!e.Key.ToString().StartsWith("VirtualKey") && e.Key != VirtualKey.Space)
            {
                var ch = GetCharFromKey(e.Key);
                if (ch != '\0')
                {
                    Text = Text.Insert(_cursorPosition, ch.ToString());
                    _cursorPosition++;
                    textChanged = true;
                }
            }
            else if (e.Key == VirtualKey.Space)
            {
                Text = Text.Insert(_cursorPosition, " ");
                _cursorPosition++;
                textChanged = true;
            }

            if (textChanged)
            {
                _cursorVisible = true;
                _cursorTimer.Stop();
                _cursorTimer.Start();
                _canvas?.Invalidate();
            }
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            _canvas?.Focus(FocusState.Programmatic);

            // 获取点击位置并设置光标位置
            var point = e.GetCurrentPoint(_canvas);
            if (_textLayout.HitTest(
                new Vector2((float)point.Position.X, (float)point.Position.Y - _scrollOffset),
                out var rect))
            {
                _cursorPosition = rect.CharacterIndex;
                _cursorVisible = true;
                _canvas?.Invalidate();
            }
        }

        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            base.OnPointerMoved(e);

        }

        public void LoadFromFile(string fileName)
        {
            using var fs = File.OpenRead(fileName);
            Load(fs);
        }

        public void Load(Stream input)
        {
            using var reader = LocationStorage.Reader(input);
            Text = reader.ReadToEnd();
        }

        public bool FindNext(string text)
        {
            return false;
        }

        public void Select(int start, int count)
        {
        }

        public void ScrollTo(int position)
        {
        }

        public void Unselect()
        {
        }

        private void CursorTimer_Tick(object? sender, object e)
        {
            _cursorVisible = !_cursorVisible;
            _canvas?.Invalidate();
        }

        private static char GetCharFromKey(VirtualKey key)
        {
            // 简化版 - 实际应用中应考虑Shift键状态等
            if (key >= VirtualKey.A && key <= VirtualKey.Z)
            {
                return (char)('a' + (key - VirtualKey.A));
            }
            else if (key >= VirtualKey.Number0 && key <= VirtualKey.Number9)
            {
                return (char)('0' + (key - VirtualKey.Number0));
            }

            return '\0';
        }
    }
}
