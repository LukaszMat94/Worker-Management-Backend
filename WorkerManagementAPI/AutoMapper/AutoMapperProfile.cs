using AutoMapper;
using WorkerManagementAPI.Data.Models.ProjectDtos;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Models.CompanyDtos;
using WorkerManagementAPI.Data.Models.TechnologyDtos;
using WorkerManagementAPI.Data.Models.RoleDtos;
using WorkerManagementAPI.Data.Models.UserDtos;

namespace WorkerManagementAPI.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Company

            CreateMap<Company, CompanyDto>();
            CreateMap<Company, ReturnCompanyDto>();
            CreateMap<CreateCompanyDto, Company>();
            CreateMap<UpdateCompanyDto, Company>();

            #endregion

            #region Worker

            CreateMap<User, UserDto>();
            CreateMap<LoginUserDto, User>();
            CreateMap<RegisterUserDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<User, UpdateUserTechnologyDto>()
                .ForMember(dest => dest.TechnologiesDto,
                    opt => opt.MapFrom(src => src.Technologies));

            #endregion

            #region Project

            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();
            CreateMap<CreateProjectDto, Project>();
            CreateMap<Project, UpdateProjectTechnologyDto>()
                .ForMember(dest => dest.TechnologiesDto,
                    opt => opt.MapFrom(src => src.Technologies)); ;
            CreateMap<Project, UpdateProjectUserDto>()
                .ForMember(dest => dest.UsersDto,
                    opt => opt.MapFrom(src => src.Users)); ;

            #endregion

            #region Technology

            CreateMap<Technology, TechnologyDto>();
            CreateMap<TechnologyDto, Technology>();
            CreateMap<CreateTechnologyDto, Technology>();

            #endregion

            #region

            CreateMap<Role, RoleDto>();

            #endregion
        }
    }
}
