namespace BO;
public class TaskInList
{
    public int Id { get; init; }
    public string? Alias { get; set; } //כינוי
    public string? Description { get; set; }//תיאור
    public Status status { get; set; }

    public override string ToString() => this.ToString_();
    public string ToString_()
    {
        return ($@"
מספר זהות= {Id} 
כינוי= {Alias} 
תיאור= {Description} 
סטטוס= {status} 
");
    }                  
}
