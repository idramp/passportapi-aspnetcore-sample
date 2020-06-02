using PassportApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetDemo.Services
{
    public class ProofApiService
    {
        private readonly PassportApi.swaggerClient _client;
        public ProofApiService(
            PassportApi.swaggerClient client)
        {
            _client = client;
        }


        public Task<ProofStateModel> GetProof(string proofId)
        {
            return _client.GetProofAsync(proofId);
        }

        public async Task<ProofRequestModel> GetEmailProof(string connectionId = null)
        {
            string emailProofId = await GetEmailProofId();
            return await _client.CreateProofAsync(new CreateProofRequestModel
            {
                ConnectionId = connectionId,
                ProofConfigId = emailProofId
            });
        }

        public Task<ProofState> GetProofState(string proofId)
        {
            return _client.GetProofStatusAsync(proofId);
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
                    FileStorage.StoreEmailProofIdFromFile(_emailProofId);
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
