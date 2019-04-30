using System;
using System.Linq;
using System.Web.Script.Serialization;

namespace eventstore
{
    static class EventSerialization
    {
        public static string Serialize(Event e) {
            var eventName = e.GetType().AssemblyQualifiedName;
            var data = new JavaScriptSerializer().Serialize(e);
            var parts = new[]{eventName, data};
            return string.Join("\n", parts);
        }

        public static Event Deserialize(string e) {
            var lines = e.Split('\n');
            var eventName = lines.First();
            var data = string.Join("\n", lines.Skip(1));
            return (Event)new JavaScriptSerializer().Deserialize(data, Type.GetType(eventName));
        }
    }
}