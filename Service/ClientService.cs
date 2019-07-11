using PointrCdn.Models;
using System;
using System.Linq;

namespace PointrCdn.Service
{
    public class ClientService : IDisposable
    {
        public string AuthenticateClient(string connectionString)
        {
            string authKey = "";
            using (CdnContext context = new CdnContext())
            {
                if (string.IsNullOrEmpty(connectionString))
                    throw new ArgumentNullException(nameof(connectionString));

                Client client = context.clients.Where(x => x.connectionString.Trim().Equals(connectionString.Trim())).FirstOrDefault();
                if (client == null)
                    throw new NullReferenceException("Connection string is not match with any client.");
                authKey = Guid.NewGuid().ToString();

                client.name = "client - " + authKey;
                client.authKey = authKey;
                context.SaveChanges();
            }
            return authKey;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
        }
    }
}