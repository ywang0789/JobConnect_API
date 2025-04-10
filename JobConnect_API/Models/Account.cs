using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JobConnect_API.Models
{
    public class Account : IdentityUser
    {
        [Required]
        public required string first_name { get; set; }

        [Required]
        public required string last_name { get; set; }

        [Required]
        public required string role { get; set; } // recruiter/applicant
    }
}
