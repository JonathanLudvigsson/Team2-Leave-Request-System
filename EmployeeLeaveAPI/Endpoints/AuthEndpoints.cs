using EmployeeLeaveAPI.DTOs;
using EmployeeLeaveAPI.Interfaces;

namespace EmployeeLeaveAPI.Endpoints;

public static class AuthEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapPost("/api/auth/login", async (IAuthService authService, LoginDTO model) =>
            {
                var result = await authService.LoginUser(model);

                if (result.IsSuccess)
                {
                    return Results.Ok(result);
                }

                return Results.BadRequest(result);
            })
            .Produces(200)
            .Produces(400)
            .Produces<LoginResult>();
    }
}