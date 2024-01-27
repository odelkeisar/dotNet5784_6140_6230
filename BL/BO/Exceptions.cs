namespace DO;
[Serializable]
public class BlDoesNotExistException : Exception //אובייקט לא קיים
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}


[Serializable]
public class BlNullPropertyException : Exception //שגיאה עם עובדים עם ערך null 
{
    public BlNullPropertyException(string? message) : base(message) { }
}

[Serializable]
public class BlAlreadyExistsException : Exception //האובייקט כבר קיים
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}

