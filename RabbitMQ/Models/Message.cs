namespace RabbitMQ.Models;

public class Message
{
    public Guid Id { get; set; }

    public Item UpdatedItem { get; set; }
}
