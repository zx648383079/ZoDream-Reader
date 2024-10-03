using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Collections.Generic;
using Windows.System;
using ZoDream.Shared.Events;
using ZoDream.Shared.Interfaces;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Controls
{
    public sealed partial class TextContainer : UserControl, ICanvasRender
    {
        public TextContainer()
        {
            this.InitializeComponent();
        }

        private List<ICanvasLayer> LayerItems = [];

        public event PageChangedEventHandler? PageChanged;
        public event CanvasReadyEventHandler? OnReady;


        public ICanvasSource Source {
            get { return (ICanvasSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ICanvasSource), typeof(TextContainer), new PropertyMetadata(null));

        private void PaintCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            LayerItems.Clear();
            Source.Animator.OnDraw(this);
            foreach (PageLayer item in LayerItems)
            {
                item.Draw(args.DrawingSession);
            }
            
        }

        private void PaintCanvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            OnReady?.Invoke(this);
            Source?.ReadyAsync(this);
        }

        private void UserControl_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Right || e.Key == VirtualKey.PageDown)
            {
                Source.Animator.TurnNext();
                return;
            }
            if (e.Key == VirtualKey.Left || e.Key == VirtualKey.PageUp)
            {
                Source.Animator.TurnPrevious();
                return;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Source.ReadyAsync(this);
            Source.Animator.Resize(ActualWidth, ActualHeight);
        }

        private void UserControl_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            Source.Animator.OnTouchStart(e.Position.X, e.Position.Y);
        }

        private void UserControl_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Source.Animator.OnTouchMove(e.Position.X, e.Position.Y);
        }

        private void UserControl_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Source.Animator.OnTouchFinish(e.Position.X, e.Position.Y);
        }

        private void UserControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(this).Position;
            Source.Animator.OnTouchStart(point.X, point.Y);
        }

        private void UserControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(this).Position;
            Source.Animator.OnTouchFinish(point.X, point.Y);
        }

        private void UserControl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(this).Position;
            Source.Animator.OnTouchMove(point.X, point.Y);
        }


        public ICanvasLayer CreateLayer(double width, double height)
        {
            var layer = new PageLayer(this);
            layer.Resize(0, 0, width, height);
            return layer;
        }

        public void Invalidate()
        {
            // Animate.Invalidate();
            PaintCanvas.Invalidate();
        }

        public void DrawLayer(ICanvasLayer layer)
        {
            if (layer is null)
            {
                return;
            }
            LayerItems.Add(layer); 
        }
    }
}
