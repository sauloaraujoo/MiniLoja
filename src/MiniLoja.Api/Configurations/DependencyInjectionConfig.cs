using MiniLoja.Core.Business.Categorias;
using MiniLoja.Core.Business.Produtos;
using MiniLoja.Core.Business.Vendedores;
using MiniLoja.Core.Data.Repository;

namespace MiniLoja.Api.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static WebApplicationBuilder AddDependencyInjection(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IVendedorRepository, VendedorRepository>();
            builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

            return builder;
        }
    }
}
