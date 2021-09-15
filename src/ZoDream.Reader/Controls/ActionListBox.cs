using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZoDream.Reader.Events;
using ZoDream.Shared.Models;

namespace ZoDream.Reader.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ZoDream.Reader.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ZoDream.Reader.Controls;assembly=ZoDream.Reader.Controls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:ActionListBox/>
    ///
    /// </summary>
    [TemplatePart(Name = "PART_Container", Type = typeof(Panel))]
    public class ActionListBox : Control
    {
        static ActionListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ActionListBox), new FrameworkPropertyMetadata(typeof(ActionListBox)));
        }

        private Panel? boxContianer;

        private double ItemWidth = 160;
        private double ItemHeight = 200;

        public event ControlEventHandler? OnAdd;

        public event ActionItemEventHandler? OnAction;

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

        private void Obj_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RefreshItems();
        }

        private void Obj_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
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
                button.MouseLeftButtonUp += (_, _) =>
                {
                    OnAdd?.Invoke(this);
                };
                if (ActionOnBefore)
                {
                    boxContianer.Children.Insert(0, button);
                } else
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
            } else
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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            boxContianer = GetTemplateChild("PART_Container") as Panel;
            RefreshItems();
        }
    }
}
