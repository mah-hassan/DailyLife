using DailyLife.Domain.Entities;
using DailyLife.Domain.Errors;
using DailyLife.Domain.Primitives;
using DailyLife.Domain.Shared;
using DailyLife.Domain.ValueObjects;
using MediatR;
using System.Collections.Frozen;

namespace DailyLife.Domain.Aggregates;

public sealed class Business 
    : AggregateRoot
{
    private Business(Id id,
        Location location,
        Address address,
        string name,
        string? description,
        Guid ownerId,
        Id categoryId) 
        : base(id)
    {
        Location = location;
        Address = address;
        OwnerId = ownerId;
        Name = name;
        Description = description;
        CategoryId = categoryId;
        SetAlbumPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
            "Businesses",
            Id.ToString(),
            "Album"));
        CreatedAt = DateTime.UtcNow;
        LastModefiedAt = DateTime.UtcNow;
    }
    private Business()
    : base(new Id(Guid.NewGuid()))
    {
    }
    public Location Location { get; private set; }
    public Address Address { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Guid OwnerId { get; init; }
    public string? ProfilePicture {  get; private set; }
    public string Album {  get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime LastModefiedAt { get; private set; }
    public Category Category { get; }
    public Id CategoryId { get; init; }
    public List<Review> Reviews { get;  set; }
    private HashSet<WorkTime> _workTimes = new();
    private bool disposedValue;

    public IReadOnlySet<WorkTime> WorkTimes => _workTimes.ToFrozenSet();
    public static Result<Business> Create(Result<Location> location,
        Result<Address> address,
        string name,
        string? description,
        Guid ownerId,
        Id categoryId)
    {
        var errors = new List<Error>();
        if (location.IsFailure)
        {
            errors.AddRange(location.Errors);
        }        
        if (address.IsFailure)
        {
            errors.AddRange(address.Errors);
        }
        if (ownerId == Guid.Empty)
        {
            var invalidOwnerId = new Error(
                nameof(ownerId),
                "Invalid Owner Id");
            errors.Add(invalidOwnerId);
        }        
        if (string.IsNullOrWhiteSpace(name))
        {
            var invalidName = new Error(
                nameof(name),
                "Invalid business name can not accept null, empty or white spaces");
            errors.Add(invalidName);
        }
        if (errors.Any())
        {
            return Result.Failure<Business>(400, errors.ToArray());
        }
        
        return new Business(
            new Id(Guid.NewGuid()),
            location.Value,
            address.Value,
            name,
            description,
            ownerId,
            categoryId);
    }
    public void SetProfilePictureUrl(string url)
    {
        if(!string.IsNullOrEmpty(ProfilePicture))
        {
            File.Delete(ProfilePicture);
        }
        ProfilePicture = url;
    }
    public void SetAlbumPath(string path)
    {
        Directory.CreateDirectory(path);
        Album = path;
    }
    public void DeleteProfilePicture() => ProfilePicture = default;
    public Result AddWorkTime(WorkTime workTime)
    {
        if (_workTimes.Count >= 7)
        {
            return Result.Failure(400,
                new Error("WorkTime", "can not add more than 7 worktimes"));
        }
        if ((workTime.End - workTime.Start) > TimeSpan.FromHours(8))
        {
            return Result.Failure(400,
                new Error("WorkTime", "Can not accept more than 8 working hours"));
        }
        return _workTimes.Add(workTime) ? 
            Result.Success() 
           : Result.Failure(400,new Error("WorkTime", $"{workTime}\nthis worktime already exsits "));
    }
    public Result UpdateWorkTime(List<WorkTime> workTimes)
    {
        if (workTimes.Count > 7)
        {
            return Result.Failure(400,
            new Error("WorkTime", "can not add more than 7 worktimes"));
        }
        _workTimes.Clear();
        foreach (var workTime in workTimes)
        {
            var result = AddWorkTime(workTime);
            if (result.IsFailure)
            {
                return result;
            }
        }
        return Result.Success();
    }
    public Result Update(string name,
        string? description,
        List<WorkTime> workTimes,
        decimal latituade,
        decimal longituade,
        string city,
        string state,
        string street,
        string? addressDescription)
    {
        var errors = new List<Error>();

        var locationResult = Location
            .Update(latituade, longituade);

        if (locationResult.IsFailure)
            errors.AddRange(locationResult.Errors);

        var worktimeResult = UpdateWorkTime(workTimes);
        if (worktimeResult.IsFailure)
            errors.AddRange(worktimeResult.Errors);
        var addressResult = Address.Update(city,
            state,
            street,
            addressDescription);
        if (addressResult.IsFailure)
            errors.AddRange(addressResult.Errors);
        if (string.IsNullOrWhiteSpace(name))
        {
             errors.Add( new Error("name"
                , "Invalid Value: can not accept null or white spaces"));
        }
        if (errors.Any())
        {
            return Result.Failure(400, errors.ToArray());
        }

        Name = name;
        Description = description;
        LastModefiedAt = DateTime.UtcNow;

        return Result.Success();
    }

  



}
