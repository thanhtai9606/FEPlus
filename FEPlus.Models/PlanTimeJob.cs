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
    [Table("PlanTimeJob")]
    public class PlanTimeJob : Entity
    {
        [Key]
        [Column("EQID")]

        public string EQID { get; set; }

        public TimeSpan? StartTime { get; set; }

        public int? CreateInDay { get; set; }

        public int? ArrivalNoticeDay { get; set; }

        public bool? NoiticeEnable { get; set; }

        public bool? MakeVoucherEnable { get; set; }

        public string UserID { get; set; }
        [NotMapped]
        public virtual ICollection<PlanTimeJob_Items> PlanTimeJob_Items { set; get; }
        public PlanTimeJob() { }

    }
}
