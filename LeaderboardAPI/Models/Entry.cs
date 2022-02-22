using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaderboardAPI.Models
{
    public record Entry
    {
        public string username { get; init; }
        public int score { get; init; }
        public int index { get; init; }
    }
}
