using OOP_first_steps.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_first_steps
{
    internal class RobotController
    {
        List<ISensor> Sensors;
        public RobotController() {
            this.Sensors = new List<ISensor>();
            this.Sensors.Add(new DistanceSensor());
            this.Sensors.Add(new DistanceSensor());
            Console.WriteLine("Controller Initiated");
            this.GetData();
        }

        public void GetData()
        {
            foreach (var sensor in this.Sensors)
            {
                sensor.GetSensorData();
            }
            Console.WriteLine("Controller Asked all Sensors to talk correctly");
        }
    }
}
