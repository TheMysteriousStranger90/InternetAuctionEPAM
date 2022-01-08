using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using BLL.Configure;
using BLL.Models;
using DAL.Entities;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(UserModel model)
        {
            try
            {
                User _model = _mapper.Map<User>(model);
                await _unitOfWork.UserManager.CreateAsync(_model);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new InternetAuctionException(ex.Message);
            }
        }

        public Task UpdateAsync(UserModel model)
        {
            if (_unitOfWork.UserManager.Users.FirstOrDefault(x => x.Id == model.Id) == null) throw new InternetAuctionException("Users not found");
            _unitOfWork.UserManager.UpdateAsync(_mapper.Map<User>(model));
            return _unitOfWork.SaveAsync();
        }

        public async Task DeleteByIdAsync(string modelId)
        {
            var user = _unitOfWork.UserManager.Users.FirstOrDefault(x => x.Id == modelId);
            if (user == null) throw new InternetAuctionException("User not found");
            await _unitOfWork.UserManager.DeleteAsync(user);
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            var user = _unitOfWork.UserManager.Users.ToList();
            if (!user.Any()) throw new InternetAuctionException("Users  not found");
            var result = new List<UserModel>();
            foreach (var u in user)
            {
                UserModel userModel = _mapper.Map<User, UserModel>(u);
                var roles =await _unitOfWork.UserManager.GetRolesAsync(u);
                if (roles.Count > 0)
                    userModel.Role = roles.FirstOrDefault();
                result.Add(userModel);
            }
            return result;
        }

        public async Task<UserModel> GetUserByEmailAsync(string email)
        {
            if (email == null) throw new InternetAuctionException("Invalid email");

            var user = await _unitOfWork.UserManager.FindByEmailAsync(email);
            return _mapper.Map<User, UserModel>(user);
        }

        public async Task<UserModel> GetUserByIdAsync(string id)
        {
            if (id == null) throw new InternetAuctionException("Invalid id");

            var user = await _unitOfWork.UserManager.FindByIdAsync(id.ToString());
            if (user == null) throw new InternetAuctionException("User not found");

            var result = _mapper.Map<User, UserModel>(user);
            var role = await _unitOfWork.UserManager.GetRolesAsync(user);
            result.Role = role.First();
            return result;
        }

        public async Task<AuthResponseModel> SignupAsync(SignupModel signup)
        {
            if (_unitOfWork.UserManager.Users.Any(x => x.Email == signup.Email))
                return new AuthResponseModel
                {
                    Errors = new[] { "User already exist" }
                };
            try
            {
                User user = new User
                {
                    UserName = signup.UserName,
                    PasswordHash = signup.Password,
                    Email = signup.Email
                };

                var user_result = await _unitOfWork.UserManager.CreateAsync(user, signup.Password);
                if (user_result.Succeeded)
                {
                    var currentUser = await _unitOfWork.UserManager.FindByNameAsync(user.UserName);
                    await _unitOfWork.UserManager.AddToRoleAsync(currentUser, "RegisteredUser");
                }
                var result = await GenerateAuthResultAsync(user);
                return result;
            }
            catch (Exception ex)
            {
                return new AuthResponseModel
                {
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<AuthResponseModel> LoginAsync(LoginModel login)
        {
            var user = _unitOfWork.UserManager.Users.SingleOrDefault(x => x.Email == login.Email);
            if (user == null)
                return new AuthResponseModel
                {
                    Errors = new[] { "User not exist" }
                };

            var verified = _unitOfWork.UserManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, login.Password);
            if (verified != PasswordVerificationResult.Success)
                return new AuthResponseModel
                {
                    Errors = new[] { "Invalid Password" }
                };
            var result = await GenerateAuthResultAsync(user);
            return result;
        }

        public IEnumerable<UserModel> GetUsersRole(string userRole)
        {
            var userrole = _unitOfWork.UserManager.GetUsersInRoleAsync(userRole).Result;
            var result = (from user in userrole
                select _mapper.Map<User, UserModel>(user)).ToList();
            foreach(var u in result)
            {
                var role=  _unitOfWork.UserManager.GetRolesAsync(_mapper.Map<User>(u)).Result;
                u.Role = role.First();
            }
            return result;
        }

        public async Task ChangeUserRole(UserModel userModel)
        {
            var user = _mapper.Map<User>(userModel);
            if (_unitOfWork.UserManager.IsInRoleAsync(user, userModel.Role).Result)
                throw new InternetAuctionException($"User already is in role {userModel.Role}");

            var _user = _unitOfWork.UserManager.FindByIdAsync(user.Id.ToString()).Result;
            var previousRole = _unitOfWork.UserManager.GetRolesAsync(user).Result.FirstOrDefault();
            if(previousRole != null)
                await _unitOfWork.UserManager.RemoveFromRoleAsync(_user, previousRole);
            await _unitOfWork.UserManager.AddToRoleAsync(_user, userModel.Role);
        }

        public async Task RemoveUserRole(UserModel userModel)
        {
            var user = await _unitOfWork.UserManager.FindByIdAsync(userModel.Id.ToString());
            if (!_unitOfWork.UserManager.IsInRoleAsync(user, userModel.Role).Result)
                throw new InternetAuctionException($"User already is in role {userModel.Role}");

            await _unitOfWork.UserManager.RemoveFromRoleAsync(user, userModel.Role);
        }

        public async Task LogoutAsync()
        {
            await _unitOfWork.SignInManager.SignOutAsync();
        }

        private async Task<AuthResponseModel> GenerateAuthResultAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim("name", user.FirstName),
                new Claim("surname", user.LastName),
                new Claim("email", user.Email),
                new Claim("role", user.Role)
            };

            var now = DateTime.UtcNow;
            var tokenOptions = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new AuthResponseModel
            {
                Success = true,
                Token = tokenString
            };
        }
    }
}
