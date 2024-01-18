using DalApi;
using DO;
namespace Dal;

using System;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Xml.Linq;

/// <summary>
/// Realization of crude functions using the XELement class
/// </summary>
internal class DependeencyImplementation : IDependeency
{
    readonly string s_dependeencies_xml = "dependeencies";
    XElement? dependeencyRoot;
    string dependeencyPath = @"dependeencies.xml";
    /// <summary>
    /// add dependeency to a collection
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Dependeency item) 
    {
        dependeencyRoot = XMLTools.LoadListFromXMLElement(s_dependeencies_xml);
        int newId = Config.NextDependeencyId;
        XElement id = new XElement("id", newId);
        XElement DependentTask = new XElement("DependentTask", item.DependentTask);
        XElement DependsOnTas = new XElement("DependsOnTask", item.DependsOnTask);

        dependeencyRoot!.Add(new XElement("dependeency", id, DependentTask, DependsOnTas));
        XMLTools.SaveListToXMLElement(dependeencyRoot, s_dependeencies_xml);
        return newId;
    }

    /// <summary>
    /// delete member from the list according the id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id) //delete dependeency
    {
        dependeencyRoot = XMLTools.LoadListFromXMLElement(s_dependeencies_xml);
        XElement? dependeency;
        dependeency = dependeencyRoot!.Elements().FirstOrDefault(e => Convert.ToInt32(e.Element("id")!.Value) == id);

        if (dependeency == null) { throw new DalDoesNotExistException($"Dependeency with ID={id} does not exist"); }
        dependeency!.Remove();
        

        XMLTools.SaveListToXMLElement(dependeencyRoot, s_dependeencies_xml);
    }

    /// <summary>
    /// The function checks which value to return by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependeency? Read(int id)
    {
        dependeencyRoot = XMLTools.LoadListFromXMLElement(s_dependeencies_xml);
        XElement? dependeency;
        dependeency = dependeencyRoot!.Elements().FirstOrDefault(e => Convert.ToInt32(e.Element("id")!.Value) == id);

        if (dependeency == null)
        {
            return null;
        }
        int Id = Convert.ToInt32(dependeency.Element("id")!.Value);
        int DependentTask = Convert.ToInt32(dependeency.Element("DependentTask")!.Value);
        int DependsOnTask = Convert.ToInt32(dependeency.Element("DependsOnTask")!.Value);
        Dependeency dependeency1 = new Dependeency(Id, DependentTask, DependsOnTask);

        return dependeency1;
    }


    /// <summary>
    /// The function checks which value to return according to the condition in the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Dependeency? Read(Func<Dependeency, bool> filter)
    {
        dependeencyRoot = XMLTools.LoadListFromXMLElement(s_dependeencies_xml);
        var dependeencies = dependeencyRoot!.Elements().Select(x => new Dependeency
        {
            Id = Convert.ToInt32(x.Element("id")!.Value),
            DependentTask = Convert.ToInt32(x.Element("DependentTask")!.Value),
            DependsOnTask = Convert.ToInt32(x.Element("DependsOnTask")!.Value)
        });
        Dependeency? dependeency = dependeencies.Where(filter).FirstOrDefault();
        return dependeency;
    }

    /// <summary>
    /// The function returns all elements in the list or it returns only those that meet the condition in the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Dependeency?>? ReadAll(Func<Dependeency, bool>? filter = null)
    {
        dependeencyRoot = XMLTools.LoadListFromXMLElement(s_dependeencies_xml);
        IEnumerable<Dependeency>? dependeencies;

        if (filter == null)
        {
            try
            {
                dependeencies = dependeencyRoot!.Elements().Select(x => new Dependeency
                {
                    Id = Convert.ToInt32(x.Element("id")!.Value),
                    DependentTask = Convert.ToInt32(x.Element("DependentTask")!.Value),
                    DependsOnTask = Convert.ToInt32(x.Element("DependsOnTask")!.Value)
                });
            }

            catch { return null; }
            return dependeencies.ToList();
        }
        try
        {
            dependeencies = dependeencyRoot!.Elements().Select(x => new Dependeency
            {
                Id = Convert.ToInt32(x.Element("id")!.Value),
                DependentTask = Convert.ToInt32(x.Element("DependentTask")!.Value),
                DependsOnTask = Convert.ToInt32(x.Element("DependsOnTask")!.Value)
            }) ;
            dependeencies = dependeencies.Where(x => filter(x));
        }
        catch { return null; }
        return dependeencies.ToList();
    }

    /// <summary>
    /// The method edits an element from the list according to the user's request
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Dependeency item)
    {
        dependeencyRoot = XMLTools.LoadListFromXMLElement(s_dependeencies_xml);
        XElement? dependeency;
        dependeency = dependeencyRoot!.Elements().FirstOrDefault(e => Convert.ToInt32(e.Element("id")!.Value) == item.Id);

        if (dependeency == null) { throw new DalDoesNotExistException($"Dependeency with ID={item.Id} does not exist"); }

        dependeency!.Element("DependentTask")!.Value = item.DependentTask.ToString();
        dependeency.Element("DependsOnTask")!.Value = item.DependsOnTask.ToString();

        XMLTools.SaveListToXMLElement(dependeencyRoot, s_dependeencies_xml);
    }

    public void DeleteAll()
    {
        dependeencyRoot = XMLTools.LoadListFromXMLElement(s_dependeencies_xml);
        dependeencyRoot.RemoveAll();
        XMLTools.SaveListToXMLElement(dependeencyRoot, s_dependeencies_xml);
    }
}


