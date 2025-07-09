using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using Windows.System;
using ZoDream.Shared.Events;
using ZoDream.Shared.Interfaces;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Controls
{
    [TemplatePart(Name = CanvasElementName, Type = typeof(CanvasControl))]
    public sealed class TextRender : Control, ICanvasRender
    {
        internal const string CanvasElementName = "PART_Canvas";
        public TextRender()
        {
            this.DefaultStyleKey = typeof(TextRender);
        }

        private CanvasControl? _canvas;
        private readonly List<ICanvasLayer> LayerItems = [];

        public event PageChangedEventHandler? PageChanged;
        public event CanvasReadyEventHandler? OnReady;
        public CanvasControl Canvas => _canvas;
        public Vector2 Size => new((float)ActualWidth, (float)ActualHeight);

        public ICanvasSource Source {
            get { return (ICanvasSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ICanvasSource), typeof(TextRender), new PropertyMetadata(null));


        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _canvas = GetTemplateChild(CanvasElementName) as CanvasControl;
            if (_canvas is not null)
            {
                _canvas.Draw += Canvas_Draw;
                _canvas.CreateResources += Canvas_CreateResources;
            }
        }

        private void Canvas_CreateResources(CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            OnReady?.Invoke(this);
            Source?.ReadyAsync(this);
        }

        private void Canvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            LayerItems.Clear();
            Source.Animator.OnDraw(this);
            foreach (TextPageLayer item in LayerItems)
            {
                item.Draw(args.DrawingSession);
            }
        }

        protected override void OnKeyUp(KeyRoutedEventArgs e)
        {
            base.OnKeyUp(e);
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

        protected override Size MeasureOverride(Size availableSize)
        {
            Source.ReadyAsync(this);
            Source.Animator.Resize(Size);
            return base.MeasureOverride(availableSize);
        }

        protected override void OnManipulationStarted(ManipulationStartedRoutedEventArgs e)
        {
            base.OnManipulationStarted(e);
            Source.Animator.OnTouchStart(new((float)e.Position.X, (float)e.Position.Y));
        }

        protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            base.OnManipulationDelta(e);
            Source.Animator.OnTouchMove(new((float)e.Position.X, (float)e.Position.Y));
        }

        protected override void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {
            base.OnManipulationCompleted(e);
            Source.Animator.OnTouchFinish(new((float)e.Position.X, (float)e.Position.Y));
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            var point = e.GetCurrentPoint(this).Position;
            Source.Animator.OnTouchStart(new((float)point.X, (float)point.Y));
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
            var point = e.GetCurrentPoint(this).Position;
            Source.Animator.OnTouchFinish(new((float)point.X, (float)point.Y));
        }

        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            base.OnPointerMoved(e);
            var point = e.GetCurrentPoint(this).Position;
            Source.Animator.OnTouchMove(new((float)point.X, (float)point.Y));
        }

        public ICanvasLayer CreateLayer(Vector2 size)
        {
            var layer = new TextPageLayer(this);
            layer.Resize(new Vector4(0, 0, size.X, size.Y));
            return layer;
        }

        public void Invalidate()
        {
            // Animate.Invalidate();
            _canvas?.Invalidate();
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
