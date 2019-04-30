using System;
using System.Collections.Generic;
using System.Linq;
using eo.pipeline;
using eventstore;
using todo.events;

namespace todo.messages.queries.tasks
{
    class TasksContextManagement : IContextManagement
    {
        private readonly EventStore _es;

        public TasksContextManagement(EventStore es)
        {
            _es = es;
        }


        public IMessageContext Load(Message message)
        {
            switch (message)
            {
                case TasksQuery _:
                    var events = _es.Replay(typeof(TaskAdded)).ToArray();

                    var tasks = events
                        .Aggregate(new List<TasksContext.TaskInfo>(),
                            (agg, e) => {
                                agg.Add(new TasksContext.TaskInfo {
                                    Id = e.Id,
                                    Subject = ((TaskAdded)e).Subject
                                });
                                return agg;
                            });
                    
                    return new TasksContext {Tasks = tasks};
                
                default:
                    throw new InvalidOperationException($"{this.GetType().Name} unable to build context for message of type {message.GetType().Name}!");

            }
        }

        
        public void Update(IEnumerable<Event> events) {}
    }
}