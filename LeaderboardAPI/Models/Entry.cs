using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaderboardAPI.Models
{
    public class Entry
    {
        public string username { get; private set; }
        public int score { get; private set; }
        public int index { get; private set; }

        public Entry(string username, int score, int index)
        {
            this.username = username;
            this.score = score;
            this.index = index;
        }
    }
}
