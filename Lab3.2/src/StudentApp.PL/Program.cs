// StudentApp.PL/Program.cs
using StudentApp.BLL.Services;
using StudentApp.MenuLogic;

namespace StudentApp.PL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            IStudentService studentService = new StudentService();

            Menu menu = new Menu(studentService);

            menu.MainMenu();
        }
    }
}