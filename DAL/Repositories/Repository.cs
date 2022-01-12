using System.Linq;
using System.Threading.Tasks;
using DAL.Context;
using DAL.Interfaces;

namespace DAL.Repositories
{
    /// <summary>
    /// Basic Repository
    /// </summary>
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly InternetAuctionContext _context;

        protected Repository(InternetAuctionContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> FindAll()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public Task<TEntity> GetByIdAsync(int id)
        {
            return _context.Set<TEntity>().FindAsync(id).AsTask();
        }

        public Task AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            return _context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            _context.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChangesAsync();
        }

        public Task DeleteByIdAsync(int id)
        {
            _context.Set<TEntity>().Remove(_context.Set<TEntity>().Find(id));
            return _context.SaveChangesAsync();
        }
    }
}
