using System;
using System.Collections.Generic;
using System.Linq;
using eo.pipeline;
using eventstore;
using todo.events;

namespace todo.messages.queries.lists
{
    class ListsContextManagement : IContextManagement
    {
        private readonly EventStore _es;

        public ListsContextManagement(EventStore es) {
            _es = es;
        }

 
        public IMessageContext Load(Message message) {
            switch (message) {
                case ListsQuery _: {
                    var events = _es.Replay(typeof(ListCreated));
                    var lists = events
                        .Select(e => (ListCreated) e)
                        .Aggregate(new List<ListsContext.ListInfo>(),
                            (m, e) => {
                                m.Add(new ListsContext.ListInfo {
                                    Id = e.Id,
                                    Name = e.Name
                                });
                                return m;
                            });
                    return new ListsContext() {Lists = lists};
                }

                default:
                    throw new InvalidOperationException($"{this.GetType().Name} unable to build context for message of type {message.GetType().Name}!");
            }
        }

        
        public void Update(IEnumerable<Event> events) {
        }
    }
}