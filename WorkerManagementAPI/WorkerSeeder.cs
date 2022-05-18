using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Entities.Enums;

namespace WorkerManagementAPI
{
    public class UserSeeder
    {
        private readonly WorkersManagementDBContext _dbContext;
        public UserSeeder(WorkersManagementDBContext dbContext)
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

        public void SeedRoles()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (_dbContext.Roles.Count() == 0)
                {
                    List<Role> roles = InitializeRoles();

                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
            }
        }

        public List<Role> InitializeRoles()
        {
            List<Role> roles = new () { 
                new Role { RoleName = RoleEnum.USER },
                new Role { RoleName = RoleEnum.MANAGER },
                new Role { RoleName= RoleEnum.ADMIN }
            };

            return roles;
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

            User firstBraveUser = new User
            {
                Name = "Sebastian",
                Surname = "Kowalczyk",
                Email = "kosemi1@gmail.com"
            };

            User secondBraveUser = new User
            {
                Name = "Łukasz",
                Surname = "Matusik",
                Email = "matusik5@yahoo.com"
            };

            User firstTeslaUser = new User
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

            #region Users
            firstBraveUser.Technologies = new List<Technology>();
            firstBraveUser.Technologies.Add(reactTechnology);
            firstBraveUser.Technologies.Add(javaTechnology);

            firstBraveUser.Projects = new List<Project>();
            firstBraveUser.Projects.Add(apiProject);
            firstBraveUser.Projects.Add(serviceProject);

            secondBraveUser.Technologies = new List<Technology>();
            secondBraveUser.Technologies.Add(csharpTechnology);

            secondBraveUser.Projects = new List<Project>();
            secondBraveUser.Projects.Add(apiProject);

            firstTeslaUser.Technologies = new List<Technology>();
            firstTeslaUser.Technologies.Add(javaTechnology);
            firstTeslaUser.Technologies.Add(csharpTechnology);
            firstTeslaUser.Technologies.Add(reactTechnology);

            firstTeslaUser.Projects = new List<Project>();
            firstTeslaUser.Projects.Add(serviceProject);
            firstTeslaUser.Projects.Add(apiProject);
            firstTeslaUser.Projects.Add(webPageProject);

            #endregion

            braveCompany.Users = new List<User>();
            braveCompany.Users.Add(firstBraveUser);
            braveCompany.Users.Add(secondBraveUser);

            teslaCompany.Users = new List<User>();
            teslaCompany.Users.Add(firstTeslaUser);

            companies.Add(braveCompany);
            companies.Add(teslaCompany);

            return companies;
        }


    }
}
