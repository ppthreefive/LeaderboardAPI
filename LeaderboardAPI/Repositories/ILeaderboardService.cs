/* Author: Phillip Pham
 * Date: 02/20/22
 * Description: This is a simple Interface that defines the LeaderboardService methods that dependency injected classes will have access to,
 *  and what methods are required to be implemented by the service.
 */

using LeaderboardAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LeaderboardAPI.Repositories
{
    public interface ILeaderboardService
    {
        public Task<ActionResult<LeaderboardDto>> GetEntriesHelperAsync(int? page, int? count, int defaultPageSize);
        public Task<LeaderboardDto> GetEntriesPaginatedAsync(int start, int count, int pageNum, int pageSize, int totalPages);
    }
}