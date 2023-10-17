using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;

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
    }
}