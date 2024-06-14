using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyQuiz.Classes
{
    interface  IAutorisation
    {
        void AddUser(string name, string pasword);
        void CheckUserInfo(string name, string password);
        void EnterUserInfo(string name, string password);
        void BecomeAdmin(string name, string password);
        bool IsUserAdmin(string name, string password);


    }
}
