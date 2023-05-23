namespace MessageQueue.Models;

public class ICorrelated
{
    public string? SpanId { get; set; }

    public string? TraceId { get; set; }
}
