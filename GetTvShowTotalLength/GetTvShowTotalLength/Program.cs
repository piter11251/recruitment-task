using System.Text.Json;
using System.Text.Json.Nodes;

class Program
{
    private static readonly HttpClient _httpClient = new HttpClient();
    public static async Task Main(string[] args)
    {
        string tvshowName = string.Join(" ", args);
        string url = $"https://api.tvmaze.com/search/shows?q={Uri.EscapeDataString(tvshowName)}";
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        if(response.IsSuccessStatusCode) 
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            var tvshowsArray = JsonNode.Parse(responseBody).AsArray();
            var latestvshow = latestTvshow(tvshowsArray);
            Console.WriteLine(latestvshow);
            if (latestvshow != null)
            {
                var episodesUrl = $"https://api.tvmaze.com/singlesearch/shows?q={Uri.EscapeDataString(tvshowName)}&embed=episodes";
                response = await _httpClient.GetAsync(episodesUrl);
                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                    var episodes = JsonNode.Parse(responseBody)["_embedded"]["episodes"].AsArray();
                    int totalRuntime = 0;
                    foreach (var episode in episodes)
                    {
                        totalRuntime += (int)episode["runtime"];
                    }
                    Console.WriteLine(totalRuntime);
                }
                else
                {
                    Environment.Exit(10);
                }
            }
        }
        else
        {
            Environment.Exit(10);
        }
    }

    public static JsonNode latestTvshow(JsonArray tvshows)
    {
        return tvshows
            .Select(show => show["show"])
            .Where(show => show["premiered"] != null)
            .OrderByDescending(show => DateTime.Parse((string)show["premiered"]))
            .FirstOrDefault();
    }
}