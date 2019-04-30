using eventstore;

namespace todo.events
{
    class ListCreated : Event {
        public string Name { get; set; }
    }
}