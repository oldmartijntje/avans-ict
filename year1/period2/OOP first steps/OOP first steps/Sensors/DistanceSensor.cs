using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_first_steps.Sensors
{
    internal class DistanceSensor : ISensor
    {
        public DistanceSensor()
        {
            Console.WriteLine("Sensor Initiated");
        }

        public double GetSensorData()
        {
            Console.WriteLine("Sensor Data Reading");
            return 1;
        }
    }
}
