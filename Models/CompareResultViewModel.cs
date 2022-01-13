using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tr3Line.Assessment.Api.Models
{
    public class CompareResultViewModel
    {
        [Required] public string StudentOne { get; set; }
        [Required] public string StudentTwo { get; set; }
        [Required] public IFormFile Student1file { get; set; }
        [Required] public IFormFile Student2file { get; set; }
    }

}
