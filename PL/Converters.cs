using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PL;

/// <summary>
/// המרת המספר למחרוזת אם זה אפס צריך להוסיף עם זה אחר צריך לעדכן
/// </summary>
class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "הוסף" : "עדכן";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// אם לא אפס מחזיר שקר כנראה בשלב שאי אפשר להוסיף נתונים
/// </summary>
class ConvertIdToBool : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value != 0 ? false:true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
/// <summary>
/// אם אפס מחזיר שקר כנראה בשלב שאי אפשר להוסיף נתונים
/// </summary>
class ConvertAssigningTaskToChef_ToBool : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? false : true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
/// <summary>
/// אם תאריך תחילת הפרויקט הוא לא נל הפונקציה מחזירה שקר
/// </summary>
class ConvertStartDateKey : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (DateTime?)value != null ? false : true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
/// <summary>
/// אם תאריך סיום הפרויקט הוא לא נל הפונקציה מחזירה שקר
/// </summary>
class ConvertFinalDateKey : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (DateTime?)value == null ? false : true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
/// <summary>
/// אם הערך לא אפס הפונקציה מחזירה אמת
/// </summary>
class ConverTtaskAssignmentToBool : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value != 0 ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// הצגת המשימות שטרם הוקצו רק כאשר הערכים המתוזמנים בלבד מוצגים
/// </summary>
class ConverTaskListCheckBoxToBool : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.Status)value == BO.Status.מתוזמן ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}