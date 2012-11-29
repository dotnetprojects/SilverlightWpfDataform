using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.dataForm1.CurrentItem = new Person() { Gender = Gender.Male, DateOfBirth = new DateTime(1988, 2, 14), Is_Admin = true };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataForm1.IsValid())
                MessageBox.Show("Bravo le formulaire est valide!!");
            else
            {
                ReadOnlyObservableCollection<ValidationError> errors = this.dataForm1.GetErrors();
                StringBuilder sb = new StringBuilder("Hum... il y a quelque(s) erreur(s).\n");
                foreach (ValidationError error in errors)
                {
                    sb.AppendLine(error.ErrorContent.ToString());
                }
                MessageBox.Show(sb.ToString());
            }
        }
    }
}
