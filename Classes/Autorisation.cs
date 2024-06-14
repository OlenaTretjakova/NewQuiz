using System;
using System.Linq;

namespace MyQuiz.Classes
{
    internal class Autorisation : IAutorisation
    {
        public void AddUser(string name, string pasword)
        {
            using (QuizDb qd = new QuizDb())
            {
                Role role = new Role();
                role.Name = name;
                role.Password = pasword;
                role.IsAdmin = false;
                qd.Roles.Add(role);
                qd.SaveChanges();
            }
            Console.WriteLine($"\r\tUser {name} was registered.");
        }

        public void BecomeAdmin(string name, string password)
        {
            using (QuizDb qd = new QuizDb())
            {
                var person = qd.Roles.FirstOrDefault(x => x.Name == name && x.Password == password);
                if (person == null)
                {
                    Console.WriteLine("\r\tThere is no person with such data in the database");
                    return;
                }

                person.IsAdmin = true;
                qd.SaveChanges();

                Console.WriteLine($"\r\t{name} has been already admin.");
            }
        }
        public void EnterUserInfo(string name, string password)
        {
            using (QuizDb qd = new QuizDb())
            {
                var user = qd.Roles.FirstOrDefault(u => u.Name == name);
                if (user != null)
                {
                    Console.WriteLine($"\r\tUser {name} alredy exsist.");
                    return;
                }
                Role role = new Role();
                role.Name = name;
                role.Password = password;

                qd.Roles.Add(role);
                qd.SaveChanges();

                Console.WriteLine($"\t\r New user {name} was added.");

            }
        }

        public void CheckUserInfo(string name, string password)
        {
            using (QuizDb qd = new QuizDb())
            {
                var person = qd.Roles.FirstOrDefault(x => x.Name == name && x.Password == password);
                if (person == null)
                {
                    Console.WriteLine("\r\tThere is no person with such data in the database.");
                }
                else
                {
                    Console.WriteLine($"\t\tHi, {name} you entered on your account.");

                }
            }
        }

        public bool IsUserAdmin(string name, string password)
        {
            using (QuizDb qd = new QuizDb())
            {
                var person = qd.Roles.FirstOrDefault(x => x.Name == name && x.Password == password);
                if (person == null)
                {
                    Console.WriteLine("\r\tThere is no  with such data in the database.");
                    return false;

                }
                person = qd.Roles.FirstOrDefault(x => x.Name == name && x.Password == password && x.IsAdmin == true);
                if (person == null)
                {
                    Console.WriteLine($"\r\t{name} is not admin.");
                    return false;
                }
                Console.WriteLine($"\r\t{name} is  admin.");
                return true;
            }
        }
    }
}
