namespace BlTest;
using BO;
using BlApi;
using System.ComponentModel.Design;
using System.Xml.Linq;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System;
using System.Collections.Generic;
internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    private DalApi.IDal _dal = Factory.Get;
    /// <summary>
    /// main menue to change entity
    /// </summary>
    /// <returns></returns>
    private static int mainMenue() //Main Menu
    {
        Console.WriteLine(@"
            please enter a number
            0 to exit
            1 to task
            2 to chef
            3 To update the project start and end date
            4 To read the project start and end date
            ");

        string? change = Console.ReadLine();
        return int.Parse(change!);
    }

    private static void TaskMenue()
    {
        Console.WriteLine(@"
            please enter a number
            0 to exit
            1 to create
            2 to read
            3 to read all
            4 to update
            5 to delete
            6 to read all possible tasks
            7 to read all per level or chef
            8 to read all per level
            9 to read all the complete tasks
            10 to read all Tasks in care
            11 to read all no chef was assigned
            12 to read all no scheduled date
            13 to update scheduled date
            ");
    }
    private static bool actTask()
    {
        TaskMenue();
        string? y = Console.ReadLine();
        int act = int.Parse(y!);
       
        while (act != 0)
        {
           switch(act)
            {
                case 1:
                    {
                        Console.WriteLine("Enter an Alias");
                        string? alias = Console.ReadLine();
                        
                        Console.WriteLine("Enter a descripation");
                        string? descripation = Console.ReadLine();
                        
                        Console.WriteLine("Enter CreatedAtDate Enter CreatedAtDate in the format 00.00.0000");
                        DateTime? createdAtDate = DateTime.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter ScheduledDate in the format 00.00.0000");
                        DateTime ScheduledDate = DateTime.Parse(Console.ReadLine()!);

                        Console.WriteLine("RequiredEffortTime");
                        Console.WriteLine("Enter num of days");
                        string day = Console.ReadLine()!;
                        Console.WriteLine("Enter num of hours");
                        string? hours = Console.ReadLine();
                        Console.WriteLine("Enter num of minutes");
                        string? minutes = Console.ReadLine();
                        TimeSpan RequiredEffortTime = new TimeSpan(int.Parse(day!), int.Parse(hours!), int.Parse(minutes!), 0);

                        Console.WriteLine("Enter Dellverables");
                        string? Dellverables = Console.ReadLine();

                        Console.WriteLine("Enter remarks");
                        string? Remarks = Console.ReadLine();

                        Console.WriteLine(@"
                        Enter Copmlexity:
                        Beginner=0
                        AdvancedBeginner=1
                        Intermediate =2
                        Advanced=3
                        Experet=4
                        ");
                        ChefExperience Copmlexity = (ChefExperience)int.Parse(Console.ReadLine()!);
                        
                        Task1 newTask=new(0,alias,descripation, Scheduled,)
                    }

            }
            TaskMenue();
            y = Console.ReadLine();
            act = int.Parse(y!);
        }

        return true;
    }
    static void Main(string[] args)
    {
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
            DalTest.Initialization.Do();

        int _object=mainMenue();
        bool flag = true;

        while (_object!=0)
        {
            try
            {
                switch(_object)
                {
                    case 1:
                    {
                            flag = actTask();
                        break;
                    }
                    case 2:
                    {
                            flag = actChef();
                         break;
                    }
                    case 3:
                    {
                          Console.WriteLine("Enter start date of project and end date:\n");
                          DateTime startProject= DateTime.Parse(Console.ReadLine()!);
                          DateTime endProject = DateTime.Parse(Console.ReadLine()!);
                          s_bl.Task1.CreateStartEndProject(startProject, endProject);
                          break;
                    }
                    case 4:
                        {
                            DateTime ?startProject = s_bl.Task1.ReadStartProject();
                            DateTime ?endProject = s_bl.Task1.ReadEndProject();

                            if (startProject == null)
                                Console.WriteLine("Start date not updated\n");

                            else
                                Console.WriteLine(startProject);

                            if(endProject == null)
                                Console.WriteLine("End date not updated\n");

                            else
                                Console.WriteLine(endProject);
                            break;

                        }

                    default:
                        {
                            Console.WriteLine("Error\n");
                            break;
                        }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            _object = mainMenue();
        }

        return;
    }
}
