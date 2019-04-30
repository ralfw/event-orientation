using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace eventstore
{
    public class EventStore
    {
        private readonly string _path;
        private readonly JavaScriptSerializer _json;

        public event Action<IEnumerable<Event>> OnRecorded = _ => { };
        
        public EventStore(string path) {
            _path = path;
            if (Directory.Exists(_path) is false)
                Directory.CreateDirectory(_path);
            
            _json = new JavaScriptSerializer();
        }

        public void Record(Event e) => Record(new[] {e});
        public void Record(IEnumerable<Event> events) {
            var index = Directory.GetFiles(_path).Length;
            events.ToList().ForEach(e => Store(e, ++index));
            OnRecorded(events);
        }


        public IEnumerable<Event> Replay()
            => Directory.GetFiles(_path, "*.txt").Select(Load);

        public IEnumerable<Event> Replay(params Type[] eventTypes) {
            var eventTypes_ = new HashSet<Type>(eventTypes);
            return Replay().Where(e => eventTypes_.Contains(e.GetType()));
        }

        
        private void Store(Event e, long index) {
            var eventName = e.GetType().AssemblyQualifiedName;
            var data = _json.Serialize(e);
            
            var filepath = Path.Combine(_path, $"{index:x8}.txt");
            File.WriteAllLines(filepath, new[]{e.Id, eventName});
            File.AppendAllText(filepath, data);
        }
        
        private Event Load(string filename) {
            var lines = File.ReadAllLines(filename);
            var eventName = lines.Skip(1).First();
            var data = string.Join("\n", lines.Skip(2));
            return (Event)_json.Deserialize(data, Type.GetType(eventName));
        }
    }
}