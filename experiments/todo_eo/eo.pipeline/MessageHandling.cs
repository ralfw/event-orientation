using System;
using System.Collections.Generic;
using System.Linq;
using eventstore;

namespace eo.pipeline
{
    public class MessageHandling {
        private class Handlers {
            public Func<Message, IMessageContext> Load;
            public Func<Message,IMessageContext,Output> Process;
            public Action<Event[]> Update;
        }

        private readonly Dictionary<Type, Handlers> _map = new Dictionary<Type, Handlers>();

        private readonly EventStore _es;


        public MessageHandling(EventStore es) => _es = es;


        public void Register<TMessage>(IContextManagement ctxManagement, IMessageProcessor processor)
            => Register<TMessage>(ctxManagement.Load, processor.Process, ctxManagement.Update);
        
        public void Register<TMessage>(Func<Message, IMessageContext> load,
                                       Func<Message, IMessageContext, Output> process, 
                                       Action<Event[]> update)
        {
            _map[typeof(TMessage)] = new Handlers {
                                         Load = load,
                                         Process = process,
                                         Update = update
                                     };
        }


        public Message Handle(Message msg) {
            var handlers = _map[msg.GetType()];
            return Handle(msg, handlers, _es);
        }
        
        private Message Handle(Message msg, Handlers handlers, EventStore es) {
            var context = handlers.Load(msg);
            
            var output = handlers.Process(msg, context);
            
            es.Record(output.Events);
            UpdateContexts(output.Events);
            
            HandleMany(output.Commands);
            
            return output.Outgoing;
        }

        void UpdateContexts(Event[] events) {
            foreach (var update in _map.Values.ToList().Select(handlers => handlers.Update))
                update(events);
        }
        
        void HandleMany(IEnumerable<Command> commands) 
            => commands.ToList().ForEach(cmd => Handle(cmd));
    }
}