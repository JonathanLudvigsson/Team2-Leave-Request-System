using AutoMapper;
using EmployeeLeaveAPI.DTOs;
using EmployeeLeaveAPI.Models;

namespace EmployeeLeaveAPI.Mapping;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<CreateUserDTO, User>();
        CreateMap<CreateRequestDTO, Request>();
        CreateMap<CreateLeaveTypeDTO, LeaveType>();
        CreateMap<LeaveTypeDaysUsedDTO, LeaveType>().ReverseMap();
    }
}