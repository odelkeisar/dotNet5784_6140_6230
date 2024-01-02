namespace DalTest;
using DalApi;
using DO;
using System.Data;

public static class Initialization
{
    private static ITask? s_dalTask;
    private static IEngineer? s_dalEngineer;
    private static IDependeency? s_dalDependeency;

    private static readonly Random s_rand = new();

    private static void createTask()
    {
        string[] TaskNames = {
        "Knowledge of work environment", "Knowing the github", "Pre-project team meeting",
        "Create a data contract of the data layer","Saving data in object lists", "Creating data access methods",
        "Exception handling", "First team meeting", "Create a data layer interface", "Create a data contract",
        "Added another implementation of saving data in XML","Creating a service contract", "Midterm exam",
        "Second team meeting", "Creating a basic graphical interface","Creating a full graphical interface",
        "Adding a simulator","Using the simulator in the graphical user interface","Final check test",
        "Summary team meeting"};

        string[] TaskNum = {"T1","T2","T3","T4","T5","T6","T7","T8","T9","T10","T11","T12","T13","T14","T15","T16",
        "T17","T18","T19","T20"};

        int x = 0; //Helps advance the dates for the next tasks
        int y = 0;

        foreach (var _name in TaskNames)
        {
            string help = (TaskNum)y;

            DateTime createDate = new DateTime(2023, 11, 28);

            DateTime startDate = new DateTime(2024, 1, 1);
            startDate.AddDays(x * 5);

            TimeSpan taskTime = new TimeSpan(4, 0, 0, 0);

            DateTime deadLine = new DateTime(2024, 1, 5);
            deadLine.AddDays(x * 5);

            Task newTask = new(_name,(TaskNum)y,createDate, startDate, taskTime, deadLine);

            if (_name!= "Knowledge of work environment"&&_name!= "Saving data in object lists"&&
              _name!= "Create a data contract") //Tasks at the same level are worked on at the same time
                x++;

            y++;
        }

    }
    private static void creatEngineer();
    private static void createDependeency();
    
    

    }
