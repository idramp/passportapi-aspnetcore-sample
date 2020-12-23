using System.Threading.Tasks;
using IdRamp.Passport;


namespace AspNetDemo.Services
{
    public class ConnectionApiService
    {
        private readonly PassportApiClient _client;

        public ConnectionApiService(PassportApiClient client)
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
