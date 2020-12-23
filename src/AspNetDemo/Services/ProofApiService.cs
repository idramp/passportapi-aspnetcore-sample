using System.Collections.Generic;
using System.Threading.Tasks;
using IdRamp.Passport;


namespace AspNetDemo.Services
{
    public class ProofApiService
    {
        private readonly PassportApiClient _client;

        public ProofApiService(PassportApiClient client)
        {
            _client = client;
        }

        public Task<ProofStateModel> GetProof(string proofId)
        {
            return _client.GetProofAsync(proofId, verify: true); // TODO : verify param switches to VerifyOption enum
        }

        public Task<ProofState> GetProofState(string proofId)
        {
            return _client.GetProofStatusAsync(proofId);
        }

        public async Task<CreateProofRequestResultModel> GetEmailProof(string connectionId = null)
        {
            string emailProofId = await GetEmailProofId();
            return await _client.CreateProofAsync(new CreateProofRequestModel
            {
                ConnectionId = connectionId,
                ProofConfigId = emailProofId
            });
        }

        private string _emailProofId = null;
        private async Task<string> GetEmailProofId()
        {
            if (_emailProofId == null)
            {
                _emailProofId = FileStorage.GetEmailProofIdFromFile();
                if (_emailProofId == null)
                {
                    _emailProofId = await CreateEmailProofConfig();
                    FileStorage.StoreEmailProofIdToFile(_emailProofId);
                }
            }
            return _emailProofId;
        }

        private async Task<string> CreateEmailProofConfig()
        {
            ProofModel model = new ProofModel() { Name = "Email Request" };

            ICollection<AttributeRestrictions> emailRestrictions = new AttributeRestrictions[]
            {
                new AttributeRestrictions()
                {
                    SchemaId = "WxM17SNiPaD8mKL5V5ertw:2:Verified Email:1.0"
                }
            };

            model.Attributes = new List<ProofAttributeModel>
            {
                new ProofAttributeModel() { Id = "aaaed304-f8c9-4e87-9cda-6c6a499a9d24", Name = "Email", Restrictions = emailRestrictions }
            };
            IdModel result = await _client.CreateProofConfigAsync(model);
            return result.Id;
        }
    }
}
