namespace BlApi;
public static class Factory
{
    public static IBl Get() => new BlImplementation.Bl();  // הפונקציה Get מחזירה מופע של BlImplementation.Bl המממש את IBl
}