using Grpc.Net.Client;
using GrpcService;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]/api")]
public class BaseController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public BaseController(IConfiguration configuration)
    {
        _configuration = configuration;
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
        string serviceAUrl = _configuration["ServiceA:BaseUrl"] ?? string.Empty;

        // Create gRPC channel to ServiceB
        using var channel = GrpcChannel.ForAddress(serviceAUrl, new GrpcChannelOptions
        {
            HttpHandler = httpHandler
        });

        // Create gRPC client for ServiceB
        var client = new Greeter.GreeterClient(channel);

        // Call the SayHello method on ServiceB and receive the response
        var response = await client.SayHelloAsync(new HelloRequest { Name = name });

        return Ok(response.Message + " from ServiceB");
    }
}