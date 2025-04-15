using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.Storage;
using Windows.UI;
using ZoDream.Shared.Storage;

namespace ZoDream.Reader.Controls
{
    public class TextEditorControl
    {


        public RichEditBox? Instance { get; set; }

        public RichEditTextDocument? Document => Instance?.Document;

        public string Text 
        {
            get {
                var val = string.Empty;
                Document?.GetText(TextGetOptions.None, out val);
                return val;
            }
            set {
                Document?.SetText(TextSetOptions.None, value);
            }
        }

        public async void LoadFromFile(string fileName)
        {
            if (Instance is null)
            {
                return;
            }
            Text = await LocationStorage.ReadAsync(fileName);
        }

        public async void LoadFromFile(IStorageFile file)
        {
            if (Instance is null)
            {
                return;
            }
            Document?.LoadFromStream(TextSetOptions.None, await file.OpenAsync(FileAccessMode.Read));
        }

        public bool FindNext(string text)
        {
            if (Instance is null)
            {
                return false;
            }
            var highlightBackgroundColor = (Color)App.Current.Resources["SystemColorHighlightColor"];
            var highlightForegroundColor = (Color)App.Current.Resources["SystemColorHighlightTextColor"];
            var selection = Document!.Selection;
            var searchRange = selection;//Document.GetRange(selection.EndPosition, selection.EndPosition);
            while (searchRange.FindText(text, TextConstants.MaxUnitCount, FindOptions.None) > 0)
            {
                searchRange.CharacterFormat.BackgroundColor = highlightBackgroundColor;
                searchRange.CharacterFormat.ForegroundColor = highlightForegroundColor;
                searchRange.ScrollIntoView(PointOptions.Start);
                return true;
            }
            return false;
        }

        public void Unselect()
        {
            var documentRange = Document!.GetRange(0, TextConstants.MaxUnitCount);
            var defaultBackground = Instance!.Background as SolidColorBrush;
            var defaultForeground = Instance.Foreground as SolidColorBrush;

            documentRange.CharacterFormat.BackgroundColor = defaultBackground!.Color;
            documentRange.CharacterFormat.ForegroundColor = defaultForeground!.Color;
        }

    }
}
