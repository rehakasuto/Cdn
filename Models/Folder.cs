using System.Collections.Generic;
using DbManager.Entity.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PointrCdn.Models
{
    public class Folder : DeletableDbTable
    {
        [Required, MaxLength(50)]
        public string name { get; set; }
        [ForeignKey("client")]
        public int client_id { get; set; }
        public virtual Client client { get; set; }
        [InverseProperty("folder")]
        public virtual IList<File> files { get; set; }

        public override void MarkRelationalModelsAsDeleted()
        {
            //throw new NotImplementedException();
        }
    }
}