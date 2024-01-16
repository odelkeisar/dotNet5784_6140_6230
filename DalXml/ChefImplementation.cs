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

    public int Create(Chef item)
    {

        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>(s_chefs_xml);
        if (listChef.Exists(chef => chef?.Id == item.Id))
            throw new DalAlreadyExistsException($"An object of type Chef with {item.Id} already exists ");
        listChef.Add(item);
        XMLTools.SaveListToXMLSerializer(listChef, s_chefs_xml);
        return item.Id;
    }

    public void Delete(int id)
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>(s_chefs_xml);

        Chef? chef = Read(id);
        if (chef == null)
            throw new DalDoesNotExistException($"Dependeency with ID={id} does not exist");
        listChef.Remove(chef!);
        XMLTools.SaveListToXMLSerializer(listChef, s_chefs_xml);
    }

    public Chef? Read(int id)
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>(s_chefs_xml);
        Chef? chef = listChef.Where(p => p?.Id == id).FirstOrDefault();
        return chef;
    }

    public Chef? Read(Func<Chef, bool> filter)
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>(s_chefs_xml);
        Chef? chef = listChef.Where(filter).FirstOrDefault();
        return chef;
    }
    public IEnumerable<Chef> ReadAll(Func<Chef, bool>? filter = null)
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>(s_chefs_xml);
        if (filter == null)
            return listChef!.Select(item => item).ToList(); //retun the list
        else
            return listChef!.Where(filter).ToList();
    }

    public void Update(Chef item)
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>(s_chefs_xml);
        Chef? item1 = Read(item.Id);

        if (item1 == null)
            throw new DalDoesNotExistException($"Dependeency with ID={item.Id} does not exist");

        listChef!.Remove(item1); //remove item1 from the list
        listChef!.Add(item); //add item to the list
        XMLTools.SaveListToXMLSerializer(listChef, s_chefs_xml);
    }

    public void DeleteAll()
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>(s_chefs_xml);
        listChef.Clear();
        XMLTools.SaveListToXMLSerializer(listChef, s_chefs_xml);
    }

}