using FEPlus.Contract.EMCS;
using FEPlus.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace FEPlus.EMCSApi.Controller
{
    [RoutePrefix("api/Gate/Checker")]
    public class GateCheckerController : ApiController
    {
        private OperationResult operationResult = new OperationResult();
        HelperBiz _helper = new HelperBiz();
        private readonly IGateCheckerService _gateCheckerService;
        public GateCheckerController(IGateCheckerService gateCheckerService) { _gateCheckerService = gateCheckerService; }


        [Route("GetCheckerByKind")]
        [HttpGet]
        public IHttpActionResult GetCheckerByKind(string kind)
        {
            return Ok(_gateCheckerService.GetCheckerByKind(kind));
        }


        [Route("GetCheckersByLevel")]
        [HttpGet]
        public IHttpActionResult GetDefCheckersByOwner(string owner, string fLowKey, string kinds, DateTime? checkDate)
        {
            return Ok(_gateCheckerService.GetDefCheckersByOwner(owner, fLowKey, kinds, checkDate));
        }


        [Route("GetCheckLevel")]
        [HttpGet]
        public IHttpActionResult GetCheckLevel(string voucherId)
        {
            return Ok(_gateCheckerService.GetCheckLevel(voucherId));
        }

        [Route("IsWorkDay")]
        [HttpGet]
        public IHttpActionResult IsWorkDay(DateTime day)
        {
            return Ok(_gateCheckerService.IsWorkDay(day));
        }

        [Route("GateQueryStatus")]
        [HttpGet]
        public IHttpActionResult GateQueryStatus(string ctype, string language, string flag)
        {
            return Ok(_gateCheckerService.GateQueryStatus(ctype, language, flag));
        }
    }
}
