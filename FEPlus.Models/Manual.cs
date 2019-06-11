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
    [Table("Manual")]
    public class Manual : Entity
    {
        [Key]
        public int MethodID { get; set; }

        public string EQID { get; set; }

        public string Name { get; set; }
        public string FileName { get; set; }

        public int Version { get; set; }

        public DateTime? Stamp { get; set; }

        public string Remark { get; set; }
        public Manual() { }

        public string State { get; set; }

    }
}
