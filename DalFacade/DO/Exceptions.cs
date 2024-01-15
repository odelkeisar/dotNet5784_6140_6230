namespace DO;
[Serializable]

public class DalDoesNotExistException : Exception //Exception for an entity that does not exist in the list
{
    public DalDoesNotExistException(string? message) : base(message) { }
}
[Serializable]

public class DalAlreadyExistsException : Exception //Exception for an ID number that already exists in the list
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}

public class DalXMLFileLoadCreateException: Exception // Exception of XmlTools
{
    public  DalXMLFileLoadCreateException(string? message) : base(message)  { }
}