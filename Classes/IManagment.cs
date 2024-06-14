namespace MyQuiz.Classes
{
    interface IManagment
    {
        void HeadMenu();
        void UsersMenu(string name, string password);
        void AdminsMenu(string name, string password);
    }
}
