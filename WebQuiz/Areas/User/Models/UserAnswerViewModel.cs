using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebQuiz.Data.Models;

namespace WebQuiz.Areas.User.Models
{
    public class UserAnswerViewModel
    {
        public ApplicationUser User { get; set; }
        public List<QuoteAnswer> UserAnswers { get; set; }
    }
}
