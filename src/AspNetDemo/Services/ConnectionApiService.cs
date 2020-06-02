using PassportApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetDemo.Services
{
    public class ConnectionApiService
    {
        private readonly PassportApi.swaggerClient _client;
        public ConnectionApiService(
            PassportApi.swaggerClient client)
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
