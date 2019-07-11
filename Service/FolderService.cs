using PointrCdn.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PointrCdn.Service
{
    public class FolderService : IDisposable
    {
        public enum FolderProcess
        {
            Added,
            Updated,
            Deleted
        }
        public List<string> GetFolders(string authKey)
        {
            List<string> result = new List<string>();

            using (CdnContext context = new CdnContext())
            {
                Client client = context.clients.Where(x => x.authKey.Trim().Equals(authKey.Trim())).FirstOrDefault();
                if (client == null)
                    throw new NullReferenceException("There is no client with this authentication key. : " + authKey);

                List<Folder> folders = client.folders.Where(x => x.isDeleted == false).ToList();
                if (folders.Count > 0)
                    result.AddRange(folders.Select(x => x.name));
            }
            return result;
        }
        public List<string> GetFilesByFolderName(string authKey, string name)
        {
            List<string> result = new List<string>();

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            using (CdnContext context = new CdnContext())
            {
                Client client = context.clients.Where(x => x.authKey.Trim().Equals(authKey.Trim())).FirstOrDefault();
                if (client == null)
                    throw new NullReferenceException("There is no client with this authentication key. : " + authKey);

                Folder folder = client.folders.FirstOrDefault(x => x.name.ToLower().Trim().Equals(name.ToLower().Trim()) && x.isDeleted == false);
                if (folder == null)
                    throw new NullReferenceException("There is no folder with this folder name. : " + name);

                List<Models.File> files = folder.files.Where(x => x.isDeleted == false).ToList();
                if (files.Count > 0)
                    result.AddRange(files.Select(x => x.name));
            }
            return result;
        }

        public FolderProcess AddOrUpdateByName(string authKey, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            FolderProcess folderProcess;

            using (CdnContext context = new CdnContext())
            {
                Client client = context.clients.Where(x => x.authKey.Trim().Equals(authKey.Trim())).FirstOrDefault();
                if (client == null)
                    throw new NullReferenceException("There is no client with this authentication key. : " + authKey);

                Folder folder = client.folders.FirstOrDefault(x => x.name.ToLower().Trim().Equals(name.ToLower().Trim()) && x.isDeleted == false);
                if (folder != null)
                {
                    folderProcess = FolderProcess.Updated;
                }
                else
                {
                    Directory.CreateDirectory(ParameterUtil.GetCdnPath() + name);
                    context.folders.Add(new Folder { name = name, client = client });
                    context.SaveChanges();
                    folderProcess = FolderProcess.Added;
                }
                return folderProcess;
            }
        }
        public FolderProcess DeleteByName(string authKey, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            FolderProcess folderProcess;

            using (CdnContext context = new CdnContext())
            {
                Client client = context.clients.Where(x => x.authKey.Trim().Equals(authKey.Trim())).FirstOrDefault();
                if (client == null)
                    throw new NullReferenceException("There is no client with this authentication key. : " + authKey);

                Folder folder = client.folders.FirstOrDefault(x => x.name.ToLower().Trim().Equals(name.ToLower().Trim()) && x.isDeleted == false);
                if (folder == null)
                    throw new Exception("There is no folder to delete with this name and client : " + name);
                else
                {
                    Directory.Delete(ParameterUtil.GetCdnPath() + name, true);
                    context.folders.Remove(folder);
                    context.SaveChanges();
                    folderProcess = FolderProcess.Deleted;
                }
            }
            return folderProcess;
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