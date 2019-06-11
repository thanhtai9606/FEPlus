using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FEPlus.Utility
{
    public class HelperBiz
    {
        public HelperBiz() { }

        public string ConvertToJson(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }
        public string ConvertToJson<TEntity>(List<TEntity> list)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(list);           
            return JSONString;
        }
        public List<string> GetPropertyList(object obj)
        {
            List<string> propertyList = new List<string>();

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            string p;
            foreach (PropertyInfo property in properties)
            {
                object o = property.GetValue(obj, null);
                if (o == null)
                    p = "";
                else if (o != null && o.GetType() == typeof(bool))
                    p = (bool)o ? "是" : "否";
                else
                    p = o.ToString();
                propertyList.Add(p);
            }

            return propertyList;
        }
      
        public DataTable ConvertListToDataTable(List<string[]> list)
        {
            // New table.
            DataTable table = new DataTable();

            // Get max columns.
            int columns = 0;
            foreach (var array in list)
            {
                if (array.Length > columns)
                {
                    columns = array.Length;
                }
            }

            // Add columns.
            for (int i = 0; i < columns; i++)
            {
                table.Columns.Add();
            }

            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array);
            }

            return table;

        }

    }
}
