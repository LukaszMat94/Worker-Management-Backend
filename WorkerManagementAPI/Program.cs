using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Text;
using WorkerManagementAPI;
using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.JwtToken;
using WorkerManagementAPI.Data.MailConfig;
using WorkerManagementAPI.Middlewares;
using WorkerManagementAPI.Services.CompanyService.Repository;
using WorkerManagementAPI.Services.CompanyService.Service;
using WorkerManagementAPI.Services.MailService.Service;
using WorkerManagementAPI.Services.PasswordService.Service;
using WorkerManagementAPI.Services.ProjectService.Repository;
using WorkerManagementAPI.Services.ProjectService.Service;
using WorkerManagementAPI.Services.RoleService.Repository;
using WorkerManagementAPI.Services.RoleService.Service;
using WorkerManagementAPI.Services.TechnologyService.Repository;
using WorkerManagementAPI.Services.TechnologyService.Service;
using WorkerManagementAPI.Services.UserService.Repository;
using WorkerManagementAPI.Services.UserService.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<WorkersManagementDBContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase")));

builder.Services.AddScoped<UserSeeder>();

builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddSwaggerGen();

#region Repositories

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITechnologyRepository, TechnologyRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

#endregion

#region Services

builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITechnologyService, TechnologyService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddTransient<IMailService, MailService>();

#endregion

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailConfiguration"));

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

JwtAuthenticationSettings jwtAuthenticationSettings = new JwtAuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(jwtAuthenticationSettings);

builder.Services.AddSingleton(jwtAuthenticationSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer ";
    options.DefaultScheme = "Bearer ";
    options.DefaultChallengeScheme = "Bearer ";
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtAuthenticationSettings.JwtIssuer,
        ValidAudience = jwtAuthenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthenticationSettings.JwtKey)),
    };
});

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
            var service = scope.ServiceProvider.GetService<UserSeeder>();
            if (service != null)
            {
                service.SeedRoles();
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

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(   
    options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()       
    );

app.Run();
