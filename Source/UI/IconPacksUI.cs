using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Home.Shared;
using FancyUI.Assets.IconPacks;
using System.IO.Compression;

namespace FancyUI.UI;

public class IconPacksUI : BaseUI
{
    public static readonly List<PackJson> Packs = [];

    public static IconPacksUI Instance { get; private set; }

    public override string Type => "Icon Pack";
    public override string Path => IPPath;

    public override void Awake()
    {
        base.Awake();
        Instance = this;
        SetupMenu();
    }

    public void OnDestroy()
    {
        Instance = null;
        GeneralUtils.SaveText("OtherPacks.json", JsonConvert.SerializeObject(Packs.Where(x => !x.FromMainRepo).ToArray(), typeof(PackJson[]), Formatting.Indented, new()), path: Path);
        IconPackTestingUI.Instance?.gameObject?.Destroy();
    }

    public override void SetupMenu()
    {
        base.SetupMenu();
        Test.GetComponent<Image>().sprite = Fancy.Assets.GetSprite("IconPack");
        Packs.ForEach(x => SetUpPack(x, () => DownloadIcons(x.Name)));
        PackTemplate.SetActive(false);
        NoPacks.SetActive(Packs.Count == 0);
    }

    public override void AfterGenerating()
    {
        var json = GenerateLinkAndAddToPackCount();
        Packs.Add(json);
        SetUpPack(json, () => DownloadIcons(json.Name));
    }

    public override void OpenTestingUI()
    {
        if (IconPackTestingUI.Instance)
        {
            IconPackTestingUI.Instance.gameObject.SetActive(true);
            gameObject.SetActive(false);
            return;
        }

        var go = Instantiate(Fancy.Assets.GetGameObject("IconPackTestingUI"), transform.parent);
        go.transform.localPosition = new(0, 0, 0);
        go.transform.localScale = Vector3.one * 1.5f;
        go.transform.SetAsLastSibling();
        go.AddComponent<IconPackTestingUI>();
        FancyUI.SetupFonts(go.transform);
        gameObject.SetActive(false);
    }

    public static void HandlePackData() => ApplicationController.ApplicationContext.StartCoroutine(CoHandlePackData());

    private static IEnumerator CoHandlePackData()
    {
        if (HandlerRunning)
            yield break;

        HandlerRunning = true;
        using var www = UnityWebRequest.Get($"{REPO}/Packs.json");
        yield return www.SendWebRequest();

        while (!www.isDone)
            yield return new WaitForEndOfFrame();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Fancy.Instance.Error(www.error);
            HandlerRunning = false;
            yield break;
        }

        Packs.Clear();
        Packs.AddRange(JsonConvert.DeserializeObject<PackJson[]>(www.downloadHandler.text));
        Packs.ForEach(x => x.FromMainRepo = true);

        var others = GeneralUtils.ReadText("OtherPacks.json", IPPath);

        if (!StringUtils.IsNullEmptyOrWhiteSpace(others))
            Packs.AddRange(JsonConvert.DeserializeObject<PackJson[]>(others));

        Packs.ForEach(x => x.SetDefaults());
        HandlerRunning = false;
        yield break;
    }

    public static void DownloadIcons(string packName) => Instance.StartCoroutine(CoDownloadIcons(packName));

    private static IEnumerator CoDownloadIcons(string packName)
    {
        packName = packName.Replace(" ", "");

        if (Running.TryGetValue(packName, out var running) && running)
        {
            Fancy.Instance.Error($"{packName} download is still running");
            LoadingUI.Instance.Finish();
            yield break;
        }

        LoadingUI.Begin(Instance.gameObject, packName);
        LoadingUI.Instance.LoadingProgress.SetText("Starting Download");
        Running[packName] = true;
        var packJson = Packs.Find(x => x.Name == packName);

        if (packJson == null)
        {
            Fancy.Instance.Error($"{packName} somehow doesn't exist");
            Running[packName] = false;
            LoadingUI.Instance.Finish();
            yield break;
        }

        LoadingUI.Instance.LoadingProgress.SetText("Clearing Old Files/Creating Folders");
        var pack = System.IO.Path.Combine(IPPath, packName);

        if (!Directory.Exists(pack))
            Directory.CreateDirectory(pack);
        else
            Directory.EnumerateFiles(pack, "*.png", SearchOption.AllDirectories).ForEach(File.Delete);

        IconPack.PopulateDirectory(pack);
        LoadingUI.Instance.LoadingProgress.SetText("Retrieving GitHub Data");
        using var www = UnityWebRequest.Get(packJson.ApiLink());
        var op = www.SendWebRequest();
        var progress = 0f;

        while (!op.isDone)
        {
            if (Instance.Abort)
            {
                www.Abort();
                break;
            }

            progress += op.progress / 1.5f;
            LoadingUI.Instance.LoadingProgress.SetText(progress <= 100 ? $"Downloading Pack: {Mathf.RoundToInt(Mathf.Clamp(progress, 0, 100))}%" : "Unity is shitting itself, please wait...");
            yield return new WaitForEndOfFrame();
        }

        if (www.result != UnityWebRequest.Result.Success || Instance.Abort)
        {
            Fancy.Instance.Error(www.error);
            Running[packName] = false;
            LoadingUI.Instance.Finish();
            yield break;
        }

        LoadingUI.Instance.LoadingProgress.SetText("Fetching Pack Data");
        var filePath = System.IO.Path.Combine(IPPath, $"{packName}.zip");
        using var task = File.WriteAllBytesAsync(filePath, www.downloadHandler.GetData());

        while (!task.IsCompleted)
        {
            if (task.Exception != null)
            {
                Fancy.Instance.Error(task.Exception);
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        LoadingUI.Instance.LoadingProgress.SetText("Extracting Icons");
        ZipFile.ExtractToDirectory(filePath, Instance.Path, true);

        var dir = Directory.EnumerateDirectories(Instance.Path, $"{packJson.RepoOwner}-{packJson.RepoName}*").FirstOrDefault();
        var time = 0f;

        foreach (var file in Directory.EnumerateFiles(dir, $"*.png", SearchOption.AllDirectories).Where(x => x.Contains(packName) || x.Contains(packJson.RepoName)))
        {
            if (Instance.Abort)
                break;

            File.Move(file, file.Replace(dir, Instance.Path));
            time += Time.deltaTime;

            if (time > 0.1f)
            {
                time -= 0.1f;
                yield return new WaitForEndOfFrame();
            }
        }

        LoadingUI.Instance.LoadingProgress.SetText("Cleaning Up");
        File.Delete(filePath);
        Directory.Delete(dir, true);

        if (Instance.Abort)
            Fancy.Instance.Warning("Process was aborted");

        LoadingUI.Instance.LoadingProgress.SetText("Loading Icon Pack");
        ModSettings.SetString("Selected Icon Pack", packName, "alchlcsystm.fancy.ui");
        TryLoadingSprites(packName, PackType.IconPacks);
        Instance.OpenDirectory();
        Running[packName] = false;
        LoadingUI.Instance.LoadingProgress.SetText("Loaded!");
        yield return new WaitForSeconds(1f);

        LoadingUI.Instance.Finish();
        yield break;
    }
}