using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace ZoDream.Reader.Controls
{
    [TemplatePart(Name = ValueTbName, Type = typeof(TextBox))]
    [TemplatePart(Name = PlusBtnName, Type = typeof(Button))]
    [TemplatePart(Name = MinusBtnName, Type = typeof(Button))]
    public sealed class NumberInput : Control
    {
        public const string ValueTbName = "PART_ValueTb";
        public const string PlusBtnName = "PART_PlusBtn";
        public const string MinusBtnName = "PART_MinusBtn";
        public NumberInput()
        {
            this.DefaultStyleKey = typeof(NumberInput);
        }


        public long Max
        {
            get { return (long)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Max.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(long), typeof(NumberInput), new PropertyMetadata(0L));

        public long Min
        {
            get { return (long)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Min.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof(long), typeof(NumberInput), new PropertyMetadata(0L));




        public uint Step
        {
            get { return (uint)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Step.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(uint), typeof(NumberInput), new PropertyMetadata((uint)1));



        public long Value
        {
            get { return (long)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(long), typeof(NumberInput), new PropertyMetadata(0L, OnValueChanged));

        public event EventHandler<long> ValueChanged;
        private TextBox ValueTb;
        private Button PlusBtn;
        private Button MinusBtn;

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NumberInput)?.UpdateValue(e.NewValue);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ValueTb = GetTemplateChild(ValueTbName) as TextBox;
            PlusBtn = GetTemplateChild(PlusBtnName) as Button;
            MinusBtn = GetTemplateChild(MinusBtnName) as Button;
            if (PlusBtn != null)
            {
                PlusBtn.Click += PlusBtn_Click;
            }
            if (MinusBtn != null)
            {
                MinusBtn.Click += MinusBtn_Click;
            }
            if (ValueTb != null)
            {
                ValueTb.TextChanged += ValueTb_TextChanged;
            }
            UpdateValue(Value);
        }

        private void UpdateValue(object val)
        {
            var v = val.ToString();
            if (v == null || ValueTb == null || ValueTb.Text.Trim() == v)
            {
                return;
            }
            ValueTb.Text = v;
        }

        private void MinusBtn_Click(object sender, RoutedEventArgs e)
        {
            var oldVal = Value;
            var val = Value - Step;
            if (val < Min)
            {
                val = Min;
            }
            Value = Convert.ToInt32(val);
            ValueTb.Text = val.ToString();
            ValueChanged?.Invoke(this, Value);
        }

        private void PlusBtn_Click(object sender, RoutedEventArgs e)
        {
            var oldVal = Value;
            var val = Value + Step;
            if (Max > 0 && val > Max)
            {
                val = Max;
            }
            Value = Convert.ToInt32(val);
            ValueTb.Text = val.ToString();
            ValueChanged?.Invoke(this, Value);
        }

        private void ValueTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            var oldVal = Value;
            var val = Convert.ToInt64((sender as TextBox).Text);
            if (val < Min)
            {
                val = Min;
            }
            else if (Max > 0 && val > Max)
            {
                val = Max;
            }
            SetValue(ValueProperty, val);
            if (!ValueTb.IsFocusEngaged)
            {
                ValueTb.Text = val.ToString();
            }
            ValueChanged?.Invoke(this, Value);
        }
    }
}
