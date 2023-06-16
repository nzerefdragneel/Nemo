using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Nemo.GUI.ValidationRules
{
    public class IUserNameValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string username = value as string;

            if (string.IsNullOrEmpty(username))
            {
                return new ValidationResult(false, "Tài khoản không được để trống.");
            }
            return ValidationResult.ValidResult;

        }
    }
}
