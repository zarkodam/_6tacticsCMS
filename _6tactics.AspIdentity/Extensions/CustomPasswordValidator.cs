using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace _6tactics.AspIdentity.Extensions
{
    public class CustomPasswordValidator : IIdentityValidator<string>
    {
        public int RequiredLength { get; set; }

        public CustomPasswordValidator(int length)
        {
            RequiredLength = length;
        }

        public Task<IdentityResult> ValidateAsync(string item)
        {
            if (string.IsNullOrEmpty(item) || item.Length < RequiredLength)
                return Task.FromResult(IdentityResult.Failed($"Password should be of length {RequiredLength}"));

            string pattern = @"^(?=.*[0-9])(?=.*[!@#$%^&*])[0-9a-zA-Z!@#$%^&*0-9]{10,}$";

            if (!Regex.IsMatch(item, pattern))
                return Task.FromResult(IdentityResult.Failed("Password should have one numeral and one special character"));

            return Task.FromResult(IdentityResult.Success);
        }
    }

}
