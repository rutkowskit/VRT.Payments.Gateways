using CSharpFunctionalExtensions;

namespace Examples.BlazorServer.Abstractions;

public interface ICommand : IRequest<Result>
{
}
