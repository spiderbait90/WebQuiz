using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Serialization;

namespace WebQuiz.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public override string Email { get; set; }

        [Required]
        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$", ErrorMessage = "Not a valid Number")]
        public override string PhoneNumber { get; set; } = string.Empty;

        public Settings Settings { get; set; }

        public List<QuoteAnswer> AnsweredQuotes { get; set; }
    }

    public enum Settings
    {
        Binary = 1,
        Multiple = 2
    }
}
