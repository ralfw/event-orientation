using System;
using System.Collections.Generic;
using System.Linq;
using eventstore;

namespace eo.pipeline
{
    public class MessageHandling : IDisposable {
        private class Handlers {
            public Func<Message, IMessageContext> LoadContext;
            public Func<Message,IMessageContext,Output> Process;
            public Action<Event[]> UpdateContext;
        }

        private readonly Dictionary<Type, Handlers> _map = new Dictionary<Type, Handlers>();

        private readonly EventStore _es;


        public MessageHandling(EventStore es) {
            _es = es;
        }


        public void Register<TMessage>(IContextManagement ctxManagement, IMessageProcessor processor)
            => Register<TMessage>(ctxManagement.Load, processor.Process, ctxManagement.Update);
        
        public void Register<TMessage>(Func<Message, IMessageContext> load,
                                       Func<Message, IMessageContext, Output> process, 
                                       Action<Event[]> update)
        {
            _map[typeof(TMessage)] = new Handlers {
                                         LoadContext = load,
                                         Process = process,
                                         UpdateContext = update
                                     };
        }


        public Message Handle(Message message) {
            var handlers = _map[message.GetType()];
            return Handle(message, handlers, _es);
        }
        
        private Message Handle(Message message, Handlers handlers, EventStore es) {
            var context = handlers.LoadContext(message);
            var output = handlers.Process(message, context);
            es.Record(output.Events);
            UpdateContexts(output.Events);
            HandleCommands();
            return output.Message;
            
            
            void HandleCommands() 
                => output.Commands.ToList().ForEach(cmd => Handle(cmd));
        }
        
        
        private void UpdateContexts(Event[] events) {
            foreach (var update in _map.Values.ToList().Select(handlers => handlers.UpdateContext))
                update(events);
        }

        
        public void Dispose() {}
    }
}