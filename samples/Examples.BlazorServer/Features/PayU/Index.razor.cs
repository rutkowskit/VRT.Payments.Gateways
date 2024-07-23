using Examples.BlazorServer.Database;
using Examples.BlazorServer.Database.Entities;
using Examples.BlazorServer.FileStorage;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using VRT.Payments.Gateways.PayU;
using VRT.Payments.Gateways.PayU.Options;

namespace Examples.BlazorServer.Features.PayU;

[Route(Routes.PayU.Index)]
public partial class Index
{
    private string? _lastError;

    private IReadOnlyCollection<Payment> _payments = [];

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Context
            .ObservablePayment
            .Throttle(TimeSpan.FromMilliseconds(500))
            .Subscribe(s => InvokeAsync(() => OnNext(s)))
            .DisposeWith(Disposables);
    }

    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public IFileStorage FileStorage { get; set; } = default!;
    [Inject] public IHttpContextAccessor HttpContextAccessor { get; set; } = default!;
    [Inject] public PaymentsDatabase Context { get; set; } = default!;
    [Inject(Key = Constants.GatewayName)] public IPaymentService PaymentService { get; set; } = default!;
    [Inject] public IOptions<PayUOptions> PaymentServiceOptions { get; set; } = default!;

    public async Task CreatePaymentLink()
    {
        _lastError = null;
        var request = CreateRequest();
        var result = await CreateOrder(request);

        if (result.IsSuccess)
        {
            var fileName = $"payu_order_{result.OrderId}.created";
            var fileContent = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result)) ?? [];

            await Result.Success()
                .TapTry(() => StorePayment(request, result))
                .TapTry(() => FileStorage.SaveFileAsync(new FileData(fileName, fileContent)))
                .Tap(() => NavigationManager.NavigateTo(result.RedirectUrl!))
                .TapError(err => _lastError = err);
        }
    }

    private async Task StorePayment(CreateOrderRequest request, CreateOrderResponse response)
    {
        await Payment
            .Create(request, response, PaymentService.GetProviderName())
            .Tap(Context.Payment.Add)
            .Tap(Context.SaveChangesAsync);
    }

    private async Task UpdateStatus(Payment payment)
    {
        var status = await PaymentService
            .GetPaymentStatus(payment.OrderId)
            .ToResult()
            .Map(status => payment.UpdateStatus(status.Status))
            .TapIf(u => u, Context.SaveChangesAsync)
            .TapIf(u => u, () => InvokeAsync(StateHasChanged));
    }
    private bool IsInFinalStatus(Payment payment)
    {
        var paymentState = payment.Status?.ToLower() switch
        {
            "completed" or "canceled" => true,
            _ => false
        };
        return paymentState;
    }

    private async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request)
    {
        var response = await PaymentService.CreateOrder(request);
        return response;
    }


    private async Task CancelOrder(Payment payment)
    {
        var response = await PaymentService
            .CancelOrder(payment.OrderId)
            .ToResult()
            .Tap(() => Task.Delay(1000))
            .Notify(Snackbar, "Order canceled");
    }
    private CreateOrderRequest CreateRequest()
    {
        var request = new CreateOrderRequest()
        {
            CurrencyCode = "PLN",
            CustomerIp = ClientIpAddress ?? "127.0.0.1",
            Description = "P³atnoœæ 456",
            TotalPrice = 666,
            NotifyUrl = PaymentServiceOptions.Value.NotificationEndpointUrl,
            ContinueUrl = NavigationManager.Uri,
            LineItems = [
                new() { Name = "Licencja zawodnicza Senior - Halina Kukurynowska",Quantity = 1, UnitPrice = 333, IsNonPhysical=true },
                new() { Name = "Licencja zawodnicza Senior - Marian Komoruski",Quantity = 1, UnitPrice = 333, IsNonPhysical = true}
            ],
            Buyer = new()
            {
                Email = "klient@klientowski.pl",
                FirstName = "Klient",
                LastName = "Testowski",
                ExtCustomerId = "SportsClub_12345",
                LanguageIso = "pl"
            },
            ExtOrderId = Guid.NewGuid().ToString()
        };
        return request;
    }

    public void OnNext(List<Payment> value)
    {
        var providerName = PaymentService.GetProviderName();
        _payments = value
            .Where(p => p.ProviderName == providerName)
            .OrderByDescending(o => o.AddedTimestamp)
            .Take(5000)
            .ToImmutableArray();
        StateHasChanged();
    }
}