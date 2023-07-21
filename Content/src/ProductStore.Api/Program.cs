using Microsoft.EntityFrameworkCore;
using ProductStore.Api;
using ProductStore.Application;
using ProductStore.Infrastructure;
using ProductStore.Infrastructure.Persistence.DataContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication()
                .AddInfrastructure()
                .AddPresentation();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    
    dbContext.Database.Migrate();


}
app.Run();
