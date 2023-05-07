using System;
using System.Linq.Expressions;
using Sat.Recruitment.Core.IServices;
using Sat.Recruitment.Infraestructure.IRepository;

namespace Sat.Recruitment.Core.Services
{
	public class Service<TEntity> : IService<TEntity>
		where TEntity : class, new()
	{
        private readonly IERepository<TEntity> repository;

        public Service(IERepository<TEntity> repository)
        {
            this.repository = repository;
        }

        public async Task<bool> AddAsync(TEntity tEntity)
        {
            return await repository.AddAsync(tEntity).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(TEntity tEntity)
        {
            return await repository.DeleteAsync(tEntity).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
                IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            return await repository.GetAllAsync(filter, orderBy, includeProperties).ConfigureAwait(false);
        }

        public async Task<bool> UpdateAsync(TEntity tEntity)
        {
            return await repository.UpdateAsync(tEntity).ConfigureAwait(false);
        }
    }
}

