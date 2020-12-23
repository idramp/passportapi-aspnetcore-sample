using System.Collections.Generic;
using System.Threading.Tasks;
using IdRamp.Passport;


namespace AspNetDemo.Services
{
    public class BasicMessageApiService
    {
        private readonly PassportApiClient _client;

        public BasicMessageApiService(PassportApiClient client)
        {
            _client = client;
        }

        public Task SendMessage(string connectionId, string message)
        {
            var body = new NewBasicMessage
            {
                ConnectionId = connectionId,
                Content = message
            };

            return _client.SendMessageAsync(body);
        }

        public Task<ICollection<BasicMessageDetail>> GetMessages(string connectionId)
        {
            return _client.GetMessagesAsync(connectionId);
        }
    }
}
