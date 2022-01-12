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
    [Produces("application/json")]
    [Route("api/trade/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TradeController : Controller
    {
        private readonly ITradeService _tradeService;
        private readonly ILogger<TradeController> _logger;

        public TradeController(ITradeService tradeService, ILogger<TradeController> logger)
        {
            _tradeService = tradeService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<TradeModel>> GetAll()
        {
            try
            {
                var result = _tradeService.GetAll();
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
        public async Task<ActionResult<TradeModel>> GetById(int id)
        {
            try
            {
                var result = await _tradeService.GetByIdAsync(id);
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
        public async Task<ActionResult> Add([FromBody] TradeModel model)
        {
            try
            {
                await _tradeService.AddAsync(model);
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
        public async Task<ActionResult> Update(TradeModel model)
        {
            try
            {
                await _tradeService.UpdateAsync(model);
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
                await _tradeService.DeleteByIdAsync(id);
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
