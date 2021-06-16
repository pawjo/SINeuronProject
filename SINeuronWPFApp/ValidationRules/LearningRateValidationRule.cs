using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace SINeuronWPFApp.ValidationRules
{
    public class LearningRateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = Convert.ToString(value);

            if (string.IsNullOrEmpty(strValue))
                return new ValidationResult(false, $"Value cannot be coverted to string.");

            bool result = false;
            var rx = new Regex(@"^-?\d+(\.\d+)?$");
            if (rx.IsMatch(strValue))
            {
                bool canConvert = false;
                double doubleVal = 0;
                canConvert = double.TryParse(strValue, out doubleVal);
                bool range = doubleVal > 0 && doubleVal < 1;
                result = canConvert && range;
            }

            return result ? new ValidationResult(true, null) : new ValidationResult(false, $"Wprowadź liczbę ");
        }
    }
}
