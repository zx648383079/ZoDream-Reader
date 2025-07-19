using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Windows.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Controls
{
    [TemplatePart(Name = CloseButtonName, Type = typeof(Button))]
    public sealed partial class FindPanel : Control
    {
        const string CloseButtonName = "PART_CloseButton";
        const string SearchInputName = "PART_SearchInput";
        const string ReplaceInputName = "PART_ReplaceInput";
        public FindPanel()
        {
            DefaultStyleKey = typeof(FindPanel);
        }



        public string SearchText {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register(nameof(SearchText), typeof(string), typeof(FindPanel), 
                new PropertyMetadata(string.Empty));



        public string ReplaceText {
            get { return (string)GetValue(ReplaceTextProperty); }
            set { SetValue(ReplaceTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReplaceText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReplaceTextProperty =
            DependencyProperty.Register(nameof(ReplaceText), typeof(string), typeof(FindPanel), new PropertyMetadata(string.Empty));



        public bool IsReplaceMode {
            get { return (bool)GetValue(IsReplaceModeProperty); }
            set { SetValue(IsReplaceModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReplaceMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReplaceModeProperty =
            DependencyProperty.Register(nameof(IsReplaceMode), typeof(bool), 
                typeof(FindPanel), new PropertyMetadata(false, OnReplaceVisibleChanged));

        

        public bool IsOpen {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(FindPanel), 
                new PropertyMetadata(false, OnOpenChanged));

        public ICommand FindCommand {
            get { return (ICommand)GetValue(FindCommandProperty); }
            set { SetValue(FindCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FindCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FindCommandProperty =
            DependencyProperty.Register(nameof(FindCommand), typeof(ICommand), typeof(FindPanel), new PropertyMetadata(null));



        public ICommand ReplaceCommand {
            get { return (ICommand)GetValue(ReplaceCommandProperty); }
            set { SetValue(ReplaceCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReplaceCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReplaceCommandProperty =
            DependencyProperty.Register(nameof(ReplaceCommand), typeof(ICommand), typeof(FindPanel), new PropertyMetadata(null));




        public Visibility ReplaceVisible {
            get { return (Visibility)GetValue(ReplaceVisibleProperty); }
            set { SetValue(ReplaceVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReplaceVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReplaceVisibleProperty =
            DependencyProperty.Register(nameof(ReplaceVisible), typeof(Visibility), typeof(FindPanel), new PropertyMetadata(Visibility.Collapsed));

        private static void OnReplaceVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FindPanel;
            if (control == null)
            {
                return;
            }
            if (e.NewValue is Visibility v)
            {
                control.IsReplaceMode = v == Visibility.Visible;
            } else
            {
                control.ReplaceVisible = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private static void OnOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FindPanel;
            if (control == null)
            {
                return;
            }
            control.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var closeBtn = GetTemplateChild(CloseButtonName) as Button;
            var searchTb = GetTemplateChild(SearchInputName) as TextBox;
            var replaceTb = GetTemplateChild(ReplaceInputName) as TextBox;
            if (closeBtn is not null)
            {
                closeBtn.Click += CloseBtn_Click;
            }
            if (searchTb is not null)
            {
                searchTb.TextChanged += SearchTb_TextChanged;
            }
            if (replaceTb is not null)
            {
                replaceTb.TextChanged += ReplaceTb_TextChanged;
            }
        }

        private void ReplaceTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            ReplaceText = (sender as TextBox)!.Text;
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchText = (sender as TextBox)!.Text;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }
    }
}
