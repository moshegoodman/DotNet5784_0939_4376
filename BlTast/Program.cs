namespace BlTest;


internal class Program
{
    readonly string s_engineers_xml = "engineers";
    static string s_data_config_xml = "data-config";



    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    private static void MainMenu()
    {
        int a;
        do
        {
            Console.WriteLine("enter entity to check");
            Console.WriteLine("enter 0 to leave main menu");
            Console.WriteLine("enter 1 to check Tasks ");
            Console.WriteLine("enter 2 to check Engineers ");
            Console.WriteLine("enter 3 to check first stage ");
            Console.WriteLine("enter 4 to check second stage ");
            Console.WriteLine("enter 5 to check third stage ");
            // Console.WriteLine("enter 4 for Data initiazation settings");
        } while (true);
    }


    private static void TaskMenu()
    {
        Console.WriteLine("Enter 1 to exit the task menu");
        Console.WriteLine("Enter 2 to add a new task ");
        Console.WriteLine("Enter 3 to delete a task ");
        Console.WriteLine("Enter 4 to designate engineer ");
        Console.WriteLine("Enter 5 to read a task ");
        Console.WriteLine("Enter 6 to read all tasks");
        Console.WriteLine("Enter 7 to update a task");
        Console.WriteLine("Enter 8 to update a task schduled date");
        Console.WriteLine("Enter 9 to Get 'Engineer In Task'  ");
        Console.WriteLine("Enter 10 to get list of dependent tasks");
        Console.WriteLine("Enter 11 to get  task's status");
        try
        {
            switch (a)
            {
                case 1:
                    return;
                case 2:
                    TaskCreate();
                    break;
                case 3
                    TaskDelete();
                    break;
                case 4:
                    TaskDesignateEngineer();
                    break;

                case 5:
                    TaskRead();
                    break;
                case 6:
                    TaskReadAll();
                    break;
                case 7:
                    TaskUpdate();
                    break;
                case 8:
                    TaskUpdateSchuledDate();
                    break;
                case 9:
                    TaskUpdate();
                    break;
                case 10:
                    TaskUpdate();
                    break;
                case 11:
                    TaskUpdate();
                    break;
                default:
                    Console.WriteLine("enter a number between 0 and 6");
                    break;
            }
        }
        catch (Exception err) { Console.WriteLine(err); }
    } while (true);
    }



static void Main(string[] args)
{
    Console.Write("Would you like to create Initial data? (Y/N)");
    string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
    if (ans == "Y")
        DalTest.Initialization.Do();
}
}

