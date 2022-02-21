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
        public ActionResult<LeaderboardDto> GetLeaderboard([FromQuery] string page, [FromQuery] string count) 
        {
            // TODO: Clean up the controller, can probably move some of this logic to the service
            int total = leaderboardService.getTotalCount();

            if (String.IsNullOrEmpty(page) && String.IsNullOrEmpty(count))
            {
                int remainder = total % this.defaultPageSize;
                int totalPages = total / this.defaultPageSize;
                if (remainder != 0)
                {
                    totalPages++;
                }

                return new LeaderboardDto(leaderboardService.getEntriesPaginated(0, this.defaultPageSize), 1, this.defaultPageSize, totalPages);
            }
            else
            {
                int pageSize = int.Parse(count);
                int pageNum = int.Parse(page) - 1;
                int start = pageSize * pageNum;
                int remainder = total % pageSize;
                int totalPages = total / pageSize;

                if (remainder != 0)
                {
                    totalPages++;
                }

                if ((pageNum + 1) == totalPages)
                {
                    start = (pageNum - 1) * pageSize;

                    return new LeaderboardDto(leaderboardService.getEntriesPaginated(start, remainder), pageNum, pageSize, totalPages);
                }

                return new LeaderboardDto(leaderboardService.getEntriesPaginated(start, pageSize), pageNum + 1, pageSize, totalPages);
            }
        }
    }
}
