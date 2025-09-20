using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.MappingProfiles;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;

namespace HR.LeaveManagement.Application.UnitTests.Features.LeaveTypes.Commands
{
    public class CreateLeaveTypeCommandHandlerTests
    {
        private readonly Mock<ILeaveTypeRepository> _mockRepo;
        private readonly IMapper _mapper;

        public CreateLeaveTypeCommandHandlerTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetMockLeaveTypeRepository();

            var mapperConfig =
                new MapperConfiguration(c => { c.AddProfile<LeaveTypeProfile>(); }, new NullLoggerFactory());
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task CreateValidLeaveTypeTest()
        {
            // Arrange
            var handler = new CreateLeaveTypeCommandHandler(_mapper, _mockRepo.Object);

            var createLeaveTypeCommand = new CreateLeaveTypeCommand
            {
                DefaultDays = 20,
                Name = "Test"
            };

            // Act
            var result = await handler.Handle(createLeaveTypeCommand, CancellationToken.None);
            var listOfLeaveType = await _mockRepo.Object.GetAsync();
            
            // Assert
            result.ShouldBe(4);
            listOfLeaveType.Count.ShouldBe(4);
        }

        [Fact]
        public async Task CreateInvalidLeaveTypeTest()
        {
            // Arrange
            var handler = new CreateLeaveTypeCommandHandler(_mapper, _mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(async () => 
                await handler.Handle(new CreateLeaveTypeCommand
                    {
                        Name = "Test Vacation",
                        DefaultDays = 10
                    }, CancellationToken.None));
        }
    }
}
    
