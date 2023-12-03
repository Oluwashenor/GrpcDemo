using Grpc.Core;
using GrpcServer.Protos;

namespace GrpcServer.Services
{
    public class CustomerService : Customer.CustomerBase
    {
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ILogger<CustomerService> logger)
        {
            _logger = logger;
        }

        public override Task<CustomerModel> GetCustomerInfo(CustomerLookUpModel request, ServerCallContext context)
        {
           CustomerModel output = new CustomerModel();
            if(request.UserId == 1)
            {
                output.FirstName = "Jamie";
                output.LastName = "Smith";
            }
            else if(request.UserId == 2)
            {
                output.FirstName = "Jane";
                output.LastName = "Doe";
            }
            else
            {
                output.FirstName = "Greg";
                output.LastName = "Thomas";
            }

            return Task.FromResult(output);
        }

        public override async Task GetNewCustomer(
            NewCustomerRequest request,
            IServerStreamWriter<CustomerModel> responseStream,
            ServerCallContext context)
        {
            List<CustomerModel> customers = new List<CustomerModel>()
            {
                new CustomerModel
                {
                    FirstName = "Tim",
                    LastName = "Corey",
                    EmailAddress = "tim@correy.com",
                    Age = 41,
                    IsAlive = true,
                },
                new CustomerModel
                {
                    FirstName = "Adesina",
                    LastName = "Adebayo",
                    EmailAddress = "uncleshenor@gmail.com",
                    Age = 26,
                    IsAlive = true,
                }
            };

            foreach (var item in customers)
            {
              // await Task.Delay(1000);
               await responseStream.WriteAsync(item);
            }
        }
    }
}
