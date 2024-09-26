using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZoDream.Reader.Events;
using ZoDream.Shared.Interfaces.Entities;
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
    [TemplatePart(Name = ContainerName, Type = typeof(Panel))]
    [TemplatePart(Name = MenuName, Type = typeof(ActionMenuBox))]
    public class ActionListBox : Control
    {

        const string ContainerName = "PART_Container";
        const string MenuName = "PART_Menu";

        static ActionListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ActionListBox), new FrameworkPropertyMetadata(typeof(ActionListBox)));
        }

        public ActionListBox()
        {
            PreviewDragOver += ActionListBox_PreviewDragOver;
            PreviewDrop += ActionListBox_PreviewDrop;
        }

        private void ActionListBox_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var items = (IEnumerable<string>)e.Data.GetData(DataFormats.FileDrop);
                if (!items.Any())
                {
                    return;
                }
                DragCommand?.Execute(items);
            }
        }

        private void ActionListBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Link;
            e.Handled = true;
        }

        private Panel? _contianer;
        private ActionMenuBox? _menuBar;
        private INovel? _selectedItem;

        private double ItemWidth = 160;
        private double ItemHeight = 200;



        public ICommand AddCommand {
            get { return (ICommand)GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.Register("AddCommand", typeof(ICommand), typeof(ActionListBox), new PropertyMetadata(null));




        public ICommand Command {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ActionListBox), new PropertyMetadata(null));



        public ICommand DragCommand {
            get { return (ICommand)GetValue(DragCommandProperty); }
            set { SetValue(DragCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DragCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DragCommandProperty =
            DependencyProperty.Register("DragCommand", typeof(ICommand), typeof(ActionListBox), new PropertyMetadata(null));


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



        public IEnumerable<INovel> Items
        {
            get { return (IEnumerable<INovel>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable<INovel>), typeof(ActionListBox), new PropertyMetadata(null, OnItemsChange));

        private static void OnItemsChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ActionListBox).RefreshItems();
        }

        private void RefreshItems()
        {
            if (_contianer == null)
            {
                return;
            }
            BindListener();
            var data = Items?.ToList();
            var count = data == null ? 0 : data.Count();
            var j = 0;
            var removeItems = new List<int>();
            for (int i = 0; i < _contianer.Children.Count; i++)
            {
                var item = _contianer.Children[i];
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
                    _contianer.Children.RemoveAt(removeItems[i]);
                }
            }
            if (j >= count)
            {
                MoveActionButton();
                return;
            }
            for (; j < count; j++)
            {
                var book = new BookListBoxItem
                {
                    Width = ItemWidth,
                    Height = ItemHeight
                };
                book.Command = new RelayCommand<ActionHanlderArgs>(e =>
                {
                    if (e is null)
                    {
                        return;
                    }
                    if (e.Action == ActionEvent.CLICK && _selectedItem == e.Source)
                    {
                        return;
                    }
                    if (e.Action == ActionEvent.NONE)
                    {
                        _selectedItem = e.Source;
                        _menuBar?.Show(GetActionPosition(book));
                        return;
                    }
                    Command?.Execute(e);
                });
                _contianer.Children.Add(book);
                book.Source = data[j];
            }
            MoveActionButton();
        }

        private Point GetActionPosition(BookListBoxItem item)
        {
            var p = item.TranslatePoint(new Point(0, 0), this);
            p.X += item.ActualWidth - 60;
            p.Y += 30;
            return p;
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
            if (_contianer == null)
            {
                return;
            }
            var index = ActionButtonIndex();
            if (index < 0)
            {
                var button = new AddListBoxItem
                {
                    Width = ItemWidth,
                    Height = ItemHeight,
                    FontSize = ItemWidth / 3
                };
                button.MouseLeftButtonUp += (_, _) =>
                {
                    AddCommand?.Execute(null);
                };
                if (ActionOnBefore)
                {
                    _contianer.Children.Insert(0, button);
                } else
                {
                    _contianer.Children.Add(button);
                }
                return;
            }
            if (index == 0 && Items.Count() < 1)
            {
                return;
            }
            if ((index == 0 && ActionOnBefore) || (index == _contianer.Children.Count - 1 
                && !ActionOnBefore))
            {
                return;
            }
            if (ActionOnBefore)
            {
                _contianer.Children.Insert(0, _contianer.Children[index]);
            } else
            {
                _contianer.Children.Add(_contianer.Children[index]);
            }
        }

        private int ActionButtonIndex()
        {
            if (_contianer == null)
            {
                return -1;
            }
            for (int i = 0; i < _contianer.Children.Count; i++)
            {
                if (_contianer.Children[i] is AddListBoxItem)
                {
                    return i;
                }
            }
            return -1;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _contianer = GetTemplateChild(ContainerName) as Panel;
            _menuBar = GetTemplateChild(MenuName) as ActionMenuBox;
            if (_menuBar is not null)
            {
                _menuBar.OnAction += (_, e) => {
                    if (_selectedItem is null)
                    {
                        return;
                    }
                    Command?.Execute(new ActionHanlderArgs(_selectedItem, e));
                    _selectedItem = null;
                };
                _menuBar.IsVisibleChanged += (_, e) => {
                    if (!(bool)e.NewValue)
                    {
                        _selectedItem = null;
                    }
                };
            }
            RefreshItems();
        }
    }
}
