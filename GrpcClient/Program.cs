using Grpc.Net.Client;
using GrpcServer;
using GrpcServer.Protos;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var input = new HelloRequest { Name = "Adesina" };

            var channel = GrpcChannel.ForAddress("https://localhost:7184");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(input);
            Console.WriteLine(reply.Message); 

            var customerClient = new Customer.CustomerClient(channel);
            var clientRequested = new CustomerLookUpModel { UserId = 1 };
            var customer = await customerClient.GetCustomerInfoAsync(clientRequested);
            Console.WriteLine($"{customer.FirstName} {customer.FirstName}");

            using (var call = customerClient.GetNewCustomer(new NewCustomerRequest()))
            {
                while(await call.ResponseStream.MoveNext(new CancellationToken()))
                {
                    var currentCustomer = call.ResponseStream.Current;
                    Console.WriteLine($"{currentCustomer.FirstName}");
                }
            }

            Console.ReadLine();
        }
    }
}