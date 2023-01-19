﻿using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.Shared.Interfaces.Domain.Validation;

public interface IDataValidator<in TModel>
    where TModel : class, IModel
{
    /// <summary>
    /// implement this method ONLY if it cannot be validated on domain level, e. g. username must be unique
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<ValidationResult> ValidateCreate(TModel model);

    /// <summary>
    /// implement this method ONLY if it cannot be validated on domain level, e. g. username must be unique
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<ValidationResult> ValidateUpdate(TModel model);

}