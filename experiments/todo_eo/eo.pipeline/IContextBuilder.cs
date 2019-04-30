using System.Collections.Generic;
using eventstore;

namespace eo.pipeline
{
    public interface IContextBuilder
    {
        void Update(IEnumerable<Event> events);
    }
}