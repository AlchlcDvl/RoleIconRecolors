using System.Net;
using Newtonsoft.Json.Linq;

namespace RecolorsWindows;

public static class Download
{
    private const string REPO = "https://raw.githubusercontent.com/AlchlcDvl/RoleIconRecolors/main";
    private static bool DownloadRunning;
    public static readonly Dictionary<string, List<Asset>> AllAssets = new();

    public static void DownloadVanilla() => DownloadIcons("Vanilla");

    public static void DownloadRecolors() => DownloadIcons("Recolors");

    private static async void DownloadIcons(string packName)
    {
        if (DownloadRunning)
            return;

        await LaunchFetcher(packName);
    }

    private static async Task LaunchFetcher(string packName)
    {
        Utils.Log($"Starting {packName} download", true);
        DownloadRunning = true;

        try
        {
            var status = await Fetch(packName);
            Utils.Log(status != HttpStatusCode.OK ? $"{packName} icons could not be downloaded" : $"Fetched {packName} icons", true);
        }
        catch (Exception e)
        {
            Utils.Log($"Unable to fetch {packName} icons\n{e}", true);
        }

        DownloadRunning = false;
    }

    private static async Task<HttpStatusCode> Fetch(string packName)
    {
        if (packName is not ("Vanilla" or "Recolors"))
        {
            Utils.Log($"Wrong pack name {packName}", true);
            return HttpStatusCode.NotFound;
        }

        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "IconPackDownloader");
            using var response = await client.GetAsync($"{REPO}/{packName}.json", HttpCompletionOption.ResponseContentRead);

            if (response.StatusCode != HttpStatusCode.OK)
                return response.StatusCode;

            if (response.Content == null)
            {
                Utils.Log($"Server returned no data: {response.StatusCode}", true);
                return response.StatusCode;
            }

            var assets = new List<Asset>();
            var json = await response.Content.ReadAsStringAsync();
            var jobj = JObject.Parse(json)[packName.ToLower()];

            if (jobj == null || !jobj.HasValues)
            {
                Utils.Log("JSON Parse failed", true);
                return HttpStatusCode.ExpectationFailed;
            }

            for (var current = jobj.First; current != null && current.HasValues; current = current.Next)
            {
                var info = new Asset()
                {
                    Name = current["name"]?.ToString() ?? "",
                    Folder = current["folder"]?.ToString() ?? "",
                    Pack = packName
                };

                Utils.Log(info.Name + " " + info.Folder + " " + info.Pack);
                assets.Add(info);
            }

            AllAssets.TryAdd(packName, assets);
            var markedfordownload = new List<Asset>();

            foreach (var data in assets)
            {
                if (!File.Exists(data.FilePath()))
                    markedfordownload.Add(data);
            }

            foreach (var file in markedfordownload)
            {
                var path = "";

                if (packName == "Vanilla")
                    path = file.Name;
                else if (packName == "Recolors")
                    path = $"{file.Folder}/{file.Name}";

                using var fileresponse = await client.GetAsync($"{REPO}/{packName}/{path}.png", HttpCompletionOption.ResponseContentRead);

                if (fileresponse.StatusCode != HttpStatusCode.OK)
                {
                    Utils.Log($"Error downloading {file.Name}: {fileresponse.StatusCode}", true);
                    continue;
                }

                var array = await fileresponse.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync(file.FilePath(), array);
            }

            if (packName != "Vanilla")
                AssetManager.TryLoadingSprites(packName);

            return HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            Utils.Log($"ISSUE: {ex}", true);
            return HttpStatusCode.ExpectationFailed;
        }
    }
}

public class Asset
{
    public string Name { get; set; }
    public string Folder { get; set; }
    public string Pack { get; set; }

    public string FilePath()
    {
        if (Folder == "" || Pack == "Vanilla")
            return Path.Combine(AssetManager.VanillaPath, $"{Name}.png");
        else
            return Path.Combine(AssetManager.ModPath, Pack, Folder, $"{Name}.png");
    }
}