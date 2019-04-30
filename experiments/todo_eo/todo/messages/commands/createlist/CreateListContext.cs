using System.Collections.Generic;
using eo.pipeline;

namespace todo.messages.commands.createlist
{
    class CreateListContext : IMessageContext {
        public IEnumerable<string> ListNames;
    }
}