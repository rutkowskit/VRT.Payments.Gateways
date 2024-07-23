namespace Examples.BlazorServer.Extensions;

internal static class SnackBarExtensions
{
    public static void ShowError(this ISnackbar snackbar,
        string message, string? key = null)
    {
        snackbar.Add(message, Severity.Error, cfg =>
        {
            cfg.VisibleStateDuration = 1000;
        }, key ?? Guid.NewGuid().ToString());
    }
    public static void ShowSuccess(this ISnackbar snackbar,
        string message, string? key = null)
    {
        snackbar.Add(message, Severity.Success, cfg =>
        {
            cfg.VisibleStateDuration = 1000;
        }, key ?? Guid.NewGuid().ToString());
    }
}
