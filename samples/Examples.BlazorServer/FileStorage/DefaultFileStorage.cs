using CSharpFunctionalExtensions;

namespace Examples.BlazorServer.FileStorage;

public sealed class DefaultFileStorage : IFileStorage
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private readonly FileStorageOptions _options;

    public DefaultFileStorage(FileStorageOptions options)
    {
        _options = options;
        Directory.CreateDirectory(_options.StoragePath);
    }

    public async Task SaveFileAsync(FileData fileData)
    {
        var filePath = Path.Combine(_options.StoragePath, fileData.FileName);
        try
        {
            await _semaphore.WaitAsync();
            await File.WriteAllBytesAsync(filePath, fileData.FileContent);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<Result<FileData>> LoadFileAsync(string fileName)
    {
        var filePath = Path.Combine(_options.StoragePath, fileName);
        if (!File.Exists(filePath))
        {
            return Result.Failure<FileData>("File not found");
        }

        var fileContent = await File.ReadAllBytesAsync(filePath);
        return new FileData(fileName, fileContent);
    }
}