using System.Linq;
using System.Threading.Tasks;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        private readonly InternetAuctionContext _context;

        public TradeRepository(InternetAuctionContext context)
        {
            _context = context;
        }

        public IQueryable<Trade> FindAll()
        {
            return _context.Trades.AsQueryable();
        }

        public Task<Trade> GetByIdAsync(int id)
        {
            return _context.Trades.FindAsync(id).AsTask();
        }

        public Task AddAsync(Trade entity)
        {
            _context.Trades.Add(entity);
            return _context.SaveChangesAsync();
        }

        public void Update(Trade entity)
        {
            _context.Trades.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(Trade entity)
        {
            _context.Trades.Remove(entity);
            _context.SaveChanges();
        }

        public Task DeleteByIdAsync(int id)
        {
            _context.Trades.Remove(_context.Trades.Find(id));
            return _context.SaveChangesAsync();
        }

        public IQueryable<Trade> FindAllWithDetails()
        {
            return _context.Trades
                .Include(x => x.User)
                .Include(x => x.Lot)
                .AsQueryable();
        }

        public async Task<Trade> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Trades
                .Include(x => x.User)
                .Include(x => x.Lot)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
