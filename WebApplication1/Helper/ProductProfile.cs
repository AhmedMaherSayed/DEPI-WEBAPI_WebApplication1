using AutoMapper;
using WebApplication1.Dtos.ProductDtos;
using WebApplication1.Entities;

namespace WebApplication1.Helper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, GetProductResponse>()
                .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Name));
        }
    }
}
