using System.Device.Gpio;
using Avans.StatisticalRobot;
using RobotProject.EventHandler;

Console.WriteLine("Hello world");

var eventHandler = new NewEventHandler();
var mqttHandler = new MqttHandler(eventHandler);
var brimnes = new WheeledRobot(eventHandler);

while (true)
{
    brimnes.Tick();
    System.Threading.Thread.Sleep(RobotConfig.LOOP_TICK_MS);
}

