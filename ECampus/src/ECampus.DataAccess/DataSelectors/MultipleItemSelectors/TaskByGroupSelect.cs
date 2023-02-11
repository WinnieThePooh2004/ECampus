﻿using ECampus.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class TaskByGroupSelect : IParametersSelector<CourseTask, TasksByGroupParameters>
{
    public IQueryable<CourseTask> SelectData(ApplicationDbContext context, TasksByGroupParameters parameters) =>
        context.Groups
            .Include(group => group.Courses)!
            .ThenInclude(course => course.Tasks)
            .Where(group => group.Id == parameters.GroupId)
            .SelectMany(group => group.Courses!)
            .SelectMany(course => course.Tasks!);
}