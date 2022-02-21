using LeaderboardAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LeaderboardAPI.Repositories
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly List<Entry> entries = new();

        public LeaderboardService()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\leaderboard_data.json");
            string json = System.IO.File.ReadAllText(file);
            this.entries = JsonSerializer.Deserialize<Leaderboard>(json).entries;
        }

        public List<Entry> getAllEntries() 
        {
            return this.entries;
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
