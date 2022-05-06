using AutoMapper;
using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Models.CompanyDto;
using WorkerManagementAPI.Models.ProjectDto;
using WorkerManagementAPI.Models.TechnologyDto;
using WorkerManagementAPI.Models.WorkerDto;

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

            #endregion

            #region Project

            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();
            CreateMap<CreateProjectDto, Project>();

            #endregion

            #region Technology

            CreateMap<Technology, TechnologyDto>();
            CreateMap<TechnologyDto, Technology>();
            CreateMap<CreateTechnologyDto, Technology>();

            #endregion
        }
    }
}
