using AutoMapper;
using EmployeeLeaveAPI.DTOs;
using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using Microsoft.AspNetCore.Mvc;

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

        app.MapGet("/api/users/{id:int}", async (int id, IRepository<User> repository, ILogger logger) =>
            {
                try
                {
                    var user = await repository.Get(id);
                    return user != null ? Results.Ok(user) : Results.NotFound();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error getting user");
                    return Results.StatusCode(500);
                }
            })
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<User>();

        app.MapPut("/api/employees/{id}", async (IRepository<User> repository, int id, [FromBody] User user,
                ILogger logger, IMapper mapper) =>
            {
                try
                {
                    if (id != user.ID)
                    {
                        return Results.BadRequest("ID in body doesn't match ID in URI");
                    }

                    var existingUser = await repository.Get(id);

                    if (existingUser == null)
                    {
                        return Results.NotFound();
                    }

                    mapper.Map(user, existingUser);

                    var updatedUser = await repository.Update(id, user);
                    return updatedUser != null ? Results.Ok(updatedUser) : Results.NotFound();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error updating employee");
                    return Results.StatusCode(500);
                }
            })
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<User>();

        app.MapPost("/api/users", async (IRepository<User> repository, ILogger logger, IMapper mapper,
                [FromBody] CreateUserDTO userDTO) =>
            {
                try
                {
                    var user = mapper.Map<User>(userDTO);
                    var createdUser = await repository.Create(user);
                    return createdUser != null
                        ? Results.Created($"/api/users/{createdUser.ID}", createdUser)
                        : Results.BadRequest();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error creating user");
                    return Results.StatusCode(500);
                }
            })
            .Produces<User>(201)
            .Produces(400)
            .Produces(500);

        app.MapDelete("/api/users/{id:int}", async (int id, IRepository<User> repository, ILogger logger) =>
            {
                try
                {
                    var deletedUser = await repository.Delete(id);
                    return deletedUser != null ? Results.Ok(deletedUser) : Results.NotFound();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error deleting user");
                    return Results.StatusCode(500);
                }
            })
            .Produces<User>(200)
            .Produces(404)
            .Produces(500);
    }
}