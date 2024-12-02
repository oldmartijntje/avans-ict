using OOP_first_steps.EventHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_first_steps
{
    internal class Robot
    {
        private RobotController Controller;
        public NewEventHandler EventHandler;
        public Robot(NewEventHandler eventHandler) { 
            this.EventHandler = eventHandler;
            this.Controller = new RobotController();
            Console.WriteLine("Robot Initiated");
        }
    }
}
