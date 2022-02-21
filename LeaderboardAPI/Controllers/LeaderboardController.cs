using LeaderboardAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public LeaderboardController(ILeaderboardService leaderboardService) 
        {
            this.leaderboardService = leaderboardService;
        }


    }
}
