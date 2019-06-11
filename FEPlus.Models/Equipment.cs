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
    [Table("Equipment")]
    public class Equipment : Entity
    {
        [Key]
        public string EQID { get; set; }

        public string AssetID { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public DateTime? UsedDate { get; set; }

        public DateTime? Stamp { get; set; }

        public string UserID { get; set; }


        public string State { get; set; }

        public string Remark { get; set; }

        public string Department { get; set; }

        public string ProcessDepartment { get; set; }
        public string AdjustType { get; set; }
        public int Frequency { get; set; }
        public Equipment() { }
        [NotMapped]
        public IEnumerable<Method> Methods { set; get; }
        [NotMapped]
        public IEnumerable<Manual> Manuals { set; get; }

        //[NotMapped]
        //public string DepartmentName { get; set; }

        //[NotMapped]
        //public string ProcessDepartmentName { get; set; }

    }
}
