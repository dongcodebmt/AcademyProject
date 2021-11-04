using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AcademyProject.Services
{
    public interface IGenericService<TEntity> where TEntity : class
    {
        Task<int> Count(Expression<Func<TEntity, bool>> where);
        Task Delete(object id);
        Task Delete(TEntity entity);
        Task<List<TEntity>> GetAll();
        Task<TEntity> GetById(object id);
        Task<TEntity> Insert(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task<IEnumerable<TEntity>> GetList(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int skip = 0,
            int take = 0);
        Task<TEntity> Get(Expression<Func<TEntity, bool>> where);
        Task<IEnumerable<TEntity>> GetMany(Expression<Func<TEntity, bool>> where);
        Task<bool> Any(Expression<Func<TEntity, bool>> where);
    }
}
