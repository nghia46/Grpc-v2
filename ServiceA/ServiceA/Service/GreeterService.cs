using Grpc.Core;
using GrpcService;

public class GreeterService : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Xin chào từ ServiceA! " + request.Name
        });
    }
}