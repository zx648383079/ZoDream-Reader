using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using ZoDream.Reader.Events;
using ZoDream.Reader.Models;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace ZoDream.Reader.Controls
{
    [TemplatePart(Name = "PART_Container", Type = typeof(Panel))]
    public sealed class ActionListBox : Control
    {
        public ActionListBox()
        {
            this.DefaultStyleKey = typeof(ActionListBox);
            this.SizeChanged += (_, __) => RefreshSize();
        }

        private Panel boxContianer;

        private double ItemWidth = 160;
        private double ItemHeight = 200;

        public event ControlEventHandler OnAdd;

        public event ActionItemEventHandler OnAction;

        public bool ActionOnBefore
        {
            get { return (bool)GetValue(ActionOnBeforeProperty); }
            set { SetValue(ActionOnBeforeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActionOnBefore.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionOnBeforeProperty =
            DependencyProperty.Register("ActionOnBefore", typeof(bool), typeof(ActionListBox), new PropertyMetadata(true, OnActionChange));

        private static void OnActionChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ActionListBox).MoveActionButton();
        }

        public IEnumerable<BookItem> Items
        {
            get { return (IEnumerable<BookItem>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable<BookItem>), typeof(ActionListBox), new PropertyMetadata(null, OnItemsChange));

        private static void OnItemsChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ActionListBox).RefreshItems();
        }

        private void RefreshItems()
        {
            if (boxContianer == null)
            {
                return;
            }
            BindListener();
            var data = Items?.ToList();
            var count = data == null ? 0 : data.Count();
            var j = 0;
            var removeItems = new List<int>();
            for (int i = 0; i < boxContianer.Children.Count; i++)
            {
                var item = boxContianer.Children[i];
                if (item is AddListBoxItem)
                {
                    continue;
                }
                if (j >= count)
                {
                    removeItems.Add(i);
                    j++;
                    continue;
                }
                (item as BookListBoxItem).Source = data[j];
                j++;
            }
            if (removeItems.Count > 0)
            {
                for (int i = removeItems.Count - 1; i >= 0; i--)
                {
                    boxContianer.Children.RemoveAt(removeItems[i]);
                }
            }
            if (j >= count)
            {
                MoveActionButton();
                return;
            }
            for (; j < count; j++)
            {
                var book = new BookListBoxItem();
                book.Width = ItemWidth;
                book.Height = ItemHeight;
                book.OnAction += (_, item, e) =>
                {
                    OnAction?.Invoke(this, item, e);
                };
                boxContianer.Children.Add(book);
                book.Source = data[j];
            }
            MoveActionButton();
            RefreshSize();
        }

        private void RefreshSize()
        {
            if (boxContianer == null || !(boxContianer is Canvas))
            {
                return;
            }
            var x = .0;
            var y = .0;
            var maxW = this.ActualWidth;
            foreach (var item in boxContianer.Children)
            {
                Canvas.SetLeft(item, x);
                Canvas.SetTop(item, y);
                x += ItemWidth;
                if (x >=  maxW - 20)
                {
                    x = 0;
                    y += ItemHeight;
                }
            }
        }

        private void BindListener()
        {
            if (Items == null)
            {
                return;
            }
            if (Items is INotifyCollectionChanged)
            {
                var obj = Items as INotifyCollectionChanged;
                obj.CollectionChanged -= Obj_CollectionChanged;
                obj.CollectionChanged += Obj_CollectionChanged;
            }
            if (Items is INotifyPropertyChanged)
            {
                var obj = Items as INotifyPropertyChanged;
                obj.PropertyChanged -= Obj_PropertyChanged;
                obj.PropertyChanged += Obj_PropertyChanged;
            }
        }

        private void Obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RefreshItems();
        }

        private void Obj_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshItems();
        }

        private void MoveActionButton()
        {
            if (boxContianer == null)
            {
                return;
            }
            var index = ActionButtonIndex();
            if (index < 0)
            {
                var button = new AddListBoxItem();
                button.Width = ItemWidth;
                button.Height = ItemHeight;
                button.FontSize = ItemWidth / 3;
                button.Tapped += (_, __) =>
                {
                    OnAdd?.Invoke(this);
                };
                if (ActionOnBefore && boxContianer.Children.Count > 0)
                {
                    boxContianer.Children.Insert(0, button);
                }
                else
                {
                    boxContianer.Children.Add(button);
                }
                return;
            }
            if (index == 0 && Items.Count() < 1)
            {
                return;
            }
            if ((index == 0 && ActionOnBefore) || (index == boxContianer.Children.Count - 1
                && !ActionOnBefore))
            {
                return;
            }
            if (ActionOnBefore)
            {
                boxContianer.Children.Insert(0, boxContianer.Children[index]);
            }
            else
            {
                boxContianer.Children.Add(boxContianer.Children[index]);
            }
        }

        private int ActionButtonIndex()
        {
            if (boxContianer == null)
            {
                return -1;
            }
            for (int i = 0; i < boxContianer.Children.Count; i++)
            {
                if (boxContianer.Children[i] is AddListBoxItem)
                {
                    return i;
                }
            }
            return -1;
        }
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            boxContianer = GetTemplateChild("PART_Container") as Panel;
            RefreshItems();
        }
    }
}
