using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.CDAs
{
    public class PhoneFormat : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var phoneNumber = PhoneNumbers.PhoneNumberUtil.GetInstance();
                var dominicanFormat = phoneNumber.ParseAndKeepRawInput(value.ToString(), "DO");

                if (phoneNumber.IsValidNumber(dominicanFormat))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult($"{validationContext.DisplayName} is in incorrect format");
            }

            return null;
        }
    }
}
