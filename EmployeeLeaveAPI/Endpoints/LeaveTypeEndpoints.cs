using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using EmployeeLeaveAPI.Repositories;
using EmployeeLeaveAPI.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmployeeLeaveAPI.Services;

namespace EmployeeLeaveAPI.Endpoints
{
    public class LeaveTypeEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            app.MapGet("/api/leavetypes", async (IRepository<LeaveType> repository, ILogger logger) =>
            {
                try
                {
                    var types = await repository.GetAll();
                    return types.Any() ? Results.Ok(types) : Results.NoContent();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error getting leavetypes");
                    return Results.StatusCode(500);
                }
            }).Produces<IEnumerable<LeaveType>>(200)
                .Produces(204)
                .Produces(500);

            app.MapGet("/api/leavetypes/{id}", async (IRepository<LeaveType> Repository, int id, ILogger logger) =>
            {
                try
                {
                    var type = await Repository.Get(id);
                    return type != null ? Results.Ok(type) : Results.NotFound();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error getting leavetype");
                    return Results.StatusCode(500);
                }
            }).Produces<LeaveType>(200)
            .Produces(404)
            .Produces(500);

            app.MapPost("/api/leavetypes", async (IRepository<LeaveType> repository, [FromBody] CreateLeaveTypeDTO newLeaveDTO, ILogger logger, [FromServices] IMapper mapper) =>
            {
                try
                {
                    LeaveType newType = mapper.Map<LeaveType>(newLeaveDTO);
                    var result = await repository.Create(newType);
                    return result != null ? Results.Created($"/api/leavetypes/{newType.LeaveTypeID}", newType) : Results.Conflict();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error creating new leavetype");
                    return Results.StatusCode(500);
                }
            }).Produces<LeaveType>(200)
            .Produces(409)
            .Produces(500);

            app.MapPut("/api/leavetypes/{id}", async(IRepository<LeaveType> repository, int id, [FromBody] CreateLeaveTypeDTO updatedLeaveDTO, ILogger logger, [FromServices] IMapper mapper) =>
            {
                try
                {
                    LeaveType updatedLeave = mapper.Map<LeaveType>(updatedLeaveDTO);
                    updatedLeave.LeaveTypeID = id;
                    var result = await repository.Update(id, updatedLeave);
                    return result != null ? Results.Ok(result) : Results.NotFound();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error updating leave");
                    return Results.StatusCode(500);
                }
            }).Produces<LeaveType>(200)
            .Produces(404)
            .Produces(500);

            app.MapDelete("/api/leavetypes/{id}", async (IRepository<LeaveType> repository, int id, ILogger logger) =>
            {
                try
                {
                    var leaveToDelete = await repository.Delete(id);
                    return leaveToDelete != null ? Results.Ok(leaveToDelete) : Results.NotFound();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error deleting leave");
                    return Results.StatusCode(500);
                }
            }).Produces<LeaveType>(200)
            .Produces(404)
            .Produces(500);

            app.MapGet("/api/leavetypes/daysused", async (IRepository<LeaveType> repository, IApprovedLeavesRepository appRepo, IApprovedLeavesService leavesService, ILogger logger, IMapper mapper) =>
            {
                try
                {
                    var types = await repository.GetAll();

                    List<LeaveTypeDaysUsedDTO> typesWithDays = new List<LeaveTypeDaysUsedDTO>();

                    foreach (var type in types)
                    {
                        typesWithDays.Add(mapper.Map<LeaveTypeDaysUsedDTO>(type));
                    }

                    foreach (var type in typesWithDays)
                    {
                        int totalDays = 0;
                        foreach (var appLeave in await appRepo.GetByLeaveType(type.LeaveTypeID))
                        {
                            totalDays += leavesService.CalculateActualLeaveDays(appLeave.StartDate, appLeave.EndDate);
                        }
                        type.TotalDaysUsed = totalDays;
                    }

                    return typesWithDays.Any() ? Results.Ok(typesWithDays) : Results.NoContent();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error getting leavetypes");
                    return Results.StatusCode(500);
                }
            });
        }
    }
}
