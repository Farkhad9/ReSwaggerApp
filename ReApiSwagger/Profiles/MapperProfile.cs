using AutoMapper;
using Microsoft.AspNetCore.Http;
using ReApiSwagger.Dtos.Categories;
using ReApiSwagger.Dtos.Products;
using ReApiSwagger.Dtos.User;
using ReApiSwagger.Models;

namespace ReApiSwagger.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile(IHttpContextAccessor httpContextAccessor)
        {
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();

           
            CreateMap<Category, CategoryReturnDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src =>
                    GetUrl(httpContextAccessor) + "CategoryImages/" + src.ImageUrl));

            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<Product, ProductReturnDto>();
            CreateMap<Category, CategoryInProductReturnDto>();
            CreateMap<CategoryInProductUpdateDto, Category>();














            CreateMap<UserRegistrDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.NickName));
            CreateMap<UserLoginDto, AppUser>()
                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.NickName));

        }

        private static string GetUrl(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext == null) return string.Empty;

            var uriBuilder = new UriBuilder
            {
                Scheme = httpContext.Request.Scheme,
                Host = httpContext.Request.Host.Host,
                Port = httpContext.Request.Host.Port ?? 80
            };

            return uriBuilder.Uri.AbsoluteUri;
        }
    }
}