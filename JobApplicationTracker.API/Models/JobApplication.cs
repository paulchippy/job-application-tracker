using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace JobApplicationTracker.API.Models
{
    public enum ApplicationStatus
    {
        Applied,
        Interviewing,
        Offered,
        Rejected
    }

    public class PaginatedResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }

    public class JobApplication
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(100)]
        public string Position { get; set; }

        public ApplicationStatus Status { get; set; }

        [Required]
        public DateTime DateApplied { get; set; }
    }
}