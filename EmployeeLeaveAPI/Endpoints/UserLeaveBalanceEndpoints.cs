using AutoMapper;
using EmployeeLeaveAPI.DTOs;
using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeLeaveAPI.Endpoints;

public class UserLeaveBalanceEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/api/user-leave-balances", async (IRepository<UserLeaveBalance> repository, ILogger logger) =>
            {
                try
                {
                    var userLeaveBalances = await repository.GetAll();
                    return userLeaveBalances.Any() ? Results.Ok(userLeaveBalances) : Results.NoContent();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error getting user leave balances");
                    return Results.StatusCode(500);
                }
            })
            .Produces(200)
            .Produces(204)
            .Produces(500)
            .Produces<IEnumerable<UserLeaveBalance>>();

        app.MapGet("/api/userleave-balances/{id:int}", async (int id, IRepository<UserLeaveBalance> repository, ILogger logger) =>
            {
                try
                {
                    var userLeaveBalance = await repository.Get(id);
                    return userLeaveBalance != null ? Results.Ok(userLeaveBalance) : Results.NotFound();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error getting user leave balance");
                    return Results.StatusCode(500);
                }
            })
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<UserLeaveBalance>();

        app.MapPut("/api/user-leave-balances/{id}", async (IRepository<UserLeaveBalance> repository, int id, [FromBody] UserLeaveBalance userLeaveBalance,
                ILogger logger, IMapper mapper) =>
            {
                try
                {
                    if (id != userLeaveBalance.ID)
                    {
                        return Results.BadRequest("ID in body doesn't match ID in URI");
                    }

                    var existingUserLeaveBalance = await repository.Get(id);

                    if (existingUserLeaveBalance == null)
                    {
                        return Results.NotFound();
                    }

                    mapper.Map(userLeaveBalance, existingUserLeaveBalance);

                    var updatedUserLeaveBalance = await repository.Update(id, userLeaveBalance);
                    return updatedUserLeaveBalance != null ? Results.Ok(updatedUserLeaveBalance) : Results.NotFound();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error updating user leave balance");
                    return Results.StatusCode(500);
                }
            })
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<UserLeaveBalance>();

        app.MapPost("/api/user-leave-balances", async (IRepository<UserLeaveBalance> repository, ILogger logger, IMapper mapper,
                [FromBody] CreateUserLeaveBalanceDTO userLeaveBalanceDTO) =>
            {
                try
                {
                    var userLeaveBalance = mapper.Map<UserLeaveBalance>(userLeaveBalanceDTO);
                    var createdUserLeaveBalance = await repository.Create(userLeaveBalance);
                    return createdUserLeaveBalance != null
                        ? Results.Created($"/api/users/{createdUserLeaveBalance.ID}", createdUserLeaveBalance)
                        : Results.BadRequest();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error creating user leave balance");
                    return Results.StatusCode(500);
                }
            })
            .Produces<UserLeaveBalance>(201)
            .Produces(400)
            .Produces(500);

        app.MapDelete("/api/user-leave-balances/{id:int}", async (int id, IRepository<UserLeaveBalance> repository, ILogger logger) =>
            {
                try
                {
                    var deletedUserLeaveBalance = await repository.Delete(id);
                    return deletedUserLeaveBalance != null ? Results.Ok(deletedUserLeaveBalance) : Results.NotFound();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error deleting user leave balance");
                    return Results.StatusCode(500);
                }
            })
            .Produces<User>(200)
            .Produces(404)
            .Produces(500);
    }
}