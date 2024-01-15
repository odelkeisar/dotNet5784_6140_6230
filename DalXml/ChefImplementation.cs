namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Linq;

internal class ChefImplementation :IChef
{
    readonly string s_chefs_xml = "chefs";
    XElement? chefRoot;
    string chefPath = @"chefs.xml";
    public ChefImplementation() //opening a file
    {
        try
        {
            XMLTools.LoadListFromXMLSerializer<Chef>("chefs");
        }

        catch (DalXMLFileLoadCreateException ex)
        {
             Console.WriteLine("File upload problem");
        }
    }

    public int Create(Chef item)
    {

        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>("chefs");
        if (listChef.Exists(chef => chef?.Id == item.Id))
            throw new DalAlreadyExistsException($"An object of type Chef with {item.Id} already exists ");
        listChef.Add(item);

        XMLTools.SaveListToXMLSerializer(listChef, "chefs");
        return item.Id;
    }

    public void Delete(int id)
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>("chefs");

        Chef? chef = Read(id);
        if(chef != null)    
            throw new DalDoesNotExistException($"Dependeency with ID={id} does not exist");
        listChef.Remove(chef!);
        XMLTools.SaveListToXMLSerializer(listChef, "chefs");
    }

    public Chef? Read(int id)
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>("chefs");
        Chef? chef = listChef.Where(p => p?.Id == id).FirstOrDefault();
        return chef;
    }

    public Chef? Read(Func<Chef, bool> filter)
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>("chefs");
        Chef? chef = listChef.Where(filter).FirstOrDefault();
        return chef;
    }
    public IEnumerable<Chef> ReadAll(Func<Chef, bool>? filter = null)
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>("chefs");
        if (filter == null)
            return listChef!.Select(item => item).ToList(); //retun the list
        else
            return listChef!.Where(filter).ToList();
    }

    public void Update(Chef item)
    {
        var listChef = XMLTools.LoadListFromXMLSerializer<Chef>("chefs");
        Chef? item1 = Read(item.Id);

        if (item1 == null)
            throw new DalDoesNotExistException($"Dependeency with ID={item.Id} does not exist");

        listChef!.Remove(item1); //remove item1 from the list
        listChef!.Add(item); //add item to the list
        XMLTools.SaveListToXMLSerializer(listChef, "chefs");
    }
}
