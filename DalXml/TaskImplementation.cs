﻿using DalApi;
using DO;
using System.Data.Common;
using System.Xml.Linq;

namespace Dal;
/// <summary>
/// Implementation of crud functions of task
/// </summary>
internal class TaskImplementation : ITask1
{
    string taskPath = @"tasks.xml";
    const string s_xml_dir = @"..\xml\";

    readonly string s_tasks_xml = "tasks";
    readonly string s_data_config_xml = "data-config";

    public TaskImplementation()
    {
        XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
    }

    /// <summary>
    /// //add dependeency to a task
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Task1 item)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
        int newId = Config.NextTask1Id;
        Task1 item1 = item with { Id = newId };
        listTask!.Add(item1);    //add item to the list
        listTask = listTask.OrderBy(task => task.Id).ToList();
        XMLTools.SaveListToXMLSerializer(listTask, s_tasks_xml);
        return newId;
    }

    /// <summary>
    /// delete member from the list according the id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
        Task1? item1 = Read(id);

        if (item1 == null)
            throw new DalDoesNotExistException($"לא קיימת משימה עם מספר זהות {id}");
        listTask!.Remove(item1);

        XMLTools.SaveListToXMLSerializer(listTask, s_tasks_xml);
    }

    /// <summary>
    /// The function checks which value to return by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task1? Read(int id)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
        return listTask!.FirstOrDefault(x => (x.Id == id)); //Returns the first entry in the list with this ID

    }

    /// <summary>
    /// The function checks which value to return according to the condition in the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Task1? Read(Func<Task1, bool> filter)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
        return listTask!.FirstOrDefault(filter); //Returns the first value in the list equal to the filter
    }

    /// <summary>
    /// The function returns all elements in the list or it returns only those that meet the condition in the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Task1?>? ReadAll(Func<Task1, bool>? filter = null)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
        if (filter == null)
            return listTask!.Select(item => item).ToList(); //retun the list
        else
            return listTask!.Where(filter).ToList();
    }

    /// <summary>
    /// The method edits an element from the list according to the user's request
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Task1 item)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);

        Task1? item1 = Read(item.Id);

        if (item1 == null)
            throw new DalDoesNotExistException($"לא קיית משימה עם מספר זהות {item.Id}");

        listTask!.Remove(item1); //remove item1 from the list

        listTask.Add(item); //add item to the list
        listTask = listTask.OrderBy(task => task.Id).ToList();

        XMLTools.SaveListToXMLSerializer(listTask, s_tasks_xml);
    }
    public void DeleteAll()
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
        listTask.Clear();
        XMLTools.SaveListToXMLSerializer(listTask, s_tasks_xml);
    }

    public void UpdateStartProject(DateTime startProject)
    {

        XElement root = XElement.Load($"{s_xml_dir + s_data_config_xml}.xml");
        root.Element("startProject")!.Value = startProject.ToString();
        root.Save($"{s_xml_dir + s_data_config_xml}.xml");
    }
    public void UpdateEndProject(DateTime endProject)
    {
        XElement root = XElement.Load($"{s_xml_dir + s_data_config_xml}.xml");
        root.Element("endProject")!.Value = endProject.ToString();
        root.Save($"{s_xml_dir + s_data_config_xml}.xml");
    }

    public DateTime? ReadStartProject()
    {
        XElement root = XElement.Load($"{s_xml_dir + s_data_config_xml}.xml");
        string? statProject = root.Element("startProject")?.Value;
        if (!string.IsNullOrEmpty(statProject))
            return DateTime.Parse(statProject);
        return null;
    }

    public DateTime? ReadEndProject()
    {
        XElement root = XElement.Load($"{s_xml_dir + s_data_config_xml}.xml");
        string? endProject = root.Element("endProject")?.Value;
        if (!string.IsNullOrEmpty(endProject))
            return DateTime.Parse(endProject);
        return null;
    }

    public void UpdateClockProject(DateTime clockProject)
    {
        XElement root = XElement.Load($"{s_xml_dir + s_data_config_xml}.xml");
        root.Element("clockProject")!.Value = clockProject.ToString();
        root.Save($"{s_xml_dir + s_data_config_xml}.xml");
    }
    public DateTime ReadClockProject()
    {
        XElement root = XElement.Load($"{s_xml_dir + s_data_config_xml}.xml");
        string ?clockProject = root.Element("clockProject")?.Value;
        if (!string.IsNullOrEmpty(clockProject))
            return DateTime.Parse(clockProject);
        return new DateTime(0, 0, 0);
       
    }


}