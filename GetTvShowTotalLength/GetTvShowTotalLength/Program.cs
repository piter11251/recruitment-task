using System.Text.Json;
using System.Text.Json.Nodes;

class Program
{
    private static readonly HttpClient _httpClient = new HttpClient();
    static async Task Main(string[] args)
    {
        try
        {
            string url = "https://api.tvmaze.com/singlesearch/shows?q=the%20office&embed=episodes";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var episodes = JsonNode.Parse(responseBody)["_embedded"]["episodes"].AsArray();
            int totalEpisodes = 0;
            int totalRuntime = 0;
            foreach(var episode in episodes)
            {
                totalRuntime += (int)episode["runtime"];
            }
            Console.WriteLine(totalRuntime);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}