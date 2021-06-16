using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace SINeuronWPFApp.ValidationRules
{
    public class NonNegativeDoubleValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = Convert.ToString(value);

            if (string.IsNullOrEmpty(strValue))
                return new ValidationResult(false, $"Value cannot be coverted to string.");

            bool canConvert = false;
            var rx = new Regex(@"^\d+(\.\d+)?$");
            if (rx.IsMatch(strValue))
            {
                double doubleVal = 0;
                canConvert = double.TryParse(strValue, out doubleVal);
            }
            return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Wprowadź liczbę rzeczywistą.");
        }
    }
}
