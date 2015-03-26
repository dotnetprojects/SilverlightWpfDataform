using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WPF.DataForm
{
    public partial class DateTimePicker : UserControl
    {
        public DateTimePicker()
        {
            InitializeComponent();
        }

        public DateTime? Value
        {
            get { return (DateTime?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(DateTime?), typeof(DateTimePicker),
                                        new PropertyMetadata(SelectedDateTime_PropertyChanged));

        private static void SelectedDateTime_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DateTimePicker;
            control.SelectedDate = (DateTime?)e.NewValue;
            control.SelectedTime = (DateTime?)e.NewValue;
        }

        private DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }
        private DateTime? SelectedTime
        {
            get { return (DateTime?)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        private static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DateTimePicker),
                                        new PropertyMetadata(SelectedDate_PropertyChanged));
        private static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(DateTime?), typeof(DateTimePicker),
                                        new PropertyMetadata(SelectedTime_PropertyChanged));

        private static void SelectedDate_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newDate = e.NewValue as DateTime?;
            var oldDate = d.GetValue(ValueProperty) as DateTime?;
            UpdateDateTimeValue(d, newDate, oldDate);
        }

        private static void SelectedTime_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newDate = e.NewValue as DateTime?;
            var oldDate = d.GetValue(ValueProperty) as DateTime?;
            UpdateDateTimeValue(d, oldDate, newDate);
        }

        private static void UpdateDateTimeValue(DependencyObject d, DateTime? dateValue, DateTime? timeValue)
        {
            if (dateValue != timeValue && (dateValue != null || timeValue != null))
            {
                dateValue = dateValue ?? DateTime.Now;
                timeValue = timeValue ?? DateTime.Today;

                var newDateTime = new DateTime(dateValue.Value.Year, dateValue.Value.Month, dateValue.Value.Day, timeValue.Value.Hour, timeValue.Value.Minute, timeValue.Value.Second);
                d.SetValue(ValueProperty, newDateTime);
            }
        }
    }
}
