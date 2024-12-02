using OOP_first_steps.EventHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_first_steps.Robots
{
    internal class WheeledRobot: Robot
    {
        public WheeledRobot(NewEventHandler eventHandler) : base(eventHandler)
        {
            Console.WriteLine("WheeledRobot Initiated");
            this.EventHandler.Emit("init", true);
        }
    }
}
