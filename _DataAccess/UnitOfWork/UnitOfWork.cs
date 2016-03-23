using _DataAccess.Contexts;
using _DataAccess.Repositories;
using System;

namespace _DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private bool _disposed;

        public IGenericRepository<T> GenericRepo<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }

        public IContentItemRepository ContentItemRepo => new ContentItemRepository(_context);
        public IUserActivityTrackingRepository UserActivityTrackingRepo => new UserActivityTrackingRepository(_context);

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}