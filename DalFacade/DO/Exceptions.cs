namespace DO;
[Serializable] 

public class DalDoesNotExistException : Exception //Exception for an entity that does not exist in the list
{
    public DalDoesNotExistException(string? message) : base(message) { }
}
[Serializable]

public class DalAlreadyExistsException : Exception // קיים מספר זהות זהה 
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}
[Serializable]
public class DalXMLFileLoadCreateException: Exception // Exception of XmlTools
{
    public  DalXMLFileLoadCreateException(string? message) : base(message)  { }
}

