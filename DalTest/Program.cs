using Dal;
using DalApi;

namespace DalTest
{
    internal class Program
    {
        private static ITask? s_dalTask = new TaskImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();
        private static IEngineer? s_dalLinks = new EngineerImplementation();

        public 
        static void Main(string[] args)
        {


            try
            {
                Do();
            }
            catch (Exception a) { Console.WriteLine(a); }
        }

    }
}