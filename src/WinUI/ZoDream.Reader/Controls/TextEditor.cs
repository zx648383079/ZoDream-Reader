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

        public bool CanBack => _cursor > 0;
        public bool CanForward => _cursorNext < Text.Length;

        public string Text { 
            get => _source; 
            set {
                _source = value;
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
            var start = _cursor;
            var fontWidth = _canvas.FontSize * 2;
            var maxColumn = Math.Floor(_canvas.ActualWidth / fontWidth);
            var maxRow = (int)Math.Floor(_canvas.ActualHeight / fontWidth * .8);
            var row = 0;
            while (true)
            {
                var line = TextFormatter.LineText(_source, start, out _cursorNext);
                start = _cursorNext;
                if (line is null)
                {
                    break;
                }
                row += (int)Math.Min(1, Math.Ceiling(line.Length / maxColumn));
                if (row >= maxRow)
                {
                    break;
                }
            }
            _canvas.Text = _source[_cursor .. _cursorNext];
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

        public void GoForward()
        {
            if (!_histories.Contains(_cursor))
            {
                _histories.Add(_cursor);
            }
            _cursor = _cursorNext;
            Render();
        }

        public void GoBack()
        {
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

        public IDictionary<char, int> Count()
        {
            var data = new Dictionary<char, int>();
            foreach (var item in _source)
            {
                if (item is '\t' or ' ' or '\n' or '\r')
                {
                    continue;
                }
                if (data.TryAdd(item, 1))
                {
                    continue;
                }
                data[item]++;
            }
            return data;
        }
    }
}
