using AutoMapper;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.DTOs.AdminDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.CitizenDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.DepartmentDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.OfficerDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.RequestDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.UserDTOs;

namespace PublicSpaceMaintenanceRequestMS.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            // Mapping User to various DTOs
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, OfficerDTO>().ReverseMap();
            CreateMap<User, AdminDTO>().ReverseMap();
            CreateMap<User, CitizenDTO>().ReverseMap();
            CreateMap<User, UserReadOnlyDTO>();
            CreateMap<User, UserFiltersDTO>().ReverseMap();

            // Mapping UserSignupDTO to User
            CreateMap<UserSignupDTO, User>();

            // Mapping UserSignupDTO to specific user roles
            CreateMap<UserSignupDTO, Citizen>();
            CreateMap<UserSignupDTO, Officer>()
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => new Department { Id = src.DepartmentId }));
            CreateMap<UserSignupDTO, Admin>();

            // Mapping UserUpdateDTO to User
            CreateMap<UserUpdateDTO, User>();

            // Mapping UserPatchDTO to User
            CreateMap<UserPatchDTO, User>();

            // Mapping Request to various DTOs
            CreateMap<Request, RequestDTO>()
                .ForMember(dest => dest.CitizenUsername, opt => opt.MapFrom(src => src.Citizen!.User!.Username))
                .ForMember(dest => dest.AssignedDepartmentTitle, opt => opt.MapFrom(src => src.AssignedDepartment!.Title))
                .ReverseMap();
            CreateMap<Request, RequestUpdateDTO>().ReverseMap();
            CreateMap<Request, RequestReadOnlyDTO>();
            CreateMap<RequestSubmitDTO, Request>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())  
                .ForMember(dest => dest.Citizen, opt => opt.Ignore()) 
                .ForMember(dest => dest.AssignedDepartment, opt => opt.Ignore()); 

            // Mapping RequestSubmitDTO to specific request status
            CreateMap<RequestSubmitDTO, Pending>();
            CreateMap<RequestSubmitDTO, InProgress>();
            CreateMap<RequestSubmitDTO, Complete>();

            // Mapping Department to various DTOs
            CreateMap<Department, DepartmentDTO>().ReverseMap();
            CreateMap<Department, DepartmentCreateDTO>().ReverseMap();
            CreateMap<Department, DepartmentReadOnlyDTO>();

            // Mapping Officer to various DTOs
            CreateMap<Officer, OfficerDTO>().ReverseMap();
            CreateMap<Officer, OfficerReadOnlyDTO>();

            // Mapping OfficerAssignDTO to Officer
            CreateMap<OfficerAssignDTO, Officer>()
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId));
        }
    }
}
