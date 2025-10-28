using AutoMapper;
using E_Commerce.Application.DTOs;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            // Product -> ProductDto
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.ImagePath) ? $"/{src.ImagePath}" : null))
                .ForMember(dest => dest.FinalPrice, opt => opt.MapFrom(src =>
                    src.Price * (1 - (src.DiscountPercentage ?? 0) / 100m)));

            // CreateProductDto -> Product
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.ImagePath, opt => opt.Ignore()) // Handled by ImageService
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            // UpdateProductDto -> Product (for updates)
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.ImagePath, opt => opt.Ignore()) // Handled by ImageService
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
