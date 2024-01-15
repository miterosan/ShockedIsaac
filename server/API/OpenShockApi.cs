using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ShockedIsaac.API;

public class OpenShockAPI 
{
    private readonly string baseAdress = "https://api.shocklink.net";
 
    private readonly HttpClient httpClient;

    private readonly string APIKey;

    public OpenShockAPI(string apiKey)
    {
        APIKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));

        httpClient = new HttpClient {
            BaseAddress = new Uri(baseAdress)
        };

        httpClient.DefaultRequestHeaders.Add("OpenShockToken", APIKey);
    }

    public async Task ControlShocker(Shocker shocker, ShockerCommandType type, int intensity, int duration, string customName = "IsaacShock") {

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

        await httpClient.PostAsync("/2/shockers/control", jsonContent);
    }

    public async Task<Device[]> GetOwnShockers() {

        var response = await httpClient.GetAsync($"/1/shockers/own");

        response.EnsureSuccessStatusCode();

        var devices = await response.Content.ReadFromJsonAsync<BaseResponse<Device>>();

        return devices?.data ?? [];
    }
}