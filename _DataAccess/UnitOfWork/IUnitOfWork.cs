using _DataAccess.Repositories;
using System;

namespace _DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        IGenericRepository<T> GenericRepo<T>() where T : class;
        IContentItemRepository ContentItemRepo { get; }
        IUserActivityTrackingRepository UserActivityTrackingRepo { get; }
    }
}
