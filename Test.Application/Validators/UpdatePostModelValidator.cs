using FluentValidation;
using TestApi.Domain.Models.Posts;

namespace TestApi.Application.Validators
{
    public class UpdatePostModelValidator : AbstractValidator<UpdatePostModel>
    {
        public UpdatePostModelValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.Content)
                .MaximumLength(2000).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.Url)
                .MaximumLength(300).WithMessage("URL cannot exceed 300 characters.")
                .Must(IsValidUrl).WithMessage("URL must be a valid URI.");
        }

        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
