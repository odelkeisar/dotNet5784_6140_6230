using System.Reflection.Emit;

namespace BO;
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
public class BlNegativeHourlyWageException : Exception //שכר שעתי שלילי
{
    public BlNegativeHourlyWageException(string? message) : base(message) { }
}

[Serializable]
public class BlWrongNegativeIdException : Exception //id שלילי
{
    public BlWrongNegativeIdException(string? message) : base(message) { }
}

[Serializable]
public class BlAlreadyExistsException : Exception //האובייקט כבר קיים
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlWrongEmailException : Exception //כתובת מייל שגויה
{
    public BlWrongEmailException(string? message) : base(message) { }
}

[Serializable]
public class BlChefOnTaskException : Exception //לא ניתן למחוק שף שכבר במשימה
{
    public BlChefOnTaskException(string? message) : base(message) { }
}

[Serializable]
public class BlEmptyStringException : Exception //מחרוזת ריקה
{
    public BlEmptyStringException(string? message) : base(message) { }
}


[Serializable]
public class BlNoChefsAccordingLevelException : Exception //חסר מהנדסים לפי רמה מסוימת
{
    public BlNoChefsAccordingLevelException(string? message) : base(message) { }
}


[Serializable]
public class BlNoUnassignedChefsException : Exception //חסר מהנדסים לפי רמה מסוימת
{
    public BlNoUnassignedChefsException(string? message) : base(message) { }
}



[Serializable]
public class BlChefLevelTooLowException : Exception //רמת מהנדס נמוכה מדי
{
    public BlChefLevelTooLowException(string? message) : base(message) { }
}

[Serializable]
public class BlTaskAlreadyAssignedException : Exception //המשימה כבר מוקצית למהנדס אחר
{
    public BlTaskAlreadyAssignedException(string? message) : base(message) { }
}


 [Serializable]
public class BlNoChangeChefAssignmentException : Exception //השף כבר מוקצה למשימה אחרת
{
    public BlNoChangeChefAssignmentException(string? message) : base(message) { }
}