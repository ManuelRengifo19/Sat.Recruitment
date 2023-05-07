using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Sat.Recruitment.Infraestructure.Common
{
	public interface IQueryableUnitOfWork : IDisposable
	{
        void Commit();
            Task<int> CommitAsync();
            DbContext GetContext();
            DbSet<TEntity> GetSet<TEntity>() where TEntity : class, new();
	}
}

