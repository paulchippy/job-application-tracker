using Xunit;        
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using JobApplicationTracker.API.Models;
using JobApplicationTracker.API.Repositories;
using JobApplicationTracker.API.Services;

public class JobApplicationServiceTests
{
    // ARRANGE
    private readonly Mock<IJobApplicationRepository> _mockRepo;
    private readonly JobApplicationService _service;

    public JobApplicationServiceTests()
    {
        // ARRANGE
        _mockRepo = new Mock<IJobApplicationRepository>();
        _service = new JobApplicationService(_mockRepo.Object);
    }


    [Fact]
    public async Task GetPaginatedAsync_ShouldDelegateToRepositoryAndReturnResult()
    {
        // ARRANGE
        const int pageNumber = 2;
        const int pageSize = 10;

        // Mock the final result we expect from the Repository. 
        var mockResult = new PaginatedResult<JobApplication>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = 25,
            TotalPages = 3,
            Data = new List<JobApplication>
            {
                new JobApplication { Id = 11, CompanyName = "Company 11" },
                new JobApplication { Id = 12, CompanyName = "Company 12" }
            }
        };

        _mockRepo.Setup(r => r.GetPaginatedAsync(pageNumber, pageSize))
                 .ReturnsAsync(mockResult);

        // ACT
        var result = await _service.GetPaginatedApplicationsAsync(pageNumber, pageSize); // Corrected service method name

        // ASSERT
        _mockRepo.Verify(r => r.GetPaginatedAsync(pageNumber, pageSize), Times.Once);

        Assert.Equal(mockResult, result);
        Assert.Equal(25, result.TotalCount);
    }

    [Fact]
    public async Task CreateAsync_ShouldCallRepoAddAndReturnApplication_WhenDateIsValid()
    {
        // ARRANGE
        var application = new JobApplication
        {
            CompanyName = "TestCorp",
            DateApplied = DateTime.UtcNow.AddDays(-1) 
        };

      
        _mockRepo.Setup(r => r.AddAsync(It.IsAny<JobApplication>()))
                 .Returns(Task.CompletedTask);

        // ACT
        var result = await _service.CreateAsync(application);

        // ASSERT
        _mockRepo.Verify(r => r.AddAsync(application), Times.Once);

        Assert.Equal(application, result);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowArgumentException_WhenDateIsFuture() // Renamed method and changed exception type
    {
        // ARRANGE
        var application = new JobApplication
        {
            CompanyName = "FutureTech",
            DateApplied = DateTime.UtcNow.AddDays(1) // Invalid date (in the future)
        };

        // ACT & ASSERT
        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateAsync(application)
        );

        // ASSERT
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<JobApplication>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateExistingApplicationAndReturnIt_WhenIdExists()
    {
        // ARRANGE
        const int appId = 5;
        var existingApp = new JobApplication { Id = appId, CompanyName = "Old Company", DateApplied = DateTime.UtcNow.AddDays(-5) };
        var updatedData = new JobApplication { CompanyName = "New Company", Position = "Dev", DateApplied = DateTime.UtcNow.AddDays(-2) };

        _mockRepo.Setup(r => r.GetByIdAsync(appId)).ReturnsAsync(existingApp!);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<JobApplication>())).Returns(Task.CompletedTask);

        // ACT
        var result = await _service.UpdateAsync(appId, updatedData);

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal("New Company", result.CompanyName);
        Assert.Equal("Dev", result.Position);

        _mockRepo.Verify(r => r.UpdateAsync(existingApp), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowArgumentException_WhenUpdatedDateIsFuture() // Renamed method and changed exception type
    {
        // ARRANGE
        const int appId = 5;
        var existingApp = new JobApplication { Id = appId, CompanyName = "Old Company", DateApplied = DateTime.UtcNow.AddDays(-5) };
        var updatedData = new JobApplication { CompanyName = "New Company", Position = "Dev", DateApplied = DateTime.UtcNow.AddDays(1) }; // Future date

        _mockRepo.Setup(r => r.GetByIdAsync(appId)).ReturnsAsync(existingApp!);

        // ACT & ASSERT
        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.UpdateAsync(appId, updatedData)
        );

        // ASSERT
        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<JobApplication>()), Times.Never);
    }


    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenIdDoesNotExist()
    {
        // ARRANGE
        const int nonExistentId = 99;
        var updatedData = new JobApplication { CompanyName = "Ghost Company" };

        _mockRepo.Setup(r => r.GetByIdAsync(nonExistentId)).ReturnsAsync((JobApplication?)null);

        // ACT
        var result = await _service.UpdateAsync(nonExistentId, updatedData);

        // ASSERT
        Assert.Null(result);

        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<JobApplication>()), Times.Never);
    }



    [Fact]
    public async Task DeleteAsync_ShouldReturnTrueAndCallRepoDelete_WhenIdExists()
    {
        // ARRANGE
        const int appId = 10;
        var existingApp = new JobApplication { Id = appId };

        _mockRepo.Setup(r => r.GetByIdAsync(appId)).ReturnsAsync(existingApp!);
        _mockRepo.Setup(r => r.DeleteAsync(appId)).Returns(Task.CompletedTask);

        // ACT
        var result = await _service.DeleteAsync(appId);

        // ASSERT
        Assert.True(result);

        _mockRepo.Verify(r => r.GetByIdAsync(appId), Times.Once);
        _mockRepo.Verify(r => r.DeleteAsync(appId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalseAndNotCallRepoDelete_WhenIdDoesNotExist()
    {
        // ARRANGE
        const int nonExistentId = 999;

        _mockRepo.Setup(r => r.GetByIdAsync(nonExistentId)).ReturnsAsync((JobApplication?)null);

        // ACT
        var result = await _service.DeleteAsync(nonExistentId);

        // ASSERT
        Assert.False(result);

        _mockRepo.Verify(r => r.GetByIdAsync(nonExistentId), Times.Once);
        _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
