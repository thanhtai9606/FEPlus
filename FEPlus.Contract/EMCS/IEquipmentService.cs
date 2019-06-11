using FEPlus.Models;
using FEPlus.Service.Pattern;
using FEPlus.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEPlus.Contract.EMCS
{
    public interface IEquipmentService
    {
        DataTable GetEquipment(string AssetID, string EQName, string Department, string ProcessDepartment, string UserID, string Lang);
        OperationResult AddEquipment(Equipment equipment);
        OperationResult UpdateEquipment(Equipment equipment);
        OperationResult DeleteEquipment(Equipment equipment);
        DataTable GetDepartment(string table, string lang);
        DataTable GetBasic(string table, string lang);
        Dictionary<string, object> FindDetail(string EQID);
        int CheckUnique(string Table, string ColumnName, string Value);
    }
}
