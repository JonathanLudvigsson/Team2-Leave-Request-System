using EmployeeLeaveAPI.DTOs;
using EmployeeLeaveAPI.Interfaces;

namespace EmployeeLeaveAPI.Endpoints;

public class UserLeaveBalanceEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/api/user-leave-balances/user/{userId:int}",
                async (IUserLeaveBalanceService userLeaveBalanceService, int userId) =>
                {
                    try
                    {
                        var userLeaveBalance =
                            await userLeaveBalanceService.GetUserDaysLeftAsync(userId);
                        if (userLeaveBalance == null)
                        {
                            return Results.NotFound();
                        }

                        return Results.Ok(userLeaveBalance);
                    }
                    catch (Exception e)
                    {
                        return Results.BadRequest(e.Message);
                    }
                })
            .Produces<List<UserLeaveBalanceDTO>>();
    }
}