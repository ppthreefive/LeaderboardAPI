/* Author: Phillip Pham
 * Date: 02/20/22
 * Description: This is the service/repository that implements the Interface ILeaderboardService.cs. This service
 *  loads the static Leaderboard data from 'leaderboard_data.json' in the Resources folder into an in-memory-repository.
 *  
 *  For the challenge, it was mentioned to do it this way to focus on implementing a class that returns paginated Leaderboard data.
 *  The challenge mentioned designing this with Redis in mind, which is used frequently as an in-memory database.
 */

using LeaderboardAPI.Dtos;
using LeaderboardAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LeaderboardAPI.Repositories
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly Leaderboard leaderboard = new();

        public LeaderboardService()
        {
            /*  This is where I am loading the static data from the json file using deserialization.
            *   If this was a class that was completely designed to be used with Redis integration, 
            *   I would be sending a 'ZREVRANGEBYSCORE leaderboardKey start count' command instead, 
            *   and not have to load the data in the API server's memory.
            *   
            *   Because for the challenge I must assume that Redis can give me the sorted set based on score,
            *   I have presorted the data from highest to lowest score. 
            */
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\leaderboard_data.json");
            string json = System.IO.File.ReadAllText(file);
            this.leaderboard = JsonSerializer.Deserialize<Leaderboard>(json);
        }

        /// <summary>
        /// This is an asynchronous helper function that is designed to logically determine which are the proper parameters 
        ///     (start index, count, page number, page size, and total amount of pages) to send to the GetEntriesPaginatedAsync()
        ///     method, which will package our data into our DTO objects and return to the controller.
        ///     
        ///     There are a few rules that I have determined is proper for a paginated leaderboard:
        ///     1.) If the page and count parameters are null, then return a Leaderboard with a list of the top entries of default page size.
        ///     2.) If only the count parameter is not null, then return the first page of the Leaderboard with a list of the top entries
        ///         of the parameter count as size.
        ///     3.) If only the page parameter is not null, then return a Leaderboard with a list of the top entries of default page size IFF
        ///         the page is within range of 1...N where N is the total number of pages determined by the default page size.
        ///     4.) If both page and count are non-null and valid, return a paginated Leaderboard with the correct page and entries per page that was
        ///         requested.
        /// </summary>
        /// <param name="page">Nullable int representing which page (from a list split into pages by a size count) that the caller wants.</param>
        /// <param name="count">Nullable int representing how many entries per page the caller wants to have.</param>
        /// <param name="defaultPageSize">Int that must be given from the caller and is the default count per page.</param>
        /// <returns>A paginated LeaderboardDto if inputs are valid, null if invalid.</returns>
        public async Task<ActionResult<LeaderboardDto>> GetEntriesHelperAsync(int? page, int? count, int defaultPageSize) 
        {
            int total = this.leaderboard.entries.Count();
            int remainder, totalPages, start;

            // We cannot have a default page size of zero or less. Automatically return null if this happens somehow to avoid dividing by zero.
            if (defaultPageSize <= 0) 
            {
                return null;
            }

            // Default case, if user does not input a page or count
            if (page == null && count == null)
            {
                totalPages = Math.DivRem(total, defaultPageSize, out remainder);
                if (remainder != 0) { totalPages++; }

                // Return the paginated result with the first page with the count of the default page size
                return await GetEntriesPaginatedAsync(0, defaultPageSize, 1, defaultPageSize, totalPages);
            } // If user does not input page, but enters a count
            else if (page == null && count != null)
            {
                // Typecast the count into an int, return null if its less than or equal to zero
                int pageSize = (int)count;
                if (pageSize <= 0) 
                { 
                    return null;
                }
                
                // Get the total number of pages
                totalPages = Math.DivRem(total, pageSize, out remainder);
                if (remainder != 0) { totalPages++; }

                // If the pageSize is larger than the count of all of our entries, return them all
                if (pageSize > this.leaderboard.entries.Count) 
                {
                    return await GetAllEntriesAsync(pageSize);
                }

                // Return the first page with the determined count
                return await GetEntriesPaginatedAsync(0, pageSize, 1, pageSize, totalPages);
            } // If user enters a page but not a count
            else if (count == null && page != null) 
            {
                // Determine the input page number and total amount of pages
                int pageNum = (int)(page - 1);
                totalPages = Math.DivRem(total, defaultPageSize, out remainder);
                if (remainder != 0) { totalPages++; }

                // The page input MUST be within 1...N where N is the total amount of pages possible, return null if false
                if (pageNum < 0 || pageNum > totalPages)
                {
                    return null;
                }

                // This will determine the starting index of the initial range of entries we want
                start = defaultPageSize * pageNum;

                // If the page is the last page and we have a remainder, we send the remainder as the count
                if ((pageNum + 1) == totalPages && remainder > 0)
                {
                    start = pageNum * defaultPageSize;
                    return await GetEntriesPaginatedAsync(start, remainder, pageNum + 1, defaultPageSize, totalPages);
                }

                // Get the paginated result with the default page size
                start = pageNum * defaultPageSize;
                return await GetEntriesPaginatedAsync(start, defaultPageSize, pageNum + 1, defaultPageSize, totalPages);
            }
            else // User has given a page number and a count
            {
                int pageSize = (int)count;
                int pageNum = (int)(page -1);

                // Page size cannot be less than or equal to zero
                if (pageSize <= 0) 
                {
                    return null;
                }

                totalPages = Math.DivRem(total, pageSize, out remainder);
                if (remainder != 0) { totalPages++; }

                // Page number must land within 1...N where N is the total number of pages
                if (pageNum < 0 || pageNum >= totalPages)
                {
                    return null;
                }

                // Determine the starting index
                start = pageSize * pageNum;

                // If we have a remainder and are on the last page, return the leaderboard with the remainder entries
                if ((pageNum + 1) == totalPages && remainder > 0)
                {
                    start = pageNum * pageSize;
                    return await GetEntriesPaginatedAsync(start, remainder, pageNum + 1, pageSize, totalPages);
                }

                // Get the paginated result with the input page and count
                start = pageNum * pageSize;
                return await GetEntriesPaginatedAsync(start, pageSize, pageNum + 1, pageSize, totalPages);
            }
        }

        /// <summary>
        /// This method allows the caller to get a LeaderboardDto with the specified parameters.
        /// Equivalent to doing 'ZREVRANGEBYSCORE leaderboardKey start count' with Redis.
        /// </summary>
        /// <param name="start">The starting index.</param>
        /// <param name="count">The requested count of entries.</param>
        /// <param name="pageNum">The page number requested.</param>
        /// <param name="pageSize">Max amount of entries per page.</param>
        /// <param name="totalPages">Total amount of pages based on page size.</param>
        /// <returns>Returns a LeaderboardDto with requested data or null depending on validity of input.</returns>
        public async Task<LeaderboardDto> GetEntriesPaginatedAsync(int start, int count, int pageNum, int pageSize, int totalPages)
        {
            LeaderboardDto leaderboardDto = null;

            // If out of bounds, return null
            if ((start + count) > this.leaderboard.entries.Count || start < 0 || start > this.leaderboard.entries.Count)
            {
                return null;
            }

            // If the specified count is larger than the amount of total entries, package all of the entries and return
            if (this.leaderboard.entries.Count <= count)
            {
                await Task.Run(() => {
                    var allEntries = this.leaderboard.entries.Select(entry => new EntryDto
                    {
                        index = entry.index,
                        username = entry.username,
                        score = entry.score
                    });

                    leaderboardDto = new LeaderboardDto
                    {
                        entries = allEntries,
                        pageSize = pageSize,
                        page = pageNum,
                        subsetCount = allEntries.Count(),
                        totalPages = totalPages
                    };
                });

                return leaderboardDto;
            }

            // Package the specified range of entries and return
            await Task.Run(() => 
            {
                var subEntries = this.leaderboard.entries.GetRange(start, count).Select(entry => new EntryDto
                {
                    index = entry.index,
                    username = entry.username,
                    score = entry.score
                });

                leaderboardDto = new LeaderboardDto
                {
                    entries = subEntries,
                    page = pageNum,
                    pageSize = pageSize,
                    totalPages = totalPages,
                    subsetCount = subEntries.Count()
                };
            });

            return leaderboardDto;
        }

        /// <summary>
        /// A private method that allows a caller within the LeaderboardService class to package all leaderboard entries into the DTO.
        /// </summary>
        /// <param name="pageSize">The requested page size.</param>
        /// <returns>a LeaderboardDto with all of the existing entries.</returns>
        private async Task<LeaderboardDto> GetAllEntriesAsync(int pageSize) 
        {
            LeaderboardDto leaderboardDto = null;

            await Task.Run(() =>
            {
                var allEntries = this.leaderboard.entries.Select(entry => new EntryDto
                {
                    index = entry.index,
                    username = entry.username,
                    score = entry.score
                });

                leaderboardDto = new LeaderboardDto
                {
                    entries = allEntries,
                    page = 1,
                    pageSize = pageSize,
                    totalPages = 1,
                    subsetCount = this.leaderboard.entries.Count()
                };
            });

            return leaderboardDto;
        }
    }
}
