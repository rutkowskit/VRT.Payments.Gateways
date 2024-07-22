using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using VRT.Payments.Gateways.PayU.Services.DataContracts;

namespace VRT.Payments.Gateways.PayU;

internal sealed partial class PayUPaymentService
{
    public async Task<GetPaymentStatusResponse> GetPaymentStatusFromNotification(HttpRequest request)
    {
        if (request?.HttpContext is null)
        {
            return GetPaymentStatusResponse.Fail("", "Http Context is empty");
        }

        var body = "";
        using (var reader = new StreamReader(request.Body, Encoding.UTF8))
        {
            body = await reader.ReadToEndAsync();
        }
        var notificationOrder = ParseNotification(body)?.order;
        if (notificationOrder is null)
        {
            return GetPaymentStatusResponse.Fail("", "Notification is empty");
        }

        var validationResult = ValidateSignature(request, body);

        return ValidateSignature(request, body).IsSuccess
            ? notificationOrder.Success()
            : notificationOrder.ToPaymentStatusResponse(validationResult);
    }

    private Notification? ParseNotification(string notification)
    {
        try
        {
            var result = System.Text.Json.JsonSerializer.Deserialize<Notification>(notification);
            return result;
        }
        catch (Exception ex)
        {
            LogException(ex);
            return null;
        }
    }

    private PaymentServiceResponse ValidateSignature(HttpRequest request, string requestBody)
    {
        var incommingSignature = request.Headers["OpenPayU-Signature"];
        var privateKeyBytes = Encoding.UTF8.GetBytes(_options.Value.SecondKey);
        var bodyBytes = Encoding.UTF8.GetBytes(requestBody);
        return ValidateSignature(incommingSignature, bodyBytes, privateKeyBytes);
    }

    private static PaymentServiceResponse ValidateSignature(string? incommingSignature, byte[] body, byte[] privateKey)
    {
        if (string.IsNullOrWhiteSpace(incommingSignature) || body.Length == 0 || privateKey.Length == 0)
        {
            return PaymentServiceResponse.Fail("Incomming signature not provided");
        }
        if (privateKey.Length == 0)
        {
            return PaymentServiceResponse.Fail("Client Secret not provided");
        }
        if (body.Length == 0)
        {
            return PaymentServiceResponse.Fail("Request body is empty");
        }
        var parts = incommingSignature
            .Split(';')
            .Select(p => p.Split('='))
            .Where(p => p.Length == 2)
            .ToDictionary(k => k[0], v => v[1]);

        if ((parts.TryGetValue("signature", out var incomingSignature) is false) || parts.TryGetValue("algorithm", out var alg) is false)
        {
            return PaymentServiceResponse.Fail("Signature is invalid");
        }

        var toHash = body.Concat(privateKey).ToArray();
        var ownSignature = alg.ToLower() switch
        {
            "sha256" => string.Join("", SHA256.HashData(toHash).Select(b => $"{b:x2}")),
            "md5" => string.Join("", MD5.HashData(toHash).Select(b => $"{b:x2}")),
            _ => ""
        };
        return incomingSignature.Equals(ownSignature, StringComparison.InvariantCultureIgnoreCase)
            ? PaymentServiceResponse.Success()
            : PaymentServiceResponse.Fail("Signature is invalid");
    }
}
