using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using TwentyDevs.MimeTypeDetective;

namespace WebApp.Attribute
{
    public class OtherFileValidate : ValidationAttribute
    {
        public bool     IsRequied                       { get; set; } = false;

        public string   RequiredErrorMessage            { get; set; }

        public int      MaximumFileSize                 { get; set; }

        public string   MaximumFileSizeErrorMessage     { get; set; }

        public string   ExtensionAcceptable             { get; set; }

        public string   ExtensionAcceptableErrorMessage { get; set; }

        private IEnumerable<string> Extensions
        {
            get
            {
                return ExtensionAcceptable.Split(',')
                    .Select(x =>
                        x.Trim().StartsWith('.')
                            ? x.Trim().ToLower()
                            : "." + x.Trim().ToLower()
                    );
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            //IsRequired Validation
            if (file == null || file.Length == 0)
            {
                if (IsRequied)
                {
                    return new ValidationResult(RequiredErrorMessage);
                }
                else
                {
                    return ValidationResult.Success;
                }
            }

            // File size validation
            if (file.Length > MaximumFileSize)
            {
                return new ValidationResult(MaximumFileSizeErrorMessage);
            }

            // Mimetype validation
            using (var stream = file.OpenReadStream())
            {
                var mimetype = stream.GetMimeType();

                if (mimetype == null)
                {
                    return new ValidationResult(ExtensionAcceptableErrorMessage);
                }

                if (!Extensions.Contains(mimetype.Extension))
                {
                    return new ValidationResult(ExtensionAcceptableErrorMessage);
                }
            }

            return ValidationResult.Success;

        }
    }
}
