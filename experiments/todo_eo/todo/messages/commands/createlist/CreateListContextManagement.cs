using System;
using System.Collections.Generic;
using System.Linq;
using eo.pipeline;
using eventstore;
using todo.events;

namespace todo.messages.commands.createlist
{
    //TODO: split into builder and loader; connect with shared persistent context
    class CreateListContextManagement : IContextManagement
    {
        private List<string> _listNames;

        public CreateListContextManagement(EventStore es) {
            Update(es.Replay());
        }

 
        public IMessageContext Load(Message message)
        {
            switch (message) {
                case CreateListCommand _: 
                    return new CreateListContext {ListNames = _listNames};
                
                default:
                    throw new InvalidOperationException($"{this.GetType().Name} unable to build context for message of type {message.GetType().Name}!");
            }
        }
        
        
        public void Update(IEnumerable<Event> events) {
            _listNames = Apply(_listNames, events);
        }
        
        List<string> Apply(List<string> listNames, IEnumerable<Event> events)
            => events.Where(e => e is ListCreated)
                     .Aggregate(listNames ?? new List<string>(),
                         (m, e) => {
                             m.Add((e as ListCreated).Name);
                             return m;
                         });
    }
}