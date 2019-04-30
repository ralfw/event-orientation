using System;
using System.Collections.Generic;
using eo.pipeline;
using eventstore;
using todo.events;

namespace todo.messages.commands.createlist
{
    class CreateListProcessor : IMessageProcessor
    {
        public Output Process(Message msg, IMessageContext model) {
            switch (msg) {
                case CreateListCommand cmd:
                    var existingListNames = new HashSet<string>((model as CreateListContext).ListNames);
                    if (existingListNames.Contains(cmd.Name))
                        return new Output(new Failure($"List name '{cmd.Name}' already exists!"));
                    var e = new ListCreated {Name = cmd.Name};
                    return new Output(new Success(), new Event[]{e});

                default:
                    throw new NotImplementedException($"Message processing not implemented for '{msg}'!");
            }
        }
    }
}