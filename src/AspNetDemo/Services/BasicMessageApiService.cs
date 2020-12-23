using System.Collections.Generic;
using System.Threading.Tasks;
using PassportApi;


namespace AspNetDemo.Services
{
    public class BasicMessageApiService
    {
        // TODO jmason : Figure out how to control the name of this client by passing params to the nswag code generation toolchain
        private readonly swaggerClient _client;
        public BasicMessageApiService(PassportApi.swaggerClient client)
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
