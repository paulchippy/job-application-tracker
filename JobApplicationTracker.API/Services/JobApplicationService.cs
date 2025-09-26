using JobApplicationTracker.API.Models;
using JobApplicationTracker.API.Repositories;
using System; // Required for DateTime.UtcNow and ArgumentException
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobApplicationTracker.API.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IJobApplicationRepository _repo;

        public JobApplicationService(IJobApplicationRepository repo)
        {
            _repo = repo;
        }

        public async Task<PaginatedResult<JobApplication>> GetPaginatedApplicationsAsync(int pageNumber, int pageSize)
        {
          
            return await _repo.GetPaginatedAsync(pageNumber, pageSize);
        }

        public async Task<JobApplication> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<JobApplication> CreateAsync(JobApplication application)
        {
            if (application.DateApplied.Date > DateTime.UtcNow.Date)
            {
                throw new ArgumentException("DateApplied cannot be in the future");
            }

            await _repo.AddAsync(application);
            return application;
        }

        public async Task<JobApplication?> UpdateAsync(int id, JobApplication application)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            if (application.DateApplied.Date > DateTime.UtcNow.Date)
            {
                throw new ArgumentException("DateApplied cannot be in the future");
            }

            existing.CompanyName = application.CompanyName;
            existing.Position = application.Position;
            existing.Status = application.Status;
            existing.DateApplied = application.DateApplied;

            await _repo.UpdateAsync(existing);
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            await _repo.DeleteAsync(id);
            return true;
        }
    }
}