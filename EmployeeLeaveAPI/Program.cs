using System;
using EmployeeLeaveAPI.Data;
using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using EmployeeLeaveAPI.Repositories;
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
            builder.Services.AddScoped<ILogger, Logger<Program>>();
            builder.Services.AddAutoMapper(typeof(Program));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            Endpoints.UserEndpoints.RegisterEndpoints(app);
            Endpoints.LeaveTypeEndpoints.RegisterEndpoints(app);
            Endpoints.RequestEndpoints.RegisterEndpoints(app);
            Endpoints.UserLeaveBalanceEndpoints.RegisterEndpoints(app);

            app.Run();
        }
    }
}