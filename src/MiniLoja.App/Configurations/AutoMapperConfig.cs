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
            CreateMap<Produto, ProdutoViewModel>().ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Categoria.Nome));
            CreateMap<ProdutoViewModel, Produto>();
        }
    }
}
