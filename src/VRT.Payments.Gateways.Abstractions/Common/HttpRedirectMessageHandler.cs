using Microsoft.Extensions.Logging;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace VRT.Payments.Gateways.Common;
public sealed class RedirectMessageHttpClientHandler : HttpClientHandler
{
    private readonly ILogger<RedirectMessageHttpClientHandler> _logger;

    public RedirectMessageHttpClientHandler(ILogger<RedirectMessageHttpClientHandler> logger)
    {
        AllowAutoRedirect = false;
        _logger = logger;
        //ServerCertificateCustomValidationCallback = ServerCertificateCustomValidation;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
#if DEBUG
        var content = await response.Content.ReadAsStringAsync();
        var requestContent = "";
        if (request.Content is not null)
        {
            requestContent = await request.Content.ReadAsStringAsync();
        }
        _logger.LogDebug("{Url}, {RequestContent},  {ResponseContent}", request.RequestUri, requestContent, content);
#endif
        if (response.StatusCode == System.Net.HttpStatusCode.Redirect)
        {
            response.StatusCode = System.Net.HttpStatusCode.OK;
        }

        //if (response.Headers.Location != null)
        //{
        //    // Allow the redirect
        //    return response;
        //}
        //else
        //{
        //    // Do not follow the redirect

        //}
        return response;
    }
    private static bool ServerCertificateCustomValidation(HttpRequestMessage requestMessage, X509Certificate2? certificate, X509Chain? chain, SslPolicyErrors sslErrors)
    {
        // It is possible to inspect the certificate provided by the server.
        if (certificate is not null)
        {
            Console.WriteLine($"Requested URI: {requestMessage.RequestUri}");
            Console.WriteLine($"Effective date: {certificate.GetEffectiveDateString()}");
            Console.WriteLine($"Exp date: {certificate.GetExpirationDateString()}");
            Console.WriteLine($"Issuer: {certificate.Issuer}");
            Console.WriteLine($"Subject: {certificate.Subject}");
            Console.WriteLine($"Errors: {sslErrors}");
            return certificate.Verify();
        }
        // Based on the custom logic it is possible to decide whether the client considers certificate valid or not        
        return false;
    }
}