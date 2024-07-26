using System.Diagnostics.CodeAnalysis;
using TodoList.Api.Extensions;
using TodoList.Application.Extensions;
using TodoList.Infrastructure.Extensions;

// Create host
var builder = WebApplication.CreateBuilder(args);

// Add the API related services to the container.
builder.Services.AddApiServices(builder.Configuration);

// Add the application related services to the container.
builder.Services.AddApplicationServices();

// Add the infrastructure related services to the container.
builder.Services.AddInfrastructure(builder.Configuration);

// Build app
var app = builder.Build();

// Configure the 
app.ConfigureApi(builder.Configuration, builder.Environment);

// Run app
await app.RunAsync();

// Partial class added for integration testing
// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0
namespace TodoList.Api
{
    [ExcludeFromCodeCoverage(Justification = "Wiring")]
    public partial class Program { }
}
