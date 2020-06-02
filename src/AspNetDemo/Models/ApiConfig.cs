using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetDemo.Models
{
    public class ApiConfig
    {
        public string BearerToken { get; set; }
        public string ApiEndpointUrl { get; internal set; }
    }
}
