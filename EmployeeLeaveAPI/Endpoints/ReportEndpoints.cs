using EmployeeLeaveAPI.Interfaces;

namespace EmployeeLeaveAPI.Endpoints;

public class ReportEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // app.MapGet("/api/reports/leave-usage", async (IReportService reportService, DateTime startDate, DateTime endDate, ILogger logger) =>
        //     {
        //         try
        //         {
        //             var report = await reportService.GenerateLeaveUsageReport(startDate, endDate);
        //             return report != null ? Results.Ok(report) : Results.NoContent();
        //         }
        //         catch (Exception e)
        //         {
        //             logger.LogError(e, "Error generating leave usage report");
        //             return Results.StatusCode(500);
        //         }
        //     }).Produces<IEnumerable<LeaveUsageReport>>(200)
        //     .Produces(204)
        //     .Produces(500);

    }
}