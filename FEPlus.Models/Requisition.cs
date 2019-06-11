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
    [Table("Requisition")]
    public class Requisition : Entity
    {
        [Key]
        public string VoucherID { get; set; }
        public string EQID { get; set; }

        public string State { get; set; }

        public string UserID { get; set; }

        public string Remark { get; set; }
        public DateTime Stamp { get; set; }
        public DateTime CreateTime { get; set; }
        public int MonthAdjust { get; set; }

        public int YearAdjust { get; set; }

        public Requisition() { }
        [NotMapped]
        public virtual ICollection<Profile> Profiles { set; get; }

        

    }
}
