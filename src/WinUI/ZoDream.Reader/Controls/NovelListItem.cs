using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ZoDream.Reader.Utils;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Interfaces.Route;
using ZoDream.Shared.Repositories.Entities;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Controls
{
    public sealed class NovelListItem : Control
    {
        public NovelListItem()
        {
            this.DefaultStyleKey = typeof(NovelListItem);
        }



        public ImageSource Cover {
            get { return (ImageSource)GetValue(CoverProperty); }
            set { SetValue(CoverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Cover.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CoverProperty =
            DependencyProperty.Register("Cover", typeof(ImageSource), typeof(NovelListItem), new PropertyMetadata(null));

        public string Title {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(NovelListItem), new PropertyMetadata(string.Empty));




        public string Author {
            get { return (string)GetValue(AuthorProperty); }
            set { SetValue(AuthorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Author.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AuthorProperty =
            DependencyProperty.Register("Author", typeof(string), typeof(NovelListItem), new PropertyMetadata(string.Empty));




        public string FirstIcon {
            get { return (string)GetValue(FirstIconProperty); }
            set { SetValue(FirstIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FirstIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstIconProperty =
            DependencyProperty.Register("FirstIcon", typeof(string), typeof(NovelListItem), new PropertyMetadata(string.Empty));




        public string FirstText {
            get { return (string)GetValue(FirstTextProperty); }
            set { SetValue(FirstTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FirstText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstTextProperty =
            DependencyProperty.Register("FirstText", typeof(string), typeof(NovelListItem), new PropertyMetadata(string.Empty));




        public string SecondIcon {
            get { return (string)GetValue(SecondIconProperty); }
            set { SetValue(SecondIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SecondIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondIconProperty =
            DependencyProperty.Register("SecondIcon", typeof(string), typeof(NovelListItem), new PropertyMetadata(string.Empty));




        public string SecondText {
            get { return (string)GetValue(SecondTextProperty); }
            set { SetValue(SecondTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SecondText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondTextProperty =
            DependencyProperty.Register("SecondText", typeof(string), typeof(NovelListItem), new PropertyMetadata(string.Empty));




        public double IconFontSize {
            get { return (double)GetValue(IconFontSizeProperty); }
            set { SetValue(IconFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconFontSizeProperty =
            DependencyProperty.Register("IconFontSize", typeof(double), typeof(NovelListItem), new PropertyMetadata(12.0));


        public INovel Source {
            get { return (INovel)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(INovel), typeof(NovelListItem), new PropertyMetadata(null, OnSourceUpdated));


        public event TappedEventHandler? Touched;
        public event TappedEventHandler? LongTouched;
        private DateTime _lastTouchStart;



        private static void OnSourceUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NovelListItem)?.UpdateSource();
        }

        private void UpdateSource()
        {
            if (Source == null)
            {
                return;
            }
            Title = Source.Name;
            Cover = Converter.ToImg(Source.Cover);
            // Author = Source.Author;
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            _lastTouchStart = DateTime.Now;
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
            OnTouchEnd();
        }

        protected override void OnManipulationStarted(ManipulationStartedRoutedEventArgs e)
        {
            base.OnManipulationStarted(e);
            _lastTouchStart = DateTime.Now;
        }

        protected override void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {
            base.OnManipulationCompleted(e);
            OnTouchEnd();
        }

        private void OnTouchEnd()
        {
            var diff = DateTime.Now - _lastTouchStart;
            var router = App.GetService<IRouter>();
            var queries = new Dictionary<string, object>
            {
                {"novel",  Source},
                {"id", Source.Id},
            };
            if (diff.TotalSeconds > 1)
            {
                LongTouched?.Invoke(this, new TappedRoutedEventArgs());
                router.GoToAsync("novel", queries);
            } else
            {
                Touched?.Invoke(this, new TappedRoutedEventArgs());
                router.GoToAsync("read", queries);
            }
        }
    }
}
