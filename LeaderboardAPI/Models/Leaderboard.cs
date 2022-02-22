using System.Collections.Generic;

namespace LeaderboardAPI.Models
{
    public record Leaderboard
    {
        public List<Entry> entries { get; init; }
    }
}