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
{
    OnDamage = async amount =>
    {
        foreach (var shocker in allShockers)
        {
            Console.WriteLine($"Got hit with {amount} damage. Sending shock...");
            await api.ControlShocker(new ControlRequest() {
                Amount = 20 + amount * 20,
                Duration = 1000,
                Name = "Isaac got hurt",
                Shocker = shocker,
                Type = ShockerCommandType.Shock
            });
        }
    },
    OnIntentionalDamage = async amount => 
    {
        foreach (var shocker in allShockers)
        {
            Console.WriteLine("Got intentional damage (sacrifice room, etc). Sending shock...");
            await api.ControlShocker(new ControlRequest() {
                Amount = 100,
                Duration = 300,
                Name = "Isaac got hurt",
                Shocker = shocker,
                Type = ShockerCommandType.Shock
            });
        }
    },
    OnDeath = async () => 
    {
        foreach (var shocker in allShockers)
        {
            Console.WriteLine("Isaac died. Sending shock...");
            await api.ControlShocker(new ControlRequest() {
                Amount = 70,
                Duration = 1500,
                Name = "Isaac died",
                Shocker = shocker,
                Type = ShockerCommandType.Shock
            });
        }
    },
};



await bridge.StartServer();