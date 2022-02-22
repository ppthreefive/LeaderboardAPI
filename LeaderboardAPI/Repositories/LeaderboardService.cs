using LeaderboardAPI.Dtos;
using LeaderboardAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LeaderboardAPI.Repositories
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly List<Entry> entries = new();

        public LeaderboardService()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\leaderboard_data.json");
            string json = System.IO.File.ReadAllText(file);
            this.entries = JsonSerializer.Deserialize<Leaderboard>(json).entries;
        }

        public async Task<ActionResult<LeaderboardDto>> GetEntriesHelper(int? page, int? count, int defaultPageSize) 
        {
            int total = this.entries.Count();
            int remainder, totalPages, start;

            // Default case, if user does not input a page or count
            if (page == null && count == null)
            {
                totalPages = Math.DivRem(total, defaultPageSize, out remainder);
                if (remainder != 0) { totalPages++; }

                return getEntriesPaginated(0, defaultPageSize, 1, defaultPageSize, totalPages);
            } // If user does not input page, but enters a count
            else if (page == null && count != null)
            {
                int pageSize = (int)count;

                if (pageSize <= 0)
                {
                    return null;
                }

                totalPages = Math.DivRem(total, pageSize, out remainder);
                if (remainder != 0) { totalPages++; }

                return getEntriesPaginated(0, pageSize, 1, pageSize, totalPages);
            } // If user enters a page but not a count
            else if (count == null && page != null) 
            {
                int pageNum = (int)(page - 1);
                totalPages = Math.DivRem(total, defaultPageSize, out remainder);
                if (remainder != 0) { totalPages++; }

                if (pageNum < 0)
                {
                    return null;
                }

                start = defaultPageSize * pageNum;

                if ((pageNum + 1) == totalPages && remainder > 0)
                {
                    start = pageNum * defaultPageSize;
                    return getEntriesPaginated(start, remainder, pageNum + 1, defaultPageSize, totalPages);
                }

                start = pageNum * defaultPageSize;
                return getEntriesPaginated(start, defaultPageSize, pageNum + 1, defaultPageSize, totalPages);
            }
            else // User has given a page number and a count
            {
                int pageSize = (int)count;
                int pageNum = (int)(page -1);

                if (pageSize <= 0 || pageNum < 0)
                {
                    return null;
                }

                start = pageSize * pageNum;
                totalPages = Math.DivRem(total, pageSize, out remainder);
                if (remainder != 0) { totalPages++; }

                if ((pageNum + 1) == totalPages && remainder > 0)
                {
                    start = pageNum * pageSize;
                    return getEntriesPaginated(start, remainder, pageNum + 1, pageSize, totalPages);
                }

                start = pageNum * pageSize;
                return getEntriesPaginated(start, pageSize, pageNum + 1, pageSize, totalPages);
            }
        }

        public LeaderboardDto getEntriesPaginated(int start, int count, int pageNum, int pageSize, int totalPages)
        {
            if ((start + count) > this.entries.Count() || start < 0 || start > this.entries.Count())
            {
                return null;
            }

            if (this.entries.Count() <= count)
            {
                var allEntries = this.entries.Select(entry => new EntryDto
                { 
                    index = entry.index,
                    username = entry.username,
                    score = entry.score
                });

                return new LeaderboardDto(allEntries, pageNum, pageSize, totalPages);
            }

            var subEntries = this.entries.GetRange(start, count).Select(entry => new EntryDto 
            {
                index = entry.index,
                username = entry.username,
                score = entry.score
            });

            return new LeaderboardDto(subEntries, pageNum, pageSize, totalPages);
        }
    }
}
