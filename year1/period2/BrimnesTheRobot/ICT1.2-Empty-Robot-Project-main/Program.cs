using System.Device.Gpio;
using Avans.StatisticalRobot;

Console.WriteLine("Hello world");

Led led5 = new Led(5);
Button button6 = new Button(6);

Led led22 = new Led(22);
Button button23 = new Button(23);

Robot.PlayNotes("C4");
Console.WriteLine($"{Robot.ReadBatteryMillivolts()}");
for (short i = 0; i < 100; i++)
{
    Robot.Motors(i, i);
    Robot.Wait(20);
}
for (short i = 100; i > 0; i--)
{
    Robot.Motors(i, i);
    Robot.Wait(20);
}
Robot.PlayNotes("C4C4G4G4A4A4G4F4F4E4E4D4D4C4");


string lastState1 = "";
string lastState2 = "";

while (true)
{
    if (button6.GetState() != lastState1)
    {
        lastState1 = button6.GetState();
        if (button6.GetState() == "Pressed")
        {
            led22.SetOn();
        }
        else
        {
            led22.SetOff();
        }
    }
    if (button23.GetState() != lastState2)
    {
        lastState2 = button23.GetState();
        if (button23.GetState() == "Pressed")
        {
            led5.SetOn();
        }
        else
        {
            led5.SetOff();
        }
    }
    Console.WriteLine(button6.GetState());
    Robot.Wait(10);
}