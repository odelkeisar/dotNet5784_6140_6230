using DalApi;
using DO;

namespace Dal;

using System.Data.Common;
using System.Xml.Linq;

/// <summary>
/// Realization of crude functions using the XELement class
/// </summary>
internal class DependeencyImplementation: IDependeency 
{
    readonly string s_dependeencies_xml = "dependeencies";
    XElement? dependeencyRoot;
    string dependeencyPath = @"dependeencies.xml";
    public DependeencyImplementation() //opening a file
    {
        if (!File.Exists(dependeencyPath))
        {
            dependeencyRoot = new XElement("dependeencies");
            dependeencyRoot.Save(dependeencyPath);
        }
        else
        {
            try { dependeencyRoot = XElement.Load(dependeencyPath); }
            catch { Console.WriteLine("File upload problem"); }
        }
    }

    public int Create(Dependeency item) //add dependeency to a collection
    {
        int newId = Config.NextDependeencyId;
        XElement id = new XElement("id", newId);
        XElement DependentTask = new XElement("DependentTask", item.DependentTask);
        XElement DependsOnTas = new XElement("DependsOnTas", item.DependsOnTask);

        dependeencyRoot!.Add(new XElement("dependeency", id, DependentTask, DependsOnTas));
        dependeencyRoot.Save(dependeencyPath);
        return newId;
    }

    public void Delete(int id) //delete dependeency
    {
        XElement? dependeency;

        dependeency = dependeencyRoot!.Elements().FirstOrDefault(e => Convert.ToInt32(e.Element("id")!.Value) == id);

        if (dependeency == null) { throw new DalDoesNotExistException($"Dependeency with ID={id} does not exist"); }
        dependeency!.Remove();
        dependeencyRoot.Save(dependeencyPath);
        
    }

    public Dependeency? Read(int id)
    {

        XElement? dependeency;
        dependeency = dependeencyRoot!.Elements().FirstOrDefault(e => Convert.ToInt32(e.Element("id")!.Value) == id);
        
        if (dependeency == null)
        {  
            return null;
        }

        Dependeency dependeency1 = new Dependeency() { Id = Convert.ToInt32(dependeency.Element("Id")!.Value),
            DependentTask = Convert.ToInt32(dependeency.Element("DependentTask")!.Value),
            DependsOnTask = Convert.ToInt32(dependeency.Element("DependsOnTask")!.Value) };


        return dependeency1;

    }

    public Dependeency? Read(Func<Dependeency, bool> filter)
    {
        var dependeencies = dependeencyRoot!.Elements().Select(x => new Dependeency
        {
            Id = Convert.ToInt32(x.Element("Id")!.Value),
            DependentTask = Convert.ToInt32(x.Element("DependentTask")!.Value),
            DependsOnTask = Convert.ToInt32(x.Element("DependsOnTask")!.Value)
        });


        Dependeency? dependeency = dependeencies.Where(filter).FirstOrDefault();

        return dependeency;  
    }

    public IEnumerable<Dependeency?> ReadAll(Func<Dependeency, bool>? filter = null)
    {
        IEnumerable<Dependeency>? dependeencies;

        if (filter == null)
        {
            
            try
            {
                dependeencies = (from p in dependeencyRoot!.Elements()
                                 select new Dependeency()
                                 {
                                     Id = Convert.ToInt32(p.Element("id")!.Value),
                                     DependentTask = Convert.ToInt32(p.Element("DependentTask")!.Value),
                                     DependsOnTask = Convert.ToInt32(p.Element("DependOnTask")!.Value)
                                 }) ;
            }

            catch { dependeencies = null; }
            return dependeencies.ToList();
        }

        else
        {
            try
            {
                dependeencies = (from p in dependeencyRoot!.Elements()
                                 where filter(new Dependeency()
                                 {
                                     Id = Convert.ToInt32(p.Element("id")!.Value),
                                     DependentTask = Convert.ToInt32(p.Element("DependentTask")!.Value),
                                     DependsOnTask = Convert.ToInt32(p.Element("DependOnTask")!.Value)
                                 })
                                 select  new Dependeency()
                                 {
                                     Id = Convert.ToInt32(p.Element("id")!.Value),
                                     DependentTask = Convert.ToInt32(p.Element("DependentTask")!.Value),
                                     DependsOnTask = Convert.ToInt32(p.Element("DependOnTask")!.Value)
                                 });
            }

            catch { dependeencies = null; }
            return dependeencies.ToList();
        }
    
    }

    public void Update(Dependeency item)
    {
        XElement? dependeency;
        dependeency = dependeencyRoot!.Elements().FirstOrDefault(e => Convert.ToInt32(e.Element("id")!.Value) == item.Id);

        if (dependeency != null) { throw new DalDoesNotExistException($"Dependeency with ID={item.Id} does not exist"); }

        dependeency!.Element("DependentTask")!.Value = item.DependentTask.ToString();
        dependeency.Element("DependsOnTask")!.Value = item.DependsOnTask.ToString();

        dependeencyRoot.Save(dependeencyPath);
        
    }
}
