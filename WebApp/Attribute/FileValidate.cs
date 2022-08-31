using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using TwentyDevs.MimeTypeDetective;

namespace WebApp.Attribute
{
    /// <summary>
    /// Attribute to validate file content based on file Content(Mimetype Detection)
    /// </summary>
    public class FileValidate : System.Attribute, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.IModelValidator
    {
        public bool     IsRequied                           { get; set; } = false;

        public string   RequiredErrorMessage                { get; set; }

        public int      MaximumFileSize                     { get; set; }

        public string   MaximumFileSizeErrorMessage         { get; set; }

        public string   ExtensionAcceptable                 { get; set; }

        public string   ExtensionAcceptableErrorMessage     { get; set; }

        private IEnumerable<string>  Extensions
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

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            var file = context.Model as IFormFile;

            //IsRequired Validation
            if (file == null || file.Length ==0 )
            {
                if (IsRequied)
                {
                    return new List<ModelValidationResult>()
                    {
                        new ModelValidationResult
                            (
                                memberName  : context.ModelMetadata.PropertyName,
                                message     : RequiredErrorMessage
                            ),
                    };
                }
                else
                {
                    return Enumerable.Empty<ModelValidationResult>();
                }
            }

            // File size validation
            if (file.Length > MaximumFileSize)
            {
                return new List<ModelValidationResult>()
                {
                    new ModelValidationResult
                    (
                        memberName  : context.ModelMetadata.PropertyName,
                        message     : MaximumFileSizeErrorMessage
                    ),
                };
            }
            // Mimetype validation
            using (var stream = file.OpenReadStream())
            {
                var mimetype = stream.GetMimeType();

                if (mimetype == null)
                {
                    return new List<ModelValidationResult>()
                    {
                        new ModelValidationResult
                        (
                            memberName  : context.ModelMetadata.PropertyName,
                            message     : ExtensionAcceptableErrorMessage
                        ),
                    };
                }

                if (!Extensions.Contains(mimetype.Extension))
                {
                    return new List<ModelValidationResult>()
                    {
                        new ModelValidationResult
                        (
                            memberName  : context.ModelMetadata.PropertyName,
                            message     : ExtensionAcceptableErrorMessage
                        ),
                    };
                }
            }

            return new List<ModelValidationResult>(); ;
        }
    }
}
