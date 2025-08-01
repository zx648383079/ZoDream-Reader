using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using System;
using ZoDream.Reader.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Controls
{
    [TemplatePart(Name = TextElementName, Type = typeof(TextBlock))]
    public sealed partial class MatchTextBlock : Control
    {
        const string TextElementName = "PART_TextBlock";
        public MatchTextBlock()
        {
            DefaultStyleKey = typeof(MatchTextBlock);
        }



        public MatchItemViewModel Source {
            get { return (MatchItemViewModel)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(MatchItemViewModel), typeof(MatchTextBlock), 
                new PropertyMetadata(null, OnSourceChanged));


        private TextBlock? _control;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _control = GetTemplateChild(TextElementName) as TextBlock;
            OnSourceChanged();
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MatchTextBlock)?.OnSourceChanged();
        }

        private void OnSourceChanged()
        {
            if (Source is null || _control is null)
            {
                return;
            }
            _control.Inlines.Clear();
            var text = Source.Source.Text;
            var matchBegin = Source.MatchBegin;
            var matchEnd = Source.MatchEnd;
            var maxLength = (int)Math.Max(ActualWidth / FontSize * .8, 30);
            var begin = Math.Max(matchBegin - (maxLength - Source.MatchLength) / 2, 0);
            var end = Math.Min(begin + maxLength, text.Length);
            if (begin > 0)
            {
                _control.Inlines.Add(new Run()
                {
                    Text = "бн",
                    FontSize = FontSize,
                    FontWeight = FontWeights.Light,
                });
            }
            if (begin < matchBegin)
            {
                _control.Inlines.Add(new Run()
                {
                    Text = text[begin..matchBegin],
                    FontSize = FontSize,
                    FontWeight = FontWeights.Light,
                });
            }
            _control.Inlines.Add(new Run()
            {
                Text = text[matchBegin..matchEnd],
                FontSize = FontSize,
                FontWeight = FontWeights.Bold,
            });
            if (end > matchEnd)
            {
                _control.Inlines.Add(new Run()
                {
                    Text = text[matchEnd..end],
                    FontSize = FontSize,
                    FontWeight = FontWeights.Light,
                });
            }
            if (end < text.Length - 1)
            {
                _control.Inlines.Add(new Run()
                {
                    Text = "бн",
                    FontSize = FontSize,
                    FontWeight = FontWeights.Light,
                });
            }
        }
    }
}
