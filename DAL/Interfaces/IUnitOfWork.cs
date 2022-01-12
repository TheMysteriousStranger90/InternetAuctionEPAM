using System;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace DAL.Interfaces
{
    /// <summary>
    /// Interface for accessing DB by repositories.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository CategoryRepository { get; }

        ILotRepository LotRepository { get; }

        ITradeRepository TradeRepository { get; }

        UserManager<User> UserManager { get; }

        SignInManager<User> SignInManager { get; }

        RoleManager<IdentityRole> RoleManager { get; }

        Task SaveAsync();
    }
}
