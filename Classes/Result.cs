using System;

namespace MyQuiz.Classes
{
    public class Result
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int NumGetingBalls { get; set; } = 0;
        public int NumMaxBalls { get; set; } = 0;
        public DateTime LastDatePassing { get; set; }
        public virtual Role Role { get; set; }
    }
}
