using System;
using System.Collections.Generic;
using System.Linq;
using eo.pipeline;
using eventstore;
using todo.events;

namespace todo.messages.queries.tasksinlist
{
    class TasksInListContextManagement : IContextManagement
    {
        private readonly EventStore _es;

        public TasksInListContextManagement(EventStore es) {
            _es = es;
        }

 
        public IMessageContext Load(Message message) {
            switch (message) {
                case TasksInListQuery query:
                    var events = _es.Replay(typeof(TaskAdded), typeof(ListExpanded)).ToArray();

                    var taskIds = events.Where(e => e is ListExpanded)
                        .Where(e => ((ListExpanded)e).ListId == query.ListId)
                        .Aggregate(new HashSet<string>(), (agg, e) => {
                            agg.Add(((ListExpanded) e).TaskId);
                            return agg;
                        });

                    var tasks = events.Where(e => e is TaskAdded)
                        .Where(e => taskIds.Contains(e.Id))
                        .Aggregate(new List<TasksInListContext.TaskInfo>(),
                            (agg, e) => {
                                agg.Add(new TasksInListContext.TaskInfo {
                                    Id = e.Id,
                                    Subject = ((TaskAdded)e).Subject
                                });
                                return agg;
                            });
                    
                    return new TasksInListContext() {Tasks = tasks};
                
                default:
                    throw new InvalidOperationException($"{this.GetType().Name} unable to build context for message of type {message.GetType().Name}!");
            }
        }


        public void Update(IEnumerable<Event> events) {
        }
    }
}