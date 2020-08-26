using System;
using System.Threading.Tasks;
using PassportApi;

namespace AspNetDemo.Services
{
    public class RevocableCredentialApiService
    {
        private readonly swaggerClient _client;
        public RevocableCredentialApiService(swaggerClient client)
        {
            _client = client;
        }

        public Task<CredentialState> GetCredentialState(string id)
        {
            return _client.GetCredentialStatusAsync(id);
        }

        public async Task<CredentialOfferModel> IssueRoleCredential(string connectionId, string role)
        {
            string emailCredDefId = await GetRoleCredDefId();
            return await _client.CreateCredentialAsync(new CreateCredentialOfferModel
            {
                ConnectionId = connectionId,
                CredentialDefinitionId = emailCredDefId,
                CredentialName = "Role",
                Values = new AttributeValue[] {
                    new AttributeValue ()
                    {
                        Name = "Role",
                        Value = role
                    }
                }
            });
        }

        public async Task RevokeRoleCredential(string credentialId)
        {
            await _client.RevokeCredentialAsync(credentialId);
        }

        private string _roleCredDefId = null;
        private async Task<string> GetRoleCredDefId()
        {
            if (_roleCredDefId == null)
            {
                _roleCredDefId = FileStorage.GetRoleCredDefIdFromFile();
                if (_roleCredDefId == null)
                {
                    _roleCredDefId = await CreateRoleCredDef();
                    FileStorage.StoreRoleCredDefIdToFile(_roleCredDefId);
                }
            }
            return _roleCredDefId;
        }

        private async Task<string> CreateRoleCredDef()
        {
            IdModel result = await _client.CreateCredentialDefinitionAsync(new CreateCredentialDefinitionModel
            {
                SchemaId = "ATfEGD9UJ2pzunx9LmoE4f:2:RC:1.0",
                Tag = "role-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
                EnableRevocation = true
            });
            return result.Id;
        }
    }
}
