using AutoMapper;
using EmployeeLeaveAPI.DTOs;
using EmployeeLeaveAPI.Models;

namespace EmployeeLeaveAPI.Mapping;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<CreateUserDTO, User>();
        CreateMap<CreateUserLeaveBalanceDTO, UserLeaveBalance>();
        CreateMap<CreateLeaveTypeDTO, LeaveType>();
    }
}