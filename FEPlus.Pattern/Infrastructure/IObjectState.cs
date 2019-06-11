using System.ComponentModel.DataAnnotations.Schema;

namespace FEPlus.Pattern.Infrastructure
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}
