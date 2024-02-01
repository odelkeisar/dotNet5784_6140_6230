using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi;

/// <summary>
/// Generic interface of CRUD functions
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICrud<T> where T : class
{
    int Create(T item); 
    T? Read(int id);
    T? Read(Func<T, bool> filter); //The function receives a boolean delegate and it returns the first object that satisfies the condition
    IEnumerable<T?> ?ReadAll(Func<T, bool>? filter = null); //The function receives a boolean delegate, it operates on each of the members of the list and returns the objects on which the delegate returns true
    void Update(T item); 
    void Delete(int id);
    void DeleteAll();

  
}

