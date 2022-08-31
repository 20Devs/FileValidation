using System.ComponentModel.DataAnnotations;
using WebApp.Attribute;

namespace WebApp.ViewModels
{
    public class FileValidationViewModel
    {
        [FileValidate(
            IsRequied = false, RequiredErrorMessage = "Please, select your logo",
            MaximumFileSize = 2 * 1024 * 1024, MaximumFileSizeErrorMessage = "Your file is too big",
            ExtensionAcceptable = "jpg,bmp,png", ExtensionAcceptableErrorMessage = "Your logo not supported"
        )]
        [DataType(DataType.Upload)]

        public IFormFile Logo { get; set; }
    }
}
