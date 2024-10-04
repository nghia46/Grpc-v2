using GrpcServiceB.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();


// Log environment detection
// builder.WebHost.ConfigureKestrel(options =>
// {
//     options.ListenAnyIP(5050, listenOptions =>
//     {
//         listenOptions.Protocols = HttpProtocols.Http2; // HTTP/2 for gRPC
//     });

// });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/v1/swagger.json", "ServiceB");
    c.RoutePrefix = string.Empty;  // Swagger at root URL
});

app.UseSwagger(options => { options.RouteTemplate = "{documentName}/swagger.json"; });

// Map gRPC service
app.MapGrpcService<GreeterService>();

// Routing for Web API
app.UseRouting();


app.UseAuthorization();

// Map Web API controllers
app.MapControllers();

app.Run();
