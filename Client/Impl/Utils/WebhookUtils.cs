using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BModv2.Client.Impl.Utils;

public static class DiscordWebhook
{
    private static readonly HttpClient Http = new HttpClient();

    public static async Task<bool> SendAsync(string webhookUrl, string message, string username = null)
    {
        try
        {
            var payload = new
            {
                content = message,
                username = username
            };

            string json = JsonSerializer.Serialize(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await Http.PostAsync(webhookUrl, content);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}