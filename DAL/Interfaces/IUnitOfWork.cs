using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
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
