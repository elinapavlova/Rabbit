using System;
using Database;
using Infrastructure.Repositories.Response;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IResponseRepository ResponseRepository { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            ResponseRepository = new ResponseRepository(context);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}