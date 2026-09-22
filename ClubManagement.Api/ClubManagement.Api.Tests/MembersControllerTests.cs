using ClubManagement.Api.Controllers;
using ClubManagement.Api.Models;
using ClubManagement.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace ClubManagement.Api.Tests
{
    public class MembersControllerTests
    {
        private Mock<IMemberRepository> _mockRepo;
        private MembersController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IMemberRepository>();
            _controller = new MembersController(_mockRepo.Object);
        }

        [Test]
        public async Task GetActiveMembers_ReturnsOk_WithData()
        {
            // Arrange
            var fakeMembers = new List<ActiveMemberDto>
            {
                new ActiveMemberDto { MemberId = 1, FirstName = "John", LastName = "Smith" },
                new ActiveMemberDto { MemberId = 2, FirstName = "Sarah", LastName = "Johnson" }
            };

            _mockRepo
                .Setup(r => r.GetActiveMembersAsync("LastName", null, null))
                .ReturnsAsync(fakeMembers);

            // Act
            var result = await _controller.GetActiveMembers("LastName", null, null);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.AreEqual(2, (okResult.Value as IEnumerable<ActiveMemberDto>).Count());
        }

        [Test]
        public async Task GetActiveMembers_ReturnsEmptyList_WhenNoMembers()
        {
            _mockRepo
                .Setup(r => r.GetActiveMembersAsync("LastName", null, null))
                .ReturnsAsync(new List<ActiveMemberDto>());

            var result = await _controller.GetActiveMembers("LastName", null, null);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var members = okResult.Value as IEnumerable<ActiveMemberDto>;
            Assert.AreEqual(0, members.Count());
        }


        [Test]
        public async Task GetActiveMembers_PassesPaginationParameters()
        {
            _mockRepo
                .Setup(r => r.GetActiveMembersAsync("LastName", 2, 20))
                .ReturnsAsync(new List<ActiveMemberDto>());

            await _controller.GetActiveMembers("LastName", 2, 20);

            _mockRepo.Verify(r => r.GetActiveMembersAsync("LastName", 2, 20), Times.Once);
        }

        [Test]
        public async Task GetActiveMembers_InvalidSortColumn_ReturnsBadRequest()
        {
            var result = await _controller.GetActiveMembers("BadColumn", null, null);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetActiveMembers_NegativePageNumber_ReturnsBadRequest()
        {
            var result = await _controller.GetActiveMembers("LastName", -1, 20);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetActiveMembers_PageNumberWithoutPageSize_ReturnsBadRequest()
        {
            var result = await _controller.GetActiveMembers("LastName", 2, null);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

    }
}
