﻿using HR.LeaveManagement.Application.Contracts.Persistence;
using Moq;

namespace HR.LeaveManagement.Application.UnitTests.Mocks
{
    public static class MockUnitOfWork
    {
        public static Mock<IUnitOfWork> GetUnitOfWork()
        {
            var mockUow = new Mock<IUnitOfWork>();
            var mockLeaveRepository = MockLeaveTypeRepository.GetLeaveTypeRepository();

            mockUow.Setup(x => x.LeaveTypeRepository).Returns(mockLeaveRepository.Object);

            return mockUow;
        }
    }
}
