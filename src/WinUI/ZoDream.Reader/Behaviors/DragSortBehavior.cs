using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;

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
            AssociatedObject.DragItemsStarting += AssociatedObject_DragItemsStarting;
            AssociatedObject.DragOver += AssociatedObject_DragOver;
            AssociatedObject.Drop += AssociatedObject_Drop;
            // AssociatedObject.DragItemsCompleted += AssociatedObject_DragItemsCompleted;
        }

        private async void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            if (!e.DataView.Contains(StandardDataFormats.Text))
            {
                return;
            }
            var target = (ListView)sender;
            var def = e.GetDeferral();
            var text = await e.DataView.GetTextAsync();
            var pos = e.GetPosition(target.ItemsPanelRoot);

            // If the target ListView has items in it, use the height of the first item
            //      to find the insertion index.
            var index = 0;
            if (target.Items.Count != 0)
            {
                // Get a reference to the first item in the ListView
                var sampleItem = (ListViewItem)target.ContainerFromIndex(0);

                // Adjust itemHeight for margins
                var itemHeight = sampleItem.ActualHeight + sampleItem.Margin.Top + sampleItem.Margin.Bottom;

                // Find index based on dividing number of items by height of each item
                index = Math.Min(target.Items.Count - 1, (int)(pos.Y / itemHeight));

                // Find the item being dropped on top of.
                var targetItem = (ListViewItem)target.ContainerFromIndex(index);

                // If the drop position is more than half-way down the item being dropped on
                //      top of, increment the insertion index so the dropped item is inserted
                //      below instead of above the item being dropped on top of.
                var positionInItem = e.GetPosition(targetItem);
                if (positionInItem.Y > itemHeight / 2)
                {
                    index++;
                }

                // Don't go out of bounds
                index = Math.Min(target.Items.Count, index);
            }
            DragCommand.Execute(new DragItemsResult([..Decode(text, target.Items)], index));
            def.Complete();
        }

        private void AssociatedObject_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move;
        }

        private void AssociatedObject_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            e.Data.SetText(Encode(e.Items));

            e.Data.RequestedOperation = DataPackageOperation.Move;
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
            AssociatedObject.DragItemsStarting -= AssociatedObject_DragItemsStarting;
            AssociatedObject.DragOver -= AssociatedObject_DragOver;
            AssociatedObject.Drop -= AssociatedObject_Drop;
            AssociatedObject.DragItemsCompleted -= AssociatedObject_DragItemsCompleted;
        }

        private static string Encode(IEnumerable<object> items)
        {
            return string.Join(',', items.Select(i => i.GetHashCode()));
        }

        private static IEnumerable<int> Decode(string text, IEnumerable<object> source)
        {
            var i = 0;
            var data = text.Split(',').Select(int.Parse).ToArray();
            foreach (var item in source)
            {

                if (data.Contains(item.GetHashCode()))
                {
                    yield return i;
                }
                i++;
            }
        }
    }

    internal record DragItemsResult(int[] ItemsIndex, int TargetIndex)
    {
        
    }
}
