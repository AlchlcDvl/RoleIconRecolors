using System.Net;
using Newtonsoft.Json.Linq;

namespace RecolorsPlatformless;

public static class Download
{
    private const string REPO = "https://raw.githubusercontent.com/AlchlcDvl/RoleIconRecolors/main";
    private static bool DownloadRunning;

    public static void DownloadVanilla() => DownloadIcons("Vanilla");

    public static void DownloadRecolors() => DownloadIcons("Recolors");

    private static void DownloadIcons(string packName)
    {
        if (DownloadRunning)
            return;

        LaunchFetcher(packName);
    }

    private static async void LaunchFetcher(string packName)
    {
        Recolors.LogMessage($"Starting {packName} download", true);
        DownloadRunning = true;

        try
        {
            var status = await Fetch(packName);
            Recolors.LogMessage(status != HttpStatusCode.OK ? $"{packName} icons could not be downloaded" : $"Fetched {packName} icons", true);
        }
        catch (Exception e)
        {
            Recolors.LogError($"Unable to fetch {packName} icons\n{e}");
        }

        DownloadRunning = false;
    }

    private static async Task<HttpStatusCode> Fetch(string packName)
    {
        if (packName is not ("Vanilla" or "Recolors"))
        {
            Recolors.LogError($"Wrong pack name {packName}");
            return HttpStatusCode.NotFound;
        }

        try
        {
            if (packName == "Vanilla" && Directory.Exists(AssetManager.VanillaPath))
                new DirectoryInfo(AssetManager.VanillaPath).GetFiles("*.png").Select(x => x.FullName).ForEach(File.Delete);
            else if (packName == "Recolors" && Directory.Exists(AssetManager.DefaultPath))
            {
                foreach (var folder in AssetManager.Folders)
                {
                    var dir = Path.Combine(AssetManager.DefaultPath, folder);

                    if (Directory.Exists(dir))
                        new DirectoryInfo(dir).GetFiles("*.png").Select(x => x.FullName).ForEach(File.Delete);
                }
            }

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "IconPackDownloader");
            using var response = await client.GetAsync($"{REPO}/{packName}.json", HttpCompletionOption.ResponseContentRead);

            if (response.StatusCode != HttpStatusCode.OK)
                return response.StatusCode;

            if (response.Content == null)
            {
                Recolors.LogError($"Server returned no data: {response.StatusCode}");
                return response.StatusCode;
            }

            var assets = new List<Asset>();
            var json = await response.Content.ReadAsStringAsync();
            var jobj = JObject.Parse(json)[packName.ToLower()];

            if (jobj == null || !jobj.HasValues)
            {
                Recolors.LogError("JSON Parse failed");
                return HttpStatusCode.ExpectationFailed;
            }

            for (var current = jobj.First; current != null; current = current.Next)
            {
                if (!current.HasValues)
                    continue;

                var info = new Asset()
                {
                    Name = current["name"]?.ToString() ?? "",
                    Folder = current["folder"]?.ToString() ?? "Vanilla",
                    Pack = packName
                };
                Recolors.LogMessage(info.Name + " " + info.Folder + " " + info.Pack);
                assets.Add(info);
            }

            foreach (var file in assets)
            {
                var path = "";

                if (packName == "Vanilla")
                    path = file.Name;
                else if (packName == "Recolors")
                    path = $"{file.Folder}/{file.Name}";

                using var fileresponse = await client.GetAsync($"{REPO}/{packName}/{path}.png", HttpCompletionOption.ResponseContentRead);

                if (fileresponse.StatusCode != HttpStatusCode.OK)
                {
                    Recolors.LogError($"Error downloading {file.Name}: {fileresponse.StatusCode}");
                    continue;
                }

                var array = await fileresponse.Content.ReadAsByteArrayAsync();
                File.WriteAllBytes(file.FilePath(), array);
            }

            AssetManager.TryLoadingSprites(packName);
            Recolors.Open();
            return HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            Recolors.LogError(e);
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