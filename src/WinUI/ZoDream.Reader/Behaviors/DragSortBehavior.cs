using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using System.Windows.Input;

namespace ZoDream.Reader.Behaviors
{
    [TypeConstraint(typeof(ListView))]
    public class DragSortBehavior: Trigger<ListView>
    {

        public ICommand DragCommand {
            get { return (ICommand)GetValue(DragCommandProperty); }
            set { SetValue(DragCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DragCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DragCommandProperty =
            DependencyProperty.Register("DragCommand", typeof(ICommand), typeof(DragSortBehavior), new PropertyMetadata(null));



        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DragItemsCompleted += AssociatedObject_DragItemsCompleted;
        }

        private void AssociatedObject_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            if (args.Items.Count == 0)
            {
                return;
            }
            DragCommand?.Execute(args.Items[0]);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.DragItemsCompleted -= AssociatedObject_DragItemsCompleted;
        }
    }
}
