﻿using ECampus.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class GroupTimetableSelector : IMultipleItemSelector<Class, GroupTimetableParameters>
{
    public IQueryable<Class> SelectData(ApplicationDbContext context, GroupTimetableParameters parameters) =>
        context.Classes
            .Include(c => c.Auditory)
            .Include(c => c.Subject)
            .Include(c => c.Teacher)
            .Where(c => c.GroupId == parameters.GroupId);
}