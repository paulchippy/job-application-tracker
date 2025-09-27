using FluentValidation;
using JobApplicationTracker.API.Models;

namespace JobApplicationTracker.API.Validators
{
    public class JobApplicationValidator : AbstractValidator<JobApplication>
    {
        public JobApplicationValidator()
        {
            RuleFor(x => x.CompanyName)
              .NotEmpty().WithMessage("Company Name value is required")
              .MaximumLength(100).WithMessage("Company Name should not exceed 100 characters.")
              .MinimumLength(2).WithMessage("Company Name must be at least 2 characters long.");

            RuleFor(x => x.Position)
              .NotEmpty().WithMessage("Position value is required")
              .MaximumLength(100).WithMessage("Position should not exceed 100 characters.")
              .MinimumLength(2).WithMessage("Position must be at least 2 characters long.");

            RuleFor(x => x.Status)
              .IsInEnum()
              .WithMessage("Status must be one of the following: Applied, Interviewing, Offered, Rejected.");

            RuleFor(x => x.DateApplied)
              .NotEmpty()
              .Must(dateApplied => dateApplied.Date <= DateTime.Today)
              .WithMessage("Date cannot be in the future");
        }
    }
}