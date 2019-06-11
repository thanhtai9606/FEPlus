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
    [Table("PlanTimeJob_Items")]
    public class PlanTimeJob_Items : Entity
    {
        [Key]
        
        public string EQID { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public bool? IsCreated { get; set; }
        public PlanTimeJob_Items() { }

    }
}
