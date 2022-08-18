using Store_TechniaclTask.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Repository.Abstraction;

namespace Store_TechniaclTask.Repository.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _DbContext;

        public ApplicationDbContext context => _DbContext;

        public UnitOfWork(ApplicationDbContext context)
        {
            this._DbContext = context;
        }
        public void Commit(IDbContextTransaction transaction)
        {
            transaction.Commit();
        }
        public async Task CommitAsync(IDbContextTransaction transaction)
        {
            await transaction.CommitAsync();
        }
        public IDbContextTransaction CreatTransaction()
        {
            return _DbContext.Database.BeginTransaction();
        }
        public async Task<IDbContextTransaction> CreatTransactionAsync()
        {
            return await _DbContext.Database.BeginTransactionAsync();
        }
        public int SaveChanges()
        {
            return this._DbContext.SaveChanges();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await this._DbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            this._DbContext.Dispose();
        }

        public async Task DisposeAsync()
        {
            await this._DbContext.DisposeAsync();
        }



        public void RollBack(IDbContextTransaction transaction)
        {
            transaction.Rollback();
            transaction.Dispose();
        }
        public async Task RollBackAsync(IDbContextTransaction transaction)
        {
            await transaction.RollbackAsync();
            await transaction.DisposeAsync();
        }

    }
}
