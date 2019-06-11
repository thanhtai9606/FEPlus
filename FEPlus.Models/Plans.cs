using FEPlus.Pattern.FakeDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEPlus.Models
{
    [Table("Plans")]
    public class Plans : Entity
    {
        [Key]
        public string VoucherID { get; set; }

        public string EQID { get; set; }

        public string State { get; set; }

        public DateTime? Stamp { get; set; }
        public DateTime? CreateTime { get; set; }

        public string UserID { get; set; }
		public Plans() { }
        [NotMapped]
        public virtual ICollection<Profile> Profiles { set; get; }
        

    }
}
