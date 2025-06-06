using API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API.Data.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {

        public Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);



        Task<TEntity?> GetByIdAsync(object id);

        Task AddAsync(TEntity entity);


        void Update(TEntity entity);

        void Delete(TEntity entity);

        IQueryable<TEntity> Search();

        void Save();
    }
}
