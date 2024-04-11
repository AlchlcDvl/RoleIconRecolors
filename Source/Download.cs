using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Home.Shared;

namespace IconPacks;

public static class Download
{
    private const string REPO = "https://raw.githubusercontent.com/AlchlcDvl/RoleIconRecolors/main";
    private static readonly string[] SupportedPacks = [ "Vanilla", "BTOS2", "Recolors" ];
    private static readonly Dictionary<string, bool> Running = [];

    public static void DownloadIcons(string packName) => ApplicationController.ApplicationContext.StartCoroutine(CoDownload(packName));

    private static IEnumerator CoDownload(string packName)
    {
        Logging.LogMessage($"Downloading {packName}");

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

        Running[packName] = true;
        var pack = Path.Combine(AssetManager.ModPath, packName);

        if (Directory.Exists(pack))
        {
            Logging.LogMessage("Clearing");
            var packinfo = new DirectoryInfo(pack);
            var dirs = packinfo.GetDirectories().Select(x => x.FullName);

            foreach (var dir in dirs)
            {
                var info = new DirectoryInfo(dir);
                info.GetFiles("*.png").Select(x => x.FullName).ForEach(File.Delete);
                info.GetFiles("*.jpg").Select(x => x.FullName).ForEach(File.Delete);
            }

            packinfo.GetFiles("*.png").Select(x => x.FullName).ForEach(File.Delete);
            packinfo.GetFiles("*.jpg").Select(x => x.FullName).ForEach(File.Delete);
        }
        else
            Directory.CreateDirectory(pack);

        var link = $"{REPO}/{packName}.json";
        var www = UnityWebRequest.Get(link);
        Logging.LogMessage($"Visiting {link}");
        yield return www.SendWebRequest();

        while (!www.isDone)
            yield return new WaitForEndOfFrame();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Logging.LogError(www.error);
            yield break;
        }

        Logging.LogMessage("Converting");
        var json = JsonConvert.DeserializeObject<List<Asset>>(www.downloadHandler.text);

        foreach (var asset in json)
        {
            Logging.LogMessage($"Setting {asset.Name}");
            asset.FileType ??= "png";
            asset.Folder ??= packName.Replace(" ", "");
            asset.Pack = packName;

            var link2 = $"{REPO}/{asset.DownloadLink()}";
            var www2 = UnityWebRequest.Get(link2);
            Logging.LogMessage($"Visiting {link2}");
            yield return www2.SendWebRequest();

            while (!www2.isDone)
                yield return new WaitForEndOfFrame();

            if (www2.result != UnityWebRequest.Result.Success)
            {
                Logging.LogError(www2.error);
                continue;
            }

            if (!Directory.Exists(asset.FolderPath()))
                Directory.CreateDirectory(asset.FolderPath());

            Logging.LogMessage($"Downloading {asset.Name}");
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

        if (packName is not ("Vanilla" or "BTOS2"))
            AssetManager.TryLoadingSprites(packName);

        Recolors.Open();
        Running[packName] = false;
        Logging.LogMessage("Done");
        yield break;
    }
}