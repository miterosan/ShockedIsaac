using ShockedIsaac;
using ShockedIsaac.API;

var apiKey = Configuration.getApiKey();

if (string.IsNullOrWhiteSpace(apiKey)) {
    Console.WriteLine("Please write your API key and press enter:");

    while((apiKey = Console.ReadLine()) == null);

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

    if (device.shockers == null) continue;

    foreach (var shocker in device.shockers) {
        Console.WriteLine($"\t{shocker.name}");

        allShockers.Add(shocker);
    }
}

ModBridge bridge = new()
{// spike -> 3 mal kurz
    OnDamage = async amount =>
    {
        Console.WriteLine($"Got hit with {amount} damage. Sending shock...");
        await api.ControlShockers(new ControlRequests() {
            Amount = 10 + amount * 20,
            Duration = 700,
            Name = "Isaac got hurt",
            Shockers = [.. allShockers],
            Type = ShockerCommandType.Shock
        });
    },
    OnIntentionalDamage = async amount => 
    {
        Console.WriteLine("Got intentional damage (sacrifice room, etc). Sending shock...");
        await api.ControlShockers(new ControlRequests() {
            Amount = 25,
            Duration = 300,
            Name = "Isaac got hurt",
            Shockers = allShockers.ToArray(),
            Type = ShockerCommandType.Shock
        });
    },
    OnDeath = async () => 
    {
        Console.WriteLine("Isaac died. Sending shock...");
        await Task.Delay(701);
        await api.ControlShockers(new ControlRequests() {
            Amount = 80,
            Duration = 1000,
            Name = "Isaac got hurt",
            Shockers = allShockers.ToArray(),
            Type = ShockerCommandType.Shock
        });

    },
};



await bridge.StartServer();