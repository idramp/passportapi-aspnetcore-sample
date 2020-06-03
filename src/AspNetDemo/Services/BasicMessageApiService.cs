using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PassportApi;

namespace AspNetDemo.Services
{
    public class BasicMessageApiService
    {
        private readonly PassportApi.swaggerClient _client;
        public BasicMessageApiService(
            PassportApi.swaggerClient client)
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
