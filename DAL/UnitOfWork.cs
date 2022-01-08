using System;
using System.Threading.Tasks;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InternetAuctionContext _context;

        public UnitOfWork(InternetAuctionContext context, RoleManager<IdentityRole> _roleManager,
            UserManager<User> _userManager, SignInManager<User> _signInManager)
        {
            _context = context;
            roleManager = _roleManager;
            userManager = _userManager;
            signInManager = _signInManager;
        }

        private ICategoryRepository _categoryRepository;

        public ICategoryRepository CategoryRepository
        {
            get { return _categoryRepository ?? (_categoryRepository = new CategoryRepository(_context)); }
        }

        private ILotRepository _lotRepository;

        public ILotRepository LotRepository
        {
            get { return _lotRepository ?? (_lotRepository = new LotRepository(_context)); }
        }

        private ITradeRepository _tradeRepository;

        public ITradeRepository TradeRepository
        {
            get { return _tradeRepository ?? (_tradeRepository = new TradeRepository(_context)); }
        }

        private readonly UserManager<User> userManager;

        public UserManager<User> UserManager
        {
            get
            {
                return userManager;
            }
        }

        private readonly SignInManager<User> signInManager;

        public SignInManager<User> SignInManager
        {
            get
            {
                return signInManager;
            }
        }

        private readonly RoleManager<IdentityRole> roleManager;

        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                return roleManager;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
