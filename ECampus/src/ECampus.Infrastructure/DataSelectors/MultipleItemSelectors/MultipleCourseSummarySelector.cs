﻿using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleCourseSummarySelector : IMultipleItemSelector<Course, CourseSummaryParameters>
{
    public IQueryable<Course> SelectData(ApplicationDbContext context, CourseSummaryParameters parameters) =>
        context.Courses
            .Include(course => course.Teachers)
            .Include(course => course.Tasks)!
            .ThenInclude(task => task.Submissions!.Where(submission => submission.StudentId == parameters.StudentId));
}