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
    [Table("Profile")]
    public class Profile : Entity
    {
        [Key]
        public string VoucherID { get; set; }

        public string FileResult { get; set; }

        public string EQID { get; set; }

        public string Temparature { get; set; }

        public string Humidity { get; set; }

        public bool? Passed { get; set; }

        public string UploadBy { get; set; }

        public DateTime? Stamp { get; set; }
        public Profile() { }

    }
}
