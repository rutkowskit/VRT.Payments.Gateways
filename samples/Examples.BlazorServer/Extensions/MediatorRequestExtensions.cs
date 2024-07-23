using CSharpFunctionalExtensions;

namespace Examples.BlazorServer.Extensions;

public static class MediatorRequestExtensions
{
    public static async Task<Result<TResponse>> SendTo<TResponse>(this IRequest<Result<TResponse>> request, ISender mediator,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(mediator);
        return await mediator.Send(request, cancellationToken);
    }
    public static async Task<Result> SendTo(this IRequest<Result> request, ISender mediator,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(mediator);
        return await mediator.Send(request, cancellationToken);
    }

    public static async Task PublishTo(this INotification message, IPublisher mediator,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(mediator);
        await mediator.Publish(message, cancellationToken);
    }

    public static Task PublishToNoWait(this INotification message, IPublisher mediator,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(mediator);
        _ = mediator.Publish(message, cancellationToken);
        return Task.CompletedTask;
    }
}
