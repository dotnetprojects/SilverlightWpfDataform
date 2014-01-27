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

        public DateTime SelectedDateTime
        {
            get { return (DateTime)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateTimeProperty =
            DependencyProperty.Register("SelectedDateTime", typeof(DateTime), typeof(DateTimePicker),
                                        new PropertyMetadata(SelectedDateTime_PropertyChanged));

        private static void SelectedDateTime_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DateTimePicker;
            control.SelectedDate = (DateTime)e.NewValue;
            control.SelectedTime = (DateTime)e.NewValue;
        }

        private DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }
        private DateTime SelectedTime
        {
            get { return (DateTime)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        private static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(DateTimePicker),
                                        new PropertyMetadata(SelectedDate_PropertyChanged));
        private static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(DateTime), typeof(DateTimePicker),
                                        new PropertyMetadata(SelectedTime_PropertyChanged));

        private static void SelectedDate_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTime newDate = (e.NewValue as DateTime?) ?? DateTime.MinValue;
            DateTime oldDate = (DateTime)d.GetValue(SelectedDateTimeProperty);
            UpdateDateTimeValue(d, newDate, oldDate);
        }

        private static void SelectedTime_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTime newDate = (e.NewValue as DateTime?) ?? DateTime.MinValue;
            DateTime oldDate = (DateTime)d.GetValue(SelectedDateTimeProperty);
            UpdateDateTimeValue(d, oldDate, newDate);
        }

        private static void UpdateDateTimeValue(DependencyObject d, DateTime dateValue, DateTime timeValue)
        {
            if (dateValue != timeValue)
            {
                var newDateTime = new DateTime(dateValue.Year, dateValue.Month, dateValue.Day, timeValue.Hour, timeValue.Minute, timeValue.Second);
                d.SetValue(SelectedDateTimeProperty, newDateTime);
            }
        }
    }
}
