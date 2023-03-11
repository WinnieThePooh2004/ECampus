﻿using ECampus.Domain.Data;
using ECampus.Domain.QueryParameters;

namespace ECampus.DataAccess.Contracts.DataAccess;

public interface IDataAccessFacade
{
    TModel Create<TModel>(TModel model) where TModel : class, IModel;
    Task<TModel> UpdateAsync<TModel>(TModel model, CancellationToken token = default) where TModel : class, IModel;
    TModel Delete<TModel>(TModel model) where TModel : class, IModel, new();
    Task<TModel> GetByIdAsync<TModel>(int id, CancellationToken token = default) where TModel : class, IModel;
    IQueryable<TModel> GetByParameters<TModel, TParameters>(TParameters parameters)
        where TModel : class, IModel
        where TParameters : IDataSelectParameters<TModel>;

    public Task<bool> SaveChangesAsync(CancellationToken token = default);
}