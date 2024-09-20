using FluentValidation;
using MinimalApiProject.Models;

public class PriestAvailabilityValidator : AbstractValidator<PriestAvailabilityInput>
{
    public PriestAvailabilityValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start date is required.");
        RuleFor(x => x.EndDate).NotEmpty().WithMessage("End date is required.");
        RuleFor(x => x.StartTime).NotEmpty().WithMessage("Start time is required.");
        RuleFor(x => x.EndTime).NotEmpty().WithMessage("End time is required.");
    }
}
