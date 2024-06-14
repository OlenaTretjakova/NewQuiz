using System.Collections.Generic;

namespace MyQuiz.Classes
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Question> Questions { get; set; } = new List<Question>();
    }
}