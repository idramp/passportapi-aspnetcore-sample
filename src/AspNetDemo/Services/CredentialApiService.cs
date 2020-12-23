using System;
using System.Threading.Tasks;
using IdRamp.Passport;


namespace AspNetDemo.Services
{
    /// <summary>
    /// Some wrapper code to help in interacting with the Credential-related methods of the Passport API.
    /// 
    /// Currently the "Email" credential is created with a hard-coded schema ID, but that could be changed to be retrieved from
    /// configuration. Be default, the tag is set to "email-" appended with a timestampe. The credential definition ID will be
    /// cached automatically by the <see cref="FileStorage"/> helpers, as creating one can take time since writing to the ledger
    /// is necessary.
    /// </summary>
    public class CredentialApiService
    {
        private readonly PassportApiClient _client;

        public CredentialApiService(PassportApiClient client)
        {
            _client = client;
        }

        public Task<CredentialState> GetCredentialState(string id)
        {
            return _client.GetCredentialStatusAsync(id);
        }

        public async Task<CredentialOfferModel> IssueEmailCredentialAsync(string connectionId, string emailAddress)
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
                Tag = "email-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss")
            });
            return result.Id;
        }
    }
}
