using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using EmployeeLeaveAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace EmployeeLeaveAPI.Endpoints
{
    public class RequestEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            //GetAll
            app.MapGet("/api/request", async (IRepository<Request> repository, ILogger logger) =>
            {
                try
                {
                    var requests = await repository.GetAll();
                    return requests.Any() ? Results.Ok(requests) : Results.NoContent();
                }
                catch (Exception e)
                {

                    logger.LogError(e, "Error getting requests");
                    return Results.StatusCode(500);
                }
            })
            .Produces(200)
            .Produces(204)
            .Produces(500)
            .Produces<IEnumerable<Request>>();

            //GetOneById
            app.MapGet("/api/request/{id}", async (IRepository<Request> repository, ILogger logger, int id) =>
            {
                try
                {
                    var request = await repository.Get(id);
                    return request != null ? Results.Ok(request) : Results.NoContent();
                }
                catch (Exception e)
                {

                    return Results.BadRequest("Internal server error" + e.Message);
                }
            })
            .Produces(200)
            .Produces(204)
            .Produces(500);

            //Post
            app.MapPost("/api/request/post", async (IRepository<Request> repository, ILogger logger, Request request) =>
            {
                try
                {
                    await repository.Create(request);
                    return request != null ? Results.Created($"/api/request/post/{request.RequestID}", request) : Results.NoContent();
                }
                catch (Exception e)
                {

                    return Results.BadRequest("Internal server error" + e.Message);
                }
            })
            .Produces(200)
            .Produces(204)
            .Produces(500)
            .Produces(201);


            // Delete
            app.MapDelete("/api/request/delete/{id}", async (IRepository<Request> repository, ILogger logger, int id) =>
            {
                try
                {
                    var deleteRequest = await repository.Delete(id);
                    return deleteRequest != null ? Results.Ok($"request with ID : {deleteRequest.RequestID} was deleted") : Results.NoContent();

                }
                catch (Exception e)
                {

                    return Results.BadRequest("Internal Server error" + e.Message);
                }
            })
            .Produces(200)
            .Produces(204)
            .Produces(500)
            .Produces(201);

            //Update  // Inte Klar Fungerar, finns buggar 
            app.MapPut("/api/request/update", async (IRepository<Request> repository, ILogger logger,int id, Request request) =>
            {
                try
                {
                    var updateRequest = await repository.Update(id, request);
                    return updateRequest != null ? Results.Ok($"request with ID : {updateRequest.RequestID} was updated") : Results.NoContent();
                }
                catch (Exception e)
                {

                    return Results.BadRequest("Internal Server error" + e.Message);
                }
            });

        }
    }
}
