using System;
using System.Linq;
using eo.pipeline;

namespace todo.messages.queries.lists
{
    class ListsProcessor : IMessageProcessor
    {
        public Output Process(Message msg, IMessageContext model) {
            switch (msg) {
                case ListsQuery query:
                    var model_ = model as ListsContext;
                    var lists = model_.Lists.Select(l => new ListsQueryResult.ListInfo {
                        Id = l.Id,
                        Name = l.Name
                    });
                    return new Output(new ListsQueryResult{Lists = lists});
                
                default:
                    throw new NotImplementedException($"Message processing not implemented for '{msg}'!");
            }
        }
    }
}