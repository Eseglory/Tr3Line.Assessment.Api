using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tr3Line.Assessment.Api.Entities
{
    public class CompareResult
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string StudentOne { get; set; }
        public string StudentTwo { get; set; }
        public string StudentOneFileName { get; set; }
        public string StudentTwoFileName { get; set; }
        public string DateCreated { get; set; } = DateTime.Now.ToLongDateString();
        public string ComparismResult { get; set; }
    }
}
