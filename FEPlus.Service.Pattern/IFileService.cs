using System.Web;

namespace FEPlus.Service.Pattern
{
    public interface IFileService
    {
        bool UploadFile(string fileName, byte[] file, string fileType, string _FilePathQC);
    }
}
