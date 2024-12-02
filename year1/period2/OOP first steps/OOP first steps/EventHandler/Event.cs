using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_first_steps.EventHandler
{
    internal class Event
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public object Caller { get; set; }
        public Action Callback { get; set; }

        public Event(int id, string eventName, object caller, Action callback) { 
            this.Id = id;
            this.EventName = eventName;
            this.Caller = caller;
            this.Callback = callback;
        }
    }
}
