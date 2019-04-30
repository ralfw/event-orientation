namespace eo.pipeline
{
    public class Message {}
    
    
    public class Request : Message {}
    public class Response : Message {}
    
    
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