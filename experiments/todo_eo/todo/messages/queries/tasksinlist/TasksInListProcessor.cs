using System;
using System.Linq;
using eo.pipeline;

namespace todo.messages.queries.tasksinlist
{
    class TasksInListProcessor : IMessageProcessor
    {
        public Output Process(Message msg, IMessageContext model) {
            switch (msg) {
                case TasksInListQuery query:
                    var model_ = model as TasksInListContext;
                    var tasks = model_.Tasks.Select(t => new TasksInListQueryResult.TaskInfo {
                        Id = t.Id,
                        Subject = t.Subject
                    });
                    return new Output(new TasksInListQueryResult {Tasks = tasks});
                
                default:
                    throw new NotImplementedException($"Message processing not implemented for '{msg}'!");
            }
        }
    }
}