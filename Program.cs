using MyQuiz.Classes;
using System;

namespace MyQuiz
{
    internal class Program
    {
        static void Main(string[] args)
        {

            QuizService quizService = new QuizService();
            Managment managment = new Managment();
            managment.HeadMenu();
            Console.ReadLine();
        }
    }
}
