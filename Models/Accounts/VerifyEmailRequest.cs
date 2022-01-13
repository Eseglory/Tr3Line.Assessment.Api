using System.ComponentModel.DataAnnotations;

namespace Tr3Line.Assessment.Models.Accounts
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; }
    }
}