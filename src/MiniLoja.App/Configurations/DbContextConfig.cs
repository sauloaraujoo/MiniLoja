using Microsoft.EntityFrameworkCore;
using MiniLoja.Core.Data.Context;
using System;

namespace MiniLoja.App.Configurations
{
    public static class DbContextConfig
    {
        public static IServiceCollection AddDbContextConfig(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<MiniLojaContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}
