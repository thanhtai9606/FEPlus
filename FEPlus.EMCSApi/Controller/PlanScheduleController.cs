using FEPlus.Contract.EMCS;
using FEPlus.Models;
using FEPlus.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace FEPlus.EMCSApi.Controller
{
    [Filter.FilterIP]
    [RoutePrefix("api/EMCS/PlanSchedule")]
    public class PlanScheduleController : ApiController
    {
        private readonly IPlanScheduleService _planScheduleService;
        HelperBiz helper = new HelperBiz();
        public PlanScheduleController(IPlanScheduleService planScheduleService) { _planScheduleService = planScheduleService; }
        [Route("GetSchedulePlan")]
        [HttpGet]
        public IHttpActionResult GetData(string departid, string type, string year, string lang) => Ok(_planScheduleService.GetData(departid, type, year, lang));

        [Route("FindSchedulePlan")]
        [HttpGet]
        public IHttpActionResult FindSchedulePlan(string eqid)=> Ok(_planScheduleService.FindSchedulePlan(eqid));

        [Route("UpdateSchedulePlan")]
        [HttpPost]
        public IHttpActionResult UpdatePlanTimeJob(PlanTimeJob item)=> Ok(_planScheduleService.UpdatePlanTimeJob(item));

        [Route("CheckItemDetail")]
        [HttpPut]
        public IHttpActionResult CheckItemDetail(PlanTimeJob_Items item) => Ok(_planScheduleService.CheckItemDetail(item));
    }
}
