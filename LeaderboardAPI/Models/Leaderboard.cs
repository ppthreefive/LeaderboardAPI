using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaderboardAPI.Models
{
    public record Leaderboard
    {
        public List<Entry> entries { get; init; }
    }
}
