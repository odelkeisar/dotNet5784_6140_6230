using DalApi;
using DO;
using DalTest;
using Dal;
using System.ComponentModel.Design;
using System.Xml.Linq;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System;

namespace DalTest
{
    internal class Program
    {
        private static ITask1? s_dalTask = new TaskImplementation(); //stage 1
        private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
        private static IDependeency? s_dalDependeency = new DependeencyImplementation(); //stage 1

        private static int mainMenue() //Main Menu
        {
            Console.WriteLine(@"
            please enter a number
            0 to exit
            1 to task
            2 to engineer
            3 to dependencies
            ");

            string? change = Console.ReadLine();
            return int.Parse(change!);   
        }

        private static void subMenue() //Sub menu
        {
            Console.WriteLine(@"
            please enter a number
            0 to exit
            1 to create
            2 to read
            3 to read all
            4 to update
            5 to delete
            ");
        }

        private static Task1 createNewTask() //Receiving new task details
        {
            Console.WriteLine("Enter Alias");
            string? alias = Console.ReadLine();

            Console.WriteLine("Enter Description");
            string? descripation = Console.ReadLine();

            Console.WriteLine("Enter CreatedAtDate");
            Console.WriteLine("Enter day");
            string? day = Console.ReadLine();
            Console.WriteLine("Enter monthe");
            string? month = Console.ReadLine();
            Console.WriteLine("Enter year");
            string? year = Console.ReadLine();
            DateTime createdAtDate = new DateTime(int.Parse(year!), int.Parse(month!), int.Parse(day!));

            Console.WriteLine("Enter ScheduledDate");
            Console.WriteLine("Enter day");
            day = Console.ReadLine();
            Console.WriteLine("Enter monthe");
            month = Console.ReadLine();
            Console.WriteLine("Enter year");
            year = Console.ReadLine();
            DateTime ScheduledDate = new DateTime(int.Parse(year!), int.Parse(month!), int.Parse(day!));

            Console.WriteLine("Enter RequiredEffortTime");
            Console.WriteLine("Enter num of days");
            day = Console.ReadLine();
            Console.WriteLine("Enter num of hours");
            string? hours = Console.ReadLine();
            Console.WriteLine("Enter num of minutes");
            string? minutes = Console.ReadLine();
            TimeSpan RequiredEffortTime = new TimeSpan(int.Parse(day!), int.Parse(hours!), int.Parse(minutes!), 0);

            DateTime DeadLine = ScheduledDate + RequiredEffortTime;

            Console.WriteLine("Enter Engineer id");
            int engineerId = int.Parse(Console.ReadLine()!);

            DateTime? startDate = null;
            DateTime? CompleteDate = null;

            Console.WriteLine(@"
            Enter Copmlexity:
            Beginner=0
            AdvancedBeginner=1
            Intermediate =2
            Advanced=3
            Experet=4
            ");
            EngineerExperience Copmlexity = (EngineerExperience)int.Parse(Console.ReadLine()!);

            string? Dellverables = null;

            Console.WriteLine("Enter remarks");
            string? Remarks = Console.ReadLine();

            Console.WriteLine("Is the task a milestone? enter yes or no");
            string? help = Console.ReadLine();
            bool isMilestone = false;

            if (help == "yes")
                isMilestone = true;

            Task1 newTask = new(0, alias, descripation, createdAtDate, ScheduledDate, RequiredEffortTime,
                DeadLine, engineerId, startDate, CompleteDate, Copmlexity, Dellverables, Remarks,
                isMilestone);

            return newTask;
        }

        private static Engineer createNewEngineer() //Receiving new engineer details
        {
            Console.WriteLine("Enter Id");
            int id = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter Name");
            string? name = Console.ReadLine();

            Console.WriteLine("Enter Email");
            string? email = Console.ReadLine();

            Console.WriteLine("Enter Cost of hour");
            double? cost = double.Parse(Console.ReadLine()!);

            Console.WriteLine(@"
            Enter Copmlexity: Beginner=0
            AdvancedBeginner=1
            Intermediate =2
            Advanced=3
            Experet=4
            ");
            EngineerExperience level = (EngineerExperience)int.Parse(Console.ReadLine()!);

            Engineer newEngineer = new Engineer(id, email, cost, name, level);
            return newEngineer;
        }

        private static bool actTask() //Actions menu for the task
        {
            subMenue();
            string? y = Console.ReadLine();
            int act = int.Parse(y!);

            switch (act)
            {
                case 0:
                    {
                        return false;
                    }
                case 1:
                    {
                        Task1 newTask = createNewTask();
                        
                        s_dalTask!.Create(newTask);

                        break;
                    }

                case 2:
                    {
                        Console.WriteLine("Enter Id of task");
                        int Id = int.Parse(Console.ReadLine()!);
                        Task1 ?task = s_dalTask!.Read(Id);
                        Console.WriteLine($@"
                        Id: {task!.Id}
                        Alias: {task.Alias}
                        Descripation:{task.Description}
                        CreatedAtDate:{task.CreatedAtDate} 
                        ScheduledDate:{task.ScheduledDate}
                        RequiredEffortTime:{task.RequiredEffortTime}
                        DeadLine:{task.DeadlineDate}
                        EngineerId:{task.EngineerId}
                        StartDate:{task.StartDate}
                        CompleteDate:{task.CompleteDate}
                        Dellverables:{task.Dellverables}
                        Remarks:{task.Remarks}
                        isMilestone:{task.isMileStone}
                        ");
                        break;
                    }

                case 3:
                    {
                        List<Task1> tasks = s_dalTask!.ReadAll();
                        int x = 1;

                        foreach (var task in tasks) 
                        { 
                            Console.WriteLine($"Task: {x++}");
                            Console.WriteLine($@"
                            Id: {task.Id}
                            Alias: {task.Alias}
                            Descripation:{task.Description}
                            CreatedAtDate:{task.CreatedAtDate} 
                            ScheduledDate:{task.ScheduledDate}
                            RequiredEffortTime:{task.RequiredEffortTime}
                            DeadLine:{task.DeadlineDate}
                            EngineerId:{task.EngineerId}
                            StartDate:{task.StartDate}
                            CompleteDate:{task.CompleteDate}
                            Dellverables:{task.Dellverables}
                            Remarks:{task.Remarks}
                            isMilestone:{task.isMileStone} 
                            ");

                        }

                        break;
                    }

                case 4:
                    {
                        Console.WriteLine("Enter Id");
                        int Id=int.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter Alias");
                        string? alias = Console.ReadLine();

                        Console.WriteLine("Enter Description");
                        string? descripation = Console.ReadLine();

                        Console.WriteLine("Enter CreatedAtDate");
                        Console.WriteLine("Enter day");
                        string? day = Console.ReadLine();
                        Console.WriteLine("Enter monthe");
                        string? month = Console.ReadLine();
                        Console.WriteLine("Enter year");
                        string? year = Console.ReadLine();
                        DateTime createdAtDate = new DateTime(int.Parse(year!), int.Parse(month!), int.Parse(day!));

                        Console.WriteLine("Enter ScheduledDate");
                        Console.WriteLine("Enter day");
                        day = Console.ReadLine();
                        Console.WriteLine("Enter monthe");
                        month = Console.ReadLine();
                        Console.WriteLine("Enter year");
                        year = Console.ReadLine();
                        DateTime ScheduledDate = new DateTime(int.Parse(year!), int.Parse(month!), int.Parse(day!));

                        Console.WriteLine("Enter RequiredEffortTime");
                        Console.WriteLine("Enter num of days");
                        day = Console.ReadLine();
                        Console.WriteLine("Enter num of hours");
                        string? hours = Console.ReadLine();
                        Console.WriteLine("Enter num of minutes");
                        string? minutes = Console.ReadLine();
                        TimeSpan RequiredEffortTime = new TimeSpan(int.Parse(day!), int.Parse(hours!), int.Parse(minutes!),0);

                        DateTime DeadLine = ScheduledDate + RequiredEffortTime;

                        Console.WriteLine("Enter Engineer id");
                        int engineerId = int.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter startDate");
                        Console.WriteLine("Enter day");
                        day = Console.ReadLine();
                        Console.WriteLine("Enter monthe");
                        month = Console.ReadLine();
                        Console.WriteLine("Enter year");
                        year = Console.ReadLine();
                        DateTime startDate = new DateTime(int.Parse(year!), int.Parse(month!), int.Parse(day!));
                        
                        Console.WriteLine("Enter CompleteDate");
                        Console.WriteLine("Enter day");
                        day = Console.ReadLine();
                        Console.WriteLine("Enter monthe");
                        month = Console.ReadLine();
                        Console.WriteLine("Enter year");
                        year = Console.ReadLine();
                        DateTime CompleteDate = new DateTime(int.Parse(year!), int.Parse(month!), int.Parse(day!));

                        Console.WriteLine(@"
            Enter Copmlexity:
            Beginner=0
            AdvancedBeginner=1
            Intermediate =2
            Advanced=3
            Experet=4
            ");
                        EngineerExperience Copmlexity = (EngineerExperience)int.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter dellverables");
                        string? Dellverables = Console.ReadLine();

                        Console.WriteLine("Enter remarks");
                        string? Remarks = Console.ReadLine();

                        Console.WriteLine("Is the task a milestone? enter yes or no");
                        string? help = Console.ReadLine();
                        bool isMilestone = false;

                        if (help == "yes")
                            isMilestone = true;

                        Task1 newTask = new(Id, alias, descripation, createdAtDate, ScheduledDate, RequiredEffortTime,
                            DeadLine, engineerId, startDate, CompleteDate, Copmlexity, Dellverables, Remarks,
                            isMilestone);

                        s_dalTask!.Update(newTask);
                        break;
                    }

                case 5:
                    {
                        Console.WriteLine("Enter Id of task");
                        int Id = int.Parse(Console.ReadLine()!);
                        s_dalTask!.Delete(Id);
                        break;
                    }

                default:
                    {
                        Console.WriteLine("Eror");
                        break;
                    }

                 
            }

            return true;
        }

        private static bool actEngineer() //Actions menu for the engineer
        { 
            subMenue();
            string? y = Console.ReadLine();
            int act = int.Parse(y!);

            switch (act)
            {
                case 0:
                    {
                        return false;
                    }
                case 1:
                    {
                        Engineer newEngineer = createNewEngineer();
                        s_dalEngineer!.Create(newEngineer);
                        break;
                    }

                case 2:
                    {
                        Console.WriteLine("Enter Id of Engineer");
                        int Id = int.Parse(Console.ReadLine()!);
                        Engineer? engineer = s_dalEngineer!.Read(Id);
                        Console.WriteLine($@"
                        Id: {engineer!.Id}
                        Name:  {engineer.Name}                       
                        Email: {engineer.Email}
                        Cost: {engineer.Cost}
                        Level: {engineer.Level}");
                        break;
                    }

                case 3:
                    {
                        List<Engineer> engineers = s_dalEngineer!.ReadAll();
                        int x = 1;

                        foreach (var engineer in engineers)
                        {
                            Console.WriteLine($"Engineer: {x++}");
                            Console.WriteLine($@"
                            Id: {engineer!.Id}
                            Name:  {engineer.Name}                       
                            Email: {engineer.Email}
                            Cost: {engineer.Cost}
                            Level: {engineer.Level}
                            ");

                        }
                        break;
                    }

                case 4:
                    {
                        Engineer newEngineer = createNewEngineer();
                        s_dalEngineer!.Update(newEngineer);
                        break;
                    }

                case 5:
                    {
                        Console.WriteLine("Enter Id of Engineer");
                        int Id = int.Parse(Console.ReadLine()!);
                        s_dalEngineer!.Delete(Id);
                        break;
                    }

                default:
                    {
                        Console.WriteLine("Eror");
                        break;
                    }


            }

            return true;
        }

        private static bool actDependeency() //Actions menu for the dependeency
        {
            subMenue();
            string? y = Console.ReadLine();
            int act = int.Parse(y!);

            switch (act)
            {
                case 0:
                    {

                        return false;
                    }
                case 1:
                    {
                        Console.WriteLine("enter DependentTask");
                        int DependentTask=int.Parse(Console.ReadLine()!);

                        Console.WriteLine("enter DependsOnTask");
                        int DependsOnTask=int.Parse(Console.ReadLine()!);

                        Dependeency neWDependeency = new Dependeency(0, DependentTask, DependsOnTask);
                        s_dalDependeency!.Create(neWDependeency);

                        break;
                    }

                case 2:
                    {
                        Console.WriteLine("Enter Id of Dependeency");
                        int Id = int.Parse(Console.ReadLine()!);
                        Dependeency? dependeency = s_dalDependeency!.Read(Id);
                        Console.WriteLine($@"
                        Id: {dependeency!.Id}
                        DependentTask  {dependeency.DependentTask}                       
                        DependsOnTask: {dependeency.DependsOnTask}
                        ");
                        break;
                    }

                case 3:
                    {
                        List<Dependeency> dependeencies = s_dalDependeency!.ReadAll();
                        int x = 1;

                        foreach (var dependeency in dependeencies)
                        {
                            Console.WriteLine($"Dependeny: {x++}");
                            Console.WriteLine($@"
                            Id: {dependeency!.Id}
                            DependentTask:  {dependeency.DependentTask}                       
                            DependsOnTask: {dependeency.DependsOnTask}
                            ");

                        }
                        break;
                    }

                case 4:
                    {
                        Console.WriteLine("enter DependentTask");
                        int DependentTask = int.Parse(Console.ReadLine()!);

                        Console.WriteLine("enter DependsOnTask");
                        int DependsOnTask = int.Parse(Console.ReadLine()!);

                        Dependeency neWDependeency = new Dependeency(0, DependentTask, DependsOnTask);
                        s_dalDependeency!.Update(neWDependeency);

                        break;
                    }

                case 5:
                    {
                        Console.WriteLine("Enter Id of Dependeency");
                        int Id = int.Parse(Console.ReadLine()!);
                        s_dalDependeency!.Delete(Id);
                        break;
                    }

                default:
                    {
                        Console.WriteLine("Eror");
                        break;
                    }

                   
            }

            return true;
        }

        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependeency);
                int _object = mainMenue();
                bool flag = true; 

                while (_object!=0)
                {

                     
                        switch (_object)
                        {
                         
                            case 1:
                            {
                                flag = actTask();
                                break;
                            }

                            case 2:
                            {
                                flag = actEngineer();
                                break;
                            }

                            case 3:
                            {
                                flag = actDependeency();
                                break;
                            }


                            default:
                            {
                                    Console.WriteLine("Eror");
                                    break;
                            }
                        }

                    if (flag == false)
                    {
                        return;
                    }

                    _object = mainMenue();


                }

            }

            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
            }

            return;
        }
    }
}
