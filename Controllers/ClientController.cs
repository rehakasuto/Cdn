using PointrCdn.Service;
using System;
using System.Web.Http;

using System.Web.Script.Serialization;

namespace PointrCdn.Controllers
{
    public class ClientController : ApiController
    {
        // POST: api/Authorize
        [Route("client/{connectionString}")]
        public string Post(string connectionString)
        {
            try
            {
                using (ClientService clientService = new ClientService())
                {
                    string authKey = clientService.AuthenticateClient(connectionString);
                    return new JavaScriptSerializer().Serialize(new { authKey = authKey });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
