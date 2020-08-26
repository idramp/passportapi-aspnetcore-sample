
namespace AspNetDemo.Models
{
    public class ApiConfig
    {
        public string BearerToken { get; set; }
        public string ApiEndpointUrl { get; internal set; }
    }
}
