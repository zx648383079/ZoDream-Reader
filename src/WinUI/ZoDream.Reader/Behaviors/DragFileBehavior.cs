using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using System;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;

namespace ZoDream.Reader.Behaviors
{
    public class DragFileBehavior: Behavior<FrameworkElement>
    {
        public ICommand Command {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DragFileBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DragOver += AssociatedObject_DragOver;
            AssociatedObject.Drop += AssociatedObject_Drop;
        }

        private async void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            if (!e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                return;
            }
            var items = await e.DataView.GetStorageItemsAsync();
            if (items.Count == 0)
            {
                return;
            }
            Command?.Execute(items);
        }

        private void AssociatedObject_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.DragOver -= AssociatedObject_DragOver;
            AssociatedObject.Drop -= AssociatedObject_Drop;
        }
    }
}
