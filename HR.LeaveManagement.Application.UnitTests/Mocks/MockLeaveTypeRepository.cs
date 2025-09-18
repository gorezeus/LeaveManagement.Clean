using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using Moq;

namespace HR.LeaveManagement.Application.UnitTests.Mocks
{
    public class MockLeaveTypeRepository
    {
        public static Mock<ILeaveTypeRepository> GetMockLeaveTypeRepository()
        {
            var leaveTypes = new List<LeaveType>
            {
                new LeaveType
                {
                    Id = 1,
                    DefaultDays = 10,
                    Name = "Test Vacation"
                },
                new LeaveType
                {
                    Id = 2,
                    DefaultDays = 15,
                    Name = "Test Sick"
                },
                new LeaveType
                {
                    Id = 3,
                    DefaultDays = 15,
                    Name = "Test Maternity"
                }
            };

            var mockRepo = new Mock<ILeaveTypeRepository>();

            mockRepo.Setup(r => r.GetAsync()).ReturnsAsync(leaveTypes);

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))!
                .ReturnsAsync((int id) =>
                {
                    var leaveType = leaveTypes.Find(p => p.Id == id);
                    return leaveType;
                });

            mockRepo.Setup(r => r.CreateAsync(It.IsAny<LeaveType>()))
                .Returns((LeaveType leaveType) =>
                {
                    leaveType.Id = leaveTypes.Max(m => m.Id) + 1;
                    leaveTypes.Add(leaveType);
                    return Task.CompletedTask;
                });

            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<LeaveType>()))
                .Returns((LeaveType leaveType) =>
                {
                    var leaveTypeToUpdate = leaveTypes.Find(p => p.Id == leaveType.Id);
                    if (leaveTypeToUpdate != null) leaveTypes.Remove(leaveTypeToUpdate);
                    leaveTypes.Add(leaveType);
                    return Task.CompletedTask;
                });

            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<LeaveType>()))
                .Returns((LeaveType leaveType) =>
                {
                    var leaveTypeToRemove = leaveTypes.Find(p => p.Id == leaveType.Id);
                    if (leaveTypeToRemove != null) leaveTypes.Remove(leaveTypeToRemove);
                    return Task.CompletedTask;
                });

            mockRepo.Setup(r => r.IsLeaveTypeUnique(It.IsAny<string>()))
                .ReturnsAsync((string name) =>
                {
                    return leaveTypes.Any(p => p.Name == name);
                });

            return mockRepo;
        }
    }
}
