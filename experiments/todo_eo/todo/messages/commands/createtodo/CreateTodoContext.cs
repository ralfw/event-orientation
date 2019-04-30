using System.Collections.Generic;
using eo.pipeline;

namespace todo.messages.commands.createtodo
{
    class CreateTodoContext : IMessageContext {
        public IEnumerable<string> ListIds;
    }
}