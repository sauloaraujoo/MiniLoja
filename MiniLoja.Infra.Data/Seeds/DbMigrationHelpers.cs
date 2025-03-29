using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniLoja.Domain.Entities;
using MiniLoja.Infra.Data.Context;

namespace MiniLoja.Infra.Data.Seeds
{
    public static class DbMigrationHelperExtension
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            DbMigrationHelpers.EnsureSeedData(app).Wait();
        }
    }

    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var context = scope.ServiceProvider.GetRequiredService<MiniLojaContext>();

            if (env.IsDevelopment())
            {
                await context.Database.MigrateAsync();

                await EnsureSeedCategorias(context);
                await EnsureSeedProdutos(context);
            }
        }

        private static async Task EnsureSeedCategorias(MiniLojaContext context)
        {
            if (context.Categorias.Any())
                return;

            var categorias = new List<Categoria>
            {
                new Categoria { Nome = "Drone", Descricao = "Drones DJI e outros" },
                new Categoria { Nome = "Celular", Descricao = "Celulares Samsung, Iphone e Motorola" },

            };

            await context.Categorias.AddRangeAsync(categorias);
            await context.SaveChangesAsync();
        }

        private static async Task EnsureSeedProdutos(MiniLojaContext context)
        {
            if (context.Produtos.Any())
                return;

            var categorias = await context.Categorias.ToListAsync();

            var categoriaDrone = categorias.FirstOrDefault(c => c.Nome == "Drone");
            var categoriaCelular = categorias.FirstOrDefault(c => c.Nome == "Celular");

            if (categoriaDrone == null || categoriaCelular == null)

                if (categoriaDrone == null || categoriaCelular == null)
                return;

            var produtos = new List<Produto>
            {
                new Produto
                {
                    Nome = "S25 Ultra",
                    Descricao = "Smartphone Samsung S25 Ultra 256GB",
                    Imagem = "",
                    Preco = 7999.99M,
                    QtdEstoque = 50,
                    CategoriaId = categoriaCelular.Id
                },
                new Produto
                {
                    Nome = "Drone DJI Mini 4 Pro",
                    Descricao = "Drone DJI Mini 4 Pro Fly More Combo (Com tela)",
                    Imagem = "",
                    Preco = 10490,
                    QtdEstoque = 20,
                    CategoriaId = categoriaDrone.Id
                }
            };

            await context.Produtos.AddRangeAsync(produtos);
            await context.SaveChangesAsync();
        }
    }
}
