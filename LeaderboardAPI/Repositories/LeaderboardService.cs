using LeaderboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaderboardAPI.Repositories
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly List<Entry> entries = new();

        public LeaderboardService()
        {
            this.entries = new List<Entry>();

        }

        public List<Entry> getEntriesPaginated(int start, int count)
        {
            if ((start + count) > this.entries.Count() || start < 0 || start > this.entries.Count())
            {
                return new List<Entry>();
            }

            if (this.entries.Count() < count)
            {
                return this.entries;
            }

            return this.entries.GetRange(start, count);
        }
    }
}
