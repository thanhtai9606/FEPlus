using FEPlus.Contract;
using FEPlus.Models;
using FEPlus.Utility;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;

namespace FEPlus.EMCSApi.Admin
{
    [Filter.FilterIP]
    [RoutePrefix("api/HSSE")]
    public class HSSEController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        public HSSEController(IEmployeeService employeeService) => _employeeService = employeeService;

        [Route("ValidateUser")]
        [HttpGet]
        public IHttpActionResult ValidateUser(string username, string password) => Ok(_employeeService.ValidateUser(username, password));

        [HttpGet]
        public IHttpActionResult ValidateUser(string token) => Ok(_employeeService.ValidateUser(token));

        [HttpGet]
        public bool CheckTCode(string username, string tcode) => _employeeService.CheckTCode(username, tcode);
        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [Route("ChangePassword")]
        [HttpPost]
        public IHttpActionResult ChangePassword(PasswordChange pass) => Ok(_employeeService.ChangePassword(pass.username, pass.oldPassword, pass.newPassword));

        [HttpPost]
        [Route("FileUploadAsync")]
        public async Task<IHttpActionResult> UploadFileAsync()
        {
            OperationResult operationResult = new OperationResult();
            try
            {
                if (!this.Request.Content.IsMimeMultipartContent())
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                MultipartMemoryStreamProvider provider = new MultipartMemoryStreamProvider();
                MultipartMemoryStreamProvider memoryStreamProvider = await this.Request.Content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(provider);
                foreach (HttpContent content in provider.Contents)
                {
                    HttpContent file = content;
                    string filename = file.Headers.ContentDisposition.FileName.Trim('"');
                    byte[] buffer = await file.ReadAsByteArrayAsync();
                    string path = ConfigurationManager.AppSettings["DocsUrl"]; ;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    System.IO.File.WriteAllBytes(path + "\\" + filename, buffer);
                    operationResult.Success = true;
                    operationResult.Caption = "Upload Successed!";
                    operationResult.Message = filename + " have been uploaded.";
                    filename = (string)null;
                    buffer = (byte[])null;
                    file = (HttpContent)null;                  
                }


            }
            catch (Exception ex)
            {
                operationResult.Success = false;
                operationResult.Caption = "Upload Failed!";
                operationResult.Message = "Some problems " + ex.ToString();
            }

            return Ok(operationResult);
        }

    }
}