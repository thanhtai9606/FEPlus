using FEPlus.Pattern.FakeDb;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FEPlus.Models
{
    [Table("vEGate_Employees")]

    public class Employee : Entity
    {
        [Key]
        public string EmployeeID { get; set; }
        public string sex { get; set; }
        public string Name { get; set; }
        public string DepartmentID { get; set; }
        public string Specification { get; set; }
        public string PositionName { get; set; }
        //public string Email { get; set; }
        public string Company { get; set; }

    }
}
