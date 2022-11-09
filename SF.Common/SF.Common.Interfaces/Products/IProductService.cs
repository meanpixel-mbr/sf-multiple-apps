using Microsoft.ServiceFabric.Services.Remoting;
using SF.Common.Poco.Products;

namespace SF.Common.Interfaces.Products;

public interface IProductService : IService
{
	Task<List<Product>> GetAsync(CancellationToken cancellationToken);
	Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
