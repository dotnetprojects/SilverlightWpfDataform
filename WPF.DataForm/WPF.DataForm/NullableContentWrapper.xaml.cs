using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Animation;

namespace WPF.DataForm
{
    public partial class NullableContentWrapper
    {
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(NullableContentWrapper), new PropertyMetadata(OnValueChanged));

        private object oldValue = null;

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = d as NullableContentWrapper;
            if (e.NewValue == null)
            {
                ctl.nullCheck.IsChecked = true;
            }
            else
            {
                ctl.oldValue = e.NewValue;
                ctl.nullCheck.IsChecked = false;
                ctl.ObjectValue = e.NewValue;
            }

        }

        public object ObjectValue
        {
            get { return (object)GetValue(ObjectValueProperty); }
            set { SetValue(ObjectValueProperty, value); }
        }

        public static readonly DependencyProperty ObjectValueProperty =
            DependencyProperty.Register("ObjectValue", typeof(object), typeof(NullableContentWrapper), new PropertyMetadata(OnObjectValueChanged));

        private static void OnObjectValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = d as NullableContentWrapper;
            if (e.NewValue == null)
            {
                ctl.nullCheck.IsChecked = true;
            }
            else
            {
                ctl.oldValue = e.NewValue;
                ctl.nullCheck.IsChecked = false;
                ctl.Value = e.NewValue;
                //ctl.GetBindingExpression(ValueProperty).UpdateSource();
            }
        }

        public Type ObjectType { get; set; }

        
        public NullableContentWrapper()
        {
            InitializeComponent();

            //this.Loaded += NullableContentWrapper_Loaded;
        }

        void NullableContentWrapper_Loaded(object sender, RoutedEventArgs e)
        {
            Value = ObjectValue;
        }

        private void nullCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (ObjectValue != null)
            {
                oldValue = Value;
                ObjectValue = null;
                Value = null;               
            }
        }

        private void nullCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            if (oldValue != null)
            {                
                ObjectValue = oldValue;
                Value = oldValue;
            }
        }
    }
}
