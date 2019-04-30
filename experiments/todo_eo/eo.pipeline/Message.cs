namespace eo.pipeline
{
    public interface Message {}
    
    public interface Incoming : Message {}
    public interface Outgoing : Message {}
    
    
    public interface Request : Incoming {}
    public interface Response : Outgoing {}
    
    public class Notification : Incoming, Outgoing {}
    
    
    public class Command : Request {}

    public class Status : Response {}
    
    public class Success : Status {}
    public class Success<T> : Success
    {
        public T Value { get; }
        
        public Success(T value) {
            Value = value;
        }   
    }
    
    public class Failure : Status {
        public string Explanation { get; }
        
        public Failure(string explanation) {
            Explanation = explanation;
        }
    }
    
    
    public class Query : Request {}

    public class Result : Response {}
    
    
    public class NoResponse : Response {}
}