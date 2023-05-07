using System;
using Sat.Recruitment.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Sat.Recruitment.Infraestructure.Common
{
	public class RecruitmentDbContext: DbContext, IQueryableUnitOfWork
	{
        private readonly string? schema;

        public RecruitmentDbContext(DbContextOptions<RecruitmentDbContext> dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            schema = configuration.GetConnectionString("SchemaName");
        }

        public DbContext GetContext()
        {
            return this;
        }

        public DbSet<TEntity> GetSet<TEntity>() where TEntity : class, new()
        {
            return Set<TEntity>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                return;
            }

            modelBuilder.HasDefaultSchema(schema);
            modelBuilder.Entity <User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_products_3213E83FB0EB14EA");
                entity.ToTable("Users");
            });

            base.OnModelCreating(modelBuilder);
        }

        public void Commit()
        {
            try
            {
                SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload(); ;
            }
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                return await SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
        }
    }
}

