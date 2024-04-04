namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Linq;
/// <summary>
/// Implementation of chef's crud functions
/// </summary>
internal class ChefImplementation : IChef
{
    readonly string s_chefs_xml = "chefs";
    string chefPath = @"chefs.xml";

    /// <summary>
    /// add a new member of type chef
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="DalAlreadyExistsException"></exception>
    public int Create(Chef item)
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>(s_chefs_xml);
        if (listChef.Exists(chef => chef?.Id == item.Id))
            throw new DalAlreadyExistsException($"An object of type Chef with {item.Id} already exists ");
        listChef.Add(item);
        listChef = listChef.OrderBy(chef => chef.Name).ToList();
        XMLTools.SaveListToXMLSerializer(listChef, s_chefs_xml);
        return item.Id;
    }

    /// <summary>
    /// delete member from the list according the id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>(s_chefs_xml);

        Chef? chef = Read(id);
        if (chef == null)
            throw new DalDoesNotExistException($"Dependeency with ID={id} does not exist");
        listChef.Remove(chef!);
        XMLTools.SaveListToXMLSerializer(listChef, s_chefs_xml);
        Chef? item2 = new Chef(chef.Id, true, chef.Email, chef.Cost, chef.Name, chef.Level);
        listChef.Add(item2);
        XMLTools.SaveListToXMLSerializer(listChef, s_chefs_xml);
    }

    /// <summary>
    /// The function checks which value to return by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Chef? Read(int id)
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>(s_chefs_xml);
        Chef? chef = listChef.Where(chef => chef.deleted == false).Where(p => p?.Id == id).FirstOrDefault();
        return chef;
    }

    /// <summary>
    /// The function checks which value to return according to the condition in the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Chef? Read(Func<Chef, bool> filter)
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>(s_chefs_xml);
        Chef? chef = listChef.Where(chef => chef.deleted == false).Where(filter).FirstOrDefault();
        return chef;
    }

    /// <summary>
    /// The function returns all elements in the list or it returns only those that meet the condition in the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Chef> ReadAll(Func<Chef, bool>? filter = null)
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>(s_chefs_xml);
        if (filter == null)
            return listChef!.Where(chef => chef.deleted == false).ToList(); //retun the list
        else
            return listChef!.Where(chef => chef.deleted == false).Where(filter).ToList();
    }

    public IEnumerable<Chef> ReadAll_deleted()
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>(s_chefs_xml);
       
        return listChef!.Where(chef => chef.deleted == true).ToList(); //retun the list
    }

    /// <summary>
    /// The method edits an element from the list according to the user's request
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Chef item)
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>(s_chefs_xml);
        Chef? item1 = Read(item.Id);

        if (item1 == null)
            throw new DalDoesNotExistException($"לא קיימת תלות עם המספר המזהה {item.Id}");


        listChef!.Remove(item1); //remove item1 from the list
        listChef!.Add(item); //add item to the list
        listChef = listChef.OrderBy(chef => chef.Name).ToList();
        XMLTools.SaveListToXMLSerializer(listChef, s_chefs_xml);
    }

    public void Recovery(Chef item)
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>(s_chefs_xml);
        Chef ? chef = listChef.Where(chef=>chef.Id==item.Id).FirstOrDefault();

        if (chef == null)
            throw new DalDoesNotExistException($"לא קיים שף עם המספר המזהה{item.Id}");

        listChef!.Remove(chef); //remove item1 from the list
        listChef!.Add(item); //add item to the list
        listChef = listChef.OrderBy(chef => chef.Name).ToList();
        XMLTools.SaveListToXMLSerializer(listChef, s_chefs_xml);
    }

    public void DeleteAll()
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>(s_chefs_xml);
        listChef.Clear();
        XMLTools.SaveListToXMLSerializer(listChef, s_chefs_xml);
    }

    public IEnumerable<Chef> ReadAllDeleted()
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>(s_chefs_xml);
        return listChef!.Where(item => item.deleted == true).ToList(); //retun the list
    }

}