using System.ComponentModel.DataAnnotations;

namespace HiSign.Application.DTOs.Account
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string TaxCode { get; set; }
        public string BankAccount { get; set; }
        public string BusinessLicense { get; set; }
    }
}
