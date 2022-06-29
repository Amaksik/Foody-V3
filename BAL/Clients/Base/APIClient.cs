using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Clients.Base
{
    public abstract class APIClient
    {
        public string Url;

        public string Token;

        public HttpClient httpClient;

    }
}
