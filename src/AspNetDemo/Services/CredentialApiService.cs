using PassportApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetDemo.Services
{
    public class CredentialApiService
    {
        private readonly PassportApi.swaggerClient _client;
        public CredentialApiService(
            PassportApi.swaggerClient client)
        {
            _client = client;
        }

        public Task<CredentialState> GetCredentialState(string id)
        {
            return _client.GetCredentialStatusAsync(id);
        }

        public async Task<CredentialOfferModel> IssueEmailCredential(string connectionId, string emailAddress)
        {
            string emailCredDefId = await GetEmailCredDefId();
            return await _client.CreateCredentialAsync(new CreateCredentialOfferModel
            {
                ConnectionId = connectionId,
                CredentialDefinitionId = emailCredDefId,
                CredentialName = "Email",
                Values = new AttributeValue[] { 
                    new AttributeValue ()
                    {
                        Name = "Email",
                        Value = emailAddress
                    } 
                }
            });
        }

        private string _emailCredDefId = null;
        private async Task<string> GetEmailCredDefId()
        {
            if (_emailCredDefId == null)
            {
                _emailCredDefId = FileStorage.GetEmailCredDefIdFromFile();
                if (_emailCredDefId == null)
                {
                    _emailCredDefId = await CreateEmailCredDef();
                    FileStorage.StoreEmailCredDefIdToFile(_emailCredDefId);
                }
            }
            return _emailCredDefId;
        }

        private async Task<string> CreateEmailCredDef()
        {
            IdModel result = await _client.CreateCredentialDefinitionAsync(new CreateCredentialDefinitionModel
            {
                SchemaId = "WxM17SNiPaD8mKL5V5ertw:2:Verified Email:1.0",
                Tag = "email-"+ DateTime.UtcNow.ToString("yyyyMMddHHmmss")
            });
            return result.Id;
        }
    }
}
