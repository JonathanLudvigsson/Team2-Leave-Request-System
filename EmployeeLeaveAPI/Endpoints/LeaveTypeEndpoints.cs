using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using EmployeeLeaveAPI.Repositories;

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

            app.MapPost("/api/leavetypes", async (IRepository<LeaveType> Repository, LeaveType newLeave, ILogger logger) =>
            {
                try
                {
                    var newType = await Repository.Create(newLeave);
                    return newType != null ? Results.Created($"/api/leavetypes/{newType.LeaveTypeID}", newType) : Results.Conflict();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error creating new leavetype");
                    return Results.StatusCode(500);
                }
            }).Produces<LeaveType>(200)
            .Produces(409)
            .Produces(500);

            app.MapPut("/api/leavetypes/{id}", async (IRepository<LeaveType> repository, int id, LeaveType updatedLeave, ILogger logger) =>
            {
                try
                {
                    var leaveToUpdate = await repository.Update(id, updatedLeave);
                    return leaveToUpdate != null ? Results.Ok(leaveToUpdate) : Results.NotFound();
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
        }
    }
}
