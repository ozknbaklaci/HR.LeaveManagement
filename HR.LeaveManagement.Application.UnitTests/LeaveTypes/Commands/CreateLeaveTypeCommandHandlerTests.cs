using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Profiles;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;
using Xunit;

namespace HR.LeaveManagement.Application.UnitTests.LeaveTypes.Commands
{
    public class CreateLeaveTypeCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveTypeRepository> _mockRepository;
        private readonly CreateLeaveTypeDto _createLeaveTypeDto;
        private readonly CreateLeaveTypeCommandHandler _handler;

        public CreateLeaveTypeCommandHandlerTests()
        {
            _mockRepository = MockLeaveTypeRepository.GetLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _handler = new CreateLeaveTypeCommandHandler(_mockRepository.Object, _mapper);
            _createLeaveTypeDto = new CreateLeaveTypeDto
            {
                DefaultDays = 15,
                Name = "Test DTO"
            };
        }

        [Fact]
        public async Task Valid_LeaveType_Added()
        {
            var result = await _handler.Handle(new CreateLeaveTypeCommand { LeaveTypeDto = _createLeaveTypeDto }, CancellationToken.None);

            var leaveTypes = await _mockRepository.Object.GetAll();

            result.ShouldBeOfType<BaseCommandResponse>();
            leaveTypes.Count.ShouldBe(4);
        }

        [Fact]
        public async Task InValid_LeaveType_Added()
        {
            _createLeaveTypeDto.DefaultDays = -1;

            var result = await _handler.Handle(new CreateLeaveTypeCommand { LeaveTypeDto = _createLeaveTypeDto }, CancellationToken.None);
            var leaveTypes = await _mockRepository.Object.GetAll();

            leaveTypes.Count.ShouldBe(3);

            result.ShouldBeOfType<BaseCommandResponse>();
        }
    }
}
