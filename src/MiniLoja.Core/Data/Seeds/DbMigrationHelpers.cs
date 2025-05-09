﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniLoja.Core.Domain.Entities;
using MiniLoja.Core.Data.Context;

namespace MiniLoja.Core.Data.Seeds
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

                await EnsureSeedVendedorAsync(context, scope.ServiceProvider);
                await EnsureSeedCategoriasAsync(context);
                await EnsureSeedProdutosAsync(context);
            }
        }

        private static async Task EnsureSeedVendedorAsync(MiniLojaContext context, IServiceProvider serviceProvider)
        {
            //if (context.Users.Any())
            //    return;

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var email = "admin@miniloja.com";
            var usarAdmin = await userManager.FindByEmailAsync(email);

            if (usarAdmin != null)
                return;

            var identityUser = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(), 
                UserName = "adminloja",
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(identityUser, "Abcd1234!");

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Erro ao criar usuário: {errors}");
            }

            var vendedor = new Vendedor
            {
                AspnetUserId = identityUser.Id
            };

            await context.Vendedores.AddAsync(vendedor);
            await context.SaveChangesAsync();
        }

        private static async Task EnsureSeedCategoriasAsync(MiniLojaContext context)
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

        private static async Task EnsureSeedProdutosAsync(MiniLojaContext context)
        {
            if (context.Produtos.Any())
                return;

            var categorias = await context.Categorias.ToListAsync();

            var categoriaDrone = categorias.FirstOrDefault(c => c.Nome == "Drone");
            var categoriaCelular = categorias.FirstOrDefault(c => c.Nome == "Celular");

            if (categoriaDrone == null || categoriaCelular == null)

                if (categoriaDrone == null || categoriaCelular == null)
                return;

            var vendedor = await context.Vendedores.FirstOrDefaultAsync();
            if (vendedor == null)
                return;

            var produtos = new List<Produto>
            {
                new Produto
                {
                    Nome = "S25 Ultra",
                    Descricao = "Smartphone Samsung S25 Ultra 256GB",
                    Imagem = "Imagens/1d5820a5-fc67-4e31-850b-ba13fd4e2276.jpg",
                    Preco = 7999.99M,
                    QtdEstoque = 50,
                    CategoriaId = categoriaCelular.Id,
                    VendedorId = vendedor.Id
                },
                new Produto
                {
                    Nome = "Drone DJI Mini 4 Pro",
                    Descricao = "Drone DJI Mini 4 Pro Fly More Combo (Com tela)",
                    Imagem = "Imagens/76aec863-6a89-4060-acae-fa4d9a82f5d7.png",
                    Preco = 10490,
                    QtdEstoque = 20,
                    CategoriaId = categoriaDrone.Id,
                    VendedorId = vendedor.Id
                }
            };

            await context.Produtos.AddRangeAsync(produtos);
            await context.SaveChangesAsync();
        }
    }
}
