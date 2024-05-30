using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Abstractions;

public interface IFileService
{
    Task<(string absolutePath, string physicalPath)> StoreAsync(IFormFile file, string path, string? fileName = null);
    string? GetAbsolutePath(string? path);
    void Delete(string? path);
}
