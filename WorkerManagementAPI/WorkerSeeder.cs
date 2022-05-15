using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Entities.Enums;

namespace WorkerManagementAPI
{
    public class WorkerSeeder
    {
        private readonly WorkersManagementDBContext _dbContext;
        public WorkerSeeder(WorkersManagementDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (_dbContext.Companies.Count() == 0)
                {
                    List<Company> companies = InitializeCompanies();

                    _dbContext.Companies.AddRange(companies);
                    _dbContext.SaveChanges();
                }
            }
        }

        private List<Company> InitializeCompanies()
        {
            List<Company> companies = new List<Company>();

            Company braveCompany = new Company
            {
                Name = "BraveAmbition"
            };

            Company teslaCompany = new Company
            {
                Name = "Tesla"
            };

            Technology csharpTechnology = new Technology
            {
                Name = "C#",
                TechnologyLevel = TechnologyLevelEnum.Advanced
            };

            Technology javaTechnology = new Technology
            {
                Name = "Java",
                TechnologyLevel = TechnologyLevelEnum.Medium
            };

            Technology reactTechnology = new Technology
            {
                Name = "React",
                TechnologyLevel = TechnologyLevelEnum.Basic
            };

            Worker firstBraveWorker = new Worker
            {
                Name = "Sebastian",
                Surname = "Kowalczyk",
                Email = "kosemi1@gmail.com"
            };

            Worker secondBraveWorker = new Worker
            {
                Name = "Łukasz",
                Surname = "Matusik",
                Email = "matusik5@yahoo.com"
            };

            Worker firstTeslaWorker = new Worker
            {
                Name = "Elon",
                Surname = "Musk",
                Email = "musk@tesla.com"
            };

            Project apiProject = new Project
            {
                Name = "Create API",
            };

            Project webPageProject = new Project
            {
                Name = "Create WebPage",
            };

            Project serviceProject = new Project
            {
                Name = "Service create"
            };

            apiProject.Technologies = new List<Technology>();
            apiProject.Technologies.Add(javaTechnology);
            apiProject.Technologies.Add(csharpTechnology);

            webPageProject.Technologies = new List<Technology>();
            webPageProject.Technologies.Add(reactTechnology);

            serviceProject.Technologies = new List<Technology>();
            serviceProject.Technologies.Add(javaTechnology);

            #region Workers
            firstBraveWorker.Technologies = new List<Technology>();
            firstBraveWorker.Technologies.Add(reactTechnology);
            firstBraveWorker.Technologies.Add(javaTechnology);

            firstBraveWorker.Projects = new List<Project>();
            firstBraveWorker.Projects.Add(apiProject);
            firstBraveWorker.Projects.Add(serviceProject);

            secondBraveWorker.Technologies = new List<Technology>();
            secondBraveWorker.Technologies.Add(csharpTechnology);

            secondBraveWorker.Projects = new List<Project>();
            secondBraveWorker.Projects.Add(apiProject);

            firstTeslaWorker.Technologies = new List<Technology>();
            firstTeslaWorker.Technologies.Add(javaTechnology);
            firstTeslaWorker.Technologies.Add(csharpTechnology);
            firstTeslaWorker.Technologies.Add(reactTechnology);

            firstTeslaWorker.Projects = new List<Project>();
            firstTeslaWorker.Projects.Add(serviceProject);
            firstTeslaWorker.Projects.Add(apiProject);
            firstTeslaWorker.Projects.Add(webPageProject);

            #endregion

            braveCompany.Workers = new List<Worker>();
            braveCompany.Workers.Add(firstBraveWorker);
            braveCompany.Workers.Add(secondBraveWorker);

            teslaCompany.Workers = new List<Worker>();
            teslaCompany.Workers.Add(firstTeslaWorker);

            companies.Add(braveCompany);
            companies.Add(teslaCompany);

            return companies;
        }


    }
}
