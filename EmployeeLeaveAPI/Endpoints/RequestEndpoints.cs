﻿using AutoMapper;
using EmployeeLeaveAPI.DTOs;
using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using EmployeeLeaveAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EmployeeLeaveAPI.Endpoints
{
    public class RequestEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
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

            app.MapPost("/api/request/post", async (IRepository<Request> repository, ILogger logger, IMapper mapper,
                    [FromBody] CreateRequestDTO requestDto, IRequestService requestService,
                    IUserLeaveBalanceService userLeaveBalanceService) =>
                {
                    try
                    {
                        var checkDates = requestService.CheckValidDates(requestDto.StartDate, requestDto.EndDate)
                            .Result;
                        bool hasEnoughDays = userLeaveBalanceService.HasEnoughDaysLeftAsync(requestDto.UserID,
                            requestDto.LeaveTypeID, requestDto.StartDate, requestDto.EndDate).Result;

                        if (!hasEnoughDays)
                        {
                            return Results.BadRequest("Not enough days left");
                        }

                        if (!checkDates.isOk)
                        {
                            return Results.BadRequest(checkDates.message);
                        }

                        var request = mapper.Map<Request>(requestDto);
                        var createdRequest = await repository.Create(request);
                        return createdRequest != null
                            ? Results.Created($"/api/request/post/{createdRequest.RequestID}", createdRequest)
                            : Results.BadRequest();
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Error creating request");
                        return Results.StatusCode(500);
                    }
                })
                .Produces<Request>(201)
                .Produces(400)
                .Produces(500);

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
                    IApprovedLeavesService approvedLeavesService, IRequestService requestService,
                    IUserLeaveBalanceService userLeaveBalanceService, IEmailService emailService) =>
                {
                    try
                    {
                        if (id != request.RequestID)
                        {
                            return Results.BadRequest($"ID:{id} does not match any existing ID");
                        }

                        var checkDates = requestService.CheckValidDates(request.StartDate, request.EndDate).Result;

                        if (!checkDates.isOk)
                        {
                            return Results.BadRequest(checkDates.message);
                        }

                        var existingRequest = await repository.Get(id);
                        if (existingRequest == null)
                        {
                            return Results.NotFound();
                        }

                        mapper.Map(request, existingRequest);

                        var updatedRequest = await repository.Update(id, request);

                        if (updatedRequest == null)
                        {
                            return Results.BadRequest("Error updating request");
                        }

                        bool hasEnoughDays = userLeaveBalanceService.HasEnoughDaysLeftAsync(request.UserID,
                            updatedRequest.LeaveTypeID, updatedRequest.StartDate, request.EndDate).Result;

                        if (!hasEnoughDays)
                        {
                            return Results.BadRequest("Not enough days left");
                        }

                        // if the request is approved or declined
                        if (updatedRequest.LeaveStatus != Status.Pending)
                        {
                            if (updatedRequest.LeaveStatus == Status.Approved)
                            {
                                await approvedLeavesService.CreateApprovedLeave(updatedRequest.StartDate,
                                updatedRequest.EndDate, updatedRequest.UserID, updatedRequest.LeaveTypeID,
                                updatedRequest.RequestID);
                            }

                            var emailResult = await emailService.CreateEmail(updatedRequest.UserID,
                                updatedRequest.LeaveStatus);
                            var saveEmailResult = await emailService.SaveEmailToDbAsync(emailResult.email);
                            var enqueueEmailResult = await emailService.EnqueueEmail(emailResult.email);

                            if (!emailResult.isSuccess || !saveEmailResult.isSuccess || !enqueueEmailResult.isSuccess)
                            {
                                return Results.Ok("Request approved but email not sent, contact admin for more info");
                            }
                        }

                        return Results.Ok(updatedRequest);
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