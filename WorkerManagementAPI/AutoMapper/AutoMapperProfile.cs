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
            CreateMap<RegisterUserDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<User, UpdateUserTechnologyDto>()
                .ForMember(updateUser => updateUser.TechnologyLevelDto, 
                action => action.MapFrom(user => user.Technologies.First()));

            #endregion

            #region Project

            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();
            CreateMap<CreateProjectDto, Project>();
            CreateMap<Project, UpdateProjectTechnologyDto>()
                .ForMember(updateProject => updateProject.Technology,
                action => action.MapFrom(project => project.Technologies.First()));
            CreateMap<Project, UpdateProjectUserDto>()
                .ForMember(updateProject => updateProject.UserDto,
                action => action.MapFrom(project => project.Users.First()));

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
