using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MyQuiz.Classes
{
    public class QuizService : IQuizSerice
    {
        public void AddQuiz()
        {
            string userName = EnterNameUser();
            string password = EnterPassword();
            using (QuizDb db = new QuizDb())
            {
                Autorisation autorisation = new Autorisation();
                if (!autorisation.IsUserAdmin(userName, password))
                {
                    Console.WriteLine("\r\tYou can not to creat the quiz!");
                    return;
                }
                string quizesName = EnterNameQuiz();
                CreateQuiz(quizesName);
                int countQuestions = 0;

                do
                {
                    string questionText = EnterTexQuestion();
                    CreateQuestion(quizesName, questionText);
                    int countAnswers = 0;
                    countQuestions++;
                    do
                    {
                        string answerText = EnterTextAnswer();
                        bool isTrueAnswer = EnterIsTrueAnswer();
                        CreateAnswer(quizesName, questionText, answerText, isTrueAnswer);
                        countAnswers++;
                    } while (countAnswers < 3);
                } while (countQuestions < 7);

                db.SaveChanges();
            }
        }
        public void CreateQuestion(string quizesName, string questionText)
        {
            using (QuizDb db = new QuizDb())
            {
                var quiz = db.Quizes.FirstOrDefault(qz => qz.Name == quizesName);
                if (quiz == null)
                {
                    Console.WriteLine($"\r\tWill create a quiz with name {quizesName} from scratch");
                    return;
                }

                Question question1 = new Question();
                question1.Text = questionText;
                question1.QuizId = quiz.Id;
                db.Questions.Add(question1);
                db.SaveChanges();
            }
        }
        public void CreateQuiz(string quizesName)
        {
            using (QuizDb db = new QuizDb())
            {
                var quiz = db.Quizes.FirstOrDefault(qu => qu.Name == quizesName);
                if (quiz != null)
                {
                    Console.WriteLine($"The quiz {quizesName} already exists.");
                    return;
                }
                Quiz q = new Quiz();
                q.Name = quizesName;

                db.Quizes.Add(q);
                db.SaveChanges();
            }
        }
        public void CreateAnswer(string quizName, string questionText, string textAnswer, bool isTrue)
        {
            using (QuizDb db = new QuizDb())
            {
                var quiz = db.Quizes.FirstOrDefault(q => q.Name == quizName);
                if (quiz == null)
                {
                    Console.WriteLine($"\r\tThe Quiz '{quizName}' was not found.");
                    return;
                }

                var question = db.Questions.FirstOrDefault(q1 => q1.QuizId == quiz.Id && q1.Text == questionText);
                if (question == null)
                {
                    Console.WriteLine($"\r\tThe Question '{questionText}' was not found.");
                    return;
                }

                var answers = db.Answers.FirstOrDefault(a => a.QuestionId == question.Id && a.Text== textAnswer);
                if (answers != null)
                {
                    Console.WriteLine($"\r\tThe answers  '{questionText}' alrediy exsist.");
                    return;
                }
                Answer answer = new Answer();
                answer.Text = textAnswer;
                answer.QuestionId = question.Id;
                answer.IsTrue = isTrue;
                db.Answers.Add(answer);
                db.SaveChanges();
            }
        }
        public string EnterNameUser()
        {
            Console.Write("\r\tEnter your name: ");
            return Console.ReadLine();
        }
        public string EnterPassword()
        {
            Console.Write("\r\tEnter your password: ");
            return Console.ReadLine();
        }
        public string EnterTexQuestion()
        {
            Console.Write("\r\tEnter text question: ");
            return Console.ReadLine();
        }
        public string EnterNameQuiz()
        {
            Console.Write("\r\tEnter the name of quiz: ");
            return Console.ReadLine();
        }
        public string EnterTextAnswer()
        {
            Console.Write("\r\tEnter text answer: ");
            return Console.ReadLine();
        }
        public bool EnterIsTrueAnswer()
        {
            Console.Write("\r\tEnter true if the answer true: ");
            string isTrue = Console.ReadLine();
            return isTrue == "true";
        }
        public void ShowQuizesName()
        {
            using (QuizDb db = new QuizDb())
            {
                var names = db.Quizes.ToList();
                int countQuizes = 1;
                foreach (var name in names)
                {
                    Console.WriteLine($"\r\t{countQuizes}. {name.Name}");
                    countQuizes++;
                }
            }
        }
        public string GetQuizByNumber(int number)
        {
            using (QuizDb db = new QuizDb())
            {
                var names = db.Quizes.ToList();

                if (names == null || number < 1 || number > names.Count)
                {
                    Console.WriteLine("\r\tEnter correct number.");
                    return null;
                }

                return names[number - 1].Name;
            }
        }

        public int GetNumberQuiz()
        {
            Console.WriteLine("\r\tEnter quizes number:");
            return int.Parse(Console.ReadLine());
        }
        public void PassingQuiz(string name, string password, int numQuiz)
        {
            //ShowQuizesName();
            numQuiz = GetNumberQuiz();

            string nameQuiz = GetQuizByNumber(numQuiz);
            using (QuizDb db = new QuizDb())
            {
                int result = 0;
                int rightAnswersToQuestion = 0;

                var quiz = db.Quizes.FirstOrDefault(q => q.Name == nameQuiz);
                if (quiz == null)
                {
                    Console.WriteLine($"\r\tQuiz {nameQuiz} was not found.");
                    return;
                }

                var questionsWithAnswers = db.Questions
                    .Where(q => q.QuizId == quiz.Id)
                    .Select(q => new
                    {
                        QuestionText = q.Text,
                        Answers = db.Answers
                                    .Where(a => a.QuestionId == q.Id)
                                    .Select(a => new { a.Text, a.IsTrue })
                                    .ToList()
                    }).ToList();

                foreach (var questionWithAnswers in questionsWithAnswers)
                {
                    Console.WriteLine($"\r\tQuestion: {questionWithAnswers.QuestionText}");
                    Console.WriteLine();
                    int answerNumber = 1;

                    foreach (var answer in questionWithAnswers.Answers)
                    {
                        Console.WriteLine($"\r\t{answerNumber}. {answer.Text}");
                        answerNumber++;
                    }

                    Console.Write("\r\tEnter your choices (comma separated numbers): ");
                    string userChoices = Console.ReadLine();
                    var chosenNumbers = userChoices.Split(',')
                        .Select(str => int.TryParse(str, out int num) ? num : -1)
                        .Where(num => num > 0 && num <= questionWithAnswers.Answers.Count)
                        .ToList();
                    rightAnswersToQuestion += questionWithAnswers.Answers.Count(a => a.IsTrue);

                    foreach (var chosenNumber in chosenNumbers)
                    {
                        if (questionWithAnswers.Answers[chosenNumber - 1].IsTrue)
                        {
                            result++;
                        }
                    }

                    Console.WriteLine();
                }
                var user = db.Roles.Where(r => r.Name == name && r.Password == password).FirstOrDefault();
                var resultUser = db.Results.FirstOrDefault(r => r.RoleId == user.Id);
                if (resultUser == null)
                {
                    resultUser = new Result
                    {
                        RoleId = user.Id,
                        NumMaxBalls = 0,
                        NumGetingBalls = 0,
                        LastDatePassing = DateTime.Now
                    };
                    db.Results.Add(resultUser);
                }

                resultUser.NumMaxBalls += rightAnswersToQuestion;
                resultUser.NumGetingBalls += result;
                resultUser.LastDatePassing = DateTime.Now.Date;

                db.SaveChanges();

                Console.WriteLine($"\r\tYour result is {result} out of {rightAnswersToQuestion}.");
            }

        }
        public List<string> GetNamesMixQuiz()
        {
            //ShowQuizesName();
            List<string> names = new List<string>();

            Console.WriteLine("\r\tEnter the numbers of quizes you want to pass by commas:");
            string quizesNumberStr = Console.ReadLine();
            List<string> quizesNumberList = quizesNumberStr.Split(',').Select(item => item.Trim()).ToList();

            foreach (var item in quizesNumberList)
            {
                if (!int.TryParse(item, out int num))
                {
                    Console.WriteLine("Enter numbers.");
                    return names;
                }
                else if (int.Parse(item) > quizesNumberList.Count() || int.Parse(item) < 1)
                {
                    Console.WriteLine($"Enter number from 1 to {quizesNumberList.Count()}.");
                    return names;
                }
                names.Add(GetQuizByNumber(int.Parse(item)));
            }

            return names;
        }
        public void CreateMixQuiz(List<string> namesList)
        {
            Random rand = new Random();

            using (QuizDb db = new QuizDb())
            {

                CreateQuiz("mix");
                int countAnswers = 0;

                while (countAnswers < 7)
                {

                    int numQuiz = rand.Next(0, namesList.Count);
                    string nameQuiz = namesList[numQuiz];

                    var quiz = db.Quizes.FirstOrDefault(q => q.Name == nameQuiz);

                    var questions = db.Questions.Where(q => q.QuizId == quiz.Id).ToList();

                    int numQuestion = rand.Next(0, 7);
                    var question = questions[numQuestion];
                    if (db.Questions.Any(q => q.Text == question.Text && q.Quiz.Name == "mix")) continue;
                    var answers = db.Answers.Where(a => a.QuestionId == question.Id).ToList();

                    CreateQuestion("mix", question.Text);
                    countAnswers++;

                    foreach (var answer in answers)
                    {
                        CreateAnswer("mix", question.Text, answer.Text, answer.IsTrue);
                    }

                }

                db.SaveChanges();
            }
        }
        public void PassMixQuiz(string name, string password)
        {
            List<string> names = GetNamesMixQuiz();
            CreateMixQuiz(names);

            using (QuizDb db = new QuizDb())
            {
                int result = 0;
                var quiz = db.Quizes.FirstOrDefault(q => q.Name == "mix");

                var questionsWithAnswers = db.Questions
                    .Where(q => q.QuizId == quiz.Id)
                    .Select(q => new
                    {
                        QuestionText = q.Text,
                        Answers = db.Answers
                                    .Where(a => a.QuestionId == q.Id)
                                    .Select(a => new { a.Text, a.IsTrue })
                                    .ToList()
                    }).ToList();
                var rightAnswersToQuestions = 0;
                foreach (var questionWithAnswers in questionsWithAnswers)
                {
                    Console.WriteLine($"\r\tQuestion: {questionWithAnswers.QuestionText}");
                    Console.WriteLine();
                    int answerNumber = 1;

                    foreach (var answer in questionWithAnswers.Answers)
                    {
                        Console.WriteLine($"\r\t{answerNumber}. {answer.Text}");
                        answerNumber++;
                    }

                    Console.Write("\r\tEnter your choices (comma separated numbers): ");
                    string userChoices = Console.ReadLine();
                    var chosenNumbers = userChoices.Split(',')
                        .Select(str => int.TryParse(str, out int num) ? num : -1)
                        .Where(num => num > 0 && num <= questionWithAnswers.Answers.Count)
                        .ToList();
                    rightAnswersToQuestions += questionWithAnswers
                                                 .Answers.Where(a => a.IsTrue)
                                                 .Select(a => a.IsTrue == true).Count();

                    foreach (var chosenNumber in chosenNumbers)
                    {
                        if (questionWithAnswers.Answers[chosenNumber - 1].IsTrue)
                        {
                            result++;
                        }
                    }
                    var user = db.Roles.Where(r => r.Name == name && r.Password == password).FirstOrDefault();
                    var resultUser = db.Results.Where(r => r.RoleId == user.Id).FirstOrDefault();
                    resultUser.NumMaxBalls += rightAnswersToQuestions;
                    resultUser.NumGetingBalls += result;
                    resultUser.LastDatePassing = DateTime.Now;
                    db.SaveChanges();


                    Console.WriteLine();
                }
                Console.WriteLine($"\r\tYour result is {result} out of {rightAnswersToQuestions}.");
            }

            DeleteQuiz("mix");
        }

        public void DeleteQuiz(string quizesName)
        {
            using (QuizDb db = new QuizDb())
            {
                var quiz = db.Quizes.FirstOrDefault(q => q.Name == quizesName);
                var questions = db.Questions.Where(q => q.QuizId == quiz.Id).ToList();
                foreach (var item in questions)
                {
                    var answers = db.Answers.Where(a => a.QuestionId == item.Id).ToList();
                    foreach (var answer in answers)
                    { 
                        db.Answers.Remove(answer);
                    }
                    db.Questions.Remove(item);
                }
                db.Quizes.Remove(quiz);
                db.SaveChanges();
                //Console.WriteLine($"Quiz {quizesName} was removed.");
            }
        }
    }
}
