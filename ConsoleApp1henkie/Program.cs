using System;
using System.Net;
using System.Net.Sockets;

// See https://aka.ms/new-console-template for more information
string userName = Environment.UserName;

// Get the IP address
string localIP = GetLocalIPAddress();

// Output to console
Console.WriteLine($"hellu {userName}, {localIP} is your IP");
Console.ReadLine();

static string GetLocalIPAddress()
{
    string localIP = string.Empty;
    foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()))
    {
        if (ip.AddressFamily == AddressFamily.InterNetwork)
        {
            localIP = ip.ToString();
            break;
        }
    }
    return localIP;
}