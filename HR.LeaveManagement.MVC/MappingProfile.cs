using AutoMapper;
using HR.LeaveManagement.MVC.Models.Identity;
using HR.LeaveManagement.MVC.Models.LeaveRequests;
using HR.LeaveManagement.MVC.Models.LeaveTypes;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateLeaveTypeDto, CreateLeaveTypeViewModel>().ReverseMap();
            CreateMap<LeaveTypeDto, LeaveTypeViewModel>().ReverseMap();
            CreateMap<RegisterViewModel, RegistrationRequest>().ReverseMap();

            CreateMap<CreateLeaveRequestDto, CreateLeaveRequestViewModel>().ReverseMap();
            CreateMap<LeaveRequestDto, LeaveRequestViewModel>()
                .ForMember(q => q.DateRequested, opt => opt.MapFrom(x => x.DateRequested.DateTime))
                .ForMember(q => q.StartDate, opt => opt.MapFrom(x => x.StartDate.DateTime))
                .ForMember(q => q.EndDate, opt => opt.MapFrom(x => x.EndDate.DateTime))
                .ReverseMap();
        }
    }
}
