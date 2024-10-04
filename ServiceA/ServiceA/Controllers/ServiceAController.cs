using Grpc.Net.Client;
using GrpcService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ServiceA.Controllers;

[ApiController]
[Route("[controller]")]
public class ServiceAController : ControllerBase
{
    private readonly IConfiguration Configuration;
    public ServiceAController(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    [HttpGet("sayhello/{name}")]
    public async Task<IActionResult> SayHello(string name)
    {
        var httpHandler = new HttpClientHandler
        {
            // For local development only - allows insecure HTTP/2
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        string serviceBUrl = Configuration["ServiceB:BaseUrl"] ?? string.Empty;
        using var channel = GrpcChannel.ForAddress(serviceBUrl, new GrpcChannelOptions
        {
            HttpHandler = httpHandler
        });
        var client = new Greeter.GreeterClient(channel);

        var reply = await client.SayHelloAsync(new HelloRequest { Name = name });

        return Ok(reply.Message);
    }
}