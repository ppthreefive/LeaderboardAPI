/* Author: Phillip Pham
 * Date: 02/20/22
 * Description: This is a record representing a LeaderboardDto. 
 * 
 * I went with record because I'm only going to store the data and init-once. This also allows us to package additional information
 * according to what the data contract I created that the normal Leaderboard record does not have.
 */

using System.Collections.Generic;

namespace LeaderboardAPI.Dtos
{
    public record LeaderboardDto
    {
        public IEnumerable<EntryDto> entries { get; init; }
        public int page { get; init; }
        public int totalPages { get; init; }
        public int pageSize { get; init; }
        public int subsetCount { get; init; }
    }
}
