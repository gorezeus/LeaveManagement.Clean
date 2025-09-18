using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType;
using HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
using HR.LeaveManagement.Application.MappingProfiles;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.UnitTests.Features.LeaveTypes.Commands
{
    public class DeleteLeaveTypeCommandHandlerTests
    {
        private readonly Mock<ILeaveTypeRepository> _mockRepo;
        private readonly IMapper _mapper;

        public DeleteLeaveTypeCommandHandlerTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetMockLeaveTypeRepository();

            var mapperConfig =
                new MapperConfiguration(c => { c.AddProfile<LeaveTypeProfile>(); }, new NullLoggerFactory());
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task UpdateLeaveTypeTest()
        {
            // Arrange
            var handler = new DeleteLeaveTypeCommandHandler(_mockRepo.Object);

            // Act
            var result = await handler.Handle(new DeleteLeaveTypeCommand { Id = 3 }, CancellationToken.None);
            var leaveTypes = await _mockRepo.Object.GetAsync();

            // Assert
            leaveTypes.Count.ShouldBe(2);
        }
    }
}
