using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ContentService.Domain.Repositories
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
    {
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);
        void Add(TEntity entity);
        Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
