using MiniLoja.Business.Categorias;
using MiniLoja.Business.Produtos;
using MiniLoja.Business.Vendedores;
using MiniLoja.Infra.Data.Repository;

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
