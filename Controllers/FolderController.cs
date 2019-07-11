using PointrCdn.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace PointrCdn.Controllers
{
    public class FolderController : ApiController
    {
        public string GetAuthKeyFromHeader()
        {
            var authKey = Request.Headers.Where(x => x.Key == "AuthenticationToken").Select(x => x.Value.FirstOrDefault()).FirstOrDefault();
            if (string.IsNullOrEmpty(authKey))
                throw new Exception("Key is required to get information. Please check your request header section.");

            return authKey;
        }

        // GET: localhost/folder
        [Route("folder")]
        public IEnumerable<string> Get()
        {
            try
            {
                using (FolderService folderService = new FolderService())
                {
                    return folderService.GetFolders(GetAuthKeyFromHeader());
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("folder/{name}")]
        // GET: localhost/folder/ikea-berlin
        public IEnumerable<string> Get(string name)
        {
            try
            {
                using (FolderService folderService = new FolderService())
                {
                    return folderService.GetFilesByFolderName(GetAuthKeyFromHeader(), name);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("folder/{name}")]
        // PUT: localhost/folder/ikea-berlin
        public string Put(string name)
        {
            try
            {
                using (FolderService folderService = new FolderService())
                {
                    FolderService.FolderProcess state = folderService.AddOrUpdateByName(GetAuthKeyFromHeader(), name);
                    return new JavaScriptSerializer().Serialize(new { state = state.ToString() });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("folder/{name}")]
        // DELETE: localhost/folder/ikea-berlin
        public string Delete(string name)
        {
            try
            {
                using (FolderService folderService = new FolderService())
                {
                    FolderService.FolderProcess state = folderService.DeleteByName(GetAuthKeyFromHeader(), name);
                    return new JavaScriptSerializer().Serialize(new { state = state.ToString() });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
