using MessageQueue.Models;

namespace MessageQueue.Interfaces;

public interface IMessageProducer
{
    void SendMessage<T>(T message) where T: ICorrelated;
}
