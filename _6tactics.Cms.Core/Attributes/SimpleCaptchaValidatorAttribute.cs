using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace _6tactics.Cms.Core.Attributes
{
    public class SimpleCaptchaValidatorAttribute : ValidationAttribute
    {
        public SimpleCaptchaValidatorAttribute(int minLength, params string[] propertyNames)
        {
            this.PropertyNames = propertyNames;
            this.MinLength = minLength;
        }

        public string[] PropertyNames { get; }
        public int MinLength { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var properties = PropertyNames.Select(validationContext.ObjectType.GetProperty);
            var values = properties.Select(p => p.GetValue(validationContext.ObjectInstance, null)).OfType<string>();
            var totalLength = values.Sum(x => x.Length) + Convert.ToString(value).Length;
            if (totalLength < MinLength)
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }
    }
}
