using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics;

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
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                Debug.Fail(msg);
            }
        }
        #endregion

        private string lastname;
        private string comments;
        private DateTime dateOfBirth;

        [Display(Name="Name", Order=2, Prompt="Lastname")]
        [Required]
        public string Lastname
        {
            get { return lastname; }
            set { lastname = value; }
        }

        [Display(Name = "Date of Birth")]
        [Required]
        public DateTime DateOfBirth {
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

        public double? Weight2 { get; set; }

        public int? Weight3 { get; set; }

        public DateTime? DateTimeField { get; set; }

        [InputType()]
        public System.Windows.Media.Color FaceColor { get; set; }

        public bool? Is_Admin { get; set; }

        public Gender Gender { get; set; }

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
    }

    public enum Gender
    {
        Male,
        Female
    }
}
