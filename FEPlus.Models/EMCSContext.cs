using FEPlus.Pattern.Factory;
using System.Data.Entity;

namespace FEPlus.Models
{
    public class EMCSContext : DataContext
    {
        public EMCSContext() : base("Name = EMCS") { }

        DbSet<Equipment> Equipments { set; get; }
        DbSet<Manual> Manuals { set; get; }
        DbSet<Method> Methods { set; get; }
        DbSet<Plans> Plans { set; get; }
        DbSet<PlanTimeJob> PlanTimeJobs { set; get; }
        DbSet<PlanTimeJob_Items> PlanTimeJob_Items { set; get; }
        DbSet<Profile> Profiles { set; get; }
        DbSet<Requisition> Requisitions { set; get; }


    }
}
