using System;
using System.Globalization;
using System.Windows.Controls;

namespace SINeuronWPFApp.ValidationRules
{
    public class IntValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = Convert.ToString(value);

            if (string.IsNullOrEmpty(strValue))
                return new ValidationResult(false, $"Value cannot be coverted to string.");

            bool canConvert = false;
            int intVal = 0;
            canConvert = int.TryParse(strValue, out intVal);
            return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Wprowadź liczbę całkowitą.");
        }
    }
}
