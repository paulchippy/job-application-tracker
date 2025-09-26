using JobApplicationTracker.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobApplicationTracker.API.Repositories
{
    public interface IJobApplicationRepository
    {
        Task<PaginatedResult<JobApplication>> GetPaginatedAsync(int pageNumber, int pageSize);
        Task<JobApplication> GetByIdAsync(int id);
        Task AddAsync(JobApplication jobapplication);
        Task UpdateAsync(JobApplication jobapplication);
        Task DeleteAsync(int id);
    }
}