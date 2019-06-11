using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEPlus.Utility
{
    public class OperationResult
    {
        public string Caption { set; get; }
        public string Message { set; get; }
        public bool Success { set; get; }
        public OperationResult() { }
        public OperationResult(string caption, string message, bool success)
        {
            this.Caption = caption;
            this.Message = message;
            this.Success = success;
        }
    }
}
