using PointrCdn.Service;
using System;
using System.Linq;
using System.Web.Http;

namespace PointrCdn.Controllers
{
    public class FileController : ApiController
    {
        private string GetAuthKeyFromHeader()
        {
            var authKey = Request.Headers.Where(x => x.Key == "AuthenticationToken").Select(x => x.Value.FirstOrDefault()).FirstOrDefault();
            if (string.IsNullOrEmpty(authKey))
                throw new Exception("Key is required to get information. Please check your request header section.");

            return authKey;
        }
        private string GetFolderFromHeader()
        {
            var folder = Request.Headers.Where(x => x.Key == "FolderName").Select(x => x.Value.FirstOrDefault()).FirstOrDefault();
            if (string.IsNullOrEmpty(folder))
                throw new Exception("Folder name is required to get information. Please check your request header section.");

            return folder;
        }
        private string GetRealFileName()
        {
            var fileName = Request.Headers.Where(x => x.Key == "RealFileName").Select(x => x.Value.FirstOrDefault()).FirstOrDefault();
            if (string.IsNullOrEmpty(fileName))
                throw new Exception("Real file name is required to get information. Please check your request header section.");

            return fileName;
        }
        private bool GetRename()
        {
            var rename = Request.Headers.Where(x => x.Key == "Rename").Select(x => x.Value.FirstOrDefault()).FirstOrDefault();
            if (string.IsNullOrEmpty(rename))
                throw new Exception("Rename value is required to get information. Please check your request header section.");

            return Convert.ToBoolean(rename);
        }

        // GET: localhost/file/a.txt
        [Route("file/{name}")]
        public string Get(string name)
        {
            string accessUrl = string.Empty;
            try
            {
                using (FileService fileService = new FileService())
                {
                    accessUrl = fileService.GetFileByName(GetAuthKeyFromHeader(), GetFolderFromHeader(), name);
                }
            }
            catch (Exception ex)
            {
                ActivityService.LogException(ex);
            }

            return accessUrl;
        }

        // POST: localhost/file/
        [Route("file")]
        public string Post()
        {
            string accessUrl = string.Empty;
            try
            {
                using (FileService fileService = new FileService())
                {
                    var stream = Request.Content.ReadAsStreamAsync().Result;
                    if (stream.Length == 0)
                        throw new Exception("File do not exist in request");

                    accessUrl = fileService.AddOrUpdateByName(GetAuthKeyFromHeader(), GetFolderFromHeader(), GetRealFileName(), stream, GetRename());
                }

            }
            catch (Exception ex)
            {
                ActivityService.LogException(ex);
            }

            return accessUrl;
        }

        // PUT: localhost/file/
        [Route("file")]
        public string Put()
        {
            string accessUrl = string.Empty;

            try
            {
                using (FileService fileService = new FileService())
                {
                    var stream = Request.Content.ReadAsStreamAsync().Result;
                    if (stream.Length == 0)
                        throw new Exception("Files are not exists in request");

                    accessUrl = fileService.AddOrUpdateByName(GetAuthKeyFromHeader(), GetFolderFromHeader(), DateTime.Now.Ticks.ToString() + "-" + GetRealFileName(), stream, GetRename());
                }

            }
            catch (Exception ex)
            {
                ActivityService.LogException(ex);
            }
            return accessUrl;

        }
        [Route("file/{name}")]
        // DELETE: localhost/file/a.txt
        public void Delete(string name)
        {
           
            try
            {
                using (FileService fileService = new FileService())
                {
                    fileService.DeleteFileByName(GetAuthKeyFromHeader(), GetFolderFromHeader(), name);
                }
            }
            catch (Exception ex)
            {
                ActivityService.LogException(ex);
            }
        }
    }
}
