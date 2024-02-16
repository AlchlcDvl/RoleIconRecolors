using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Home.Shared;

namespace IconPacks;

public static class Download
{
    private const string REPO = "https://raw.githubusercontent.com/AlchlcDvl/RoleIconRecolors/main";
    private static readonly string[] SupportedPacks = { "Vanilla", "BTOS2", "Recolors" };
    private static readonly Dictionary<string, bool> Running = new();

    public static void DownloadIcons(string packName) => ApplicationController.ApplicationContext.StartCoroutine(CoDownload(packName));

    private static IEnumerator CoDownload(string packName)
    {
        if (!SupportedPacks.Contains(packName))
        {
            Logging.LogError($"Wrong pack name {packName}");
            yield break;
        }

        if (Running.TryGetValue(packName, out var running) && running)
        {
            Logging.LogError($"{packName} download is still running");
            yield break;
        }

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

        var www = UnityWebRequest.Get($"{REPO}/{packName}.json");
        yield return www.SendWebRequest();

        while (!www.isDone)
            yield return new WaitForEndOfFrame();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Logging.LogError(www.error);
            yield break;
        }

        var json = JsonConvert.DeserializeObject<List<Asset>>(www.downloadHandler.text);

        foreach (var asset in json)
        {
            asset.FileType ??= "png";
            asset.Folder ??= packName.Replace(" ", "");
            asset.Pack = packName;

            var www2 = UnityWebRequest.Get($"{REPO}/{asset.DownloadLink()}");
            yield return www2.SendWebRequest();

            while (!www2.isDone)
                yield return new WaitForEndOfFrame();

            if (www2.result != UnityWebRequest.Result.Success)
            {
                Logging.LogError(www2.error);
                continue;
            }

            var persistTask = File.WriteAllBytesAsync(asset.FilePath(), www2.downloadHandler.data);

            while (!persistTask.IsCompleted)
            {
                if (persistTask.Exception != null)
                {
                    Logging.LogError(persistTask.Exception);
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        yield break;
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
        if (Pack == "Vanilla")
            return Path.Combine(AssetManager.VanillaPath, $"{Name}.{FileType}");
        else if (Pack == "BTOS2")
            return Path.Combine(AssetManager.BTOS2Path, $"{Name}.{FileType}");
        else
            return Path.Combine(AssetManager.ModPath, Pack, Folder, $"{Name}.{FileType}");
    }

    public string DownloadLink()
    {
        if (Pack == "Vanilla")
            return $"Vanilla/{Name}.{FileType}";
        else if (Pack == "BTOS2")
            return $"BTOS2/{Name}.{FileType}";
        else
            return $"{Pack}/{Folder}/{Name}.{FileType}";
    }
}