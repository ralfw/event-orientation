namespace eo.pipeline
{
    public interface IMessageProcessor
    {
        Output Process(Message msg, IMessageContext model);
    }
}