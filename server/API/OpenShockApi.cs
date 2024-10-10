using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ShockedIsaac.API;

public class OpenShockAPI 
{
    private readonly string baseAdress = "https://api.shocklink.net";
 
    private readonly HttpClient httpClient;

    public OpenShockAPI(string apiKey)
    {
        ArgumentNullException.ThrowIfNull(apiKey);

        httpClient = new HttpClient {
            BaseAddress = new Uri(baseAdress)
        };

        httpClient.DefaultRequestHeaders.Add("OpenShockToken", apiKey);
    }

    public async Task ControlShocker(ControlRequest controlRequest) 
    {
        using StringContent jsonContent = new(
            JsonSerializer.Serialize(new
            {
                shocks = new[] {
                    new { 
                        controlRequest.Shocker.id,
                        type = (int)controlRequest.Type,
                        intensity = controlRequest.Amount,
                        controlRequest.Duration,
                    },
                },
                customName = controlRequest.Name
            }),
            Encoding.UTF8,
            "application/json"
        );

//        Console.WriteLine($"Sending shock with intensity: {controlRequest.Amount}, duration: {controlRequest.Duration}");

        await httpClient.PostAsync("/2/shockers/control", jsonContent);
    }

    public async Task<Device[]> GetOwnShockers() {

        var response = await httpClient.GetAsync($"/1/shockers/own");

        response.EnsureSuccessStatusCode();

        var devices = await response.Content.ReadFromJsonAsync<BaseResponse<Device>>();

        return devices?.data ?? [];
    }
}