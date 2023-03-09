// See https://aka.ms/new-console-template for more information
using FirCommon.Define;
using FirCommon.Utility;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcGateway;
using msg1;

var channel = GrpcChannel.ForAddress("http://localhost:5203");

var client = new Gateway.GatewayClient(channel);

var reqConn = new ConnectRequest()
{
    Uid = "100",
    Name = "aaa",
    Message = "HelloWorld!"
};
var connect = client.ReqConnect();
await connect.RequestStream.WriteAsync(reqConn);

var recvTask = Task.Run(async () =>
{
	try
	{
		await foreach (var reply in connect.ResponseStream.ReadAllAsync(CancellationToken.None))
		{
            await HandleResponse(reply); 
        }
	}
    catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
    {
		Console.WriteLine($"Connection broken");
	}
	catch (RpcException ex)
    {
		Console.WriteLine($"Error {ex.InnerException?.Message}");
	}
});

async Task HandleResponse(HelloReply reply)
{
	switch (reply.ProtoName)
	{
		case Protocal.Connected:
			await OnConnected(reply.ByteData);
			break;
		case Protocal.ResLogin:
			await OnLoginOK(reply.ByteData);
			break;
	}
}
recvTask.Wait();

async Task OnConnected(Google.Protobuf.ByteString byteData)
{
    var connReply = ConnectReply.Parser.ParseFrom(byteData);
    Console.WriteLine($"Hello OnConnected! Token:{connReply.Token}");

    var reqData = new ReqMsg() { Name = "aaa" };
    var request = new HelloRequest { 
		Token = connReply.Token, 
		ProtoName = Protocal.ReqLogin,
		ByteData = reqData.Serialize() 
	};
    var gateway = client.ReqGateway();
    await gateway.RequestStream.WriteAsync(request);
}

async Task OnLoginOK(Google.Protobuf.ByteString byteData)
{
	var reply = ResMsg.Parser.ParseFrom(byteData);
	Console.WriteLine("Login Message: " + reply.Message);
	await Task.CompletedTask;
}
