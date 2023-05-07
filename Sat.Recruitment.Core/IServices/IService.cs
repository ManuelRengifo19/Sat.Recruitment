using System;
using System.Linq.Expressions;

namespace Sat.Recruitment.Core.IServices
{
	public interface IService<TEntity>
        where TEntity : class, new()
	{
        Task<bool> AddAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        Task<bool> UpdateAsync(TEntity entity);
    }
}

