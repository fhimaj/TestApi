using FluentValidation;
using TestApi.Domain.Models.Users;
using TestApi.Infrastructure.Extensions;

namespace TestApi.Application.Validators
{
    public class AddUserModelValidator : AbstractValidator<AddUserModel>
    {
        public AddUserModelValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name can't be longer than 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Last name can't be longer than 100 characters.");

            RuleFor(x => x.DOB)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username can't be longer than 50 characters.");

            RuleFor(x => x.Preferences)
                .Must(x => x.IsValidJson()).WithMessage("Preferences must be valid json.")
                .MaximumLength(1000).WithMessage("Preferences can't exceed 1000 characters.");
        }
    }
}
