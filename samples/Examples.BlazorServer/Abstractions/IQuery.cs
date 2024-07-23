using CSharpFunctionalExtensions;

namespace Examples.BlazorServer.Abstractions;

public interface IQuery<TDto> : IRequest<Result<TDto>>
{
}