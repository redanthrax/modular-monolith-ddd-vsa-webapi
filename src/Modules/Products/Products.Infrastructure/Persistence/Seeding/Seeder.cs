using Common.InterModuleRequests.Contracts;
using Common.InterModuleRequests.IAM;
using Products.Application.Persistence;

namespace Products.Infrastructure.Persistence.Seeding;

internal sealed partial class Seeder(
    IInterModuleRequestClient<GetSeedUserIdsRequest, GetSeedUserIdsResponse> requestClient,
    IProductsDbContext dbContext
    )
{
    /// <summary>
    /// Keep <see cref="ProductCount"/> divisible by <see cref="StoreCount"/>
    /// </summary>
    private const int StoreCount = 2;
    private const int ProductCount = 4;
    public async Task SeedDbAsync(CancellationToken cancellationToken = default)
    {
        var storeIds = await SeedStoresAsync(cancellationToken);
        var productIds = await SeedProductTemplatesAsync(cancellationToken);

        var storeProductCountPerStore = ProductCount / StoreCount;

        await SeedProductAsync(storeIds, productIds, storeProductCountPerStore, cancellationToken);
    }
}
