namespace DalTest;
using DalApi;
using DO;




//creates a bunch of tasks engineers dependencies randomly
public static class Initialization
{
    // Constants for setting range limits and salary boundaries
    private const int MIN_ID = 200000000;
    private const int MAX_ID = 900000000;
    private const int LOW_SAL = 50;
    private const int HIGH_SAL = 300;


    //private static IDependency? s_dalDependency;
    //private static ITask? s_dalTask;
    //private static IEngineer? s_dalEngineer;
    private static IDal? s_dal;

    private static readonly Random s_rand = new();

    // Method to create engineers with random attributes

    private static void createEngineer()
    {
        string[] engineerNames =
        {
            "Edward","Boris","Moses","David","Joseph"
        };
        foreach (var _name in engineerNames)
        {
            int _id;
            do
                _id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dal!.Engineer.Read(_id) is not null);

            string _email = $"{_name.ToLower()}@eng.com";

            double _cost = s_rand.NextDouble() * (HIGH_SAL - LOW_SAL) + LOW_SAL;

            DO.EngineerExperience _level = (DO.EngineerExperience)s_rand.Next(4);

            Engineer newEng = new(_id, _email, _cost, _name, _level);

            s_dal!.Engineer.Create(newEng);

        }

    }

    // Method to create tasks with random attributes
    private static void createTask()
    {
        string[] taskAliases = new string[]
        {
     "SiteAssessment",
    "GeotechnicalSurvey",
    "PermittingProcess",
    "BridgeDesign",
    "EnvImpactAssessment",
    "MaterialProcurement",
    "FoundationExcavation",
    "PileDriving",
    "ConcreteFormwork",
    "ConcretePouring",
    "SteelStructureFabrication",
    "BridgeDeckInstallation",
    "AbutmentConstruction",
    "ExpansionJointInstallation",
    "TrafficManagement",
    "BridgeInspection",
    "WaterwayProtection",
    "QualityControlTesting",
    "LandscapingRestoration",
    "DocumentationHandover"
        };

        string[] taskDescriptions = new string[]
        {
    "Evaluate the construction site for the bridge.",
    "Conduct a survey to assess soil and subsurface conditions.",
    "Obtain necessary permits for bridge construction.",
    "Develop detailed engineering plans for the bridge.",
    "Assess and mitigate potential environmental impacts.",
    "Source and procure construction materials.",
    "Excavate the foundation for bridge supports.",
    "Install piles for foundation support.",
    "Set up formwork for concrete structures.",
    "Pour concrete for bridge components.",
    "Fabricate steel components for the bridge.",
    "Install the bridge deck.",
    "Construct abutments for bridge support.",
    "Install expansion joints for temperature variations.",
    "Implement traffic management during construction.",
    "Conduct regular inspections during construction.",
    "Implement measures to protect waterways during construction.",
    "Perform quality control tests on materials and structures.",
    "Restore the construction site and landscape.",
    "Compile project documentation and hand over the bridge."
        };
        string[] taskDeliverables = new string[]
{
    "Site assessment report.",
    "Geotechnical report.",
    "Approved permits.",
    "Bridge design documents.",
    "Environmental impact assessment report.",
    "Material inventory.",
    "Excavation completion report.",
    "Pile installation log.",
    "Completed formwork.",
    "Concrete curing log.",
    "Fabricated steel structures.",
    "Completed bridge deck.",
    "Completed abutments.",
    "Installed expansion joints.",
    "Traffic management plan.",
    "Inspection reports.",
    "Water protection plan.",
    "Quality control reports.",
    "Landscaping completion report.",
    "Project documentation."
};

        string[] taskRemarks = new string[]
        {
    "Consider environmental impact and soil stability.",
    "Critical for foundation design.",
    "Compliance with local regulations is crucial.",
    "Ensure structural integrity and safety.",
    "Comply with environmental regulations.",
    "Ensure materials meet engineering specifications.",
    "Follow geotechnical recommendations.",
    "Monitor pile integrity during installation.",
    "Ensure proper alignment and reinforcement.",
    "Follow engineering specifications.",
    "Quality control is essential.",
    "Ensure proper alignment and load-bearing capacity.",
    "Follow engineering plans for stability.",
    "Allow for bridge movement.",
    "Minimize disruptions and ensure safety.",
    "Identify and address any issues promptly.",
    "Prevent environmental damage.",
    "Ensure compliance with standards.",
    "Minimize the project's environmental impact.",
    "Ensure all records are complete for future reference."
        };
        for (int i = 0; i < 20; ++i)
        {

            string _alias = taskAliases[i];

            string _discription = taskDescriptions[i];


            //string _deliverables = taskDeliverables[i];
            //string _remarks = taskRemarks[i];
            DateTime _createdAtDate = DateTime.Now.AddDays(-s_rand.Next(365));


            DO.EngineerExperience _complexity = (DO.EngineerExperience)s_rand.Next(4);

            Task newTask = new(i, _alias, _discription, _createdAtDate, _complexity);

            s_dal!.Task.Create(newTask);

        }

    }


    // Method to create dependencies between tasks
    private static void createDependency()
    {

        for (int i = 0; i < 40; ++i)
        {

            int _dependentTask;
            do
                _dependentTask = s_rand.Next(1010, 1020);
            while (s_dal!.Task.Read(_dependentTask) is null);

            int _dependentOnTask;
            do
                _dependentOnTask = s_rand.Next(1000, 1010);
            while (s_dal!.Task.Read(_dependentOnTask) is null || _dependentOnTask >= _dependentTask);

            Dependency newDep = new(i, _dependentTask, _dependentOnTask);

            s_dal!.Dependency.Create(newDep);

        }
    }


    // Main method to initiate the data creation process
    public static void Do(IDal dal)
    {
        //s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        //s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        //s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2

        createTask();
        createEngineer();
        createDependency();
        Console.WriteLine("create dependency done");


    }



}
