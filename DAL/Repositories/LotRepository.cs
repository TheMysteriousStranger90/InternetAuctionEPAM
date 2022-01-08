using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class LotRepository : ILotRepository
    {
        private readonly InternetAuctionContext _context;

        public LotRepository(InternetAuctionContext context)
        {
            _context = context;
        }

        public IQueryable<Lot> FindAll()
        {
            return _context.Lots.Include(x=>x.Category).AsQueryable();
        }

        public Task<Lot> GetByIdAsync(int id)
        {
            return _context.Lots.FindAsync(id).AsTask();
        }

        public Task AddAsync(Lot entity)
        {
            _context.Lots.Add(entity);
            return _context.SaveChangesAsync();
        }

        public void Update(Lot entity)
        {
            var el = _context.Lots.FirstOrDefault(x=>x.Id==entity.Id);
            foreach (var proptr in entity.GetType().GetProperties())
            {
                var value = proptr.GetValue(entity, null);
                if (proptr.Name == "Category")
                {
                    proptr.SetValue(el, _context.Categories.Find(entity.Category.Id));
                }
                else if (proptr.Name != "Id" && value!= proptr.GetValue(el, null))
                    proptr.SetValue(el, value, null);

            }
            _context.Entry(el).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Lot entity)
        {
            _context.Lots.Remove(entity);
            _context.SaveChanges();
        }

        public Task DeleteByIdAsync(int id)
        {
            _context.Lots.Remove(_context.Lots.Find(id));
            return _context.SaveChangesAsync();
        }

        public IQueryable<Lot> GetAllWithDetails()
        {
            return _context.Lots
                .Include(x => x.Name)
                .Include(x => x.Trade)
                .AsQueryable();
        }

        public async Task<Lot> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Lots
                .Include(x => x.Name)
                .Include(x => x.Trade)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Lot>> GetSoldLotsAsync()
        {
            return await _context.Lots.Where(x => x.Status == false).AsNoTracking().ToListAsync();
        }
    }
}
