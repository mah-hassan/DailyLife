using DailyLife.Application.Abstractions;
using DailyLife.Infrastructure.Data.Business;
using DailyLife.Infrastructure.Data.Identity;

namespace DailyLife.Infrastructure.Services
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly BusinessDbContext _businessDbContext;

        public UnitOfWork(AppIdentityDbContext context, BusinessDbContext businessDbContext)
        {
            _identityDbContext = context;
            _businessDbContext = businessDbContext;
        }

        public Task<int> SaveBusinessChangesAsync(CancellationToken cancellationToken)
        {
            return _businessDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveIdentityChangesAsync(CancellationToken cancellationToken)
        {
            return await _identityDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
