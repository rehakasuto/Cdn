using DbManager.Entity.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PointrCdn.Models
{
    public class File : DeletableDbTable
    {
        [Required, MaxLength(50)]
        public string name { get; set; }
        public string path { get; set; }
        public string accessUrl { get; set; }
        [ForeignKey("folder")]
        public int folder_id { get; set; }
        public virtual Folder folder { get; set; }
        [Required, MaxLength(10)]
        public string version { get; set; }

        public override void MarkRelationalModelsAsDeleted()
        {
            //throw new NotImplementedException();
        }
    }
}