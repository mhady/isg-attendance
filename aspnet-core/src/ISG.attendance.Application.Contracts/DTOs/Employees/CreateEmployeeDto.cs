using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISG.attendance.DTOs.Employees
{
    public class CreateEmployeeDto : IValidatableObject
    {
        [Required]
        [StringLength(256)]
        public string FullName { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        public bool CreateUserAccount { get; set; } = false;

        [StringLength(256)]
        public string Password { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string EmployeeCode { get; set; }

        public Guid? LocationId { get; set; }

        public bool IsActive { get; set; } = true;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CreateUserAccount)
            {
                // When creating user account, email and password are required
                if (string.IsNullOrWhiteSpace(Email))
                {
                    yield return new ValidationResult(
                        "Email is required when creating a user account",
                        new[] { nameof(Email) });
                }
                else
                {
                    // Validate email format only when provided
                    var emailAttribute = new EmailAddressAttribute();
                    if (!emailAttribute.IsValid(Email))
                    {
                        yield return new ValidationResult(
                            "Invalid email format",
                            new[] { nameof(Email) });
                    }
                }

                if (string.IsNullOrWhiteSpace(Password))
                {
                    yield return new ValidationResult(
                        "Password is required when creating a user account",
                        new[] { nameof(Password) });
                }
            }
            else
            {
                // When not creating user account, validate email format only if provided
                if (!string.IsNullOrWhiteSpace(Email))
                {
                    var emailAttribute = new EmailAddressAttribute();
                    if (!emailAttribute.IsValid(Email))
                    {
                        yield return new ValidationResult(
                            "Invalid email format",
                            new[] { nameof(Email) });
                    }
                }
            }
        }
    }
}
