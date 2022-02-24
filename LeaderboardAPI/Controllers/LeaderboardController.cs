/* Author: Phillip Pham
 * Date: 02/20/22
 * Description: This is a simple controller which will manage our Get requests to get Leaderboard data. Note that it is using
 *  dependency injection for our repository/service, and also the default page size which is passed in from a configuration in
 *  startup.cs.
 *  
 *  Here we have defined only one Get request, which takes in optional page and count queries from the client. We are using nullable
 *  integers because integers are structs and will always default to zero if we don't make it nullable, which in turn makes it more
 *  difficult to design logic in our service/repository.
 */

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

        /// <summary>
        /// An asynchronous GET request which allows a client to recieve leaderboard information based on query paramaters.
        /// </summary>
        /// <param name="page">Optional query parameter of which page of the leaderboard the client wants to recieve.</param>
        /// <param name="count">Optional query parameter of the maximum entries per page the client wants to recieve.</param>
        /// <returns>An ActionResult of type LeaderboardDto or a NotFoundResult (404 code) depending on validity of client input.</returns>
        /// <response code="404">If the query parameters are not valid or the resource cannot be found.</response>
        [HttpGet]
        public async Task<ActionResult<LeaderboardDto>> GetLeaderboardAsync([FromQuery] int? page, [FromQuery] int? count) 
        {   
            var data = await leaderboardService.GetEntriesHelperAsync(page, count, this.defaultPageSize);

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
