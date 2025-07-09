using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.Xaml.Interactivity;
using System;
using System.Windows.Input;

namespace ZoDream.Reader.Behaviors
{
    public class LongTouchBehavior: Trigger<FrameworkElement>
    {
        private const double OneSecond = 1000;
        private const int LongSpacing = (int)(OneSecond * 1.5);


        public ICommand Touched {
            get { return (ICommand)GetValue(TouchedProperty); }
            set { SetValue(TouchedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Touched.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TouchedProperty =
            DependencyProperty.Register("Touched", typeof(ICommand), typeof(LongTouchBehavior), new PropertyMetadata(null));



        public ICommand LongTouched {
            get { return (ICommand)GetValue(LongTouchedProperty); }
            set { SetValue(LongTouchedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LongTouched.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LongTouchedProperty =
            DependencyProperty.Register("LongTouched", typeof(ICommand), typeof(LongTouchBehavior), new PropertyMetadata(null));


        private int _lastTouchStart;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PointerPressed += AssociatedObject_PointerPressed;
            AssociatedObject.PointerReleased += AssociatedObject_PointerReleased;
            AssociatedObject.ManipulationStarted += AssociatedObject_ManipulationStarted;
            AssociatedObject.ManipulationCompleted += AssociatedObject_ManipulationCompleted;
        }

        private void AssociatedObject_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            OnTouchEnd();
        }

        private void AssociatedObject_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            OnTouchStart();
        }

        private void AssociatedObject_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            OnTouchEnd();
        }

        private void AssociatedObject_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            OnTouchStart();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }

        private void OnTouchStart()
        {
            _lastTouchStart = Environment.TickCount;
        }
 
        private void OnTouchEnd()
        {
            var diff = Environment.TickCount - _lastTouchStart;
            if (diff > LongSpacing)
            {
                LongTouched.Execute(this);
            } else
            {
                Touched.Execute(this);
            }
        }
    }
}
