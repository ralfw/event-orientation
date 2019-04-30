using System;
using eo.pipeline;

namespace todo.messages.queries.tasks
{
    class TasksProcessor : IMessageProcessor
    {
        public Output Process(Message msg, IMessageContext model) {
            switch (msg) {
                case TasksQuery _:
                    throw new NotImplementedException();
                
                default:
                    throw new NotImplementedException($"Message processing not implemented for '{msg}'!");
            }
        }
    }
}