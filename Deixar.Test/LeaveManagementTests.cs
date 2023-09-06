using Deixar.API.Controllers.V2;
using Deixar.Data.Contexts;
using Deixar.Domain.DTOs;
using Deixar.Domain.Entities;
using Deixar.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Principal;

namespace Deixar.Test
{
    public class LeaveManagementTests
    {
        public readonly Mock<ILeaveRepository> _leaveRepository = new();
        public readonly Mock<IEmailUtility> _emailUtility = new();
        public readonly Mock<ApplicationDBContext> _applicationContext = new();

        [Fact]
        public void AddLeaveRequest_Success()
        {
            // Arrange
           
            var leaveController = new LeaveManagementController(_leaveRepository.Object,_applicationContext.Object,_emailUtility.Object);

            // Act
            var leave = new LeaveRequestModel()
            {
                Reason = "Sick leave",
                IsPaidLeave = true,
                LeaveDate = DateTime.Now.AddDays(1)
            };
            var result = leaveController.AddLeaveRequestAsync(leave);
            var objectResult = result.Result as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            _leaveRepository.Verify(x => x.CreateLeaveRequestAsync(It.IsAny<Leave>()), Times.Once);
        }
    }
}
