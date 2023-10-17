using AutoMapper;
using EmployeeLeaveAPI.DTOs;
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

            app.MapGet("/api/request/user/{id}", async (IRequestRepository repository, ILogger logger, int id) =>
                {
                    try
                    {
                        var request = await repository.GetRequestsFromUser(id);
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
            app.MapPost("/api/request/post", async (IRepository<Request> repository, ILogger logger, IMapper mapper,
                    [FromBody] CreateRequestDTO requestDto) =>
                {
                    try
                    {
                        var request = mapper.Map<Request>(requestDto);
                        var createdRequest = await repository.Create(request);
                        return createdRequest != null
                            ? Results.Created($"/api/request/post/{createdRequest.RequestID}", createdRequest)
                            : Results.BadRequest();
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Error creating user");
                        return Results.StatusCode(500);
                    }
                })
                .Produces<Request>(201)
                .Produces(400)
                .Produces(500);


            // Delete
            app.MapDelete("/api/request/delete/{id}", async (IRepository<Request> repository, ILogger logger, int id) =>
                {
                    try
                    {
                        var deleteRequest = await repository.Delete(id);
                        return deleteRequest != null
                            ? Results.Ok($"request with ID : {deleteRequest.RequestID} was deleted")
                            : Results.NoContent();
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


            app.MapPut("/api/request/update/{id}", async (IRepository<Request> repository, ILogger logger,
                    IMapper mapper, int id, Request request,
                    IApprovedLeavesService approvedLeavesService) =>
                {
                    try
                    {
                        if (id != request.RequestID)
                        {
                            return Results.BadRequest($"ID:{id} does not match any existing ID");
                        }

                        var existingRequest = await repository.Get(id);
                        if (existingRequest == null)
                        {
                            return Results.NotFound();
                        }

                        mapper.Map(request, existingRequest);

                        var updatedRequest = await repository.Update(id, request);

                        if (updatedRequest is { LeaveStatus: Status.Approved })
                        {
                            await approvedLeavesService.CreateApprovedLeave(updatedRequest.StartDate,
                                updatedRequest.EndDate, updatedRequest.UserID, updatedRequest.LeaveTypeID,
                                updatedRequest.RequestID);
                        }

                        return updatedRequest != null ? Results.Ok(updatedRequest) : Results.NoContent();
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Error While updating the request");
                        return Results.StatusCode(500);
                    }
                })
                .Produces(200)
                .Produces(404)
                .Produces(500)
                .Produces<Request>();
        }
    }
}