using System;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;

namespace WPF.DataForm
{
    public class AttributeValidationRule : ValidationRule
    {
        private ValidationAttribute attribute;
        private string propName;

        public AttributeValidationRule(ValidationAttribute attribute, string propName)
        {
            this.attribute = attribute;
            this.propName = propName;
        }

        public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                this.attribute.Validate(value, this.propName);

                return new System.Windows.Controls.ValidationResult(true, null);
            }
            catch (FormatException fex)
            {
                return new System.Windows.Controls.ValidationResult(false, fex.Message);
            }
            catch (ValidationException ex)
            {
                return new System.Windows.Controls.ValidationResult(false, ex.Message);
            }
        }
    }
}
