using System.Net;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace RecolorsMac;

public static class Download
{
    private const string REPO = "https://raw.githubusercontent.com/AlchlcDvl/RoleIconRecolors/main";
    private static bool DownloadRunning = false;
    public static readonly List<Asset> AllVanillaAssets = new();
    public static readonly List<Asset> AllRecolorAssets = new();
    private static HttpClient Client = null;

    public static void DownloadVanilla() => DownloadIcons("Vanilla");

    public static void DownloadRecolors() => DownloadIcons("Recolors");

    private static void SetUpClient()
    {
        if (Client != null)
            return;

        Client = new();
        Client.DefaultRequestHeaders.Add("User-Agent", "IconPackDownloader");
        Client.DefaultRequestHeaders.CacheControl = new() { NoCache = true };
		Client.Timeout = TimeSpan.FromSeconds(30);
    }

    private static void DownloadIcons(string packName)
    {
        SetUpClient();

        if (DownloadRunning)
            return;

        Utils.Log($"Starting {packName} download", true);
        DownloadRunning = true;
        _ = LaunchFetcher(packName);
        Utils.Log($"Fetched {packName} icons", true);
    }

    private static async Task LaunchFetcher(string packName)
    {
        try
        {
            var status = await Fetch(packName);

            if (status != HttpStatusCode.OK)
                Utils.Log($"{packName} icons could not be loaded", true);
            else
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
        if (Client == null)
        {
            Utils.Log("Client was null", true);
            return HttpStatusCode.NotFound;
        }
        else if (packName is not ("Vanilla" or "Recolors"))
        {
            Utils.Log($"Wrong pack name {packName}", true);
            return HttpStatusCode.NotFound;
        }

        try
        {
            var response = await Client.GetAsync(new Uri($"{REPO}/{packName}.json"), HttpCompletionOption.ResponseContentRead);

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

            var json = await response.Content.ReadAsStreamAsync();
            var datas = JsonSerializer.Deserialize<AssetJson>(Encoding.UTF8.GetString(json.ReadFully()), new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip
            });
            var markedfordownload = new List<Asset>();
            var folder = packName == "Vanilla" ? AssetManager.VanillaPath : AssetManager.DefaultPath;

            if (packName == "Vanilla")
                AllVanillaAssets.AddRange(datas.Assets);
            else if (packName == "Recolors")
                AllRecolorAssets.AddRange(datas.Assets);

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
                    path = $"{REPO}/Vanilla/{file.Name}.png";
                else if (packName == "Recolors")
                    path = $"{REPO}/Recolors/{file.Folder}/{file}.png";

                var fileresponse = await Client.GetAsync(path, HttpCompletionOption.ResponseContentRead);

                if (fileresponse.StatusCode != HttpStatusCode.OK)
                {
                    Utils.Log($"Error downloading {file.Name}: {fileresponse.StatusCode}", true);
                    continue;
                }

                var imagebytes = await fileresponse.Content.ReadAsByteArrayAsync();
                var filePath = "";

                if (packName == "Vanilla")
                    filePath = $"{folder}\\{file}.png";
                else if (packName == "Recolors")
                    filePath = $"{folder}\\{file.Folder}\\{file}.png";

                if (File.Exists(filePath ))
                    File.Delete(filePath);

                await File.WriteAllBytesAsync(filePath, imagebytes);
            }
        }
        catch (Exception ex)
        {
            Utils.Log(ex, true);
        }

        if (packName == "Vanilla")
            AssetManager.TryLoadingSprites("Recolors");

        return HttpStatusCode.OK;
    }

    private static byte[] ReadFully(this Stream input)
    {
        using var memoryStream = new MemoryStream();
        input.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}

public class Asset
{
    public string Name { get; set; }
    public string Folder { get; set; }
    public string Sanitised => Name.SanitisePath();
}

public class AssetJson
{
    public Asset[] Assets { get; set; }
}