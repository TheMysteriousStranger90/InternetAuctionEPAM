using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Interfaces
{
    public interface IUserService<TModel> where TModel : class
    {
        Task AddAsync(TModel model);

        Task UpdateAsync(TModel model);

        Task DeleteByIdAsync(string modelId);

        Task<IEnumerable<UserModel>> GetAllAsync();

        Task<UserModel> GetUserByEmailAsync(string email);

        Task<UserModel> GetUserByIdAsync(string id);

        Task<AuthResponseModel> SignupAsync(SignupModel signup);

        Task<AuthResponseModel> LoginAsync(LoginModel login);

        IEnumerable<UserModel> GetUsersRole(string userRole);

        Task ChangeUserRole(UserModel userModel);

        Task RemoveUserRole(UserModel userModel);
    }
}
