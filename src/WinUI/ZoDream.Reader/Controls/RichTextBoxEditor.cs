using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Windows.UI;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Text;

namespace ZoDream.Reader.Controls
{
    public class RichTextBoxEditor(RichEditBox control) : ITextEditor
    {
        public char NewLine => '\r';

        public string Text {
            get {
                control.Document.GetText(TextGetOptions.None, out var res);
                return res;
            }
            set {
                control.Document.SetText(TextSetOptions.None, value);
            }
        }

        public string SelectedText 
        {
            get {
                var range = control.Document.Selection;
                range.GetText(TextGetOptions.None, out var res);
                return res;
            }
        }
        public bool CanBack => false;

        public bool CanForward => false;

        public bool CanUndo => control.Document.CanUndo();

        public bool CanRedo => control.Document.CanRedo();

        public int SelectionStart => control.Document.Selection.StartPosition;

        public int SelectionLength => control.Document.Selection.Length;




        public void GoBack()
        {
        }

        public void GoForward()
        {
        }

        public void Load(Stream input)
        {
            control.Document.LoadFromStream(TextSetOptions.None, input.AsRandomAccessStream());
        }

        public void LoadFromFile(string fileName)
        {
            using var fs = File.OpenRead(fileName);
            Load(fs);
        }

        public void Redo()
        {
            control.Document.Redo();
        }

        public void ScrollTo(int position)
        {
            Select(position, 0);
        }

        public bool FindBack(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            var currentPosition = control.Document.Selection.StartPosition;
            var rangeBefore = control.Document.GetRange(0, currentPosition);
            rangeBefore.GetText(TextGetOptions.None, out string textBefore);
            var lastIndex = textBefore.LastIndexOf(text);
            if (lastIndex >= 0)
            {
                Select(lastIndex, text.Length);
                return true;
            }

            return false;
        }

        public bool FindNext(string text)
        {
            var range = control.Document.Selection;
            var next = control.Document.GetRange(range.EndPosition, TextConstants.MaxUnitCount);
            if (next.FindText(text, next.Length, FindOptions.None) > 0)
            {
                Select(next);
                return true;
            }
            return false;
        }
        private void Select(ITextRange range)
        {
            Select(range.StartPosition, range.Length);
        }
        public void Select(int start, int count)
        {
            var range = control.Document.Selection;
            control.Focus(FocusState.Programmatic);
            range.SetRange(start, start + count);
            range.ScrollIntoView(PointOptions.Start);
        }

        public void Undo()
        {
            control.Document.Undo();
        }

        public void Unselect()
        {
            var selected = control.Document.Selection;
            var position = selected.EndPosition;
            RemoveHighlights();

            control.Focus(FocusState.Programmatic);
            selected.SetRange(position, position);
        }

        private void HighlightMatches(string textToFind)
        {
            RemoveHighlights();

            var highlightBackgroundColor = (Color)App.Current.Resources["SystemColorHighlightColor"];
            var highlightForegroundColor = (Color)App.Current.Resources["SystemColorHighlightTextColor"];

            if (textToFind != null)
            {
                var searchRange = control.Document.GetRange(0, 0);
                while (searchRange.FindText(textToFind, TextConstants.MaxUnitCount, FindOptions.None) > 0)
                {
                    searchRange.CharacterFormat.BackgroundColor = highlightBackgroundColor;
                    searchRange.CharacterFormat.ForegroundColor = highlightForegroundColor;
                }
            }
        }

        private void RemoveHighlights()
        {
            var range = control.Document.GetRange(0, TextConstants.MaxUnitCount);
            var defaultBackground = control.Background as SolidColorBrush;
            var defaultForeground = control.Foreground as SolidColorBrush;

            range.CharacterFormat.BackgroundColor = defaultBackground.Color;
            range.CharacterFormat.ForegroundColor = defaultForeground.Color;
        }

        public IDictionary<char, int> Count()
        {
            var data = new EncodingBuilder();
            data.Append(Text);
            return data;
        }
    }
}
