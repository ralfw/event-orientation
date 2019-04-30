using System;

namespace eventstore
{
    public abstract class Event {
        public string Id { get; set; }

        public Event() { Id = Guid.NewGuid().ToString(); }
    }
}