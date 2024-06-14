using System.Data.Entity;

namespace MyQuiz.Classes
{
    public class QuizDb : DbContext
    {
        public QuizDb() : base("QuizDatabase") { }
        public DbSet<Quiz> Quizes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Result> Results { get; set; }

    }
}
