using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Store_TechniaclTask.DAL.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Abstraction
{
   public interface IUnitOfWork:IDisposable
    {
        void Commit(IDbContextTransaction transaction);
        Task CommitAsync(IDbContextTransaction transaction);
        int SaveChanges();
        Task<int> SaveChangesAsync();
        ApplicationDbContext context { get; }
        IDbContextTransaction CreatTransaction( );
        Task<IDbContextTransaction> CreatTransactionAsync( );
        Task RollBackAsync(IDbContextTransaction transaction);
        void RollBack(IDbContextTransaction transaction);

    }
}
