using Refit;
using VRT.Payments.Gateways.PayU.Services.DataContracts;

namespace VRT.Payments.Gateways.PayU.Services;

[Headers("Content-Type: application/x-www-form-urlencoded")]
internal interface IPayUAuthClient
{
    [Post("/pl/standard/user/oauth/authorize")]
    internal Task<ApiResponse<Authorize.Response>> Authorize(
        [Body(BodySerializationMethod.UrlEncoded)] Authorize.Request request);
}
