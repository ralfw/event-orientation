using System.Collections.Generic;
using eo.pipeline;

namespace todo.messages.queries.tasks
{
    class TasksContext : IMessageContext {
        public class TaskInfo {
            public string Id;
            public string Subject;
        }

        public IEnumerable<TaskInfo> Tasks;
    }
}