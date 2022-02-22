using LeaderboardAPI.Dtos;

namespace LeaderboardAPI.Repositories
{
    public interface ILeaderboardService
    {
        public LeaderboardDto GetEntriesHelper(int? page, int? count, int defaultPageSize);
        public LeaderboardDto getEntriesPaginated(int start, int count, int pageNum, int pageSize, int totalPages);
    }
}