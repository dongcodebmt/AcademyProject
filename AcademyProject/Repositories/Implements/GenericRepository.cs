using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using AcademyProject.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AcademyProject.Repositories.Implements
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal AcademyProjectContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(AcademyProjectContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public async Task<int> Count()
        {
            return await dbSet.CountAsync<TEntity>();
        }

        public async Task<int> Count(Expression<Func<TEntity, bool>> where)
        {
            return await dbSet.CountAsync<TEntity>(where);
        }

        public async Task Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            await Delete(entityToDelete);
        }

        public async Task Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            await context.SaveChangesAsync();
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<TEntity> GetById(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<TEntity> Insert(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            //dbSet.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetList(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int skip = 0,
            int take = 0)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            if (take > 0)
            {
                query = query.Take(take);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> where)
        {
            return await dbSet.Where(where).FirstOrDefaultAsync<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetMany(Expression<Func<TEntity, bool>> where)
        {
            return await dbSet.Where(where).ToListAsync();
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> where)
        {
            return await dbSet.Where(where).AnyAsync<TEntity>();
        }
    }
}
