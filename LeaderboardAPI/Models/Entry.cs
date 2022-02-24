/* Author: Phillip Pham
 * Date: 02/20/22
 * Description: This is a record representing a Leaderboard entry. 
 * 
 * I went with record because I'm only going to store the data and init-once, and should not be modifying entries.
 */

namespace LeaderboardAPI.Models
{
    public record Entry
    {
        public string username { get; init; }
        public int score { get; init; }
        public int index { get; init; }
    }
}