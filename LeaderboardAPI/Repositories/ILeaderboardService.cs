using LeaderboardAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LeaderboardAPI.Repositories
{
    public interface ILeaderboardService
    {
        public Task<ActionResult<LeaderboardDto>> GetEntriesHelper(int? page, int? count, int defaultPageSize);
        public LeaderboardDto getEntriesPaginated(int start, int count, int pageNum, int pageSize, int totalPages);
    }
}