/*using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Home.Shared;

namespace IconPacks;

public static class Download
{
    private const string REPO = "https://raw.githubusercontent.com/AlchlcDvl/RoleIconRecolors/main";
    private static readonly Dictionary<string, bool> Running = [];
    private static bool HandlerRunning;
    private static readonly Dictionary<string, PackJson> Packs = [];

    public static void HandlePackData() => ApplicationController.ApplicationContext.StartCoroutine(CoHandlePackData());

    private static IEnumerator CoHandlePackData()
    {
        if (HandlerRunning)
            yield break;

        HandlerRunning = true;

        var www = UnityWebRequest.Get($"{REPO}/Packs.json");
        yield return www.SendWebRequest();

        while (!www.isDone)
            yield return new WaitForEndOfFrame();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Logging.LogError(www.error);
            HandlerRunning = false;
            yield break;
        }

        var json = JsonConvert.DeserializeObject<List<PackJson>>(www.downloadHandler.text);

        foreach (var jsonPack in json)
        {
            jsonPack.Branch ??= "main";
            jsonPack.RepoOwner ??= "AlchlcDvl";
            jsonPack.RepoName ??= "RoleIconRecolors";
            jsonPack.Name ??= jsonPack.RepoName;
            jsonPack.JsonName ??= jsonPack.Name;
            Packs.Add(jsonPack.Name, jsonPack);
        }

        HandlerRunning = false;
        yield break;
    }

    public static void DownloadIcons(string packName) => ApplicationController.ApplicationContext.StartCoroutine(CoDownload(packName));

    private static IEnumerator CoDownload(string packName)
    {
        if (Running.TryGetValue(packName, out var running) && running)
        {
            Logging.LogError($"{packName} download is still running");
            yield break;
        }

        Running[packName] = true;
        var pack = Path.Combine(AssetManager.ModPath, packName);

        if (Directory.Exists(pack))
        {
            var packinfo = new DirectoryInfo(pack);
            var dirs = packinfo.GetDirectories().Select(x => x.FullName);

            foreach (var dir in dirs)
            {
                var info = new DirectoryInfo(dir);
                var dir2 = info.GetDirectories().Select(x => x.FullName);

                foreach (var dir3 in dir2)
                {
                    var inf2 = new DirectoryInfo(dir3);
                    inf2.GetFiles("*.png").Select(x => x.FullName).ForEach(File.Delete);
                    inf2.GetFiles("*.jpg").Select(x => x.FullName).ForEach(File.Delete);
                }

                info.GetFiles("*.png").Select(x => x.FullName).ForEach(File.Delete);
                info.GetFiles("*.jpg").Select(x => x.FullName).ForEach(File.Delete);
            }

            packinfo.GetFiles("*.png").Select(x => x.FullName).ForEach(File.Delete);
            packinfo.GetFiles("*.jpg").Select(x => x.FullName).ForEach(File.Delete);
        }
        else
            Directory.CreateDirectory(pack);

        var www = UnityWebRequest.Get($"{REPO}/{packName}.json");
        yield return www.SendWebRequest();

        while (!www.isDone)
            yield return new WaitForEndOfFrame();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Logging.LogError(www.error);
            Running[packName] = false;
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

            if (!Directory.Exists(asset.FolderPath()))
                Directory.CreateDirectory(asset.FolderPath());

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
        yield break;
    }
}*/