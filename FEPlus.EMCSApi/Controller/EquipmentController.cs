using FEPlus.Contract.EMCS;
using FEPlus.Models;
using FEPlus.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace FEPlus.EMCSApi.Controller
{
    [Filter.FilterIP]
    [RoutePrefix("api/EMCS/EQ")]
    public class EquipmentController : ApiController
    {
        private OperationResult operationResult = new OperationResult();
        HelperBiz _helper = new HelperBiz();

        private readonly IEquipmentService _equipmentService;
        public EquipmentController(IEquipmentService equipmentService) { _equipmentService = equipmentService; }
        [Route("GetDepartment")]
        [HttpGet]
        public IHttpActionResult GetDepartment(string Table, string Lang) => Ok(_equipmentService.GetDepartment(Table, Lang));

        //[Route("GetEquipment")]
        //[HttpGet]
        //public IHttpActionResult GetData() => Ok(_equipmentService.GetData().ToList());

        [Route("GetEquipment")]
        [HttpGet]
        public IHttpActionResult GetEquipment(string AssetID, string EQName, string Department, string ProcessDepartment, string UserID, string Lang)
        {
            return Ok(_equipmentService.GetEquipment(AssetID, EQName, Department, ProcessDepartment,  UserID, Lang));
        }

        [Route("AddEquipment")]
        [HttpPost]
        public IHttpActionResult AddEquipment(Equipment equipment) => Ok(_equipmentService.AddEquipment(equipment));

        [Route("UpdateEquipment")]
        [HttpPut]
        public IHttpActionResult UpdateEquipment(Equipment equipment) =>  Ok(_equipmentService.UpdateEquipment(equipment));

        [Route("DeleteEquipment")]
        [HttpPut]
        public IHttpActionResult DeleteEquipment(Equipment equipment)=> Ok(_equipmentService.DeleteEquipment(equipment));

        [Route("GetDetailEquipment")]
        [HttpGet]
        public IHttpActionResult FindDetail(string EQID) => Ok(_equipmentService.FindDetail(EQID));

        [Route("GetBasic")]
        [HttpGet]
        public IHttpActionResult GetBasic(string table, string lang)
        {
            return Ok(_equipmentService.GetBasic(table, lang));
        }

        [Route("CheckUnique")]
        [HttpGet]
        public IHttpActionResult CheckUnique(string Table, string ColumnName, string Value)
        {
            return Ok(_equipmentService.CheckUnique(Table, ColumnName, Value));
        }      
    }
}
