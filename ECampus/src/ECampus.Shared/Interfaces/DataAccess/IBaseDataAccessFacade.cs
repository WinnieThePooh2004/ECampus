﻿using ECampus.Shared.Interfaces.Data.Models;

namespace ECampus.Shared.Interfaces.DataAccess;

public interface IBaseDataAccessFacade<TEntity> where TEntity : class, IModel
{
    public Task<TEntity> GetByIdAsync(int id);
    public Task<TEntity> CreateAsync(TEntity entity);
    public Task<TEntity> UpdateAsync(TEntity entity);
    public Task<TEntity> DeleteAsync(int id);
}