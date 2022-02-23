using FakeItEasy;
using LeaderboardAPI.Controllers;
using LeaderboardAPI.Dtos;
using LeaderboardAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace LeaderboardAPI.Tests
{
    public class LeaderboardControllerTests
    {
        [Fact]
        public async Task GetLeaderboard_Returns_Correct_Number_Of_Entries()
        {
            var fakeLeaderboard = A.Fake<LeaderboardDto>();
            int count = fakeLeaderboard.pageSize;
            var service = A.Fake<ILeaderboardService>();

            A.CallTo(() => service.GetEntriesHelperAsync(null, null, count)).Returns(Task.FromResult(new ActionResult<LeaderboardDto>(fakeLeaderboard)));
            var controller = new LeaderboardController(service, count.ToString());

            var actionResult = await controller.GetLeaderboardAsync(null, null);
            Assert.Equal(count, actionResult.Value.pageSize);
        }
    }
}
