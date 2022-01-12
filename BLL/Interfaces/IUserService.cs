using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Interfaces
{
    /// <summary>
    /// IUserService
    /// </summary>
    public interface IUserService
    {
        Task AddAsync(UserModel model);

        Task UpdateAsync(UserModel model);

        Task DeleteByIdAsync(string modelId);

        Task<IEnumerable<UserModel>> GetAllAsync();

        Task<UserModel> GetUserByEmailAsync(string email);

        Task<UserModel> GetUserByIdAsync(string id);

        Task<UserModel> SignupAsync(SignupModel signup);

        Task<UserModel> LoginAsync(LoginModel login);

        IEnumerable<UserModel> GetUsersRole(string userRole);

        Task ChangeUserRole(UserModel userModel);

        Task RemoveUserRole(UserModel userModel);

        Task LogoutAsync();
    }
}
