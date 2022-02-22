using LeaderboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaderboardAPI.Dtos
{
    public class LeaderboardDto
    {
        public List<Entry> entries { get; set; }
        public int page { get; set; }
        public int totalPages { get; set; }
        public int pageSize { get; set; }

        public LeaderboardDto(List<Entry> entries, int page, int pageSize, int totalPages)
        {
            this.entries = entries;
            this.page = page;
            this.pageSize = pageSize;
            this.totalPages = totalPages;
        }
    }
}
