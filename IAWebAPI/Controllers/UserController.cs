using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.Logs;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebAPI.Controllers
{
    /// <summary>
    /// User controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/users/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        //[Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAll()
        {
            try
            {
                var result = await _userService.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("usersRole")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<UserModel>> GetUsersRole(string userRole)
        {
            try
            {
                var result = _userService.GetUsersRole(userRole);
                return Ok(result);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryModel>> GetUserById(string id)
        {
            try
            {
                var result = await _userService.GetUserByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Add([FromBody] UserModel model)
        {
            try
            {
                await _userService.AddAsync(model);
                return CreatedAtAction(nameof(GetUserById), new {id = model.Id}, model);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<ActionResult> Update(UserModel userModel)
        {
            try
            {
                await _userService.UpdateAsync(userModel);
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("changeUserRole")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeUserRole(UserModel userModel)
        {
            try
            {
                await _userService.ChangeUserRole(userModel);
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("removeUserRole")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveUserRole(UserModel user)
        {
            try
            {
                await _userService.RemoveUserRole(user);
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("remove/{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _userService.DeleteByIdAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(SignupModel userRegistration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage)));
            }

            var authResponse = await _userService.SignupAsync(userRegistration);
            if (!authResponse.Success)
            {
                return BadRequest(authResponse.Errors);
            }

            return Ok(authResponse.Token);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage)));
            }

            var authResponse = await _userService.LoginAsync(login);
            if (!authResponse.Success)
            {
                return BadRequest(authResponse.Errors);
            }
            return Ok(authResponse.Token);
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return Ok();
        }
    }
}
