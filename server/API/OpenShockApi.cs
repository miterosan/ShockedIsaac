using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ShockedIsaac.API;

public class OpenShockAPI 
{
    private readonly string baseAdress = "https://api.shocklink.net";
    private static HttpClient httpClient;

    private HttpClient getClient() 
    {
        if (httpClient == null) {
            httpClient = new()
            {
                BaseAddress = new Uri(baseAdress)
            };
            httpClient.DefaultRequestHeaders.Add("OpenShockToken", APIKey);
        }

        return httpClient;
    }

    private readonly string APIKey;

    public OpenShockAPI(string apiKey)
    {
        APIKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
    }

    public async Task ControlShocker(Shocker shocker, ShockerCommandType type, int intensity, int duration, string customName = "IsaacShock") {
        var client = getClient();

        using StringContent jsonContent = new(
            JsonSerializer.Serialize(new
            {
                shocks = new[] {
                    new { 
                        shocker.id,
                        type = (int)type,
                        intensity,
                        duration,
                    },
                },
            customName
            }),
            Encoding.UTF8,
            "application/json"
        );

        Console.WriteLine($"Sending shock with intensity: {intensity}, duration: {duration}");

        await client.PostAsync("/2/shockers/control", jsonContent);
    }

    public async Task<Device[]> GetOwnShockers() {
        var client = getClient();

        var response = await client.GetAsync($"/1/shockers/own");

        response.EnsureSuccessStatusCode();

        var devices = await response.Content.ReadFromJsonAsync<BaseResponse<Device>>();

        return devices?.data ?? [];
    }
}