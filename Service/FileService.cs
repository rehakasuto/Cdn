using PointrCdn.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace PointrCdn.Service
{
    public class FileService : IDisposable
    {
        public enum FileProcess
        {
            Added,
            Updated,
            Deleted
        }

        public string GetFileByName(string authKey, string folderName, string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));
                if (string.IsNullOrEmpty(folderName))
                    throw new ArgumentNullException(nameof(folderName));

                string path = "";
                using (CdnContext context = new CdnContext())
                {
                    Client client = context.clients.Where(x => x.authKey.Trim().Equals(authKey.Trim())).FirstOrDefault();
                    if (client != null)
                    {
                        Folder folder = client.folders.FirstOrDefault(x => x.name.ToLower().Trim().Equals(folderName.ToLower().Trim()) && x.isDeleted == false);
                        if (folder != null)
                        {
                            Models.File file = folder.files.FirstOrDefault(x => x.name.ToLower().Trim().Equals(name.ToLower().Trim()) && x.isDeleted == false);
                            if (file != null)
                                path = file.accessUrl;
                        }
                    }
                }
                return path;
            }
            catch (Exception ex)
            {
                ActivityService.LogException(ex);
                throw ex;
            }

        }
        public void DeleteFileByName(string authKey, string folderName, string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                if (string.IsNullOrEmpty(folderName))
                    throw new ArgumentNullException(nameof(folderName));

                using (CdnContext context = new CdnContext())
                {
                    Client client = context.clients.Where(x => x.authKey.Trim().Equals(authKey.Trim())).FirstOrDefault();
                    if (client == null)
                        throw new NullReferenceException("There is no client with this authentication key. : " + authKey);

                    Folder folder = client.folders.FirstOrDefault(x => x.name.ToLower().Trim().Equals(folderName.ToLower().Trim()) && x.isDeleted == false);
                    if (folder == null)
                        throw new Exception("There is no folder with this name and client : " + folderName);

                    Models.File file = folder.files.FirstOrDefault(x => x.name.ToLower().Trim().Equals(name.ToLower().Trim()) && x.isDeleted == false);
                    if (file == null)
                        throw new Exception("There is no file to delete under " + folderName + " with this file name. : " + name);
                    else
                    {
                        System.IO.File.Delete(ParameterUtil.GetCdnPath() + folderName + "\\" + name);
                        context.files.Remove(file);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ActivityService.LogException(ex);
                throw ex;
            }

        }
        public string AddOrUpdateByName(string authKey, string folderName, string name, Stream stream, bool rename)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                if (rename)
                    name = DateTime.Now.Ticks.ToString() + "-" + name;

                if (string.IsNullOrEmpty(folderName))
                    throw new ArgumentNullException(nameof(folderName));

                string accessUrl = "";

                using (CdnContext context = new CdnContext())
                {
                    Client client = context.clients.Where(x => x.authKey.Trim().Equals(authKey.Trim())).FirstOrDefault();
                    if (client == null)
                        throw new NullReferenceException("There is no client with this authentication key. : " + authKey);

                    Folder folder = client.folders.FirstOrDefault(x => x.name.ToLower().Trim().Equals(folderName.ToLower().Trim()) && x.isDeleted == false);
                    if (folder == null)//if folder name is not exists, create a new one with sent folder name.
                        throw new NullReferenceException("There is no folder with this folder name and client. : " + folderName);

                    Models.File file = folder.files.FirstOrDefault(x => x.name.ToLower().Trim().Equals(name.ToLower().Trim()) && x.isDeleted == false);

                    if (file == null)
                    {
                        FileStream fs = new FileStream(ParameterUtil.GetCdnPath() + folderName + "\\" + name, FileMode.Create);
                        stream.CopyTo(fs);
                        fs.Close();

                        context.files.Add(new Models.File
                        {
                            name = name,
                            path = ParameterUtil.GetCdnPath() + folderName + "\\" + name,
                            accessUrl = accessUrl = ParameterUtil.GetCdnUrl() + folderName + "/" + name,
                            folder_id = folder.id,
                            version = "1"
                        });
                        context.SaveChanges();
                    }
                }
                return accessUrl;
            }
            catch (Exception ex)
            {
                ActivityService.LogException(ex);
                throw ex;
            }
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