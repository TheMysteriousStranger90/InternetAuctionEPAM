using System.Linq;
using System.Threading.Tasks;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly InternetAuctionContext _context;

        public CategoryRepository(InternetAuctionContext context)
        {
            _context = context;
        }

        public IQueryable<Category> FindAll()
        {
            return _context.Categories.Include(x=>x.Lots).AsQueryable();
        }

        public Task<Category> GetByIdAsync(int id)
        {
            return _context.Categories.FindAsync(id).AsTask();
        }

        public Task AddAsync(Category entity)
        {
            _context.Categories.Add(entity);
            return _context.SaveChangesAsync();
        }

        public void Update(Category entity)
        {
            var element = _context.Categories.FirstOrDefault(x => x.Id == entity.Id);
            foreach (var proptr in entity.GetType().GetProperties())
            {
                var value = proptr.GetValue(entity, null);
                if (proptr.Name != "Id" && proptr.Name != "Lots" && value != proptr.GetValue(element, null))
                    proptr.SetValue(element, value, null);

            }
            _context.Entry(element).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Category entity)
        {
            _context.Categories.Remove(entity);
            _context.SaveChanges();
        }

        public Task DeleteByIdAsync(int id)
        {
            _context.Categories.Remove(_context.Categories.Find(id));
            return _context.SaveChangesAsync();
        }

        public IQueryable<Category> GetAllWithDetails()
        {
            return _context.Categories.Include(x => x.Lots).AsQueryable();
        }

        public IQueryable<Lot> GetLotByCategoryId(int id)
        {
            return _context.Categories.Include(x => x.Lots).First(x => x.Id == id).Lots.AsQueryable();
        }
    }
}
