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
using Tr3Line.Assessment.Api.Models;
using Tr3Line.Assessment.Api.Repository.Interface;
using Tr3Line.Assessment.Api.Helpers;

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
        private readonly ICompareResultRepository _compareResult;
        #endregion


        public FileAttachmentController(
            IMapper mapper,
            IWebHostEnvironment hostEnvironment,
            IAccountService accountService,
            ICompareResultRepository compareResult
            )
        {
            _webHostEnvironment = hostEnvironment;
            _mapper = mapper;
            _accountService = accountService;
            _compareResult = compareResult;
        }

        [HttpPost("CompareUploadedFiles")]
        public async System.Threading.Tasks.Task<IActionResult> CompareUploadedFiles([FromForm]  CompareResultViewModel model)
        {
            try
            {
                string text1 = "";
                string text2 = "";

                if (model.Student1file.Length > 100000)
                {
                    return BadRequest(new { message = "your file can not be more than 100 kilobytes" });
                }

                if (model.Student1file.FileName == model.Student2file.FileName)
                {
                    return BadRequest(new { message = "the two files can not have the same name" });
                }
                if (model.Student1file.FileName != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadedfiles");
                    string filePath = Path.Combine(uploadsFolder, model.Student1file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Student1file.CopyTo(fileStream);
                    }
                    string copyOfUploadStudent1File = GetFile(model.Student1file.FileName);
                    text1 = System.IO.File.ReadAllText(copyOfUploadStudent1File);
                }

                if (model.Student2file.FileName != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadedfiles");
                    string filePath = Path.Combine(uploadsFolder, model.Student2file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Student2file.CopyTo(fileStream);
                    }
                    string copyOfUploadStudent1File = GetFile(model.Student2file.FileName);
                    text2 = System.IO.File.ReadAllText(copyOfUploadStudent1File);
                }
                var result = CompareFileContent.CompareFileHandler(text1, text2);
                CompareResult compareResult = new CompareResult()
                {
                    StudentOneFileName = model.Student1file.FileName,
                    StudentOne = model.StudentOne,
                    StudentTwo = model.StudentTwo,
                    StudentTwoFileName = model.Student1file.FileName,
                    ComparismResult = result,
                };
                await _compareResult.CreateAsync(compareResult);
                await _compareResult.SaveChangesAsync();
                return Ok(new { message = result });
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
