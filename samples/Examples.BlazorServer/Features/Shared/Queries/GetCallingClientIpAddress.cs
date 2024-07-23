using Examples.BlazorServer.Abstractions;

namespace Examples.BlazorServer.Features.Shared.Queries;

public static class GetCallingClientIpAddress
{
    internal sealed class Handler(IHttpContextAccessor httpAccessor) : IRequestHandler<Query, Result<Response>>
    {
        private readonly IHttpContextAccessor _httpAccessor = httpAccessor;

        public Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var ipAddress = _httpAccessor.HttpContext?.Request.Headers
                .Where(h => h.Key == "Cf-Connecting-Ip")
                .Select(v => v.Value)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                ipAddress = _httpAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
            }
            var result = ipAddress is null
                ? Result.Failure<Response>("Ip address is empty")
                : Result.Success(new Response(ipAddress!));

            return Task.FromResult(result);
        }
    }

    public record Query() : IQuery<Response>;
    public record Response(string IpAddress);
}
