using System.ComponentModel.DataAnnotations;

namespace WebQuiz.Data.Models
{
    public class Quote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string Author { get; set; }
    }
}
