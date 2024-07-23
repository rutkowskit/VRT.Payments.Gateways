using Examples.BlazorServer.Features.Shared.Queries;
using System.Reactive.Disposables;

namespace Examples.BlazorServer.Features.Shared;

public abstract class BaseAppComponent : ComponentBase, IDisposable
{
    protected readonly CompositeDisposable Disposables = [];
    private bool _disposedValue;

    [Inject] public NavigationManager Navigation { get; set; } = null!;
    [Inject] public ISender Sender { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;

    protected string? ClientIpAddress { get; private set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await RefreshClientIpAddress();
        }
    }

    public string GetReturnUrl()
    {
        var uri = new Uri(Navigation.Uri).GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
        var returnUrl = Uri.EscapeDataString(uri);
        return returnUrl;
    }

    protected async Task RefreshClientIpAddress()
    {
        await new GetCallingClientIpAddress.Query()
            .SendTo(Sender)
            .Tap(ip => InvokeAsync(() => ClientIpAddress = ip.IpAddress))
            .NotifyError(Snackbar);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                Disposables.Dispose();
            }
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
