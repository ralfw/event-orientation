using System.Collections.Generic;
using eo.pipeline;

namespace todo.messages.queries.lists
{
    class ListsContext : IMessageContext {
        public class ListInfo {
            public string Id;
            public string Name;
        }

        public IEnumerable<ListInfo> Lists;
    }
}