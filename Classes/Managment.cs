using System;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace MyQuiz.Classes
{
    internal class Managment : IManagment
    {
        public void HeadMenu()
        {
            Autorisation autorisation = new Autorisation();

            Console.WriteLine();
            Console.WriteLine("\t_________________");
            Console.WriteLine();
            Console.WriteLine("\t***  Quizes  ***");
            Console.WriteLine("\t_________________");
            Console.WriteLine();
            Console.WriteLine("\tMain Menu:");
            Console.WriteLine();

            Console.Write("\r\tEnter your name:");
            string name = Console.ReadLine();

            Console.Write("\r\tEnter your password:");
            string password = Console.ReadLine();
            bool finish = true;

            Autorisation aut = new Autorisation();
            aut.CheckUserInfo(name, password);

            using (QuizDb quiz = new QuizDb())
            {
                var user = quiz.Roles.FirstOrDefault(u => u.Name == name && u.Password == password);
                if (user == null)
                {
                    Console.WriteLine("\tDo you want to registr? Select Yes/no.");
                    string answer = Console.ReadLine();
                    if (answer == "yes")
                    {
                        aut.AddUser(name, password);
                    }
                    else
                    {
                        Console.WriteLine($"Goodbye {name}!");
                    }
                }
            }

            if (!aut.IsUserAdmin(name, password))
            {
                UsersMenu(name, password);
            }
            else
            {
                AdminsMenu(name, password);
            }
        }

        public void UsersMenu(string name, string password)
        {
            QuizService service = new QuizService();

            Console.WriteLine();
            Console.WriteLine("\t_________________");
            Console.WriteLine();
            Console.WriteLine("\t***  User Menu  ***");
            Console.WriteLine("\t_________________");
            bool finish = true;

            while (finish)
            {
                Console.WriteLine();
                Console.WriteLine("\t1. Pass quiz:");
                Console.WriteLine("\t2. Table the best results:");
                Console.WriteLine("\t3. Finish.");
                Console.WriteLine();
                Console.WriteLine("\t\rSelect a menu item:");
                string userChoice = Console.ReadLine();
                switch (userChoice)
                {
                    case "1":

                        service.ShowQuizesName();
                        string userChoice1 = Console.ReadLine();
                        if (userChoice1.Count() > 1)
                        {
                            service.PassMixQuiz(name,password);
                        }
                        else
                        {
                            service.PassingQuiz(name, password,int.Parse(userChoice1));
                        }
                        break;
                    case "2":
                        GetTabeleBestResults();
                        break;

                    case "3":
                        finish = false;
                        break;
                    default:
                        break;
                }
            }
        }
        public void AdminsMenu(string name, string password)
        {
            Console.WriteLine();
            Console.WriteLine("\t_________________");
            Console.WriteLine();
            Console.WriteLine("\t***  Admin Menu  ***");
            Console.WriteLine("\t_________________");
            Console.WriteLine();
            QuizService q = new QuizService();

            bool finish = true;
            while (finish)
            {
                Console.WriteLine();
                Console.WriteLine("\t1. Create Quiz:");
                Console.WriteLine("\t2. Create Question:");
                Console.WriteLine("\t3. Create Answer:");
                Console.WriteLine("\t4. Delete Quize:");
                Console.WriteLine("\t5. Pass Quiz:");
                Console.WriteLine("\t6. Best results:");
                Console.WriteLine("\t7. Main menu:");
                Console.WriteLine("\t8. Finish.");

                string userChoice = Console.ReadLine();
                switch (userChoice)
                {
                    case "1":
                        Console.WriteLine("\r\tEnter quiz name:");
                        string nameQuiz = Console.ReadLine();
                        q.CreateQuiz(nameQuiz);
                        break;
                    case "2":
                        Console.WriteLine("\r\tEnter quiz name:");
                        string name1 = Console.ReadLine();
                        Console.WriteLine("\r\tEnter questions text:");
                        string questionsText1 = Console.ReadLine();
                        q.CreateQuestion(name1, questionsText1);
                        break;
                    case "3":
                        Console.WriteLine("\r\tEnter quiz name:");
                        string name2 = Console.ReadLine();
                        Console.WriteLine("\r\tEnter questions text:");
                        string questionsText2 = Console.ReadLine();
                        Console.WriteLine("\r\tEnter answers text:");
                        string answer3 = Console.ReadLine();
                        Console.WriteLine("\r\tEnter true or false:");
                        string isTrueStr = Console.ReadLine();
                        bool isTrueBool = (isTrueStr == "true") ? true : false;
                        q.CreateAnswer(name2, questionsText2, answer3, isTrueBool);
                        break;
                    case "4":
                        Console.WriteLine("\r\tEnter quiz name:");
                        string name3 = Console.ReadLine();
                        q.DeleteQuiz(name3);
                        break;
                    case "5":
                        q.ShowQuizesName();
                        string userChoice1 = Console.ReadLine();
                        if (userChoice1.Count() > 1)
                        {
                            q.PassMixQuiz(name, password);
                        }
                        else
                        {
                            q.PassingQuiz(name, password, int.Parse(userChoice1));
                        }
                        break;
                    case "6":
                        GetTabeleBestResults();
                        break;
                    case "7":
                        finish = false;
                        HeadMenu();
                        break;
                    case "8":
                        finish = false;
                        break;
                    default:
                        break;
                }
            }

        }

        public void GetTabeleBestResults()
        {
            using(QuizDb db = new QuizDb())
            {
                int count = 1;
                var nameUsers = db.Roles;
                var bestsResults = db.Results.OrderBy(r => r.NumGetingBalls/r.NumMaxBalls)
                    .Take(20)
                    .ToList();
                Console.WriteLine("\t***Smartests***");
                Console.WriteLine("\t________________");

                foreach (var best in bestsResults)
                {
                    Console.WriteLine($"\t{count}.{best.Role.Name} right:{best.NumGetingBalls} from:{best.NumMaxBalls} last date of visit:{best.LastDatePassing}.");
                    count++;
                }
                Console.WriteLine("\t________________");
            }
        }
    }
}
