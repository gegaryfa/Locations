using AutoMapper;
using Locations.Core.Application.Features.Products.Commands.CreateProduct;
using Locations.Core.Application.Features.Products.Queries.GetAllProducts;
using Locations.Core.Domain.Entities;

namespace Locations.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
        }
    }
}
