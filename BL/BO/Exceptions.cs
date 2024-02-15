using System;
using System.Reflection.Emit;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
public class BlChefLevelTooLowException : Exception //רמת  נמוכה מדי
{
    public BlChefLevelTooLowException(string? message) : base(message) { }
}

[Serializable]
public class BlTaskAlreadyAssignedException : Exception //המשימה כבר מוקצית לשף אחר
{
    public BlTaskAlreadyAssignedException(string? message) : base(message) { }
}


 [Serializable]
public class BlNoChangeChefAssignmentException : Exception //השף כבר מוקצה למשימה אחרת
{
    public BlNoChangeChefAssignmentException(string? message) : base(message) { }
}
[Serializable]
public class BlScheduledStartDateNoUpdatedException : Exception //למשימות קודמות אין תאריך התחלה מתוכנן
{
    public BlScheduledStartDateNoUpdatedException(string? message) : base(message) { }
}


  [Serializable]
    public class BlEarlyFinishDateFromPreviousTaskException : Exception //תאריך סיום לעידכון אינו יכול להיות מוקדם יותר ממשימה שקודמת לו
{
    public BlEarlyFinishDateFromPreviousTaskException(string? message) : base(message) { }
}
[Serializable]
    public class BlATaskCannotBeDeletedException: Exception //אסור למחוק משימה שיש לה תלויות
{
    public BlATaskCannotBeDeletedException(string? message) : base(message) { }
}
[Serializable]
public class BlNoTasksToCompleteException : Exception //אסור למחוק משימה שיש לה תלויות
{
    public BlNoTasksToCompleteException(string? message) : base(message) { }
}

[Serializable]
public class BlWrongDateException : Exception //,תאריך שגוי
{
    public BlWrongDateException(string? message) : base(message) { }
}

[Serializable]
public class BllackingInLevelException : Exception //חסר רמה
{
    public BllackingInLevelException(string? message) : base(message) { }
}
[Serializable]
public class BlChefLevelNoEnteredException : Exception  //לא הוכנס רמת מהנדס
{
    public BlChefLevelNoEnteredException(string? message) : base(message) { }
}
[Serializable]
public class BlNoTasksbyCriterionException : Exception //אין משימות מתאימות לקרטריון המבוקש
{
    public BlNoTasksbyCriterionException(string? message) : base(message) { }
}
[Serializable]

public class BlInappropriateStepException : Exception //הפעולה לא מתאימה לשלב הפרוקיט
{
    public BlInappropriateStepException(string? message) : base(message) { }
}
[Serializable]
public class BlUnablToAssociateException : Exception //לא ניתן לשיוך
{
    public BlUnablToAssociateException(string? message) : base(message) { }
}
[Serializable]
public class BlScheduledStartDateMayNotBeChangedException : Exception //לא ניתן לשנות תאריך מתוכנן להתחלה
{
    public BlScheduledStartDateMayNotBeChangedException(string? message) : base(message) { }
}

[Serializable]
public class BlProblemAboutRequiredEffortTimeException : Exception //שגיאה משך זמן המשימה
{
    public BlProblemAboutRequiredEffortTimeException(string? message) : base(message) { }
} 