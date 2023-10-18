using EmployeeLeaveAPI.Interfaces;

namespace EmployeeLeaveAPI.Endpoints;

public class UserLeaveBalanceEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/api/user-leave-balances/user/{userId:int}/leave-type/{leaveTypeId:int}",
                async (IUserLeaveBalanceService userLeaveBalanceService, int userId, int leaveTypeId) =>
                {
                    var userLeaveBalance =
                        await userLeaveBalanceService.GetUserDaysLeftByLeavetype(userId, leaveTypeId);
                    return Results.Ok(userLeaveBalance);
                })
            .Produces<int>(200);
    }
}