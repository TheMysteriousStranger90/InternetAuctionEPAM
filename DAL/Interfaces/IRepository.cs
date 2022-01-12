﻿using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    /// <summary>
    /// IRepository
    /// </summary>
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> FindAll();

        Task<TEntity> GetByIdAsync(int id);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task DeleteByIdAsync(int id);
    }
}
