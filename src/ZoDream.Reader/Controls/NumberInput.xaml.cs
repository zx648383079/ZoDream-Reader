﻿using System;
using System.Collections.Generic;
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

namespace ZoDream.Reader.Controls
{
    /// <summary>
    /// NumberInput.xaml 的交互逻辑
    /// </summary>
    public partial class NumberInput : UserControl
    {
        public NumberInput()
        {
            InitializeComponent();
        }



        public int Max
        {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Max.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(int), typeof(NumberInput), new PropertyMetadata(0));




        public int Min
        {
            get { return (int)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Min.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof(int), typeof(NumberInput), new PropertyMetadata(0));




        public uint Step
        {
            get { return (uint)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Step.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(uint), typeof(NumberInput), new PropertyMetadata((uint)1));



        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(NumberInput), new PropertyMetadata(0, OnValueChanged));

        public event ValueChangedEventHandler<int>? ValueChanged;

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tb = d as NumberInput;
            tb.NumberTb.Text = e.NewValue.ToString();
        }

        private void MinusBtn_Click(object sender, RoutedEventArgs e)
        {
            var val = Value - Step;
            if (val < Min)
            {
                val = Min;
            }
            Value = Convert.ToInt32(val);
            NumberTb.Text = val.ToString();
            ValueChanged?.Invoke(this, Value);
        }

        private void PlusBtn_Click(object sender, RoutedEventArgs e)
        {
            var val = Value + Step;
            if (Max > 0 && val > Max)
            {
                val = Max;
            }
            Value = Convert.ToInt32(val);
            NumberTb.Text = val.ToString();
            ValueChanged?.Invoke(this, Value);
        }

        private void NumberTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            var val = Convert.ToInt32((sender as TextBox).Text);
            if (val < Min)
            {
                val = Min;
            } else if (Max > 0 && val > Max)
            {
                val = Max;
            }
            Value = val;
            NumberTb.Text = val.ToString();
            ValueChanged?.Invoke(this, val);
        }
    }
}