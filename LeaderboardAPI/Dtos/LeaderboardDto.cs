using System.Collections.Generic;
using System.Linq;

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
