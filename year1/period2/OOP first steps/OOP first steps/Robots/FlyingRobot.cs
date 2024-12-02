using OOP_first_steps.EventHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_first_steps.Robots
{
    internal class FlyingRobot: Robot
    {
        public FlyingRobot(NewEventHandler eventHandler) : base(eventHandler) {
            Console.WriteLine("FlyingRobot Initiated");
            this.EventHandler.On("init", this, FlyingRobot.BattleCry);
        }

        static void BattleCry()
        {
            Console.WriteLine("BattleCry Initiated");
        }

    }
}
