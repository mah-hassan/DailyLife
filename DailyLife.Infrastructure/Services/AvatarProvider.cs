
using DailyLife.Application.Abstractions;
using DailyLife.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using System.Text;

internal class AvatarProvider : IAvatarProvider
{
    private readonly HttpClient _httpClient;

    public AvatarProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<string?> TryGenerateAvatarAsync(FullName fullName, string? pictureName = null)
    {
        var intials = GetInitials(fullName.Value);
        if (pictureName is { })
        {
            if (HasAvatar(pictureName, fullName))
            {
                return null;
            }
        }
        var url = $"https://ui-avatars.com/api/?name={intials}&background=random&bold=true&rounded=true";
        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var fileName = intials + Path.GetRandomFileName() + ".png";
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "user");
            Directory.CreateDirectory(uploadPath);

            string physicalPath = Path.Combine(uploadPath, fileName);
            using var stream = new FileStream(physicalPath, FileMode.Create);
            await response.Content.CopyToAsync(stream);
            return physicalPath;
        }
        else
            throw new InvalidOperationException("unable to get avatar");
    }

    public bool HasAvatar(string pictureName, FullName fullName)
        => pictureName.StartsWith(GetInitials(fullName.Value));

    private string GetInitials(string fullName)
    {
        StringBuilder initials = new StringBuilder();
        foreach (string part in fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            initials.Append(part[0]);
        }
        return initials.ToString().ToUpper();
    }
}
