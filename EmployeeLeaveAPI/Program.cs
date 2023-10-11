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

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            
            app.MapGet("/api/employees", async (IRepository<User> repository) =>
            {
                try
                {
                    var employees = await repository.GetAll();
                    return employees.Any() ? Results.Ok(employees) : Results.NotFound();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
            
            app.MapPut("/api/employees/{id}", async (IRepository<User> repository, int id, User employee) =>
            {
                try
                {
                    var updatedEmployee = await repository.Update(id, employee);
                    return updatedEmployee != null ? Results.Ok(updatedEmployee) : Results.NotFound();
                }
                catch (Exception e)
                {
                    return Results.BadRequest("Internal server error: " + e.Message);
                }
            });

            app.Run();
        }
    }
}