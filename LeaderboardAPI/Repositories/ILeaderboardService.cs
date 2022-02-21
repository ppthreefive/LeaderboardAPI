using LeaderboardAPI.Models;
using System.Collections.Generic;

namespace LeaderboardAPI.Repositories
{
    public interface ILeaderboardService
    {
        List<Entry> getAllEntries();
        List<Entry> getEntriesPaginated(int start, int count);
    }
}