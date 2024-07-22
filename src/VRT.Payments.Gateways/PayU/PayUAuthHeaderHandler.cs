using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using VRT.Payments.Gateways.PayU.Options;
using VRT.Payments.Gateways.PayU.Services;
using VRT.Payments.Gateways.PayU.Services.DataContracts;

namespace VRT.Payments.Gateways.PayU;

internal sealed class PayUAuthHeaderHandler : DelegatingHandler
{
    private readonly IPayUAuthClient _authClient;
    private readonly IOptions<PayUOptions> _options;
    private static Authorize.Response? _latestResponse;

    public PayUAuthHeaderHandler(IPayUAuthClient authClient, IOptions<PayUOptions> options)
    {
        _authClient = authClient;
        _options = options;
        // InnerHandler must be left as null when using DI, but must be assigned a value when
        // using RestService.For<IMyApi>
        // InnerHandler = new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await GetToken();

        //potentially refresh token here if it has expired etc.

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        return response;
    }
    private async Task<string> GetToken()
    {
        if (_latestResponse is null || _latestResponse.ShouldRefresh)
        {
            _latestResponse = await Authorize();
        }
        return _latestResponse?.AccessToken ?? throw new UnauthorizedAccessException();
    }
    private async Task<Authorize.Response?> Authorize()
    {
        var authRequest = new Authorize.Request()
        {
            ClientId = _options.Value.ClientId,
            ClientSecret = _options.Value.ClientSecret
        };
        var result = await _authClient.Authorize(authRequest);
        var response = result.IsSuccessStatusCode
            ? result.Content
            : null;
        return response;
    }
}
