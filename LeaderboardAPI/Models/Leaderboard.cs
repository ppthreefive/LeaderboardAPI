using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaderboardAPI.Models
{
    public class Leaderboard
    {
        public List<Entry> entries { get; private set; }

        public Leaderboard()
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

        public override string ToString()
        {
            string result = "";

            this.entries.ForEach(entry => {
                result += "User: " + entry.username + ", Score: " + entry.score + "\n";
            });

            return result;
        }
    }
}
