using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace MiniLoja.App.Configurations
{
    public static class GlobalizationConfig
    {
        public static IApplicationBuilder AddGlobalizationConfig(this IApplicationBuilder app)
        {
            var defaultCulture = new CultureInfo("pt-BR");
            var supportedCultures = new[] { defaultCulture };

            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            app.UseRequestLocalization(localizationOptions);

            return app;
        }
    }
}
