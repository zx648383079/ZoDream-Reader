﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.IO;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Storage;
using ZoDream.Shared.Text;

namespace ZoDream.Reader.Controls
{
    public class TextBoxEditor(TextBox control) : ITextEditor
    {
        public char NewLine => '\r';
        public string Text { get => control.Text; set => control.Text = value; }

        public bool CanBack => false;

        public bool CanForward => false;

        public bool CanUndo => control.CanUndo;

        public bool CanRedo => control.CanUndo;

        public int SelectionStart => control.SelectionStart;

        public int SelectionLength => control.SelectionLength;

        private int SelectionEnd => control.SelectionStart + control.SelectionLength;


        public IDictionary<char, int> Count()
        {
            var data = new EncodingBuilder();
            data.Append(Text);
            return data;
        }

        public bool FindNext(string text)
        {
            var lastIndex = SelectionEnd;
            var i = Text.IndexOf(text, lastIndex);
            if (i < 0)
            {
                return false;
            }
            Select(i, text.Length);
            return true;
        }

        public bool FindBack(string text)
        {
            var lastIndex = SelectionStart;
            if (lastIndex < text.Length)
            {
                return false;
            }
            var i = Text.LastIndexOf(text, lastIndex - 1);
            if (i < 0)
            {
                return false;
            }
            Select(i, text.Length);
            return true;
        }

        public void GoBack()
        {
            
        }

        public void GoForward()
        {
        }

        public void Load(Stream input)
        {
            using var reader = LocationStorage.Reader(input);
            Text = reader.ReadToEnd();
        }

        public void LoadFromFile(string fileName)
        {
            using var fs = File.OpenRead(fileName);
            Load(fs);
        }

        public void Redo()
        {
            control.Redo();
        }

        public void ScrollTo(int position)
        {
            Select(position, 0);
        }

        public void Select(int start, int count)
        {
            control.Focus(FocusState.Programmatic);
            control.Select(start, count);
        }

        public void Undo()
        {
            control.Undo();
        }

        public void Unselect()
        {
            control.Select(0, 0);
        }
    }
}
