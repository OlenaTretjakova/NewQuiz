using System.Collections.Generic;

namespace MyQuiz.Classes
{
    interface IQuizSerice
    {
        void AddQuiz();
        void CreateQuestion(string quizesName, string Text);
        void CreateQuiz(string name);
        void CreateAnswer(string quizName, string questionText, string textAnser, bool isTrue);
        string EnterNameUser();
        string EnterPassword();
        string EnterTexQuestion();
        string EnterNameQuiz();
        string EnterTextAnswer();
        bool EnterIsTrueAnswer();
        void ShowQuizesName();
        string GetQuizByNumber(int number);
        int GetNumberQuiz();
        List<string> GetNamesMixQuiz();
        void CreateMixQuiz(List<string> namesList);
        void PassMixQuiz(string name, string password);
        void DeleteQuiz(string quizesName);
        void PassingQuiz(string name, string password, int numQuiz);
    }
}
