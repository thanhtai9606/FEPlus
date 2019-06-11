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
    public interface IVoucherService
    {      
        DataTable GetDataVoucher(string departid, string type, string year, string status);
        Dictionary<string,object> FindVoucher(string voucherID);
        OperationResult AddRequsitionVoucher(Requisition requisition);
        OperationResult UpdateVouchers(Requisition requisition);
        OperationResult DeleteVoucher(string voucherid);
        OperationResult UpdateVoucherState(string voucherid, string state);
    }
}
