using LeaderboardAPI.Dtos;
using LeaderboardAPI.Models;
using System.Collections.Generic;

namespace LeaderboardAPI.Repositories
{
    public interface ILeaderboardService
    {
        public LeaderboardDto GetEntriesHelper(int? page, int? count, int defaultPageSize);
        public LeaderboardDto getEntriesPaginated(int start, int count, int pageNum, int pageSize, int totalPages);
    }
}