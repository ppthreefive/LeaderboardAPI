using LeaderboardAPI.Dtos;
using LeaderboardAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
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
