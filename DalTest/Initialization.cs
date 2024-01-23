namespace DalTest;
using DalApi;
using DO;
using System.Xml.Linq;




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

    private static void CreateEngineer()
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
    private static void CreateTask()
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
        string[] taskDeliverables = new[]
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

        string[] taskRemarks = new[]
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
    private static void CreateDependency()
    {
        int[,] taskDependencies = new int[,]
    {
    {1003, 1002},  // BridgeDesign depends on PermittingProcess
    {1004, 1003},  // EnvImpactAssessment depends on BridgeDesign
    {1005, 1004},  // MaterialProcurement depends on EnvImpactAssessment
    {1006, 1005},  // FoundationExcavation depends on MaterialProcurement
    {1007, 1006},  // PileDriving depends on FoundationExcavation
    {1008, 1007},  // ConcreteFormwork depends on PileDriving
    {1009, 1008},  // ConcretePouring depends on ConcreteFormwork
    {1010, 1009},  // SteelStructureFabrication depends on ConcretePouring
    {1011, 1010},  // BridgeDeckInstallation depends on SteelStructureFabrication
    {1012, 1011},  // AbutmentConstruction depends on BridgeDeckInstallation
    {1013, 1012},  // ExpansionJointInstallation depends on AbutmentConstruction
    {1010, 1005},  // SteelStructureFabrication also depends on MaterialProcurement
    {1013, 1009},  // ExpansionJointInstallation also depends on ConcretePouring
    {1014, 1011},  // TrafficManagement also depends on BridgeDeckInstallation
    {1016, 1012},  // WaterwayProtection also depends on AbutmentConstruction
    {1017, 1012},  // QualityControlTesting also depends on AbutmentConstruction
    {1019, 1013},  // DocumentationHandover also depends on ExpansionJointInstallation
    {1018, 1019},  // LandscapingRestoration also depends on DocumentationHandover
    {1019, 1002},  // DocumentationHandover also depends on PermittingProcess
    {1011, 1017},   // BridgeDeckInstallation also depends on QualityControlTesting
    {1006, 1004},  // FoundationExcavation also depends on EnvImpactAssessment
    {1008, 1002},  // ConcreteFormwork also depends on PermittingProcess
    {1009, 1006},  // ConcretePouring also depends on FoundationExcavation
    {1011, 1006},  // BridgeDeckInstallation also depends on FoundationExcavation
    {1013, 1011},  // ExpansionJointInstallation also depends on BridgeDeckInstallation
    {1016, 1014},  // WaterwayProtection also depends on TrafficManagement
    {1019, 1003},  // DocumentationHandover also depends on BridgeDesign
    {1019, 1016},  // DocumentationHandover also depends on WaterwayProtection
    {1015, 1009},  // BridgeInspection also depends on ConcretePouring
    {1018, 1011},  // LandscapingRestoration also depends on BridgeDeckInstallation
    {1015, 1013},  // BridgeInspection also depends on ExpansionJointInstallation
    {1017, 1004},  // QualityControlTesting also depends on EnvImpactAssessment
    {1016, 1013},  // WaterwayProtection also depends on ExpansionJointInstallation
    {1014, 1007},  // TrafficManagement also depends on PileDriving
    {1012, 1010},  // AbutmentConstruction also depends on SteelStructureFabrication
    {1019, 1015},  // DocumentationHandover also depends on BridgeInspection
    {1015, 1004},  // BridgeInspection also depends on EnvImpactAssessment
    {1019, 1009},  // DocumentationHandover also depends on ConcretePouring
    {1014, 1012},  // TrafficManagement also depends on AbutmentConstruction
    {1013, 1017}   // ExpansionJointInstallation also depends on QualityControlTesting

};

        for (int i = 0; i < 40; ++i)
        {



            int _dependentTask = taskDependencies[i, 0];


            int _dependentOnTask = taskDependencies[i, 1];

            Dependency newDep = new(i, _dependentTask, _dependentOnTask);

            s_dal!.Dependency.Create(newDep);

        }
    }

    //method to delete all data

    private static void DeleteData()
    {
        IEnumerable<Task> taskList = s_dal!.Task.ReadAll()!;
        foreach (Task task in taskList)
            s_dal.Task.Delete(task.Id);
        IEnumerable<Engineer> engineerList = s_dal!.Engineer.ReadAll()!;
        foreach (Engineer engineer in engineerList)
            s_dal.Engineer.Delete(engineer.Id);
        IEnumerable<Dependency> dependencyList = s_dal!.Dependency.ReadAll()!;
        foreach (Dependency dependency in dependencyList)
            s_dal.Dependency.Delete(dependency.Id);
    }


    // Main method to initiate the data creation process
    public static void Do(IDal dal)
    {
        s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        DeleteData();
        CreateTask();
        CreateEngineer();
        CreateDependency();


    }



}
