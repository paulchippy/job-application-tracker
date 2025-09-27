using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using JobApplicationTracker.API.Models;
using JobApplicationTracker.API.Repositories;
using JobApplicationTracker.API.Services;
using JobApplicationTracker.API.Exceptions;

public class JobApplicationServiceTests
{
    private readonly Mock<IJobApplicationRepository> _mockRepo;
    private readonly JobApplicationService _service;

    public JobApplicationServiceTests()
    {
        _mockRepo = new Mock<IJobApplicationRepository>();
        _service = new JobApplicationService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetPaginatedApplicationsAsync_ShouldReturnPaginatedResult()
    {
        const int pageNumber = 2;
        const int pageSize = 10;

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

        var result = await _service.GetPaginatedApplicationsAsync(pageNumber, pageSize);

        _mockRepo.Verify(r => r.GetPaginatedAsync(pageNumber, pageSize), Times.Once);
        Assert.Equal(mockResult, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnApplication_WhenExists()
    {
        const int appId = 5;
        var app = new JobApplication { Id = appId, CompanyName = "TestCorp" };

        _mockRepo.Setup(r => r.GetByIdAsync(appId)).ReturnsAsync(app);

        var result = await _service.GetByIdAsync(appId);

        Assert.Equal(app, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenNotExists()
    {
        const int appId = 99;
        _mockRepo.Setup(r => r.GetByIdAsync(appId)).ReturnsAsync((JobApplication?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(appId));
    }

    [Fact]
    public async Task CreateAsync_ShouldCallRepoAddAndReturnApplication()
    {
        var application = new JobApplication { CompanyName = "TestCorp" };

        _mockRepo.Setup(r => r.AddAsync(It.IsAny<JobApplication>()))
                 .Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(application);

        _mockRepo.Verify(r => r.AddAsync(application), Times.Once);
        Assert.Equal(application, result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateExistingApplication_WhenIdExists()
    {
        const int appId = 5;
        var existingApp = new JobApplication { Id = appId, CompanyName = "Old Company" };
        var updatedData = new JobApplication { CompanyName = "New Company", Position = "Dev" };

        _mockRepo.Setup(r => r.GetByIdAsync(appId)).ReturnsAsync(existingApp);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<JobApplication>())).Returns(Task.CompletedTask);

        var result = await _service.UpdateAsync(appId, updatedData);

        Assert.NotNull(result);
        Assert.Equal("New Company", result.CompanyName);
        Assert.Equal("Dev", result.Position);

        _mockRepo.Verify(r => r.UpdateAsync(existingApp), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenIdDoesNotExist()
    {
        const int nonExistentId = 99;
        var updatedData = new JobApplication { CompanyName = "Ghost Company" };

        _mockRepo.Setup(r => r.GetByIdAsync(nonExistentId)).ReturnsAsync((JobApplication?)null);

        var result = await _service.UpdateAsync(nonExistentId, updatedData);

        Assert.Null(result);
        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<JobApplication>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenIdExists()
    {
        const int appId = 10;
        var existingApp = new JobApplication { Id = appId };

        _mockRepo.Setup(r => r.GetByIdAsync(appId)).ReturnsAsync(existingApp);
        _mockRepo.Setup(r => r.DeleteAsync(appId)).Returns(Task.CompletedTask);

        var result = await _service.DeleteAsync(appId);

        Assert.True(result);
        _mockRepo.Verify(r => r.DeleteAsync(appId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenIdDoesNotExist()
    {
        const int nonExistentId = 999;

        _mockRepo.Setup(r => r.GetByIdAsync(nonExistentId)).ReturnsAsync((JobApplication?)null);

        var result = await _service.DeleteAsync(nonExistentId);

        Assert.False(result);
        _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
