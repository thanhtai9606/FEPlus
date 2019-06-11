using FEPlus.Contract.EMCS;
using FEPlus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace FEPlus.EMCSApi.Controller
{
    [Filter.FilterIP]
    [RoutePrefix("api/EMCS/Voucher")]
    public class VoucherController : ApiController
    {
        private readonly IVoucherService _voucherService;
        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }
        [Route("AddVoucher")]
        [HttpPost]
        public IHttpActionResult AddRequsitionVoucher(Requisition requisition) //requistion and plans share a same colum
        {
            return Ok(_voucherService.AddRequsitionVoucher(requisition));
        }
        [Route("UpdateVoucher")]
        [HttpPost]
        public IHttpActionResult UpdateVouchers(Requisition requisition) //requistion and plans share a same colum
        {
            return Ok(_voucherService.UpdateVouchers(requisition));
        }
        [Route("GetVoucher")]
        [HttpGet]

        public IHttpActionResult GetData(string departid, string type, string year, string status)
        {
            return Ok(_voucherService.GetDataVoucher(departid, type, year, status));
        }
        [Route("DeleteVoucher")]
        [HttpDelete]

        public IHttpActionResult DeleteVoucher(string voucherid)
        {
            return Ok(_voucherService.DeleteVoucher(voucherid));
        }


        [Route("FindVoucher")]
        [HttpGet]

        public IHttpActionResult FindVoucher(string VoucherID)
        {
            return Ok(_voucherService.FindVoucher(VoucherID));

        }


        [Route("UpdateVoucherState")]
        [HttpGet]

        public IHttpActionResult UpdateVoucherState(string voucherid, string state)
        {
            return Ok(_voucherService.UpdateVoucherState(voucherid, state));
        }
    }
}
