﻿using System.Net;
using System.Net.Sockets;
using System.Text;
using loltest;
using loltest.API;

var apiKey = Configuration.getApiKey();

if (apiKey == null) {
    Console.WriteLine("Please write your API key and press enter:");
    apiKey = Console.ReadLine();

    Configuration.setApiKey(apiKey);

    // todo check if valid
}

var api = new OpenShockAPI(apiKey);

var devices = await api.GetOwnShockers();

Console.WriteLine("Found following devices:");

var allShockers = new List<Shocker>();

foreach (var device in devices)
{
    Console.WriteLine();
    Console.WriteLine(device.name);

    foreach (var shocker in device.shockers) {
        Console.WriteLine($"\t{shocker.name}");

        allShockers.Add(shocker);
    }
}

ModBridge bridge = new ModBridge
{
    OnDamage = async amount =>
    {
        foreach (var shocker in allShockers)
        {
            Console.WriteLine("sending shock...");
            await api.ControlShocker(shocker, ShockerCommandType.Shock, amount * 20, 1000, "Isaac got hurt");
        }
    },
    OnIntentionalDamage = async amount => 
    {
        foreach (var shocker in allShockers)
        {
            Console.WriteLine("sending intentional shock...");
            await api.ControlShocker(shocker, ShockerCommandType.Shock, 100, 300, "Isaac hurt himself");
        }
    }
};



await bridge.StartServer();