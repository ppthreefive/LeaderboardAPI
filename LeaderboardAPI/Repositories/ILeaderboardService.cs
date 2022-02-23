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