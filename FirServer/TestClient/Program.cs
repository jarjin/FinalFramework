// See https://aka.ms/new-console-template for more information
using FirCommon.Utility;
using Grpc.Net.Client;
using GrpcGateway;
using msg1;

var channel = GrpcChannel.ForAddress("http://localhost:5000");

var client = new Gateway.GatewayClient(channel);
var reqData = new ReqMsg() { Name = "aaa" };
var request = new HelloRequest { Name = "World", ByteData = reqData.Serialize() };

if (request != null)
{
    var reply = await client.ReqGatewayAsync(request);
    Console.WriteLine("Gateway: " + reply.Name);
}
