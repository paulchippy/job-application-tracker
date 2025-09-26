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
        // ARRANGE: Instantiate the validator once for all tests
        _validator = new JobApplicationValidator();
    }

    // --- CompanyName Tests ---

    [Fact]
    public void CompanyName_ShouldHaveError_WhenEmpty()
    {
        // ACT & ASSERT
        var model = new JobApplication { CompanyName = "" };
        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.CompanyName)
              .WithErrorMessage("Company Name is required");
    }

    [Fact]
    public void CompanyName_ShouldHaveError_WhenExceedsMaxLength()
    {
        // ACT & ASSERT
        var model = new JobApplication { CompanyName = new string('A', 101) };
        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.CompanyName);
        // Note: FluentValidation doesn't use the custom message for MaxLength unless specified
    }

    [Fact]
    public void CompanyName_ShouldNotHaveError_WhenValid()
    {
        // ACT & ASSERT
        var model = new JobApplication { CompanyName = "Google" };
        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.CompanyName);
    }

    // --- Position Tests ---

    [Fact]
    public void Position_ShouldHaveError_WhenEmpty()
    {
        // ACT & ASSERT
        var model = new JobApplication { Position = "" };
        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Position)
              .WithErrorMessage("Position is required");
    }

    // ... Add Position MaxLength test similar to CompanyName ...

    // --- Status Tests (Assuming ApplicationStatus is your enum) ---

    [Fact]
    public void Status_ShouldNotHaveError_WhenValidEnumValue()
    {
        // ARRANGE: Use a valid enum value (assuming 1 is a valid enum value like Submitted)
        var model = new JobApplication { Status = (ApplicationStatus)1 };

        // ACT & ASSERT
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Status);
    }

    [Fact]
    public void Status_ShouldHaveError_WhenInvalidEnumValue()
    {
        // ARRANGE: Use an integer that is NOT a defined enum value (e.g., 99)
        var model = new JobApplication { Status = (ApplicationStatus)99 };

        // ACT & ASSERT
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Status)
              .WithErrorMessage("Invalid status");
    }

    // --- DateApplied Tests ---

    [Fact]
    public void DateApplied_ShouldHaveError_WhenInTheFuture()
    {
        // ARRANGE: Set the date to tomorrow
        var model = new JobApplication { DateApplied = DateTime.Today.AddDays(1) };

        // ACT & ASSERT
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DateApplied)
              .WithErrorMessage("Date cannot be in the future");
    }

    [Fact]
    public void DateApplied_ShouldNotHaveError_WhenToday()
    {
        // ARRANGE
        var model = new JobApplication { DateApplied = DateTime.Today };

        // ACT & ASSERT
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.DateApplied);
    }

    [Fact]
    public void DateApplied_ShouldNotHaveError_WhenInThePast()
    {
        // ARRANGE
        var model = new JobApplication { DateApplied = DateTime.Today.AddDays(-5) };

        // ACT & ASSERT
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.DateApplied);
    }
}