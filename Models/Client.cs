using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DbManager.Entity.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace PointrCdn.Models
{
    public class Client : ModifiableDbTable
    {
        [MaxLength(50)]
        public string name { get; set; }
        [Required, MaxLength(200)]
        public string connectionString { get; set; }
        [MaxLength(200)]
        public string authKey { get; set; }
        [InverseProperty("client")]
        public virtual IList<Folder> folders { get; set; }
    }
}