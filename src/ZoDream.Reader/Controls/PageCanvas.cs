using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ZoDream.Shared.Animations;
using ZoDream.Shared.Controls;
using ZoDream.Shared.Events;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;
using ZoDream.Shared.Utils;

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
    ///     <MyNamespace:PageCanvas/>
    ///
    /// </summary>
    [TemplatePart(Name = LayerPanelName, Type = typeof(Canvas))]
    public class PageCanvas : Control, ICanvasRender
    {
        public const string LayerPanelName = "PART_LayerPanel";
        static PageCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageCanvas), new FrameworkPropertyMetadata(typeof(PageCanvas)));
        }

        public PageCanvas()
        {
            Loaded += PageCanvas_Loaded;
            Unloaded += PageCanvas_Unloaded;
        }

        private Canvas? LayerPanel;
        private readonly List<ICanvasLayer> LayerItems = [];
        public ICanvasSource Source {
            get { return (ICanvasSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ICanvasSource), typeof(PageCanvas), new PropertyMetadata(null));


        public Vector2 Size => new((float)ActualWidth, (float)ActualHeight);

        public event PageChangedEventHandler? PageChanged;
        public event CanvasReadyEventHandler? OnReady;


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            LayerPanel = GetTemplateChild(LayerPanelName) as Canvas;
            // InitLayer();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Source!.ReadyAsync(this);
            foreach (var item in LayerItems)
            {
                item.Resize(new Vector4(0, 0, (float)ActualWidth, (float)ActualHeight));
            }
        }

        private void PageCanvas_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void PageCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            OnReady?.Invoke(this);
            Source?.ReadyAsync(this);
        }


        #region 设置layer的数据
        //private void SetPage(ICanvasLayer dist, ICanvasLayer source)
        //{
        //    SetPage(dist, source.Page, source.Content);
        //}

        //private void SetPage(int dist, int page, IList<PageItem> items)
        //{
        //    if (dist < 0 || dist >= LayerItems.Count)
        //    {
        //        return;
        //    }
        //    SetPage(LayerItems[dist], page, items);
        //}

        //private void SetPage(ICanvasLayer dist, int page, IList<PageItem> items)
        //{
        //    App.Current.Dispatcher.Invoke(() =>
        //    {
        //        dist.Page = page;
        //        if (items != null)
        //        {
        //            dist.Add(items);
        //        } else
        //        {
        //            dist.Clear();
        //        }
        //    });
        //}

        //private void SetPage(int dist, int source)
        //{
        //    if (dist < 0 || dist >= LayerItems.Count || source < 0 || source >= LayerItems.Count)
        //    {
        //        return;
        //    }
        //    SetPage(LayerItems[dist], LayerItems[source]);
        //}
        #endregion



        public void Flush()
        {

        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            var p = e.GetPosition(this);
            Source.Animator.OnTouchStart(new((float)p.X, (float)p.Y));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }
            var p = e.GetPosition(this);
            Source.Animator.OnTouchMove(new((float)p.X, (float)p.Y));
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            var p = e.GetPosition(this);
            Source.Animator.OnTouchFinish(new((float)p.X, (float)p.Y));
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Right || e.Key == Key.PageDown)
            {
                Source.Animator.TurnNext();
                return;
            }
            if (e.Key == Key.Left || e.Key == Key.PageUp)
            {
                Source.Animator.TurnPrevious();
                return;
            }
        }


        private void InitLayer()
        {
            if (LayerPanel == null)
            {
                return;
            }
            LayerItems.Clear();
            LayerPanel.Children.Clear();
            for (int i = 0; i < 3; i++)
            {
                var layer = new PageLayer
                {
                    FontFamily = FontFamily,
                    FontSize = FontSize,
                    Background = Background,
                    Foreground = Foreground
                };
                LayerItems.Add(layer);
                LayerPanel.Children.Add(layer);
                layer.Resize(0, 0, ActualWidth, ActualHeight);
                Panel.SetZIndex(layer, i + 1);
            }
        }


        public ICanvasLayer CreateLayer(Vector2 size)
        {
            return new PageLayer
            {
                Width = size.X,
                Height = size.Y
            };
        }

        public void Invalidate()
        {
            Source.Animator.OnDraw(this);
        }

        public void DrawLayer(ICanvasLayer layer)
        {
            if (layer is null)
            {
                return;
            }
            var l = (FrameworkElement)layer;
            Canvas.SetZIndex(l, 1);
            if (LayerPanel!.Children.Contains(l))
            {
                return;
            }
            LayerPanel.Children.Add(l);
        }

        public async Task ReloadAsync()
        {
            if (Source is null)
            {
                return;
            }
            await Source.InvalidateAsync();
            Invalidate();
        }
    }
}
