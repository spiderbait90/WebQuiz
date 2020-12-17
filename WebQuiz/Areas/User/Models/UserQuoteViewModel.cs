using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebQuiz.Data.Models;

namespace WebQuiz.Areas.User.Models
{
    public class UserQuoteViewModel
    {
        public int RndIndex;

        public ApplicationUser AppUser { get; set; }

        public Quote Quote { get; set; }

        public List<string> PossibleAnswers { get; set; }
    }
}
