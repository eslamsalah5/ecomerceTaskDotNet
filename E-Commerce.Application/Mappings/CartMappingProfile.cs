using AutoMapper;
using E_Commerce.Application.DTOs;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Mappings
{
    public class CartMappingProfile : Profile
    {
        public CartMappingProfile()
        {
            // Cart -> CartDto
            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src =>
                    src.Items.Sum(item => item.Quantity * item.UnitPriceAtAdd)))
                .ForMember(dest => dest.DiscountTotal, opt => opt.MapFrom(src =>
                    src.Items.Sum(item => item.Quantity * item.UnitPriceAtAdd * (item.DiscountPercentageAtAdd ?? 0) / 100m)))
                .ForMember(dest => dest.GrandTotal, opt => opt.MapFrom(src =>
                    src.Items.Sum(item => item.Quantity * item.UnitPriceAtAdd * (1 - (item.DiscountPercentageAtAdd ?? 0) / 100m))))
                .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.Items.Sum(i => i.Quantity)));

            // CartItem -> CartItemDto
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(src => src.Product.ProductCode))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.Product.ImagePath) ? $"/{src.Product.ImagePath}" : null))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPriceAtAdd))
                .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.DiscountPercentageAtAdd))
                .ForMember(dest => dest.LineTotal, opt => opt.MapFrom(src =>
                    src.Quantity * src.UnitPriceAtAdd * (1 - (src.DiscountPercentageAtAdd ?? 0) / 100m)));
        }
    }
}
