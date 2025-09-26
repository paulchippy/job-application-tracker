using JobApplicationTracker.API.Data;
using JobApplicationTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.API.Repositories
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public JobApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<JobApplication>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            var query = _context.JobApplications.AsNoTracking().AsQueryable();

            var totalCount = await query.CountAsync();

            var items = await query.OrderByDescending(a => a.DateApplied)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

            return new PaginatedResult<JobApplication>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<JobApplication> GetByIdAsync(int id) =>
            await _context.JobApplications.FindAsync(id);

        public async Task AddAsync(JobApplication application)
        {
            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobApplication application)
        {
            _context.JobApplications.Update(application);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var applicationToDelete = await GetByIdAsync(id);
            if (applicationToDelete != null)
            {
                _context.JobApplications.Remove(applicationToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}