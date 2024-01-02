namespace DalTest;
using DalApi;
using DO;
public static class Initialization
{
    private static ITask? s_dalTask;
    private static IEngineer? s_dalEngineer;
    private static IDependeency? s_dalDependeency;

    private static readonly Random s_rand = new();

    private static void createTask();
    private static void creatEngineer()
    {
        string[] EngineertNames = {"Dani Levi", "Eli Amar", "Meir Cohen","Ariel Levin", "David Klein"};
        string[] EngineertMail = { "Dani@gmail.com", "Eli@gmail.com", "Meir@gmail.com", "Ariel@gmail.com", "David@gmail.com" };
        int x = 0;
        foreach (var _name in EngineertNames)
        {
            int _id;
            do
                _id = s_rand.Next(100000000, 900000000);
            while (s_dalEngineer!.Read(_id) != null);
            
            string _email = EngineertMail[x++];
            double _cost = s_rand.Next(50, 300);
            DO.EngineerExperience? _Level = (DO.EngineerExperience)s_rand.Next(0,4);

            Engineer newEngineer = new(_id, _email,_cost, _name, _Level);

            s_dalEngineer!.Create(newEngineer);
        }

    }
    private static void createDependeency();



}
