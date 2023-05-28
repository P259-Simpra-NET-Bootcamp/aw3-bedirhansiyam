using AutoMapper;
using SimApi.Data;

namespace SimApi.Schema;

public class MapperProfile : Profile 
{
    public MapperProfile()
    {
        CreateMap<Category, CategoryResponse>()
            .ForMember(dest => dest.Products, opt => opt
            .MapFrom(src => src.Products
            .Select(x => x.Name)));
        CreateMap<CategoryRequest, Category>();

        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.Category, opt => opt
            .MapFrom(src => src.Category.Name));
        CreateMap<ProductRequest, Product>();
    }
}
