using System.Collections.Generic;
using System.Linq;

namespace LeaderboardAPI.Dtos
{
    public class LeaderboardDto
    {
        public IEnumerable<EntryDto> entries { get; set; }
        public int page { get; set; }
        public int totalPages { get; set; }
        public int pageSize { get; set; }
        public int subsetCount { get; set; }

        public LeaderboardDto(IEnumerable<EntryDto> entries, int page, int pageSize, int totalPages)
        {
            this.entries = entries;
            this.page = page;
            this.pageSize = pageSize;
            this.totalPages = totalPages;
            this.subsetCount = entries.Count();
        }
    }
}
