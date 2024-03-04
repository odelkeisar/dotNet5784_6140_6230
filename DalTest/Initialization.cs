namespace DalTest;
using DalApi;
using DO;
using System.Data;
using System.Xml.Linq;

public static class Initialization
{

    private static IDal? s_dal; //A field that replaces the fields of all entities


    private static readonly Random s_rand = new();

    /// <summary>
    /// Adds to the list all the elements that have already been defined in advance
    /// </summary>
    private static void createTask()
    {
        string[] TaskNames = {"Buying food and disposable products", "Making dough", "Washing and peeling vegetables",
            "Baking the dough for bread", "Cutting vegetables","Making salads", "Cooking soup", "Baking fish", "Preparing meat", "Preparing a side dish for meat",
            "making dessert","kitchen cleaning","Designing serving dishes" ,"Dessert design", "table design","chair design", "Serving drinks", "Serving dishes",
            "Order and cleanliness of the place", "Submitting an opinion on the meal"};
    

        string[] TaskNum = {"T1","T2","T3","T4","T5","T6","T7","T8","T9","T10","T11","T12","T13","T14","T15","T16",
        "T17","T18","T19","T20"};

        int x = 0; //Helps advance the dates for the next tasks
        int y = 0;
       
        DateTime date= s_dal.Task1.ReadClockProject();
       
        foreach (var _name in TaskNames)
        {
            string alias = TaskNum[y++];

            DateTime createDate = date;

            DateTime scheduledDate = date;
            scheduledDate = scheduledDate.AddHours(x);

            TimeSpan taskTime = new TimeSpan(0, 1, 0, 0);

            DateTime deadLine = scheduledDate + taskTime;


            Task1 newTask = new(0, alias, _name, createDate, scheduledDate, taskTime, deadLine, 0, null, null, DO.ChefExperience.Beginner);
            s_dal!.Task1!.Create(newTask);


            if (_name != "Making dough" && _name != "Baking the dough for bread" &&
              _name != "Cooking soup" && _name != "Preparing meat" && _name != "table design"&&_name != "Designing serving dishes" &&
              _name != "Serving drinks") //Tasks at the same level are worked on at the same time
                x++;

        }

    }

    /// <summary>
    /// Adds to the list all the elements that have already been defined in advance
    /// </summary>
    private static void createChef()
    {
        string[] EngineertNames = { "Dani Levi", "Eli Amar", "Meir Cohen", "Ariel Levin", "David Klein" };
        string[] EngineertMail = { "Dani@gmail.com", "Eli@gmail.com", "Meir@gmail.com", "Ariel@gmail.com", "David@gmail.com" };
        int x = 0;
        foreach (var _name in EngineertNames)
        {
            int _id = s_rand.Next(100000000, 900000000);
            string _email = EngineertMail[x++];
            double _cost = s_rand.Next(50, 300);
            DO.ChefExperience _Level = (DO.ChefExperience)s_rand.Next(1, 5);

            Chef newChef = new(_id, false, _email, _cost, _name, _Level);

            s_dal!.Chef!.Create(newChef);
        }

    }

    /// <summary>
    /// Adds to the list all the elements that have already been defined in advance
    /// </summary>
    private static void createDependeency()
    {
        foreach (var _task in s_dal!.Task1!.ReadAll()!)
        {
            foreach (var _task1 in s_dal!.Task1.ReadAll()!)
            {
                if (_task?.Id == _task1!.Id) //Finish scanning previous tasks  
                    break;

                if (_task?.ScheduledDate > _task1!.ScheduledDate)
                {
                    Dependeency _dependeency = new(0, _task.Id, _task1.Id);
                    s_dal!.Dependeency.Create(_dependeency);

                }
            }
        }
    }


    /// <summary>
    /// A method that dispatches to the initialization methods of the three entities
    /// </summary>
    /// <param name="dal"></param>
    /// <exception cref="NullReferenceException"></exception>
    public static void Do()
    {
        s_dal = Factory.Get;

        s_dal.Dependeency.DeleteAll();
        s_dal.Task1.DeleteAll();
        s_dal.Chef.DeleteAll();

        string s_data_config_xml = "data-config";
        const string s_xml_dir = @"..\xml\";

        XElement root = XElement.Load($"{s_xml_dir + s_data_config_xml}.xml");
        root.Element("clockProject")!.Value = DateTime.Now.ToString();
        root.Element("endProject")!.Value = " ";
        root.Element("startProject")!.Value = DateTime.Now.ToString();

        root.Element("NextTask1Id")!.Value = "1";
        root.Element("NextDependeencyId")!.Value = "1";
        root.Save($"{s_xml_dir + s_data_config_xml}.xml");

        createTask();
        createChef();
        createDependeency();
    }
    public static void Reset()
    {
        s_dal = Factory.Get;

        s_dal.Dependeency.DeleteAll();
        s_dal.Task1.DeleteAll();
        s_dal.Chef.DeleteAll();

        string s_data_config_xml = "data-config";
        const string s_xml_dir = @"..\xml\";

        XElement root = XElement.Load($"{s_xml_dir + s_data_config_xml}.xml");
        root.Element("clockProject")!.Value = DateTime.Now.ToString();
        root.Element("endProject")!.Value = " ";
        root.Element("startProject")!.Value = " ";

        root.Element("NextTask1Id")!.Value = "1";
        root.Element("NextDependeencyId")!.Value = "1";
        root.Save($"{s_xml_dir + s_data_config_xml}.xml");
    }
}

