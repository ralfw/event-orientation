using System;
using System.Collections.Generic;
using eo.pipeline;
using eventstore;
using todo.events;

namespace todo.messages.commands.createtodo
{
    class CreateTodoProcessor : IMessageProcessor
    {
        public Output Process(Message msg, IMessageContext model) {
            switch (msg) {
                case CreateTodoCommand cmd:
                    var existingListIds = new HashSet<string>((model as CreateTodoContext).ListIds);
                    if (existingListIds.Contains(cmd.ListId) is false)
                        return new Output(new Failure($"Invalid parent list provided!"));

                    var e = new TaskAdded {Subject = cmd.Subject};
                    var events = new Event[] {
                        e,
                        new ListExpanded{ListId = cmd.ListId, TaskId = e.Id} 
                    };
                    return new Output(new Success(), events);
                
                default:
                    throw new NotImplementedException($"Message processing not implemented for '{msg}'!");
            }
        }
    }
}