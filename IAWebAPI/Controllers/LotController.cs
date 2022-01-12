using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.Logs;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Lot controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/lots/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LotController : Controller
    {
        private readonly ILotService _lotService;
        private readonly ILogger<LotController> _logger;

        public LotController(ILotService lotService, ILogger<LotController> logger)
        {
            _lotService = lotService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<LotModel>> GetAll()
        {
            try
            {
                var result = _lotService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getsoldlots")]
        [AllowAnonymous]
        public async Task<ActionResult<List<LotModel>>> GetSoldLots()
        {
            try
            {
                var result = await _lotService.GetSoldLotsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<LotModel>> GetById(int id)
        {
            try
            {
                var result = await _lotService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getuserlots/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<LotModel>>> GetLotsByUserId(string id)
        {
            try
            {
                var result = await _lotService.GetLotsByUserIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest();
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Add([FromBody] LotModel model)
        {
            try
            {
                await _lotService.AddAsync(model);
                return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(LotModel cardModel)
        {
            try
            {
                await _lotService.UpdateAsync(cardModel);
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("remove/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _lotService.DeleteByIdAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }
    }
}
