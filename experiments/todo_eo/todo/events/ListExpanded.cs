using eventstore;

namespace todo.events
{
    class ListExpanded : Event {
        public string ListId { get; set; }
        public string TaskId { get; set; }
    }
}