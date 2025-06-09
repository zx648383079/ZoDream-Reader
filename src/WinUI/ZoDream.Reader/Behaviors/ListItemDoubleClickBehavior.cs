using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using System.Windows.Input;

namespace ZoDream.Reader.Behaviors
{
    public class ListItemDoubleClickBehavior : Behavior<FrameworkElement>
    {
        public ICommand Command {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ListItemDoubleClickBehavior), new PropertyMetadata(null));


        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DoubleTapped += AssociatedObject_DoubleTapped;
        }



        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.DoubleTapped -= AssociatedObject_DoubleTapped;
        }
        private void AssociatedObject_DoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            if (sender is not Selector s)
            {
                Command.Execute(null);
                return;
            }
            if (s.SelectedItem is null)
            {
                return;
            }
            Command.Execute(s.SelectedItem);
        }


    }
}
