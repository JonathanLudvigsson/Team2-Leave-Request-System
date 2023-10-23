using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using EmployeeLeaveAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeLeaveAPI.Endpoints;

public class ApprovedLeavesEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/api/approved-leaves/{id:int}",
                async (int id, IRepository<ApprovedLeave> repository, ILogger logger) =>
                {
                    try
                    {
                        var approvedLeave = await repository.Get(id);
                        return approvedLeave != null ? Results.Ok(approvedLeave) : Results.NotFound();
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Error getting approved leave");
                        return Results.StatusCode(500);
                    }
                })
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<ApprovedLeave>();

        app.MapGet("/api/approved-leaves",
                async (IRepository<ApprovedLeave> repository, ILogger logger) =>
                {
                    try
                    {
                        var approvedLeaves = await repository.GetAll();
                        return approvedLeaves.Any() ? Results.Ok(approvedLeaves) : Results.NoContent();
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Error getting approved leaves");
                        return Results.StatusCode(500);
                    }
                })
            .Produces(200)
            .Produces(204)
            .Produces(500)
            .Produces<IEnumerable<ApprovedLeave>>();

        app.MapGet("/api/approved-leaves/leavetypes/{leaveTypeId:int}",
                async (IApprovedLeavesRepository approvedLeavesRepository, ILogger logger, int leaveTypeId) =>
                {
                    try
                    {
                        var approvedLeaves = await approvedLeavesRepository.GetByLeaveType(leaveTypeId);
                        return approvedLeaves.Any() ? Results.Ok(approvedLeaves) : Results.NoContent();
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Error getting approved leaves by leave type");
                        return Results.StatusCode(500);
                    }
                })
            .Produces(200)
            .Produces(204)
            .Produces(500)
            .Produces<IEnumerable<ApprovedLeave>>();

        app.MapGet("/api/approved-leaves/timerange/{leaveTypeid:int}",
                async (IApprovedLeavesRepository approvedLeavesRepository, ILogger logger, int leaveTypeid,
                    DateTime from, DateTime to) =>
                {
                    try
                    {
                        var approvedLeaves = await approvedLeavesRepository.GetByLeaveTypeAndDateRange(leaveTypeid,
                            from, to);
                        return approvedLeaves.Any() ? Results.Ok(approvedLeaves) : Results.NoContent();
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Error getting approved leaves by leave type and time range");
                        return Results.StatusCode(500);
                    }
                })
            .Produces(200)
            .Produces(204)
            .Produces(500)
            .Produces<IEnumerable<ApprovedLeave>>();


        // ger fel
        app.MapDelete("/api/approved-leaves/request/{requestId:int}", async (IApprovedLeavesRepository appLeaveRepo, IRepository<ApprovedLeave> repo, ILogger logger, int requestId) =>
        {
            try
            {
                ApprovedLeave leaveToDelete = await appLeaveRepo.GetByRequestId(requestId);
                var deletedLeave = await repo.Delete(leaveToDelete.ApprovedLeaveId); // här, eftersom ApprovedLeaveId är null ifall det inte är är "approved"
                return deletedLeave != null ? Results.Ok(deletedLeave) : Results.NotFound();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error deleting approved leave");
                return Results.StatusCode(500);
            }
        })
            .Produces<ApprovedLeave>(200)
            .Produces(404)
            .Produces(500);
    }
}