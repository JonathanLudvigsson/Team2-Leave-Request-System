using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;

namespace EmployeeLeaveAPI.Endpoints;

public static class UserEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/api/users", async (IRepository<User> repository, ILogger logger) =>
            {
                try
                {
                    var users = await repository.GetAll();
                    return users.Any() ? Results.Ok(users) : Results.NoContent();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error getting users");
                    return Results.StatusCode(500);
                }
            })
            .Produces(200)
            .Produces(204)
            .Produces(500)
            .Produces<IEnumerable<User>>();
        
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
    }
}