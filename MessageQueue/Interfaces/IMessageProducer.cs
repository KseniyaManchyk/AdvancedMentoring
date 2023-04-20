namespace MessageQueue.Interfaces;

public interface IMessageProducer
{
    void SendMessage<T>(T message);
}
