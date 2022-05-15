using AutoMapper;
using WorkerManagementAPI.Data.Models.ProjectDtos;
using WorkerManagementAPI.Data.Models.WorkerDtos;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Models.CompanyDtos;
using WorkerManagementAPI.Data.Models.TechnologyDtos;

namespace WorkerManagementAPI.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Company

            CreateMap<Company, CompanyDto>();
            CreateMap<CreateCompanyDto, Company>();
            CreateMap<UpdateCompanyDto, Company>();

            #endregion

            #region Worker

            CreateMap<Worker, WorkerDto>();
            CreateMap<CreateWorkerDto, Worker>();
            CreateMap<UpdateWorkerDto, Worker>();
            CreateMap<Worker, UpdateWorkerTechnologyDto>()
                .ForMember(updateWorker => updateWorker.TechnologyLevelDto, 
                action => action.MapFrom(worker => worker.Technologies.First()));

            #endregion

            #region Project

            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();
            CreateMap<CreateProjectDto, Project>();
            CreateMap<Project, UpdateProjectTechnologyDto>()
                .ForMember(updateProject => updateProject.Technology,
                action => action.MapFrom(project => project.Technologies.First()));
            CreateMap<Project, UpdateProjectWorkerDto>()
                .ForMember(updateProject => updateProject.WorkerDto,
                action => action.MapFrom(project => project.Members.First()));

            #endregion

            #region Technology

            CreateMap<Technology, TechnologyDto>();
            CreateMap<TechnologyDto, Technology>();
            CreateMap<CreateTechnologyDto, Technology>();

            #endregion
        }
    }
}
