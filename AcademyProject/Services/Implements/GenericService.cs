using AcademyProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AcademyProject.Services.Implements
{
    public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
    {
        private IGenericRepository<TEntity> genericRepository;
        

        public GenericService(IGenericRepository<TEntity> genericRepository)
        {
            this.genericRepository = genericRepository;
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> where)
        {
            return await genericRepository.Any(where);
        }

        public async Task<int> Count()
        {
            return await genericRepository.Count();
        }

        public async Task<int> Count(Expression<Func<TEntity, bool>> where)
        {
            return await genericRepository.Count(where);
        }

        public async Task Delete(object id)
        {
            await genericRepository.Delete(id);
        }

        public async Task Delete(TEntity entity)
        {
            await genericRepository.Delete(entity);
        }

        public async Task<IEnumerable<TEntity>> GetList(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int skip = 0, int take = 0)
        {
            return await genericRepository.GetList(filter, orderBy, includeProperties, skip, take);
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> where)
        {
            return await genericRepository.Get(where);
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await genericRepository.GetAll();
        }

        public async Task<TEntity> GetById(object id)
        {
            return await genericRepository.GetById(id);
        }

        public async Task<IEnumerable<TEntity>> GetMany(Expression<Func<TEntity, bool>> where)
        {
            return await genericRepository.GetMany(where);
        }

        public async Task<TEntity> Insert(TEntity entity)
        {
            return await genericRepository.Insert(entity);
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            return await genericRepository.Update(entity);
        }
    }
}
