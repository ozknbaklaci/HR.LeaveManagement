using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HR.LeaveManagement.Application.Constants;
using HR.LeaveManagement.Application.Contracts.Infrastructure;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Models;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateLeaveRequestCommandHandler(IMapper mapper,
            IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<BaseCommandResponse> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateLeaveRequestDtoValidator(_unitOfWork.LeaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request.LeaveRequestDto, cancellationToken);
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.Uid)?.Value;

            var allocation =
                await _unitOfWork.LeaveAllocationRepository.GetUserAllocations(userId, request.LeaveRequestDto.LeaveTypeId);

            if (allocation is null)
            {
                validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.LeaveRequestDto.LeaveTypeId),
                    "You do not have any allocations for this leave type."));
            }
            else
            {
                int daysRequested = (int)(request.LeaveRequestDto.EndDate - request.LeaveRequestDto.StartDate).TotalDays;
                if (daysRequested > allocation.NumberOfDays)
                {
                    validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(
                        nameof(request.LeaveRequestDto.EndDate), "You do not have enough days for this request"));
                }
            }


            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Creation Failed";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            }
            else
            {

                var leaveRequests = _mapper.Map<LeaveRequest>(request.LeaveRequestDto);
                leaveRequests.RequestingEmployeeId = userId;
                leaveRequests = await _unitOfWork.LeaveRequestRepository.Add(leaveRequests);
                await _unitOfWork.Save();

                response.Success = true;
                response.Message = "Creation Successful";
                response.Id = leaveRequests.Id;

                try
                {
                    var emailAddress = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

                    var email = new Email
                    {
                        To = emailAddress,
                        Body = $"Your leave request for {request.LeaveRequestDto.StartDate:D} to {request.LeaveRequestDto.EndDate:D} has been submitted successfully.",
                        Subject = "Leave Request Submitted"
                    };

                    await _emailSender.SendEmail(email);
                }
                catch (Exception e)
                {
                    //Log or handle error, but don't throw...
                }
            }


            return response;
        }
    }
}
