using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using SF.Common.Interfaces.Products;
using SF.Common.Poco.Products;

namespace SF.Products.Service
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Service : StatelessService, IProductService
    {
        public Service(StatelessServiceContext context)
            : base(context)
        { }

        #region Service Fabric

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
	        return new[]
	        {
		        new ServiceInstanceListener(serviceContext => new FabricTransportServiceRemotingListener(serviceContext,
				        this,
				        new FabricTransportRemotingListenerSettings
				        {
					        UseWrappedMessage = true,
					        EndpointResourceName = "V2_1Listener"
				        }),
			        "V2_1Listener")
	        };
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

        #endregion

        #region Implementation of IProductService

        /// <inheritdoc />
        public async Task<List<Product>> GetAsync(CancellationToken cancellationToken)
        {
	        return await Task.FromResult(new List<Product>()
	        {
		        new Product() { Id = Guid.Parse("0fc1523b-1279-4f6f-a3b4-67623110519e"), Name = "Default Product" },
		        new Product() { Id = Guid.Parse("4c24bc4e-65b0-438e-ac27-88ee44fb0624"), Name = "Alternate Product" },
		        new Product() { Id = Guid.Parse("684bb39e-b7d1-4b85-a259-3c29bb7f8fa7"), Name = "Outdated Product" }
	        });
        }

        /// <inheritdoc />
        public async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
	        return await Task.FromResult(new Product()
	        {
		        Id = id, 
		        Name = "Default Product"
	        });
        }

        #endregion
    }
}
