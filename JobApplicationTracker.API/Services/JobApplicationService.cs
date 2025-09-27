using JobApplicationTracker.API.Models;
using JobApplicationTracker.API.Repositories;
using JobApplicationTracker.API.Exceptions;

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

        public async Task<JobApplication?> GetByIdAsync(int id)
        {
            var application = await _repo.GetByIdAsync(id);
 
            if (application == null)
            {
                throw new NotFoundException($"Job application with ID {id} is not found.");
            }
            return application;
        }

        public async Task<JobApplication> CreateAsync(JobApplication application)
        {
            await _repo.AddAsync(application);
            return application;
        }

        public async Task<JobApplication?> UpdateAsync(int id, JobApplication application)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

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