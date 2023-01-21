﻿using FluentValidation;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Domain.Validation.FluentValidators;

public class CourseDtoValidator : AbstractValidator<CourseDto>
{
    public CourseDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Course name cannot be empty");

        RuleFor(c => c.Teachers)
            .NotEmpty()
            .WithMessage("Course must have at least one teacher");
        
        RuleFor(c => c.Groups)
            .NotEmpty()
            .WithMessage("Course must have at least one group");
    }
}