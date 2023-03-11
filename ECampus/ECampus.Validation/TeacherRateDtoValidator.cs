using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using FluentValidation;

namespace ECampus.Validation;

public class TeacherRateDtoValidator : AbstractValidator<TeacherRateDto>
{
    private static readonly int RateTypesCount = Enum.GetValues<RateType>().Length;

    public TeacherRateDtoValidator()
    {
        RuleFor(t => t.Feedback)
            .MaximumLength(450);

        RuleFor(t => t.Rates.Count)
            .Equal(RateTypesCount)
            .WithMessage("You must rate all to be able to submit this rate");

        RuleForEach(t => t.Rates)
            .Must(rate => rate.Value <= 10)
            .WithMessage("Max value of each rate is 10")
            .Must(rate => rate.Value >= 0)
            .WithMessage("Min value of each rate is 0");

        RuleFor(t => t.TotalRate)
            .LessThanOrEqualTo((byte)10)
            .WithMessage("Max total rate is 10")
            .GreaterThanOrEqualTo((byte)0)
            .WithMessage("Min total rate is 0");
        
        RuleFor(t => t.KnowledgeEsteem)
            .LessThanOrEqualTo((byte)10)
            .WithMessage("Max knowledge esteem value is 10")
            .GreaterThanOrEqualTo((byte)0)
            .WithMessage("Min knowledge esteem value is 0");
    }
}