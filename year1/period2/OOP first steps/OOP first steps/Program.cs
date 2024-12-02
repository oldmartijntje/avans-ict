// See https://aka.ms/new-console-template for more information
using OOP_first_steps;
using OOP_first_steps.EventHandler;
using OOP_first_steps.Robots;

Console.WriteLine("Hello, World!");
NewEventHandler globalEventHandler = new NewEventHandler();
var virtualBrimnes = new FlyingRobot(globalEventHandler);
var harry = new WheeledRobot(globalEventHandler);