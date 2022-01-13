using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;
using System.Linq;
using Tr3Line.Assessment.Services;
using Tr3Line.Assessment.Api.Entities;

namespace Tr3Line.Assessment.Api.Controllers
{
    [Route("api/v1/fileattachment")]
    [Authorize]
    [ApiController]
    public class FileAttachmentController : BaseController
    {
        #region Dependencies
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        #endregion


        public FileAttachmentController(
            IMapper mapper,
            IWebHostEnvironment hostEnvironment,
            IAccountService accountService
            )
        {
            _webHostEnvironment = hostEnvironment;
            _mapper = mapper;
            _accountService = accountService;
        }

        [HttpPost("UploadProfilePicture")]
        public IActionResult UploadFile(IFormFile student1file, CompareResult model)
        {
            try
            {
                string uniqueSTudent1FileName = null;

                if(student1file.Length > 100000)
                {
                    return BadRequest(new { message = "your image can not be more than 100 kilobytes" });
                }
                if (student1file.FileName != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadedfiles");
                    uniqueSTudent1FileName = Guid.NewGuid().ToString() + "_" + student1file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueSTudent1FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        student1file.CopyTo(fileStream);
                    }
                    string copyOfUploadStudent1File = GetFile(uniqueSTudent1FileName);
                    string text1 = System.IO.File.ReadAllText(copyOfUploadStudent1File);

                    model.StudentOneFile = uniqueSTudent1FileName;
                   // var savefilename = _accountService.UpdateProfilePicture(Account.Id, uniqueFileName);
                }
                return Ok(new { message = "" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "operation failed" + ex.Message });
            }
        }


        [BindProperty]
        public List<string> ImageList { get; set; }

        private string GetFile(string filename)
        {
            var provider = new PhysicalFileProvider(_webHostEnvironment.WebRootPath);
            var contents = provider.GetDirectoryContents(Path.Combine("uploadedfiles"));
            var objFiles = contents.OrderBy(m => m.LastModified);
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            ImageList = new List<string>();
            foreach (var item in objFiles.ToList())
            {
                ImageList.Add(item.Name);
            }
            var getUserfile = ImageList.FirstOrDefault(x => x.Contains(filename));

            return baseUrl + "/uploadedfiles/" + getUserfile;
        }

    }
}
