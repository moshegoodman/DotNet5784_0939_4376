using Dal;
using DalApi;

namespace DalTest
{
    internal class Program
    {
        private void MenuTasks()
        {

            Console.WriteLine("Enter 1 to exit the main menu");
            Console.WriteLine("Enter 2 to add a new task ");
            Console.WriteLine("Enter 3 to read a task ");
            Console.WriteLine("Enter 4 to read all tasks ");
            Console.WriteLine("Enter 5 to update a task ");
            Console.WriteLine("Enter 6 to delete a task");
            string a = Console.ReadLine();
            do
                switch (a)
                {
                    case "2": TaskCreate();
                } while (true)
        }
        private void TaskCreate()
        {
            string _alias = Console.ReadLine();
            string _description = Console.ReadLine();
            //DateTime _createdAtDate=
            bool _isMilestone = Console.ReadLine();


        }
        private void TaskRead()
        {

        }
        private void TaskReadAll()
        {

        }
        private void TaskUpdate()
        {

        }
        private void TaskDelete()
        {

        }



        private void MenuDependents()
        {

        }
        private void MenuEngineers()
        {

        }
        private static ITask? s_dalTask = new TaskImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();
        private static IEngineer? s_dalLinks = new EngineerImplementation();

        public
        static void Main(string[] args)
        {


            try
            {
                Initialization.Do(s_dalTask, s_dalDependency, s_dalLinks);

            }
            catch (Exception a) { Console.WriteLine(a); }
        }

        private int main_menu()
        {
            Console.WriteLine("enter entity to check");
            Console.WriteLine("enter 0 to leave main menu");
            Console.WriteLine("enter 1 to check Tasks ");
            Console.WriteLine("enter 2 to check Dependencies ");
            Console.WriteLine("enter 3 to check Engineers ");
            string a = Console.ReadLine()!;

            do
                switch (a)
                {
                    case "1": MenuTasks();
                    case "2": MenuDependents();
                    case "3": MenuEngineers();
                    case "0": return 0;
                }
            while (true);
        }

    }
}



