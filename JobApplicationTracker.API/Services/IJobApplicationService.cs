using JobApplicationTracker.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobApplicationTracker.API.Services
{
    public interface IJobApplicationService
    {
        Task<PaginatedResult<JobApplication>> GetPaginatedApplicationsAsync(int pageNumber, int pageSize);

        Task<JobApplication> GetByIdAsync(int id);
        Task<JobApplication> CreateAsync(JobApplication application);

        Task<JobApplication?> UpdateAsync(int id, JobApplication application);
        Task<bool> DeleteAsync(int id);
    }
}