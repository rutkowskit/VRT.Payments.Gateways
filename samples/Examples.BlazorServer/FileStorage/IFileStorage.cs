using CSharpFunctionalExtensions;

namespace Examples.BlazorServer.FileStorage;

public interface IFileStorage
{
    Task SaveFileAsync(FileData fileData);
    Task<Result<FileData>> LoadFileAsync(string fileName);
}
