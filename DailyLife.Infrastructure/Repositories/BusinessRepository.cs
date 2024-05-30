using DailyLife.Domain.Aggregates;
using DailyLife.Domain.Repositories;
using DailyLife.Infrastructure.Data.Business;
using Microsoft.EntityFrameworkCore;

namespace DailyLife.Infrastructure.Repositories;

internal sealed class BusinessRepository
    : BaseRepository<BusinessAggregate>, IBusinessRepository
{
    private readonly BusinessDbContext _context;

    public BusinessRepository(BusinessDbContext context)
        : base(context)
    {
        _context = context;
    }


    public IQueryable<BusinessAggregate> GetActive()
    {
        var currentDateTime = DateTime.UtcNow;
        return _context.Businesses
            .FromSql($"""""
            SELECT b.*
            FROM [Business].[Businesses] b
            JOIN [Business].[WorkTime] wt ON b.[Id] = wt.[BusinessId]
            WHERE wt.[Day] = {currentDateTime.DayOfWeek}
            AND wt.[Start] <= CONVERT(TIME, GETDATE())
            AND wt.[End] >= CONVERT(TIME, GETDATE())     
            """"")
            .Include(b => b.Category)
            .AsNoTracking();
    }

    public override async Task<BusinessAggregate?> GetById(Id id)
    {
        return await _context.Businesses.Include(b => b.Category)
             .Include(b => b.WorkTimes)
             .FirstOrDefaultAsync(b => b.Id == id);
    }

    public IQueryable<BusinessAggregate> GetNearest(decimal currentLatitude, decimal currentLongitude, int skip, int take)
    {
        return _context.Businesses.FromSql($"""""
            SELECT * FROM [Business].[Businesses] b
            ORDER BY GEOGRAPHY::Point({currentLatitude}, {currentLongitude}, 4326).STDistance(GEOGRAPHY::Point(b.Location_Latituade, b.Location_Longituade, 4326))
            OFFSET {skip} ROWS
            FETCH NEXT {take} ROWS ONLY
            """"")
            .Include(b => b.Category)
            .AsNoTracking();
    }

    public async Task<List<BusinessAggregate>> GetTopRated(int skip, int take)
    {
       return await _context.Businesses.Include(b => b.Category)
            .Where(b => b.Reviews.Any())
            .OrderByDescending(b => b.Reviews.Count())
            .Skip(skip)
            .Take(take)
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<bool> IsNameUniqe(string name)
    {
        return !await _context.Businesses
            .AnyAsync(b =>
            b.Name.ToLower() == name.ToLower());
    }

    public async Task<bool> OwnerExist(Guid ownerId)
    {
        return await _context.Businesses
            .AnyAsync(b => b.OwnerId == ownerId);
    }

    public IQueryable<BusinessAggregate> SearchByName(string searchTerm)
    {
        return _context.Businesses
            .FromSql<Business>($""""
            SELECT * FROM Business.Businesses AS b
            WHERE SOUNDEX(b.NAME) = SOUNDEX({searchTerm})
            OR b.NAME LIKE({'%'+searchTerm}+'%')
            """")
            .Include(b => b.Category)
            .AsNoTracking()
            .AsQueryable();
        
    }

}
