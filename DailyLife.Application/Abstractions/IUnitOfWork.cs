namespace DailyLife.Application.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveIdentityChangesAsync(CancellationToken cancellationToken);
    Task<int> SaveBusinessChangesAsync(CancellationToken cancellationToken);
}
