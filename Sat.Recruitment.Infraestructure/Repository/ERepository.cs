using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sat.Recruitment.Infraestructure.Common;
using Sat.Recruitment.Infraestructure.IRepository;

namespace Sat.Recruitment.Infraestructure.Repository
{
	public class ERepository<TEntity> : IERepository<TEntity>
            where TEntity: class, new ()
	{
        private readonly IQueryableUnitOfWork unitOfWork;

        public ERepository(IQueryableUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            ValidateEntity(entity);
            try
            {
                await unitOfWork.GetSet<TEntity>().AddAsync(entity).ConfigureAwait(false);
                return await unitOfWork.CommitAsync().ConfigureAwait(false) > 0;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            ValidateEntity(entity);
            try
            {
                unitOfWork.GetSet<TEntity>().Remove(entity);
                return await unitOfWork.CommitAsync().ConfigureAwait(false) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            return await BuildQuery(filter, orderBy, includeProperties).ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            ValidateEntity(entity);
            try
            {
                unitOfWork.GetSet<TEntity>().Update(entity);
                return await unitOfWork.CommitAsync().ConfigureAwait(false) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void ValidateEntity(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "The object cannot be null");
            }
        }

        private IQueryable<TEntity> BuildQuery(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = unitOfWork.GetSet<TEntity>().AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(property =>
            {
                query = query.Include(property);
            });

            if (orderBy != null)
            {
                return orderBy(query);
            }

            return query;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
        }
    }
}

