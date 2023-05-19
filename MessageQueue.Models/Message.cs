namespace MessageQueue.Models;

public class Message: ICorrelated
{

    public Guid Id { get; set; }

    public Item UpdatedItem { get; set; }
}
