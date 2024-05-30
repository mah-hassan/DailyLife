using DailyLife.Application.Abstractions;
using DailyLife.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Infrastructure.Services;

internal class FileService : IFileService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public FileService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public void Delete(string? path)
    {
        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            return;

        File.Delete(path);
    }

    public string? GetAbsolutePath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
        {
            return default;
        }

        var serverUrl = $"{_contextAccessor.HttpContext.Request.Scheme}://" +
                        $"{_contextAccessor.HttpContext.Request.Host}";

        string relativePath = path
            .Substring(path.IndexOf("wwwroot") + "wwwroot".Length);
        relativePath = relativePath.Replace("\\", "/");

        // Remove the leading "/wwwroot" part In production env
        if (relativePath.StartsWith("/wwwroot"))
        {
            relativePath = relativePath.Substring(8); // Length of "/wwwroot"
        }

        return $"{serverUrl}{relativePath}";
    }

    public async Task<(string absolutePath, string physicalPath)> StoreAsync(IFormFile file,
        string path,
        string? fileName = null)
    {
        if(string.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException(nameof(path));

        string extention = Path.GetExtension(file.FileName);
        string[] allawedExtention = [".png", ".jpg", ".jpeg"];
        if (!allawedExtention.Contains(extention))
            throw new InValidExtentionException(extention);
        if (string.IsNullOrWhiteSpace(fileName))
        {
            fileName = Path.GetRandomFileName()
                + extention;
        }
        var _path = Path.Combine(path,fileName);
        
        using var stream = new FileStream(_path,
            FileMode.Create);
        await file.CopyToAsync(stream);
        return (GetAbsolutePath(_path)!,_path);
    }

}
