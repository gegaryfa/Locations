using System.Threading.Tasks;
using Locations.Core.Domain.Entities;

namespace Locations.Core.Application.Interfaces.Repositories
{
    public interface IProductRepositoryAsync : IGenericRepositoryAsync<Product>
    {
        Task<bool> IsUniqueBarcodeAsync(string barcode);
    }
}
