using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_first_steps.EventHandler
{
    internal class NewEventHandler
    {
        private List<Event> Callbacks;
        private int LastId;

        public NewEventHandler()
        {
            this.LastId = 0;
            this.Callbacks = new List<Event>();
            Console.WriteLine("EventHandler Initiated");
        }

        // this the "This event has happened" function. It emits the events to everything subscribed to it.
        public void Emit(string eventName, object value)
        {
            foreach (var callback in Callbacks)
            {
                if (callback.EventName == eventName)
                {
                    callback.Callback();
                }
            }
            Console.WriteLine("Event Emitted");
        }

        // subscribe to an event
        public int On(string eventName, object caller, Action callback) {
            this.LastId++;
            this.Callbacks.Add(new Event(this.LastId, eventName, caller, callback));
            return this.LastId;
        }

        // unsubscribe to an event
        public void Off(int eventId)
        {
            this.Callbacks.RemoveAll(callback => callback.Id == eventId);
        }

        // unsubscribe from all events :3
        // this is why we have the caller, this also means we can't use the event function outside an object.
        public void Unsubscribe(object caller)
        {
            this.Callbacks.RemoveAll(callback => callback.Caller == caller);
        }
    }
}
