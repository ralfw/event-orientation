using System.Collections.Generic;
using eo.pipeline;

namespace todo.messages.queries.tasksinlist
{
    class TasksInListQueryResult : Result
    {
        public class TaskInfo  {
            public string Id;
            public string Subject;
        }

        public IEnumerable<TaskInfo> Tasks;
    }
}