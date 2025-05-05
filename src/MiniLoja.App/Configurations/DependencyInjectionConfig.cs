using MiniLoja.Core.Business.Categorias;
using MiniLoja.Core.Business.Produtos;
using MiniLoja.Core.Business.Vendedores;
using MiniLoja.Core.Data.Repository;

namespace MiniLoja.App.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IVendedorRepository, VendedorRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();

            return services;
        }
    }
}
