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
        public async Task GetLeaderboard_Returns_Correct_Page_Size()
        {
            var fakeLeaderboard = A.Fake<LeaderboardDto>();
            int count = fakeLeaderboard.pageSize;
            var service = A.Fake<ILeaderboardService>();

            A.CallTo(() => service.GetEntriesHelperAsync(null, null, count)).Returns(Task.FromResult(new ActionResult<LeaderboardDto>(fakeLeaderboard)));
            var controller = new LeaderboardController(service, count.ToString());

            var actionResult = await controller.GetLeaderboardAsync(null, null);
            Assert.Equal(count, actionResult.Value.pageSize);
        }

        [Fact]
        public async Task GetLeaderboard_Returns_NotFound()
        {
            var service = A.Fake<ILeaderboardService>();

            A.CallTo(() => service.GetEntriesHelperAsync(-1, -1, 50)).Returns(Task.FromResult<ActionResult<LeaderboardDto>>(null));
            var controller = new LeaderboardController(service, "50");

            var actionResult = await controller.GetLeaderboardAsync(-1, -1);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
    }
}
