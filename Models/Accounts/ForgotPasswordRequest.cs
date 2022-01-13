using System.ComponentModel.DataAnnotations;

namespace Tr3Line.Assessment.Models.Accounts
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}