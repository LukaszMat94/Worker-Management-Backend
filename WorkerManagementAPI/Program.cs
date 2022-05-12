using Microsoft.EntityFrameworkCore;
using NLog.Web;
using WorkerManagementAPI;
using WorkerManagementAPI.Context;
using WorkerManagementAPI.Middlewares;
using WorkerManagementAPI.Services.CompanyService.Repository;
using WorkerManagementAPI.Services.CompanyService.Service;
using WorkerManagementAPI.Services.ProjectService.Repository;
using WorkerManagementAPI.Services.ProjectService.Service;
using WorkerManagementAPI.Services.TechnologyService.Repository;
using WorkerManagementAPI.Services.TechnologyService.Service;
using WorkerManagementAPI.Services.WorkerService.Repository;
using WorkerManagementAPI.Services.WorkerService.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<WorkersManagementDBContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase")));

builder.Services.AddScoped<WorkerSeeder>();

builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddSwaggerGen();

#region Repositories

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IWorkerRepository, WorkerRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITechnologyRepository, TechnologyRepository>();

#endregion

#region Services

builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IWorkerService, WorkerService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITechnologyService, TechnologyService>();

#endregion

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Host.UseNLog();

var app = builder.Build();
SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    if (scopedFactory != null)
    {
        using (var scope = scopedFactory.CreateScope())
        {
            var service = scope.ServiceProvider.GetService<WorkerSeeder>();
            if (service != null)
            {
                service.Seed();
            }

        };
    }
}
// Configure the HTTP request pipeline.

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Workers Management API");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(   
    options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()       
    );

app.Run();
