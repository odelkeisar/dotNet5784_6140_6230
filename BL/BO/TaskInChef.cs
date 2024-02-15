namespace BO;
/// <summary>
/// Id and alias of task
/// </summary>
public class TaskInChef
{
    public int? Id { get; init; }
    public string? Alias { get; set; }
    public override string ToString() => ToString_();
    public string ToString_()
    {
        return $"{Id}, {Alias}";
    }
}
