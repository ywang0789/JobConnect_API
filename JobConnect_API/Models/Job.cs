using System.ComponentModel.DataAnnotations;

namespace JobConnect_API.Models
{
    public class Job
    {
        [Key]
        public int job_id { get; set; }

        [Required]
        public required string title { get; set; }

        [Required]
        public required string description { get; set; }

        [Required]
        public required double salary { get; set; }

        [Required]
        public required string location { get; set; }
    }
}
