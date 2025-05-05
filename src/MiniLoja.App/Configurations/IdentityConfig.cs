using Microsoft.AspNetCore.Identity;
using MiniLoja.Core.Data.Context;
using System;

namespace MiniLoja.App.Configurations
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>()
                     .AddEntityFrameworkStores<MiniLojaContext>();
            return services;
        }
    }
}
