using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Text;
using ZoDream.Shared.Storage;
using ZoDream.Shared.Tokenizers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Controls
{
    [TemplatePart(Name = TextRender.CanvasElementName, Type = typeof(TextBox))]
    public sealed class TextEditor : Control, ITextEditor
    {
        public TextEditor()
        {
            this.DefaultStyleKey = typeof(TextEditor);
        }

        private TextBox? _canvas;
        private int _cursor;
        private int _cursorNext;
        private string _source = string.Empty;
        private readonly List<int> _histories = [0];

        public char NewLine => '\r';

        public bool CanBack => _cursor > 0;
        public bool CanForward => _cursorNext < Text.Length;


        public bool CanUndo => _canvas?.CanUndo == true;

        public bool CanRedo => _canvas?.CanRedo == true;

        public int SelectionStart => _canvas?.SelectionStart ?? 0;

        public int SelectionLength => _canvas?.SelectionLength ?? 0;

        public string Text { 
            get {
                TrySave();
                return _source;
            } 
            set {
                _source = value.Replace("\r\n", "\n").Replace('\r', '\n');
                _cursor = 0;
                Render();
            }
        }



        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _canvas = GetTemplateChild(TextRender.CanvasElementName) as TextBox;
        }


        private void Render()
        {
            if (_canvas is null)
            {
                return;
            }
            if (_cursor == 0)
            {
                _histories.Clear();
            }
            var (maxColumn, maxRow) = GetLetterRange();
            ReadNext(maxColumn, maxRow);
            _canvas.Text = Current;
        }
        public string Current => _source[_cursor.._cursorNext];

        private int SelectionEnd => _canvas is not null ? _canvas.SelectionStart + _canvas.SelectionLength : 0;


        private (int, int) GetLetterRange()
        {
            if (_canvas is null)
            {
                return (0, 0);
            }
            var fontWidth = _canvas.FontSize * 1.4;
            var maxColumn = (int)Math.Floor(_canvas.ActualWidth / fontWidth);
            var maxRow = (int)Math.Floor(_canvas.ActualHeight / fontWidth);
            return (maxColumn, maxRow);
        }

        private void TrySave()
        {
            if (_canvas is null)
            {
                return;
            }
            var begin = _cursor;
            var end = _cursorNext;
            if (begin == end)
            {
                return;
            }
            var source = _source[begin..end];
            var text = _canvas.Text;
            if (text[^1] != '\n' && end < _source.Length)
            {
                text += '\n';
            }
            if (text == source)
            {
                return;
            }
            _source = _source[..begin] + text + _source[end..];
            _cursorNext = begin + text.Length;
        }

        private void ReadNext(int maxColumn, int maxRow)
        {
            _cursorNext = ReadNext(maxColumn, maxRow, _cursor);
        }
        private int ReadNext(int maxColumn, int maxRow, int index)
        {
            var start = index;
            int next;
            var row = 0;
            while (true)
            {
                var line = TextFormatter.LineText(_source, start, out next);
                start = next;
                if (line is null)
                {
                    break;
                }
                row += (int)Math.Max(1, Math.Ceiling((double)line.Length / maxColumn));
                if (row >= maxRow)
                {
                    break;
                }
            }
            return next;
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
            if (_canvas is null)
            {
                return false;
            }
            TrySave();
            var lastIndex = _cursor;
            var selectionEnd = SelectionEnd;
            lastIndex += selectionEnd;
            var i = _source.IndexOf(text, lastIndex);
            if (i < 0)
            {
                return false;
            }
            var (maxColumn, maxRow) = GetLetterRange();
            while (i >= _cursorNext)
            {
                selectionEnd = 0;
                if (!_histories.Contains(_cursor))
                {
                    _histories.Add(_cursor);
                }
                _cursor = _cursorNext;
                ReadNext(maxColumn, maxRow);
            }
            _canvas.Text = Current;
            _canvas.Focus(FocusState.Pointer);
            _canvas.Select(_canvas.Text.IndexOf(text, selectionEnd), text.Length);
            return true;
        }

        public bool FindBack(string text)
        {
            return false;
        }

        public void Select(int start, int count)
        {
            if (_canvas is null)
            {
                return;
            }
            TrySave();
            _canvas.Focus(FocusState.Pointer);
            _canvas.Select(start - _cursor, count);
        }

        public void ScrollTo(int position)
        {
            if (_canvas is null)
            {
                return;
            }
            TrySave();
            if (position < _cursor)
            {
                for (var i = _histories.Count - 1; i >= 0; i--)
                {
                    var item = _histories[i];
                    if (position > item)
                    {
                        _cursor = item;
                        break;
                    }
                    _cursorNext = item;
                    _histories.RemoveAt(i);
                }

            } 
            else
            {
                var (maxColumn, maxRow) = GetLetterRange();
                while (position >= _cursorNext)
                {
                    if (!_histories.Contains(_cursor))
                    {
                        _histories.Add(_cursor);
                    }
                    _cursor = _cursorNext;
                    ReadNext(maxColumn, maxRow);
                }
            }
            _canvas.Text = Current;
        }

        public void Unselect()
        {
            TrySave();
            _canvas?.Select(0, 0);
        }

        public void GoForward()
        {
            TrySave();
            if (!_histories.Contains(_cursor))
            {
                _histories.Add(_cursor);
            }
            _cursor = _cursorNext;
            Render();
        }

        public void GoBack()
        {
            TrySave();
            if (_cursor == 0)
            {
                return;
            }
            var i = _histories.IndexOf(_cursor);
            if (i > 0)
            {
                _histories.RemoveRange(i, _histories.Count - i);
            } else
            {
                i = _histories.Count;
            }
            _cursor = _histories[i - 1];
            Render();
        }

        public void Undo()
        {
            _canvas?.Undo();
        }

        public void Redo()
        {
            _canvas?.Redo();
        }

        public IDictionary<char, int> Count()
        {
            TrySave();
            var data = new EncodingBuilder();
            data.Append(_source);
            return data;
        }


    }
}
