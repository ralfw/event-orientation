using eventstore;

namespace eo.pipeline
{
    public struct Output
    {
        public Output(Message message) {
            Message = message;
            Events = new Event[0];
            Commands = new Command[0];
        }

        public Output(Message message, Event[] events) {
            Message = message;
            Events = events;
            Commands = new Command[0];
        }

        public Output(Command[] commands) {
            Message = new NoResponse();
            Events = new Event[0];
            Commands = commands;
        }
        
        
        public readonly Message Message;
        public readonly Event[] Events;
        public readonly Command[] Commands;
    }
}