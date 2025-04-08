using Microsoft.EntityFrameworkCore;
using MiniLoja.Infra.Data.Context;

namespace MiniLoja.Api.Configurations
{
    public static class DbContextConfig
    {
        public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<MiniLojaContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            return builder;
        }
    }
}
