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

namespace Demo.SL
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            this.dataForm1.CurrentItem = new Person() { Gender = Gender.Male, DateOfBirth = new DateTime(1988, 2, 14), Is_Admin = true };
        }
    }
}
