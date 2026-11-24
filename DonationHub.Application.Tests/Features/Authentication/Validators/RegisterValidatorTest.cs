using DonationHub.Application.Features.Authentication.Register;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Tests.Features.Authentication.Validators
{
    public class RegisterValidatorTest
    {
        private readonly RegisterValidator _validator = new();
        private static RegisterCommand CreateValidRgisterComand()
        {
            return new RegisterCommand("TEST", "test@gmail.com", "P@ssw0rd", "TESTTEST");

        }
        [Theory]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("abcdefgh")]   // Missing uppercase and digit
        [InlineData("ABCDEFGH")]   // Missing lowercase and digit
        [InlineData("Abcdefgh")]   // Missing digit
        [InlineData("Abcdefg1")]   // Valid (don't use as invalid)
        [InlineData("12345678")]   // Missing uppercase and lowercase
        [InlineData("ABCDEFG1")]   // Missing lowercase
        [InlineData("abcdefg1")]   // Missing uppercase
        public void Validate_WhenPasswordBeNotValid(string password)
        {
            // Arrange
            var command = CreateValidRgisterComand();
            command.Password = "Passw0rd";

            // Act 
            var actual = _validator.TestValidate(command);


            // Assert
            actual.ShouldHaveValidationErrorFor(x => x.Password);



        }

    }
}
