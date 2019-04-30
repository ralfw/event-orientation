using System;
using System.Collections.Generic;
using System.Linq;
using eo.pipeline;
using eventstore;
using todo.events;

namespace todo.messages.commands.createtodo
{
    class CreateTodoContextManagement :IContextManagement
    {
        private readonly EventStore _es;

        public CreateTodoContextManagement(EventStore es) {
            _es = es;
        }

 
        public IMessageContext Load(Message message) {
            switch (message) {
                case CreateTodoCommand _: {
                    var events = _es.Replay(typeof(ListCreated));
                    var listIds = events
                        .Select(e => (ListCreated) e)
                        .Aggregate(new List<string>(),
                            (m, e) => {
                                m.Add(e.Id);
                                return m;
                            });
                    return new CreateTodoContext() {ListIds = listIds};
                }
                
                default:
                    throw new InvalidOperationException($"{this.GetType().Name} unable to build context for message of type {message.GetType().Name}!");
            }
        }


        public void Update(IEnumerable<Event> events) { }
    }
}