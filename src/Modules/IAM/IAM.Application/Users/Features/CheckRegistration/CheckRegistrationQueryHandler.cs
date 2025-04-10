using Common.Domain.ResultMonad;
using Common.Application.CQS;
using IAM.Application.Persistence;
using Common.Application.Persistence;

namespace IAM.Application.Users.Features.CheckRegistration;

public sealed class CheckRegistrationQueryHandler(IIAMDbContext dbContext) : IQueryHandler<CheckRegistrationQuery, bool>
{
    public async Task<Result<bool>> Handle(CheckRegistrationQuery request, CancellationToken cancellationToken)
        => await dbContext
                .Users
                .TagWith(nameof(CheckRegistrationQueryHandler))
                .Where(u => u.PhoneNumber == request.PhoneNumber)
                .AnyAsResultAsync(cancellationToken);
}
