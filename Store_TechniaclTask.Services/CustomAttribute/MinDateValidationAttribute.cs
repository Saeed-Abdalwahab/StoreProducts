using Store_TechniaclTask.Services.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace Store_TechniaclTask.Services.CustomAttribute
{
    public class MinDateValidationAttribute : ValidationAttribute
    {
        DateTime minDate;
        bool truncatetime;
        string dependentProperty;
        public MinDateValidationAttribute(bool Truncatetime = true)
        {
            minDate = truncatetime ? DateTime.Now.Date : DateTime.Now;
            truncatetime = Truncatetime;
        }
        public MinDateValidationAttribute(DateTime MinDate, bool Truncatetime = true)
        {
            minDate = Truncatetime ? minDate.Date : MinDate;

            truncatetime = Truncatetime;

        }
        public MinDateValidationAttribute(string dependentProperty)
        {
            this.dependentProperty = dependentProperty;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _localizationService = (LocService)validationContext.GetService(typeof(LocService));

            if (dependentProperty != null)
            {
                var containerType = validationContext.ObjectInstance.GetType();

                var field = containerType.GetProperty(this.dependentProperty);
                if (field != null)
                {
                    var dependentvalue = field.GetValue(validationContext.ObjectInstance, null);
                    minDate = Convert.ToDateTime(dependentvalue);
                }
            }

            DateTime targetDate = truncatetime ? Convert.ToDateTime(value).Date : Convert.ToDateTime(value);
            if (value == null || targetDate >= minDate)
            {
                return ValidationResult.Success;
            }
            var minDateErr = truncatetime ? minDate.ToShortDateString() : minDate.ToString("dd-MM-yyyy HH:mm");
            var Result = new ValidationResult(_localizationService.GetLocalizedHtmlString(this.ErrorMessage) + minDateErr, new[] { validationContext.MemberName });
            return Result;
        }



    }

}
