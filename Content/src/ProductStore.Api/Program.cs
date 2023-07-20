using ProductStore.Api;
using ProductStore.Application;
using ProductStore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication()
                .AddInfrastructure()
                .AddPresentation();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
