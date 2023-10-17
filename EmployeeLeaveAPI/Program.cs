using System;
using System.Text.Json.Serialization;
using EmployeeLeaveAPI.Data;
using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using EmployeeLeaveAPI.Repositories;
using EmployeeLeaveAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped(typeof(IUserLeaveBalanceRepository), typeof(UserLeaveBalanceRepository));
            builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            builder.Services.AddScoped(typeof(IApprovedLeavesService), typeof(ApprovedLeavesService));
            builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
            builder.Services.AddScoped(typeof(IRequestService), typeof(RequestService));
            builder.Services.AddScoped(typeof(IRequestRepository), typeof(RequestRepository));
            builder.Services.AddScoped(typeof(IApprovedLeavesRepository), typeof(ApprovedLeavesRepository));
            builder.Services.AddScoped<ILogger, Logger<Program>>();
            builder.Services.AddAutoMapper(typeof(Program));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

            builder.Services.AddCors((setup) =>
            {
                setup.AddPolicy("default", (options) =>
                {
                    options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("default");

            Endpoints.UserEndpoints.RegisterEndpoints(app);
            Endpoints.LeaveTypeEndpoints.RegisterEndpoints(app);
            Endpoints.RequestEndpoints.RegisterEndpoints(app);
            Endpoints.UserLeaveBalanceEndpoints.RegisterEndpoints(app);
            Endpoints.AuthEndpoints.RegisterEndpoints(app);
            Endpoints.ApprovedLeavesEndpoints.RegisterEndpoints(app);

            app.Run();
        }
    }
}