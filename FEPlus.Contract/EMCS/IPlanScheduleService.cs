using FEPlus.Models;
using FEPlus.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEPlus.Contract.EMCS
{
    public interface IPlanScheduleService
    {
        DataTable GetData(string DepartId, string Type, string Year, string lang);
        Dictionary<string,object> FindSchedulePlan(string EQId);
        OperationResult UpdatePlanTimeJob(PlanTimeJob planTimeJob);
        OperationResult CheckItemDetail(PlanTimeJob_Items planTimejobItems);
    }
}
