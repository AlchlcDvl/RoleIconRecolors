using System.Net;
using Newtonsoft.Json.Linq;

namespace IconPacks;

public static class Download
{
    private const string REPO = "https://raw.githubusercontent.com/AlchlcDvl/RoleIconRecolors/main";
    private static bool DownloadRunning;
    private static string[] SupportedPacks = { "Vanilla", "BTOS2", "Recolors" };

    public static void DownloadIcons(string packName)
    {
        if (DownloadRunning)
            return;

        LaunchFetcher(packName);
    }

    private static async void LaunchFetcher(string packName)
    {
        Logging.LogMessage($"Starting {packName} download", true);
        DownloadRunning = true;

        try
        {
            Logging.LogMessage(await Fetch(packName) != HttpStatusCode.OK ? $"{packName} icons could not be downloaded" : $"Fetched {packName} icons", true);
        }
        catch (Exception e)
        {
            Logging.LogError($"Unable to fetch {packName} icons\n{e}");
        }

        DownloadRunning = false;
    }

    private static async Task<HttpStatusCode> Fetch(string packName)
    {
        if (!SupportedPacks.Contains(packName))
        {
            Logging.LogError($"Wrong pack name {packName}");
            return HttpStatusCode.NotFound;
        }

        try
        {
            if (packName == "Vanilla" && Directory.Exists(AssetManager.VanillaPath))
                new DirectoryInfo(AssetManager.VanillaPath).GetFiles("*.png").Select(x => x.FullName).ForEach(File.Delete);
            else if (packName == "BTOS2" && Directory.Exists(AssetManager.BTOS2Path))
                new DirectoryInfo(AssetManager.BTOS2Path).GetFiles("*.png").Select(x => x.FullName).ForEach(File.Delete);
            else if (Directory.Exists(Path.Combine(AssetManager.ModPath, packName)))
            {
                var pack = new DirectoryInfo(Path.Combine(AssetManager.ModPath, packName)).GetDirectories().Select(x => x.FullName);

                foreach (var dir in pack)
                {
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
                Logging.LogError($"Server returned no data: {response.StatusCode}");
                return response.StatusCode;
            }

            var json = await response.Content.ReadAsStringAsync();
            var jobj = JObject.Parse(json)[packName.Replace(" ", "").ToLower()];

            if (jobj == null || !jobj.HasValues)
            {
                Logging.LogError("JSON Parse failed");
                return HttpStatusCode.ExpectationFailed;
            }

            var assets = new List<Asset>();

            for (var current = jobj.First; current != null; current = current.Next)
            {
                if (!current.HasValues)
                    continue;

                var info = new Asset()
                {
                    Name = current["name"]?.ToString(),
                    FileType = current["type"]?.ToString() ?? "png",
                    Folder = current["folder"]?.ToString() ?? packName.Replace(" ", ""),
                    Pack = packName
                };
                Logging.LogMessage(info.DownloadLink());
                assets.Add(info);
            }

            foreach (var file in assets)
            {
                using var fileresponse = await client.GetAsync($"{REPO}/{file.DownloadLink()}", HttpCompletionOption.ResponseContentRead);

                if (fileresponse.StatusCode != HttpStatusCode.OK)
                {
                    Logging.LogError($"Error downloading {file.Name}: {fileresponse.StatusCode}");
                    continue;
                }

                var array = await fileresponse.Content.ReadAsByteArrayAsync();
                File.WriteAllBytes(file.FilePath(), array);
            }

            if (packName is not ("Vanilla" or "BTOS2"))
                AssetManager.TryLoadingSprites(packName);

            Recolors.Open();
            return HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            Logging.LogError(e);
            return HttpStatusCode.ExpectationFailed;
        }
    }
}

public class Asset
{
    public string Name { get; set; }
    public string Folder { get; set; }
    public string Pack { get; set; }
    public string FileType { get; set; }

    public string FilePath()
    {
        if (Folder is null or "" or "Vanilla" || Pack == "Vanilla")
            return Path.Combine(AssetManager.VanillaPath, $"{Name}.{FileType}");
        else if (Pack == "BTOS2")
            return Path.Combine(AssetManager.BTOS2Path, $"{Name}.{FileType}");
        else
            return Path.Combine(AssetManager.ModPath, Pack, Folder, $"{Name}.{FileType}");
    }

    public string DownloadLink()
    {
        if (Folder is "" or "Vanilla" || Pack == "Vanilla")
            return $"Vanilla/{Name}.{FileType}";
        else if (Pack == "BTOS2")
            return $"BTOS2/{Name}.{FileType}";
        else
            return $"{Pack}/{Folder}/{Name}.{FileType}";
    }
}