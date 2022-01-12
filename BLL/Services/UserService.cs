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
using BLL.Configuration;
using BLL.Models;
using DAL.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JwtConfig _jwtConfig;


        public UserService(IUnitOfWork unitOfWork, IMapper mapper,
            IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _jwtConfig = optionsMonitor.CurrentValue;
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

        public async Task<UserModel> SignupAsync(SignupModel signup)
        {
            /*
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
            */

            var existingUser = await _unitOfWork.UserManager.FindByEmailAsync(signup.Email);

            if (existingUser is not null)
            {
                return new UserModel()
                {
                    Errors = new List<string>() {
                        "Email already in use"
                    },
                    Success = false
                };
            }

            var newUser = new IdentityUser() { Email = signup.Email, UserName = signup.UserName };
            var isCreated = await _unitOfWork.UserManager.CreateAsync((User) newUser, signup.Password);
            if (isCreated.Succeeded)
            {
                await _unitOfWork.UserManager.AddToRoleAsync((User) newUser, "User");
                var jwtToken = await GenerateJwtToken((User) newUser);

                return new UserModel()
                {
                    Success = true,
                    Token = jwtToken.ToString()
                };
            }
            else
            {
                return new UserModel()
                {
                    Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                    Success = false
                };
            }
        }

        public async Task<UserModel> LoginAsync(LoginModel login)
        {
            var existingUser = await _unitOfWork.UserManager.FindByEmailAsync(login.Email);

            if (existingUser is null)
            {
                return new UserModel()
                {
                    Errors = new List<string>() {
                        "Invalid login request"
                    },
                    Success = false
                };
            }

            var isCorrect = await _unitOfWork.UserManager.CheckPasswordAsync(existingUser, login.Password);

            if (!isCorrect)
            {
                return new UserModel()
                {
                    Errors = new List<string>() {
                        "Invalid password for user"
                    },
                    Success = false
                };
            }

            var jwtToken = await GenerateJwtToken(existingUser);

            return new UserModel()
            {
                Success = true,
                Token = jwtToken.ToString()
            };

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

        private async Task<object> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _unitOfWork.UserManager.GetRolesAsync(user);
            AddRolesToClaims(claims, roles);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(
                null,
                null,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static void AddRolesToClaims(List<Claim> claims, IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);
                claims.Add(roleClaim);
            }
        }
    }
}
