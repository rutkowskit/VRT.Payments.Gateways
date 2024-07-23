

namespace Examples.BlazorServer.Extensions;

internal static class DialogServiceExtensions
{
    public static async Task<Result<string>> ToResult(this Task<IDialogReference> dialog, string? errorMessage = null)
    {
        var dialogResult = await dialog;
        var result = await dialogResult.Result;

        if (result is null || result.Canceled)
        {
            return Result.Failure<string>("*");
        }
        var reason = result.Data?.ToString();
        return string.IsNullOrWhiteSpace(reason)
            ? Result.Failure<string>(errorMessage ?? "Nie podano wymaganych danych")
            : reason;
    }

    public static async Task<Result> Confirm(this IDialogService DialogService, string message, string title = "Proszę potwierdzić")
    {
        var dialog = await DialogService.ShowAsync<ConfirmationDialog>(title, new DialogParameters()
        {
            [nameof(ConfirmationDialog.Message)] = message
        });

        var result = await dialog.Result;

        if (result is null || result.Canceled)
        {
            return Result.Failure<string>("*");
        }
        return Result.Success();
    }
}
