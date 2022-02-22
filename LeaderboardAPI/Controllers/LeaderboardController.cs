using LeaderboardAPI.Dtos;
using LeaderboardAPI.Models;
using LeaderboardAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaderboardAPI.Controllers
{
    [Route("api/leaderboard")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService leaderboardService;
        private readonly int defaultPageSize;

        public LeaderboardController(ILeaderboardService leaderboardService, string defaultPageSize) 
        {
            this.leaderboardService = leaderboardService;
            this.defaultPageSize = int.Parse(defaultPageSize);
        }

        [HttpGet]
        public async Task<ActionResult<LeaderboardDto>> GetLeaderboard([FromQuery] int? page, [FromQuery] int? count) 
        {
            LeaderboardDto data = leaderboardService.GetEntriesHelper(page, count, this.defaultPageSize);

            if (data == null)
            {
                return NotFound();
            }
            else 
            {
                return data;
            }
        }
    }
}
