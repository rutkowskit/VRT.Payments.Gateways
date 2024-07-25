using Examples.BlazorServer.Database;
using Examples.BlazorServer.FileStorage;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using VRT.Payments.Gateways.PayU;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Examples.BlazorServer.Features.PayU.Notifications;
public static class NotificationsEndpoints
{
    internal record NotificationResponse(string Status);
    public static void MapPayUEndpoints(this IEndpointRouteBuilder webApp)
    {
        webApp
            .MapPost("/api/notifications/payu", OnPostNotification)
            .Accepts<HttpRequest>("application/json")
            .DisableAntiforgery()
            .DisableRateLimiting()
            .Produces<IResult>()
            .AllowAnonymous();
    }

    private static async Task<IResult> OnPostNotification(
        HttpRequest request,
        [FromServices] IFileStorage storage,
        [FromServices] PaymentsDatabase context,
        [FromKeyedServices(key: Constants.GatewayName)] IPaymentService paymentService)
    {
        if (request?.HttpContext is null)
        {
            return Results.BadRequest();
        }

        var paymentStatus = await paymentService
            .GetPaymentStatusFromNotification(request);
        if (paymentStatus.IsSuccess == false)
        {
            return Results.Problem(paymentStatus.ErrorMessage, statusCode: paymentStatus.HttpStatusCode);
        }

        var body = "";
        using (var reader = new StreamReader(request.Body, Encoding.UTF8))
        {
            body = await reader.ReadToEndAsync();
        }
        var bodyBytes = Encoding.UTF8.GetBytes(body);

        if (paymentStatus.IsSuccess)
        {
            await context.UpdatePayment(paymentStatus);

            if (paymentStatus.IsSuccess && paymentStatus.Status == PaymentStatus.Completed)
            {
                var fileName = $"payu_payment_{paymentStatus.OrderId}.{paymentStatus.Status.Name}";
                var content = bodyBytes;
                await storage.SaveFileAsync(new FileData(fileName, content));
            }
        }
        return Results.Ok(new NotificationResponse("ok"));
    }

    private static async Task<Result> UpdatePayment(this PaymentsDatabase context, GetPaymentStatusResponse notification)
    {
        var payment = context.Payment
            .Where(p => p.OrderId == notification.OrderId)
            .FirstOrDefault();
        if (payment is null)
        {
            return Result.Failure("Payment not found");
        }
        payment.UpdateNotification(notification);
        payment.UpdateStatus(notification.Status);
        await context.SaveChangesAsync();
        return Result.Success();
    }
}
