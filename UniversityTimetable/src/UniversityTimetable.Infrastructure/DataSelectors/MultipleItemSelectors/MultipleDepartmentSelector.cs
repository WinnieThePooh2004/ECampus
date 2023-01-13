﻿using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleDepartmentSelector : IMultipleItemSelector<Department, DepartmentParameters>
{
    public IQueryable<Department> SelectData(DbSet<Department> data, DepartmentParameters parameters)
    {
        var result = data.Search(d => d.Name, parameters.DepartmentName);
        if (parameters.FacultyId == 0)
        {
            return result;
        }
        return result.Where(d => d.FacultyId == parameters.FacultyId);
    }
}