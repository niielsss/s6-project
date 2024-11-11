using Microsoft.EntityFrameworkCore;
using MWL.ContentService.Domain.Repositories;
using MWL.ContentService.Storage.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ContentService.Storage.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly ContentContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public BaseRepository(ContentContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await DbSet.AddAsync(entity, cancellationToken);
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await DbSet.ToListAsync(cancellationToken);
        }

        public virtual async Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await DbSet.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await Context.SaveChangesAsync(cancellationToken);
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }
    }
}
