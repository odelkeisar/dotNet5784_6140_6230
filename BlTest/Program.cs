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
            3 To update the project start date
            4 To read the project start and end date
            5 To update the project end date
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
            7 to read all per level of chef
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
            switch (act)
            {
                case 1:
                    {
                        Console.WriteLine("Enter an Alias");
                        string? alias = Console.ReadLine();

                        Console.WriteLine("Enter a descripation");
                        string? descripation = Console.ReadLine();
                        
                        DateTime? createdAtDate = DateTime.Now;
                        
                        DateTime? ScheduledDate = null;
                        Console.WriteLine("Would you like to update the Scheduled Task Start Date now (Y/N)?");
                        if (Console.ReadLine() == "Y")
                        {
                            Console.WriteLine("Enter ScheduledDate in the format 00.00.0000");
                            ScheduledDate = DateTime.Parse(Console.ReadLine()!);
                        }

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
                        { Id = 0, Alias = alias, Description = descripation, status = ScheduledDate == null ? Status.בלתי_מתוכנן : Status.מתוזמן, dependeencies = tasksInList, CreatedAtDate = createdAtDate, ScheduledDate = ScheduledDate, RequiredEffortTime = RequiredEffortTime, Dellverables = Dellverables, Remarks = Remarks, Copmlexity = Copmlexity };
                        s_bl.Task1.Create(newTask);
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("Enter a task ID number");
                        BO.Task1 task = s_bl.Task1.Read(int.Parse(Console.ReadLine()!))!;
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
                        if (task.dependeencies != null)
                            foreach (var x in task.dependeencies)
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
                        IEnumerable<BO.TaskInList> tasks = s_bl.Task1.ReadAll();
                        print(tasks);
                        break;
                    }
                case 4:
                    {
                        string? ans;

                        Console.WriteLine("Enter an ID number");
                        int id = int.Parse(Console.ReadLine()!);
                        BO.Task1 task = s_bl.Task1.Read(id)!;

                        Console.WriteLine("Enter an Alias");
                        string? alias = Console.ReadLine();

                        Console.WriteLine("Enter a descripation");
                        string? descripation = Console.ReadLine();

                        Console.WriteLine("Enter Scheduled Date in the format 00.00.0000");
                        ans = Console.ReadLine();
                        DateTime?ScheduledDate = ans==" "?null:DateTime.Parse(ans);
  

                        Console.WriteLine("Enter Start Date in the format 00.00.0000");
                        ans = Console.ReadLine();
                        DateTime? StartDate = ans == " " ? null : DateTime.Parse(ans);
                        

                        Console.WriteLine("Enter Complete Date in the format 00.00.0000");
                        ans = Console.ReadLine();
                        DateTime? CompleteDate = ans == " " ? null : DateTime.Parse(ans);


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

                        Console.WriteLine("Do you want to add chef? Y/N");

                        ChefInTask? chef = null;

                        if (Console.ReadLine() == "Y")
                        {
                            Console.WriteLine("Enter id of chef");
                            int _id = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter name of chef");
                            string? _name = Console.ReadLine();
                            chef = new() { Id = _id, Name = _name };
                        }

                        else
                        {
                            chef = s_bl.Task1.Read(id)!.chef;
                        }

                        

                        Console.WriteLine(@"
                        Enter Copmlexity:
                        Beginner=0
                        AdvancedBeginner=1
                        Intermediate =2
                        Advanced=3
                        Experet=4
                        ");
                        ChefExperience Copmlexity = (ChefExperience)int.Parse(Console.ReadLine()!);

                        BO.Task1? botask = s_bl.Task1.Read(id); //בדיקה שהמשימה קיימת

                        if (botask == null)
                            throw new BlDoesNotExistException($"Task with ID={id} does not exists");
                        
                        Status _status = Status.בלתי_מתוכנן;
                        if (CompleteDate != null)
                            _status = Status.בוצע;
                        else if (StartDate != null)
                            _status = Status.בתהליך;
                        else if (ScheduledDate != null)
                            _status = Status.מתוזמן;


                        Task1 newTask = new()
                        { Id = id, Alias = alias, Description = descripation, status =_status,
                          dependeencies= s_bl.Task1.Read(id).dependeencies, CreatedAtDate= s_bl.Task1.Read(id)!.CreatedAtDate,
                          ScheduledDate = ScheduledDate,StartDate=StartDate, CompleteDate=CompleteDate,
                          RequiredEffortTime = RequiredEffortTime, Dellverables = Dellverables, Remarks = Remarks, chef = chef,
                          Copmlexity = Copmlexity };
                        s_bl.Task1.Update(newTask);
                       
                        break;
                    }
                case 5:
                    {
                        Console.WriteLine("Enter an ID number to delete");
                        s_bl.Task1.Delete(int.Parse(Console.ReadLine()!));
                        break;
                    }
                 case 6:
                    {
                        Console.WriteLine("Enter an ID of chef");
                        int id=int.Parse(Console.ReadLine()!);
                        Chef chef = s_bl.Chef.Read(id)!;
                        IEnumerable<BO.TaskInList> tasks = s_bl.Task1.ReadAllPossibleTasks(chef);
                        print(tasks);
                        break;
                    }
                 case 7:
                    {
                        Console.WriteLine("Enter an ID of chef");
                        int id = int.Parse(Console.ReadLine()!);
                        Chef chef = s_bl.Chef.Read(id)!;
                        IEnumerable<BO.TaskInList> tasks = s_bl.Task1.ReadAllPerLevelOfChef(chef);
                        print(tasks);
                        break;
                    }
                 case 8:
                    {
                        Console.WriteLine(@"
                        Enter Copmlexity:
                        Beginner=0
                        AdvancedBeginner=1
                        Intermediate =2
                        Advanced=3
                        Experet=4
                        ");
                        ChefExperience Copmlexity = (ChefExperience)int.Parse(Console.ReadLine()!);
                        IEnumerable<BO.TaskInList> tasks = s_bl.Task1.ReadAllPerLevel(Copmlexity);
                        print(tasks);
                        break;
                    }
                 case 9: 
                    {
                        IEnumerable<BO.TaskInList> tasks = s_bl.Task1.ReadAllCompleted();
                        print(tasks);
                        break;
                    }
                 case 10:
                    {
                        IEnumerable<BO.TaskInList> tasks = s_bl.Task1.ReadAllTasksInCare();
                        print(tasks);
                        break;
                    }
                 case 11:
                    {
                        IEnumerable<BO.TaskInList> tasks = s_bl.Task1.ReadAllNoChefWasAssigned();
                        print(tasks);
                        break;
                    }
                case 12:
                    {
                        IEnumerable<BO.TaskInList> tasks = s_bl.Task1.ReadAllNoScheduledDate();
                        print(tasks);
                        break;
                    }
                case 13: 
                    {
                        Console.WriteLine("Enter an ID of task");
                        int id = int.Parse(Console.ReadLine()!);
                        Console.WriteLine("Enter a schedule date to task");
                        DateTime scheduleDate = DateTime.Parse(Console.ReadLine()!);
                        s_bl.Task1.UpdateScheduledDate(id, scheduleDate);
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Error");
                        break;
                    }
            }
            
            act = TaskMenue();
        }

        return;
    }

    private static int ChefMenue()
    {
        Console.WriteLine(@"
            please enter a number
            0 to exit
            1 to create
            2 to read
            3 to read all
            4 to update
            5 to delete
            6 to read all Per level
            7 to read all not assigned
            ");
        string? change = Console.ReadLine();
        return int.Parse(change!);
    }

    private static void actChef()
    {
        int act = ChefMenue();

        while (act != 0)
        {
            switch (act)
            {
                case 1: 
                    {
                        Console.WriteLine("Enter an Id");
                        int id = int.Parse(Console.ReadLine()!);
                        Console.WriteLine("Enter a mail");
                        string? email= Console.ReadLine();
                        Console.WriteLine("Enter a cost");
                        double cost= double.Parse(Console.ReadLine()!);
                        Console.WriteLine("Enter a name");
                        string? name = Console.ReadLine();
                        Console.WriteLine(@"
                        Enter a level:
                        Beginner=0
                        AdvancedBeginner=1
                        Intermediate =2
                        Advanced=3
                        Experet=4
                        ");
                        ChefExperience Level = (ChefExperience)int.Parse(Console.ReadLine()!);

                        Chef chef= new() { Id = id, Email=email, Cost = cost, Name = name,Level = Level ,deleted=false};
                        s_bl.Chef.Create(chef);
                        break;
                    }
                    case 2: 
                    {
                        Console.WriteLine("Enter an Id");
                        int id = int.Parse(Console.ReadLine()!);
                        Chef chef = s_bl.Chef.Read(id)!;
                       
                        if (chef.task == null)
                        {
                            Console.WriteLine($@"
ID: {chef.Id}
Name: {chef.Name}
Email: {chef.Email}
Cost of hour: {chef.Cost}
Task of the chef: {chef.task}
");
                        }

                        else
                        {
                            Console.WriteLine($@"
ID: {chef.Id}
Name: {chef.Name}
Email: {chef.Email}
Cost of hour: {chef.Cost}
Task of the chef: Id- {chef.task.Id} Alias- {chef.task.Alias}
");
                        }
                        break;
                    }
                case 3:
                    {
                        IEnumerable<BO.Chef> chefs =s_bl.Chef.ReadAll();
                        foreach(BO.Chef chef in chefs)
                        {
                            if (chef.task == null)
                            {
                                Console.WriteLine($@"
ID: {chef.Id}
Name: {chef.Name}
Email: {chef.Email}
Cost of hour: {chef.Cost}
Task of the chef: {chef.task}
");
                            }

                            else
                            {
                                Console.WriteLine($@"
ID: {chef.Id}
Name: {chef.Name}
Email: {chef.Email}
Cost of hour: {chef.Cost}
Task of the chef: Id- {chef.task.Id} Alias- {chef.task.Alias}
");
                            }
                        }
                        break;
                    }
                case 4: 
                    {
                        Console.WriteLine("Enter an Id");
                        int id=int.Parse(Console.ReadLine()!);
                        Console.WriteLine("Enter a mail");
                        string? email = Console.ReadLine();
                        Console.WriteLine("Enter a cost");
                        double cost = double.Parse(Console.ReadLine()!);
                        Console.WriteLine("Enter a name");
                        string? name = Console.ReadLine();
                        Console.WriteLine(@"
                        Enter a level:
                        Beginner=0
                        AdvancedBeginner=1
                        Intermediate =2
                        Advanced=3
                        Experet=4
                        ");
                        ChefExperience Level = (ChefExperience)int.Parse(Console.ReadLine()!);
                        
                        Console.WriteLine("Do you want to add task? Y/N");

                        TaskInChef? _task = null;

                        if (Console.ReadLine() == "Y")
                        {
                            Console.WriteLine("Enter Id of the task");
                            int? taskId = int.Parse(Console.ReadLine()!);
                            Console.WriteLine("Enter Alias of the task");
                            string? alias = Console.ReadLine();
                            _task = new() { Id = taskId, Alias = alias };
                        }

                        else
                            _task = s_bl.Chef.Read(id).task;

                        Chef chef = new() { Id=id, Name=name, Email=email, Cost=cost, Level=Level, task=_task};
                        s_bl.Chef.Update(chef);
                        break;
                    }
                    case 5:
                    {
                        Console.WriteLine("Enter an Id");
                        int id = int.Parse(Console.ReadLine()!);
                        s_bl.Chef.Delete(id);
                        break;
                    }
                    case 6:
                    {
                        Console.WriteLine(@"
                        Enter a level:
                        Beginner=0
                        AdvancedBeginner=1
                        Intermediate =2
                        Advanced=3
                        Experet=4
                        ");
                        ChefExperience Level = (ChefExperience)int.Parse(Console.ReadLine()!);

                        IEnumerable<BO.Chef> chefs = s_bl.Chef.ReadAllPerLevel(Level);
                        foreach (BO.Chef chef in chefs)
                        {
                            if (chef.task == null)
                            {
                                Console.WriteLine($@"
ID: {chef.Id}
Name: {chef.Name}
Email: {chef.Email}
Cost of hour: {chef.Cost}
Task of the chef: {chef.task}
");
                            }

                            else
                            {
                                Console.WriteLine($@"
ID: {chef.Id}
Name: {chef.Name}
Email: {chef.Email}
Cost of hour: {chef.Cost}
Task of the chef: Id- {chef.task.Id} Alias- {chef.task.Alias}
");
                            }
                        }
                            break;

                    }
                case 7:
                    {
                        IEnumerable<BO.Chef> chefs = s_bl.Chef.ReadAllNotAssigned();
                        foreach (BO.Chef chef in chefs)
                        {
                            if (chef.task == null)
                            {
                                Console.WriteLine($@"
ID: {chef.Id}
Name: {chef.Name}
Email: {chef.Email}
Cost of hour: {chef.Cost}
Task of the chef: {chef.task}
");
                            }

                            else
                            {
                                Console.WriteLine($@"
ID: {chef.Id}
Name: {chef.Name}
Email: {chef.Email}
Cost of hour: {chef.Cost}
Task of the chef: Id- {chef.task.Id} Alias- {chef.task.Alias}
");
                            }
                        }
                        break;
                    }

                default:
                    {
                        Console.WriteLine("Error");
                        break;
                    }
            }

            act = ChefMenue();
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
        {
            DalTest.Initialization.Do();
            string s_data_config_xml = "data-config";
            const string s_xml_dir = @"..\xml\";

            XElement root = XElement.Load($"{s_xml_dir + s_data_config_xml}.xml");
            root.Element("endProject")!.Value = " ";
            root.Element("startProject")!.Value = " ";
            root.Save($"{s_xml_dir + s_data_config_xml}.xml");
        }
            

        int _object=mainMenue();

        while (_object != 0)
        {
            try
            {
                switch (_object)
                {
                    case 1:
                        {
                            actTask();
                            break;
                        }
                    case 2:
                        {
                            actChef();
                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine("Enter start date of project date:");
                            DateTime startProject = DateTime.Parse(Console.ReadLine()!);
                            s_bl.Task1.CreateStartProject(startProject);
                            break;
                        }
                    case 4:
                        {
                            DateTime? startProject = s_bl.Task1.ReadStartProject();
                            DateTime? endProject = s_bl.Task1.ReadEndProject();

                            if (startProject == null)
                                Console.WriteLine("Start date not updated");

                            else
                                Console.WriteLine(startProject);

                            if (endProject == null)
                                Console.WriteLine("End date not updated");

                            else
                                Console.WriteLine(endProject);
                            break;

                        }
                    case 5:
                        {
                            Console.WriteLine("Enter end date of project date:");
                            DateTime endProject = DateTime.Parse(Console.ReadLine()!);
                            s_bl.Task1.CreateEndProject(endProject);
                            break;
                        }

                    default:
                        {
                            Console.WriteLine("Error");
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            _object = mainMenue();
        }

        return;
    }
}