using eventstore;

namespace eo.pipeline
{
    public struct Output
    {
        public Output(Message outgoing) {
            Outgoing = outgoing;
            Events = new Event[0];
            Commands = new Command[0];
        }

        public Output(Message outgoing, Event[] events) {
            Outgoing = outgoing;
            Events = events;
            Commands = new Command[0];
        }

        public Output(Command[] commands) {
            Outgoing = new NoResponse();
            Events = new Event[0];
            Commands = commands;
        }
        
        
        public readonly Message Outgoing;
        public readonly Event[] Events;
        public readonly Command[] Commands;
    }
}