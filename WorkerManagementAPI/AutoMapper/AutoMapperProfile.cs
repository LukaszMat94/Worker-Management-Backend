﻿using AutoMapper;
using WorkerManagementApi.Data.Models.ProjectDtos;
using WorkerManagementApi.Data.Models.WorkerDtos;
using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Models.CompanyDtos;
using WorkerManagementAPI.Models.ProjectDtos;
using WorkerManagementAPI.Models.TechnologyDtos;
using WorkerManagementAPI.Models.WorkerDtos;

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
            CreateMap<Project, UpdateProjectWorkerDto>()
                .ForMember(updateProject => updateProject.Worker,
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
