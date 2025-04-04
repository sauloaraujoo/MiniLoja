using MiniLoja.Api.Configurations;
using MiniLoja.Infra.Data.Seeds;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiConfig()
       .AddCorsConfig()
       .AddSwaggerConfig()
       .AddDbContextConfig()
       .AddIdentityConfig();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("Development");
}
else
{
    app.UseCors("Production");
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseDbMigrationHelper();

app.Run();
