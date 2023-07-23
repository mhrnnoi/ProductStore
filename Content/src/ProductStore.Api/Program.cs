using Microsoft.EntityFrameworkCore;
using ProductStore.Api;
using ProductStore.Api.Middlewares;
using ProductStore.Application;
using ProductStore.Infrastructure;
using ProductStore.Infrastructure.Persistence.DataContext;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddApplication()
                .AddInfrastructure(config)
                .AddPresentation();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<IsValidAuthenticated>();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    dbContext.Database.Migrate();
}
app.Run();
