using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhoneBookAPI.Application.DTOs;
using PhoneBookAPI.Application.Services;

namespace PhoneBookAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IPersonService _personService;

        public StatisticsController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet("location")]
        public async Task<ActionResult<IEnumerable<LocationStatisticsDto>>> GetLocationStatistics()
        {
            var statistics = await _personService.GetLocationStatisticsAsync();
            return Ok(statistics);
        }
    }
}