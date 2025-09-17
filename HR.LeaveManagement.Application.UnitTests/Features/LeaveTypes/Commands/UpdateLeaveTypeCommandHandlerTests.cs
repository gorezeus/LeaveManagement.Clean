using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;
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
    public class UpdateLeaveTypeCommandHandlerTests
    {
        private readonly Mock<ILeaveTypeRepository> _mockRepo;
        private readonly IMapper _mapper;
        private readonly Mock<IAppLogger<UpdateLeaveTypeCommandHandler>>
            _mockUpdateAppLogger;

        public UpdateLeaveTypeCommandHandlerTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetMockLeaveTypeRepository();

            var mapperConfig =
                new MapperConfiguration(c => { c.AddProfile<LeaveTypeProfile>(); }, new NullLoggerFactory());
            _mapper = mapperConfig.CreateMapper();
            _mockUpdateAppLogger = new Mock<IAppLogger<UpdateLeaveTypeCommandHandler>>();
        }

        [Fact]
        public async Task UpdateLeaveTypeTest()
        {
            // Arrange
            var handler = new UpdateLeaveTypeCommandHandler(_mapper, _mockRepo.Object, _mockUpdateAppLogger.Object);
            var handlerQuery = new GetLeaveTypeDetailsQueryHandler(_mapper, _mockRepo.Object);

            var updateLeaveTypeCommand = new UpdateLeaveTypeCommand
            {
                Id = 1,
                DefaultDays = 20,
                Name = "Ganti Nama"
            };

            // Act
            var result = await handler.Handle(updateLeaveTypeCommand, CancellationToken.None);
            var leaveType = await handlerQuery.Handle(new GetLeaveTypeDetailsQuery(1), CancellationToken.None);


            // Assert
            leaveType.Id.ShouldBe(1);
            leaveType.Name.ShouldBe("Ganti Nama");
            
        }
    }
}
