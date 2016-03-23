using System.ComponentModel.DataAnnotations;
using System.Web;

namespace _6tactics.Cms.Core.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSizeInMb)
        {
            _maxFileSize = maxFileSizeInMb * 1024 * 1024;
        }

        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            return file?.ContentLength <= _maxFileSize;
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(_maxFileSize.ToString());
        }
    }
}
