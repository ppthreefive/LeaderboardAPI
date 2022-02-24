/* Author: Phillip Pham
 * Date: 02/20/22
 * Description: This is a record representing a Leaderboard. I'm using this class to store a list of entries,
 *  and one of the reasons I am choosing to do this instead of just using List<Entry> collection is that perhaps
 *  my data source could one day provide more than just a list of entries in the future.
 * 
 * I went with record because I'm only going to store the data and init-once, and should not be modifying this data.
 */

using System.Collections.Generic;

namespace LeaderboardAPI.Models
{
    public record Leaderboard
    {
        public List<Entry> entries { get; init; }
    }
}