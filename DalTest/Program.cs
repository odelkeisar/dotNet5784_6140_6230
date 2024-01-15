using DalApi;
using DO;
using DalTest;
using Dal;
using System.ComponentModel.Design;
using System.Xml.Linq;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System;
using System.Collections.Generic;


namespace DalTest
{
    internal class Program
    {

        // static readonly IDal s_dal = new DalList(); //stage2
         static readonly IDal s_dal = new DalXml(); //stage 3


        private static int mainMenue() //Main Menu
        {
            Console.WriteLine(@"
            please enter a number
            0 to exit
            1 to task
            2 to chef
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
            

           DateTime DeadLine = ScheduledDate + RequiredEffortTime;

            Console.WriteLine("Enter Chef id");
            int chefId = int.Parse(Console.ReadLine()!);

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
            ChefExperience Copmlexity = (ChefExperience)int.Parse(Console.ReadLine()!);

            string? Dellverables = null;

            Console.WriteLine("Enter remarks");
            string? Remarks = Console.ReadLine();

            Console.WriteLine("Is the task a milestone? enter true or false");
            bool isMilestone = bool.Parse(Console.ReadLine()!);

            Task1 newTask = new(0, alias, descripation, createdAtDate, ScheduledDate, RequiredEffortTime,
                DeadLine, chefId, startDate, CompleteDate, Copmlexity, Dellverables, Remarks,
                isMilestone);

            return newTask;
        }

        private static Chef createNewChef() //Receiving new engineer details
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
            ChefExperience level = (ChefExperience)int.Parse(Console.ReadLine()!);

            Chef newChef = new Chef(id, email, cost, name, level);
            return newChef;
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
                        
                        s_dal!.Task1!.Create(newTask);

                        break;
                    }

                case 2:
                    {
                        Console.WriteLine("Enter Id of task");
                        int Id = int.Parse(Console.ReadLine()!);
                        Task1 ?task = s_dal!.Task1!.Read(Id);
                        if (task == null)
                            throw new DalDoesNotExistException($"task with ID={Id} does not exist");
                        Console.WriteLine($@"
                        Id: {task!.Id}
                        Alias: {task.Alias}
                        Descripation:{task.Description}
                        CreatedAtDate:{task.CreatedAtDate} 
                        ScheduledDate:{task.ScheduledDate}
                        RequiredEffortTime:{task.RequiredEffortTime}
                        DeadLine:{task.DeadlineDate}
                        ChefId:{task.ChefId}
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
                        List<Task1> tasks = (List<Task1>)s_dal!.Task1!.ReadAll();
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
                            ChefId:{task.ChefId}
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

                        Console.WriteLine("Enter CreatedAtDate in the format 00.00.00");
                        DateTime createdAtDate = DateTime.Parse(Console.ReadLine()!);   

                        Console.WriteLine("Enter ScheduledDate in the format 00.00.00");
                        DateTime ScheduledDate = DateTime.Parse(Console.ReadLine()!);   

                        Console.WriteLine("RequiredEffortTime ");
                        Console.WriteLine("Enter num of days");
                        string day = Console.ReadLine()!;
                        Console.WriteLine("Enter num of hours");
                        string? hours = Console.ReadLine();
                        Console.WriteLine("Enter num of minutes");
                        string? minutes = Console.ReadLine();
                        TimeSpan RequiredEffortTime = new TimeSpan(int.Parse(day!), int.Parse(hours!), int.Parse(minutes!), 0);
                

                        DateTime DeadLine = ScheduledDate + RequiredEffortTime;

                        Console.WriteLine("Enter Engineer id");
                        int engineerId = int.Parse(Console.ReadLine()!);

                        Console.WriteLine("Enter startDate in the format 00.00.00");
                        DateTime startDate = DateTime.Parse(Console.ReadLine()!);  

                        Console.WriteLine("Enter CompleteDate in the format 00.00.00");
                        DateTime CompleteDate = DateTime.Parse(Console.ReadLine()!);

                        Console.WriteLine(@"
            Enter Copmlexity:
            Beginner=0
            AdvancedBeginner=1
            Intermediate =2
            Advanced=3
            Experet=4
            ");
                        ChefExperience Copmlexity = (ChefExperience)int.Parse(Console.ReadLine()!);

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

                        s_dal!.Task1!.Update(newTask);
                        break;
                    }

                case 5:
                    {
                        Console.WriteLine("Enter Id of task");
                        int Id = int.Parse(Console.ReadLine()!);
                        s_dal!.Task1!.Delete(Id);
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

        private static bool actChef() //Actions menu for the engineer
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
                        Chef newChef = createNewChef();
                        s_dal!.Chef!.Create(newChef);
                        break;
                    }

                case 2:
                    {
                        Console.WriteLine("Enter Id of Chef");
                        int Id = int.Parse(Console.ReadLine()!);
                        Chef? chef = s_dal!.Chef!.Read(Id);
                        if (chef == null)
                            throw new DalDoesNotExistException($"Chef with ID={Id} does not exist");
                        Console.WriteLine($@"
                        Id: {chef!.Id}
                        Name:  {chef.Name}                       
                        Email: {chef.Email}
                        Cost: {chef.Cost}
                        Level: {chef.Level}");
                        
                        break;
                    }

                case 3:
                    {
                        List<Chef> chefs = (List<Chef>)s_dal!.Chef!.ReadAll();
                        int x = 1;

                        foreach (var chef in chefs)
                        {
                            Console.WriteLine($"Chef: {x++}");
                            Console.WriteLine($@"
                            Id: {chef!.Id}
                            Name:  {chef.Name}                       
                            Email: {chef.Email}
                            Cost: {chef.Cost}
                            Level: {chef.Level}
                            ");

                        }
                        break;
                    }

                case 4:
                    {
                        Chef newChef = createNewChef();
                        s_dal!.Chef!.Update(newChef);
                        break;
                    }

                case 5:
                    {
                        Console.WriteLine("Enter Id of Chef");
                        int Id = int.Parse(Console.ReadLine()!);
                        s_dal!.Chef!.Delete(Id);
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
                        s_dal!.Dependeency!.Create(neWDependeency);

                        break;
                    }

                case 2:
                    {
                        Console.WriteLine("Enter Id of Dependeency");
                        int Id = int.Parse(Console.ReadLine()!);
                        Dependeency? dependeency = s_dal!.Dependeency!.Read(Id);
                        if (dependeency == null)
                            throw new DalDoesNotExistException($"dependeency with ID={Id} does not exist");
                        Console.WriteLine($@"
                        Id: {dependeency!.Id}
                        DependentTask  {dependeency.DependentTask}                       
                        DependsOnTask: {dependeency.DependsOnTask}
                        ");
                        break;
                    }

                case 3:
                    {
                        List<Dependeency> dependeencies = (List < Dependeency >)s_dal!.Dependeency!.ReadAll();
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
                        s_dal!.Dependeency!.Update(neWDependeency);

                        break;
                    }

                case 5:
                    {
                        Console.WriteLine("Enter Id of Dependeency");
                        int Id = int.Parse(Console.ReadLine()!);
                        s_dal!.Dependeency!.Delete(Id);
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
                Initialization.Do(s_dal);
                int _object = mainMenue();
                bool flag = true; 

                while (_object!=0)
                {
                    try
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
                                    flag = actChef();
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
       
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    if (flag == false)
                    {
                        return;
                    }
                    _object = mainMenue();

                }
            }
            catch (DalDoesNotExistException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
            }

            return;
        }
    }
}
