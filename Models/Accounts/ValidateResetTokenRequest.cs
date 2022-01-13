using System.ComponentModel.DataAnnotations;

namespace Tr3Line.Assessment.Models.Accounts
{
    public class ValidateResetTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}