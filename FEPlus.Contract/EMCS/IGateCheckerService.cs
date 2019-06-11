using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEPlus.Models;
using FEPlus.Service.Pattern;
using FEPlus.Utility;
namespace FEPlus.Contract.EMCS
{
    public interface IGateCheckerService
    {
        DataTable GetCheckerByKind(string kind);
        Dictionary<string, object> GetDefCheckersByOwner(string owner, string fLowKey, string kinds, DateTime? checkDate);

        Dictionary<string, string> GetCheckLevel(string voucherId);
        Dictionary<string, string> IsWorkDay(DateTime day);
        DataTable GateQueryStatus(string ctype, string language, string flag);

    }
}
