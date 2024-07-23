using System.Reactive.Disposables;

namespace Examples.BlazorServer.Extensions;

public static class DisposableExtensions
{
    public static T DisposeWith<T>(this T instance, CompositeDisposable disposables)
        where T : IDisposable
    {
        disposables.Add(instance);
        return instance;
    }
}
