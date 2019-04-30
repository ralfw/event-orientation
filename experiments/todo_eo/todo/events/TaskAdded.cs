using eventstore;

namespace todo.events
{
    class TaskAdded : Event {
        public string Subject { get; set; }
    }
}