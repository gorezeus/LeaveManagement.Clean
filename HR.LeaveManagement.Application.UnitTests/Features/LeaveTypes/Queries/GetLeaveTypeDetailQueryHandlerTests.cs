using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
using HR.LeaveManagement.Application.MappingProfiles;

namespace HR.LeaveManagement.Application.UnitTests.Features.LeaveTypes.Queries
{
    public class GetLeaveTypeDetailQueryHandlerTests
    {
        private readonly Mock<ILeaveTypeRepository> _mockRepo;
        private readonly IMapper _mapper;

        public GetLeaveTypeDetailQueryHandlerTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetMockLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveTypeProfile>();
            }, new NullLoggerFactory());
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task GetLeaveTypeByIdTest()
        {
            // Arrange
            var handler = new GetLeaveTypeDetailsQueryHandler(_mapper, _mockRepo.Object);

            // Act
            var leaveType = await handler.Handle(
                new GetLeaveTypeDetailsQuery(1),
                CancellationToken.None);

            // Assert
            leaveType.ShouldBeOfType<LeaveTypeDetailsDto>();
            leaveType.Id.ShouldBe(1);
            leaveType.Name.ShouldBe("Test Vacation");
        }
    }
}
