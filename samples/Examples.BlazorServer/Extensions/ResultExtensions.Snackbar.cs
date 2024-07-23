using CSharpFunctionalExtensions;
using MudBlazor;

namespace Examples.BlazorServer.Extensions;

public static class ResultExtensions
{
    public static Task<Result<T>> NotifyError<T>(this Task<Result<T>> resultTask, ISnackbar snackbar)
    {
        return resultTask.TapErrorIf(
            err => err != "*",
            err => snackbar.ShowError(err));
    }

    public static Task<Result> NotifyError(this Task<Result> resultTask, ISnackbar snackbar)
    {
        return resultTask.TapErrorIf(
            err => err != "*",
            err => snackbar.ShowError(err));
    }


    public static Task<Result<T>> Notify<T>(this Task<Result<T>> resultTask,
        ISnackbar snackbar,
        string? successMessage = null)
    {
        return resultTask
            .Tap(() => snackbar.ShowSuccess(successMessage ?? "Wykonano pomyślnie"))
            .NotifyError(snackbar);
    }

    public static Task<Result> Notify(this Task<Result> resultTask,
        ISnackbar snackbar,
        string? successMessage = null)
    {
        return resultTask
            .Tap(() => snackbar.ShowSuccess(successMessage ?? "Wykonano pomyślnie"))
            .NotifyError(snackbar);
    }
}
