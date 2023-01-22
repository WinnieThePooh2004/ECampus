﻿using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.SingleItemSelectors;

public class SingleSubjectSelector : ISingleItemSelector<Subject>
{
    public async Task<Subject?> SelectModel(int id, DbSet<Subject> dataSource)
        => await dataSource.Include(t => t.Teachers)
            .FirstOrDefaultAsync(s => s.Id == id);
}