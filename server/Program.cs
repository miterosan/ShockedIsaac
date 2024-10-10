using ShockedIsaac;
using ShockedIsaac.API;

Console.WriteLine("Shocked Isaac server");

var configuration = new Configuration();

if (string.IsNullOrWhiteSpace(configuration.ApiKey))
{
    Console.WriteLine("No Apikey was set in the settings.ini. Add one and start the server again.");
    Console.ReadLine();
    return;
}


var api = new OpenShockAPI(configuration.ApiKey);

var devices = await api.GetOwnShockers();

Console.WriteLine("Found following hubs with shockers:");

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

// todo validate shockers in configuration.



async Task punish(int amount, int duration, int playerIndex, string reason, ShockerCommandType type = ShockerCommandType.Shock)
{
    string[] shockerNames;

    switch (playerIndex)
    {
        case 0: shockerNames = configuration.Player1Shockers;
            
            break;
        case 1: shockerNames = configuration.Player2Shockers;

            break;
        case 2: shockerNames = configuration.Player3Shockers;

            break;
        case 3: shockerNames = configuration.Player4Shockers;

            break;
        default: 
            return;
    }

    var shockers = shockerNames.Select(s => allShockers.First(aS => aS.name == s));

    foreach (var shocker in shockers)
    {
        await api.ControlShocker(new ControlRequest() {
            Amount = amount,
            Duration = duration,
            Name = reason,
            Shocker = shocker,
            Type = type
        });
    }
}


ModBridge bridge = new()
{
    OnDamage = async (amount, playerIndex) =>
    {
        await punish(configuration.HitStrength, configuration.HitDuration, playerIndex, $"Player {playerIndex} got hurt");
    },
    OnIntentionalDamage = async (amount, playerIndex) => 
    {
        ShockerCommandType type = ShockerCommandType.Shock;

        if (configuration.IntentionalDamageMode == "Shock") type = ShockerCommandType.Shock;
        if (configuration.IntentionalDamageMode == "Vibrate") type = ShockerCommandType.Vibrate;
        
        await punish(configuration.IntentionalHitStrength, configuration.IntentionalHitDuration, playerIndex, $"Player {playerIndex} got intentionally hurt", type);
    },
    OnDeath = async (playerIndex) => 
    {
        await punish(configuration.DeathStrength, configuration.DeathDuration, playerIndex, $"Player {playerIndex} died" );
    }
};



await bridge.StartServer();