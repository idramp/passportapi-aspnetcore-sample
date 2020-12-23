using System.Threading.Tasks;
using PassportApi;

namespace AspNetDemo.Services
{
    public class ConnectionApiService
    {
        private readonly swaggerClient _client;

        public ConnectionApiService(swaggerClient client)
        {
            _client = client;
        }

        public Task<ConnectionStateModel> GetConnection(string connectionId)
        {
            return _client.GetConnectionAsync(connectionId);
        }

        public Task<ConnectionOfferModel> CreateConnection()
        {
            return _client.CreateConnectionAsync(new CreateConnectionModel
            {
                AliasName = "Demo"
            });
        }
    }
}
