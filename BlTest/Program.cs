namespace BlTest;
using BO;
using BlApi;
using System.ComponentModel.Design;
using System.Xml.Linq;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Threading.Channels;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

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

    private static int TaskMenue()
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
        string? change = Console.ReadLine();
        return int.Parse(change!);
    }
    private static void actTask()
    {
        int act = TaskMenue();

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
                        
                        //Console.WriteLine("Enter CreatedAtDate Enter CreatedAtDate in the format 00.00.0000");
                        DateTime? createdAtDate = DateTime.Now;
                        //DateTime? createdAtDate = DateTime.Parse(Console.ReadLine()!);

                        DateTime? ScheduledDate = null;
                        Console.WriteLine("Would you like to update the Scheduled Task Start Date now (Y/N)?");
                        if (Console.ReadLine() == "Y")
                        { Console.WriteLine("Enter ScheduledDate in the format 00.00.0000");
                           ScheduledDate = DateTime.Parse(Console.ReadLine()!); }
                        
                        Console.WriteLine("RequiredE ffort Time");
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

                        Console.WriteLine("Does the task depend on other tasks? (Y/N)");
                        List<BO.TaskInList> tasksInList = new List<BO.TaskInList>();
                        if (Console.ReadLine() == "Y")
                        {
                            Console.WriteLine("Insert a list of task IDs that the current task depends on. Write with a space between the strings");
                            string? dependeencies = Console.ReadLine();
                            // פיצול המחרוזת למספרי תעודות זהות
                            string[] idNumbers = dependeencies!.Split(new char[] { '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string id in idNumbers)
                            {
                                BO.Task1? task_ = s_bl.Task1.Read(int.Parse(id));
                                tasksInList.Add(new BO.TaskInList() { Id = task_!.Id, Alias = task_.Alias, Description = task_.Description, status = task_.status });
                            }
                        }

                        Task1 newTask = new()
                        { Id= 0,Alias= alias,Description= descripation,status = ScheduledDate == null ? Status.Unscheduled : Status.Scheduled ,dependeencies= tasksInList, CreatedAtDate= createdAtDate, ScheduledDate= ScheduledDate, RequiredEffortTime= RequiredEffortTime,Dellverables= Dellverables, Remarks= Remarks, Copmlexity= Copmlexity };
                        s_bl.Task1.Create(newTask);
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("Enter a task ID number");
                        BO.Task1 task= s_bl.Task1.Read(int.Parse( Console.ReadLine()!))!;
                        Console.WriteLine($@"
ID= {task.Id}
Alias= {task.Alias}
Description= {task.Description}
Status= {task.status.ToString()}
CreatedAtDate= {task.CreatedAtDate}
ScheduledDate= {task.ScheduledDate}
StartDate= {task.StartDate}
ForecastDate= {task.ForecastDate}
CompleteDate= {task.CompleteDate}
RequiredEffortTime= {task.RequiredEffortTime}
Dellverables= {task.Dellverables}
Remarks= {task.Remarks}
Copmlexity= {task.Copmlexity}                   
chef= {task.chef}      
");
                        if(task.dependeencies!=null)
                         foreach(var x in task.dependeencies)
                            {
                                Console.WriteLine($@"
ID= {x.Id}
Alias= {x.Alias}
Description= {x.Description}
status= {x.status}
");                          
                            }
                        break;
                    }
                case 3:
                    {
                        IEnumerable< BO.TaskInList> tasks = s_bl.Task1.ReadAll();
                        print(tasks);
                        break;
                    }
                case 4: 
                    {
                        
                        Console.WriteLine("Enter an ID number");
                        int id=int.Parse(Console.ReadLine()!);
                        BO.Task1 task = s_bl.Task1.Read(id)!;
                       
                   
                        Console.WriteLine("Enter Alias");
                        string? alias =Console.ReadLine()!;  
                        if(string.IsNullOrEmpty(alias) ) 
                            alias = null;


                      



                        break;
                    }
                 case 5: 
                    {
                        Console.WriteLine("Enter an ID number to delete");
                        s_bl.Task1.Delete(int.Parse(Console.ReadLine()!));
                        break;
                    }
            }
            
            act = TaskMenue();
        }

        return;
    }
    public static void print(IEnumerable<BO.TaskInList> tasks)
    {
        foreach (BO.TaskInList task in tasks)
        {
            Console.WriteLine($@"
ID= {task.Id}
Alias= {task.Alias}
Description= {task.Description}
status={task.status}
");
        }
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
                        actTask();
                        break;
                    }
                    case 2:
                    {
                         //actChef();
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
