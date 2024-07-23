using System.Text.Json.Serialization;

namespace Examples.BlazorServer.Database.Entities;

public class Payment
{
    [JsonConstructor]
    private Payment() { }
    public static Result<Payment> Create(CreateOrderRequest request, CreateOrderResponse serviceResponse, string providerName)
    {
        if (request is null)
        {
            return Result.Failure<Payment>("Request is empty");
        }
        if (serviceResponse is null)
        {
            return Result.Failure<Payment>("Response is empty");
        }
        if (serviceResponse.OrderId is null)
        {
            return Result.Failure<Payment>("OrderId is required");
        }
        if (string.IsNullOrWhiteSpace(serviceResponse.RedirectUrl))
        {
            return Result.Failure<Payment>("Order URL is invalid");
        }
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        return new Payment()
        {
            Id = Ulid.NewUlid().ToString(),
            OrderId = serviceResponse.OrderId,
            PaymentUrl = serviceResponse.RedirectUrl,
            AddedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Request = request,
            LastUpdateTimestamp = now,
            Status = PaymentStatus.None,
            ProviderName = providerName ?? "Unknown"
        };
    }

    [JsonInclude]
    required public string Id { get; init; }
    [JsonInclude]
    public string OrderId { get; init; } = default!;

    [JsonInclude]
    required public string PaymentUrl { get; init; }
    [JsonInclude]
    required public CreateOrderRequest Request { get; init; }

    [JsonInclude]
    required public string ProviderName { get; init; }

    [JsonInclude]
    public string? Status { get; private set; }

    [JsonInclude]
    public GetPaymentStatusResponse? Notification { get; private set; }
    [JsonInclude]
    public long AddedTimestamp { get; private set; }

    [JsonInclude]

    public long LastUpdateTimestamp { get; private set; }

    public bool UpdateNotification(GetPaymentStatusResponse? newNotification)
    {
        if (Notification == newNotification)
        {
            return false;
        }
        Notification = newNotification;
        LastUpdateTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        if (newNotification?.Status is not null)
        {
            UpdateStatus(newNotification.Status);
        }
        return true;
    }

    public bool UpdateStatus(PaymentStatus newStatus)
    {
        if (Status == newStatus)
        {
            return false;
        }
        Status = newStatus;
        LastUpdateTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        return true;
    }
}
