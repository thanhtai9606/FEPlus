using FEPlus.Pattern.Factory;
using System.Data.Entity;

namespace FEPlus.Models
{
    public class IMContext : DataContext
    {
        public IMContext() : base("Name = GATE") { }

        DbSet<Employee> Employees { set;get; }
    }
}
