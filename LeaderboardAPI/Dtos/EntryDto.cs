/* Author: Phillip Pham
 * Date: 02/20/22
 * Description: This is a record representing an EntryDto. 
 * 
 * I went with record because I'm only going to store the data and init-once, and should not be modifying entries.
 */

namespace LeaderboardAPI.Dtos
{
    public record EntryDto
    {
        public string username { get; init; }
        public int score { get; init; }
        public int index { get; init; }
    }
}