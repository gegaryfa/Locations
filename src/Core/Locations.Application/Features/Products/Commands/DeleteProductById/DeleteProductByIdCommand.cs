using System.Threading;
using System.Threading.Tasks;
using Locations.Core.Application.Exceptions;
using Locations.Core.Application.Interfaces.Repositories;
using Locations.Core.Application.Wrappers;
using MediatR;

namespace Locations.Core.Application.Features.Products.Commands.DeleteProductById
{
    public class DeleteProductByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, Response<int>>
        {
            private readonly IProductRepositoryAsync _productRepository;
            public DeleteProductByIdCommandHandler(IProductRepositoryAsync productRepository)
            {
                _productRepository = productRepository;
            }
            public async Task<Response<int>> Handle(DeleteProductByIdCommand command, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetByIdAsync(command.Id);
                if (product == null) throw new ApiException($"Product Not Found.");
                await _productRepository.DeleteAsync(product);
                return new Response<int>(product.Id);
            }
        }
    }
}
