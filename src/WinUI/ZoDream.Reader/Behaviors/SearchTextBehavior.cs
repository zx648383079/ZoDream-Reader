using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using System.Windows.Input;

namespace ZoDream.Reader.Behaviors
{
    public class SearchTextBehavior : Behavior<FrameworkElement>
    {
        public ICommand Command {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(SearchTextBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject is AutoSuggestBox suggestBox)
            {
                suggestBox.QuerySubmitted += AutoSuggestBox_QuerySubmitted;
            } else if (AssociatedObject is TextBox tb)
            {
                tb.KeyDown += TextBox_KeyDown;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject is AutoSuggestBox suggestBox)
            {
                suggestBox.QuerySubmitted -= AutoSuggestBox_QuerySubmitted;
            }
            else if (AssociatedObject is TextBox tb)
            {
                tb.KeyDown -= TextBox_KeyDown;
            }
        }

        private void TextBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                e.Handled = true;
                Command?.Execute((sender as TextBox).Text);
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            Command?.Execute(args.QueryText);
        }
    }
}
