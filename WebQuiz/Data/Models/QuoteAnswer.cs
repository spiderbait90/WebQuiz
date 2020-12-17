using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebQuiz.Data.Models
{
    public class QuoteAnswer
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Author { get; set; }

        public string AnswerAuthor { get; set; }
    }
}
