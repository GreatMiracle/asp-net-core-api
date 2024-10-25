using FluentValidation;
using WebApplication1.Core.Utils;
using WebApplication1.DTOs.Request;

namespace WebApplication1.Validators
{
    public class SearchFileRequestValidator : AbstractValidator<GoogleDriveSearchOptionsRequest>
    {
        public SearchFileRequestValidator()
        {
            RuleFor(x => x.MimeType)
                .Must(IsValidMimeType)
                .WithMessage("Invalid MimeType. Please provide a valid Google Drive mime type.");
        }

        private bool IsValidMimeType(string mimeType)
        {
            return typeof(GoogleDriveMimeType).GetFields()
                .Any(f => f.GetValue(null).ToString() == mimeType);
        }
    }
}
