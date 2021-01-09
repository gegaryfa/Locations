using System.ComponentModel.DataAnnotations;

namespace Locations.Core.Application.DTOs.Account
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
