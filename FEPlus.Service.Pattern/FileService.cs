using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FEPlus.Service.Pattern
{
    public class FileService:IFileService
    {
        public bool UploadFile(string fileName, byte[] fileData,  string fileType, string _FilePathQC)
        {
            Console.WriteLine("DocService-UpLoadFile():" + fileData.Length.ToString() + "-" + DateTime.Now.ToString());
            Console.WriteLine("configurationAppSettings.GetValue:" + _FilePathQC);
            string filePathServer = string.Empty;
            try
            {
                filePathServer = string.Format("{0}-{1}.{2}", fileName, DateTime.Now.ToString("yyMMddHHmmss"), fileType);
                string filepath = string.Format("{0}{1}", _FilePathQC, filePathServer);
                Console.WriteLine("filepath:" + filepath);

                MemoryStream meoeryStream = new MemoryStream(fileData);
                FileStream fileStream = new FileStream(filepath, FileMode.Create);
                meoeryStream.WriteTo(fileStream);
                fileStream.Close();
                meoeryStream.Close();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public byte[] ConverToBytes(HttpPostedFileBase file)
        {
            var length = file.InputStream.Length; //Length: 103050706
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                fileData = binaryReader.ReadBytes(file.ContentLength);
            }
            return fileData;
        }
    }
}
