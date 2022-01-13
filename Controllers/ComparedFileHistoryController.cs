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
using System.Threading.Tasks;

namespace Tr3Line.Assessment.Api.Controllers
{
    [Route("api/v1/fileattachment")]
    [Authorize]
    [ApiController]
    public class ComparedFileHistoryController : BaseController
    {
        #region Dependencies
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly ICompareResultRepository _compareResult;
        #endregion


        public ComparedFileHistoryController(
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

        [HttpGet("GetComparedFilesByStudentName/{studentname}")]
        public async Task<IActionResult> CompareUploadedFiles(string studentname)
        {
            try
            {
                return Ok(await _compareResult.GetCompareResultsByStudenetName(studentname));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "operation failed" + ex.Message });
            }
        }

        [HttpGet("GetComparedResultsForFirstStudenet/{studentname}")]
        public async Task<IActionResult> GetComparedResultsForFirstStudenet(string studentname)
        {
            try
            {
                return Ok(await _compareResult.GetCompareResultsForFirstStudenet(studentname));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "operation failed" + ex.Message });
            }
        }

        [HttpGet("GetComparedResultsForSecondStudenet/{studentname}")]
        public async Task<IActionResult> GetComparedResultsForSecondStudenet(string studentname)
        {
            try
            {
                return Ok(await _compareResult.GetCompareResultsForSecondStudenet(studentname));
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
