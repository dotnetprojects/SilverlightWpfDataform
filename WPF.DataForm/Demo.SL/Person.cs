using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using WPF.DataForm;

namespace Demo
{
    public class Person : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event if needed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
           
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        
        #endregion

        private string lastname;
        private string comments;
        private DateTime dateOfBirth;
        private DateTimeOffset dateTest = DateTimeOffset.Now;
        private double? _weight2;
        private int? _weight3;


        [Display(Name = "Name", Order = 2, Prompt = "Lastname")]
        [Required]
        public string Lastname
        {
            get { return lastname; }
            set { lastname = value; }
        }

        [Display(Name = "Date of Birth")]
        [Required]
        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                if (dateOfBirth.Equals(value))
                    return;

                dateOfBirth = value;
                this.Age = Person.CalculateAge(dateOfBirth);

                this.OnPropertyChanged("DateOfBirth");
                this.OnPropertyChanged("Age");
            }
        }

        [Display(Name = "DateTimeOffset Test")]
        [Required]
        public DateTimeOffset DateTest
        {
            get { return dateTest; }
            set
            {
                if (dateTest.Equals(value))
                    return;

                dateTest = value;
                
                this.OnPropertyChanged("DateTest");                
            }
        }

        [Display(Name = "Age")]
        [Range(1, 100)]
        [Required]
        public int Age
        {
            get;
            set;
        }

        [Editable(false)]
        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        public double Weight { get; set; }

        public string Test1 { get; set; }

        public string Test2 { get; set; }

        public string Test3 { get; set; }

        public double? Weight2
        {
            get { return _weight2; }
            set
            {
                _weight2 = value;
                this.OnPropertyChanged("Weight2");
            }
        }

        public int? Weight3
        {
            get { return _weight3; }
            set
            {
                _weight3 = value;
                this.OnPropertyChanged("Weight3");
            }
        }

        public DateTime? DateTimeField { get; set; }

        [InputType()]
        public System.Windows.Media.Color FaceColor { get; set; }

        public bool? Is_Admin { get; set; }

        public Gender Gender { get; set; }

        public Gender? Gender2 { get; set; }

        public static int CalculateAge(DateTime birthDate)
        {
            // cache the current time
            DateTime now = DateTime.Today; // today is fine, don't need the timestamp from now
            // get the difference in years
            int years = now.Year - birthDate.Year;
            // subtract another year if we're before the
            // birth day in the current year
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                --years;

            return years;
        }

        public Color Color { get; set; }

        public Brush Brush { get; set; }

        public Guid Guid { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
