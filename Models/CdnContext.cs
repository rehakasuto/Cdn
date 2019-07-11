using System.Data.Entity;
using DbManager;

namespace PointrCdn.Models
{
    public class CdnContext : DbTableContext
    {
        public DbSet<Client> clients { get; set; }
        public DbSet<Folder> folders { get; set; }
        public DbSet<File> files { get; set; }
        public DbSet<ExceptionLog> exceptions { get; set; }
    }
}