using AutoMapper;
using MiniLoja.App.Models;
using MiniLoja.Core.Domain.Entities;

namespace MiniLoja.App.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Categoria, CategoriaViewModel>().ReverseMap();
            CreateMap<Produto, ProdutoViewModel>()
                .ForMember(dest => dest.Imagem, opt => opt.Ignore()) 
                .ForMember(dest => dest.ImagemPath, opt => opt.MapFrom(src => src.Imagem))
                .ReverseMap()
                .ForMember(dest => dest.Imagem, opt => opt.Ignore());
        }
    }
}
