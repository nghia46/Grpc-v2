using Grpc.Net.Client;
using GrpcService;
using Microsoft.AspNetCore.Mvc;

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
    [HttpPost("sayhello/{name}")]
    public async Task<IActionResult> SayHello(string name)
    {
        var httpHandler = new HttpClientHandler
        {
            // For local development only - allows insecure HTTP/2
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        // Get the base URL for ServiceB from configuration
        string serviceBUrl = Configuration["ServiceB:BaseUrl"];
        
        // Tạo kênh gRPC đến ServiceB
        using var channel = GrpcChannel.ForAddress(serviceBUrl, new GrpcChannelOptions
        {
            HttpHandler = httpHandler
        });
        // Tạo client gRPC cho ServiceB
        var client = new Greeter.GreeterClient(channel);

        // Gọi phương thức SayHello trên ServiceB
        // và nhận phản hồi
        var response = await client.SayHelloAsync(new HelloRequest { Name = name });

        return Ok(response.Message + " from ServiceA");
    }
}
