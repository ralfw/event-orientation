namespace eo.pipeline
{
    public interface IContextLoader
    {
        IMessageContext Load(Message message);
    }
}