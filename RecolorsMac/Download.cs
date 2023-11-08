using System.Net;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace RecolorsMac;

public static class Download
{
    private const string REPO = "https://raw.githubusercontent.com/AlchlcDvl/RoleIconRecolors/main";
    private static bool DownloadRunning = false;
    public static readonly List<Asset> AllVanillaAssets = new();
    public static readonly List<Asset> AllRecolorAssets = new();

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
            Process.Start("open", $"\"{AssetManager.DefaultPath}\\{packName}\"");
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
            client.DefaultRequestHeaders.CacheControl = new() { NoCache = true };
            client.Timeout = TimeSpan.FromSeconds(30);
            using var response = await client.GetAsync($"{REPO}/{packName}.json", HttpCompletionOption.ResponseContentRead);

            if (response.StatusCode != HttpStatusCode.OK)
                return response.StatusCode;

            if (response.Content == null)
            {
                Utils.Log($"Server returned no data: {response.StatusCode}", true);
                return response.StatusCode;
            }

            if (packName == "Vanilla")
                AllVanillaAssets.Clear();
            else if (packName == "Recolors")
                AllRecolorAssets.Clear();

            var json = await response.Content.ReadAsStringAsync();
            var jobj = JObject.Parse(json)[packName.ToLower()];

            if (jobj == null || !jobj.HasValues)
            {
                Utils.Log("JSON Parse failed", true);
                return HttpStatusCode.ExpectationFailed;
            }

            for (var current = jobj.First; current != null && current.HasValues; current = current.Next)
            {
                var info = new Asset() { Name = current["name"]?.ToString() ?? "" };

                if (info.Name is null or "") //Required
                    continue;

                info.Folder = current["folder"]?.ToString() ?? "";
                Utils.Log(info.Name + " " + info.Folder);

                if (packName == "Vanilla")
                    AllVanillaAssets.Add(info);
                else if (packName == "Recolors")
                    AllRecolorAssets.Add(info);
            }

            var markedfordownload = new List<Asset>();
            var folder = packName == "Vanilla" ? AssetManager.VanillaPath : AssetManager.DefaultPath;

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (packName == "Vanilla")
            {
                foreach (var data in AllVanillaAssets)
                {
                    if (!File.Exists(folder + "\\" + data.Name + ".png"))
                        markedfordownload.Add(data);
                }
            }
            else if (packName == "Recolors")
            {
                foreach (var data in AllRecolorAssets)
                {
                    if (!File.Exists(folder + "\\" + data.Folder + "\\" + data.Name + ".png"))
                        markedfordownload.Add(data);
                }
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

                if (packName == "Recolors" && !Directory.Exists(folder + "\\" + file.Folder))
                    Directory.CreateDirectory(folder + "\\" + file.Folder);

                var array = await fileresponse.Content.ReadAsByteArrayAsync();
                File.Create(file.FilePath());
                await File.WriteAllBytesAsync(file.FilePath(), array);
            }

            if (packName == "Recolors")
                AssetManager.TryLoadingSprites("Recolors");

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

    public string FilePath()
    {
        if (Folder == "")
            return $"{AssetManager.VanillaPath}\\{Name}.png";
        else
            return $"{AssetManager.ModPath}\\{Folder}\\{Name}.png";
    }
}