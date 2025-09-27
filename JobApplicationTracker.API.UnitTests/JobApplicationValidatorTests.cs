using Xunit;
using FluentValidation.TestHelper;
using JobApplicationTracker.API.Models;
using JobApplicationTracker.API.Validators;
using System;

public class JobApplicationValidatorTests
{
    private readonly JobApplicationValidator _validator;

    public JobApplicationValidatorTests()
    {
        _validator = new JobApplicationValidator();
    }

    // --- CompanyName Tests ---
    [Fact]
    public void CompanyName_ShouldHaveError_WhenEmpty()
    {
        var model = new JobApplication { CompanyName = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.CompanyName)
              .WithErrorMessage("Company Name value is required");
    }

    [Fact]
    public void CompanyName_ShouldHaveError_WhenTooShort()
    {
        var model = new JobApplication { CompanyName = "A" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.CompanyName)
              .WithErrorMessage("Company Name must be at least 2 characters long.");
    }

    [Fact]
    public void CompanyName_ShouldHaveError_WhenTooLong()
    {
        var model = new JobApplication { CompanyName = new string('A', 101) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.CompanyName)
              .WithErrorMessage("Company Name should not exceed 100 characters.");
    }

    [Fact]
    public void CompanyName_ShouldNotHaveError_WhenValid()
    {
        var model = new JobApplication { CompanyName = "Google" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.CompanyName);
    }

    // --- Position Tests ---
    [Fact]
    public void Position_ShouldHaveError_WhenEmpty()
    {
        var model = new JobApplication { Position = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Position)
              .WithErrorMessage("Position value is required");
    }

    [Fact]
    public void Position_ShouldHaveError_WhenTooShort()
    {
        var model = new JobApplication { Position = "A" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Position)
              .WithErrorMessage("Position must be at least 2 characters long.");
    }

    [Fact]
    public void Position_ShouldHaveError_WhenTooLong()
    {
        var model = new JobApplication { Position = new string('B', 101) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Position)
              .WithErrorMessage("Position should not exceed 100 characters.");
    }

    [Fact]
    public void Position_ShouldNotHaveError_WhenValid()
    {
        var model = new JobApplication { Position = "Senior Engineer" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Position);
    }

    // --- Status Tests ---
    [Fact]
    public void Status_ShouldNotHaveError_WhenValidEnumValue()
    {
        var model = new JobApplication { Status = ApplicationStatus.Applied };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Status);
    }

    [Fact]
    public void Status_ShouldHaveError_WhenInvalidEnumValue()
    {
        // Cast an integer outside the defined enum range
        var model = new JobApplication { Status = (ApplicationStatus)99 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Status)
              .WithErrorMessage("Status must be one of the following: Applied, Interviewing, Offered, Rejected.");
    }

    // --- DateApplied Tests ---
    [Fact]
    public void DateApplied_ShouldHaveError_WhenInTheFuture()
    {
        var model = new JobApplication { DateApplied = DateTime.Today.AddDays(1) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DateApplied)
              .WithErrorMessage("Date cannot be in the future");
    }

    [Fact]
    public void DateApplied_ShouldNotHaveError_WhenToday()
    {
        var model = new JobApplication { DateApplied = DateTime.Today };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.DateApplied);
    }

    [Fact]
    public void DateApplied_ShouldNotHaveError_WhenInThePast()
    {
        var model = new JobApplication { DateApplied = DateTime.Today.AddDays(-5) };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.DateApplied);
    }
}
