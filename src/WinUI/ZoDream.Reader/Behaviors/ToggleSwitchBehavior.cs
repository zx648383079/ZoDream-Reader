using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using System.Windows.Input;
using Windows.System;

namespace ZoDream.Reader.Behaviors
{
    [TypeConstraint(typeof(ToggleSwitch))]
    public class ToggleSwitchBehavior : Trigger<ToggleSwitch>
    {




        public ICommand ToggleCommand {
            get { return (ICommand)GetValue(ToggleCommandProperty); }
            set { SetValue(ToggleCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToggleCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToggleCommandProperty =
            DependencyProperty.Register("ToggleCommand", typeof(ICommand), typeof(ToggleSwitchBehavior), new PropertyMetadata(null));




        public object CommandParameter {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(ToggleSwitchBehavior), new PropertyMetadata(null));



        protected override void OnAttached()
        {
            AssociatedObject.Toggled += AssociatedObject_Toggled;
        }

        private void AssociatedObject_Toggled(object sender, RoutedEventArgs e)
        {
            if (!ToggleCommand.CanExecute(CommandParameter))
            {
                return;
            }
            ToggleCommand?.Execute(CommandParameter);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Toggled -= AssociatedObject_Toggled;
        }
    }
}
