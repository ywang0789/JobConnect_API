using JobConnect_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace JobConnect_API.Models
{
    public class Application
    {
        [Key]
        public int application_id { get; set; }

        [ForeignKey("Job")]
        public int? job_id { get; set; }
        public Job? Job { get; set; } // Nav

        [ForeignKey("Account")]
        public string? account_id { get; set; }
        public Account? Account { get; set; } // Nav

        [Required]
        public string? content { get; set; }

        [Required]
        public string? status { get; set; } = "pending";// pending/accepted/rejected
    }
}
