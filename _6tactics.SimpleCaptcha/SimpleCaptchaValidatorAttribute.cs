using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace _6tactics.SimpleCaptcha
{
    public class SimpleCaptchaValidatorAttribute : ValidationAttribute
    {
        public string Captcha { get; }

        public SimpleCaptchaValidatorAttribute(string captchaName = "Captcha")
        {
            Captcha = captchaName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo captchaProperty = validationContext.ObjectType.GetProperty(Captcha);
            string captchaTask = captchaProperty.GetValue(validationContext.ObjectInstance, null).ToString();

            return value != null && Utilities.IsCaptchaValid(value.ToString(), captchaTask)
                ? null
                : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
