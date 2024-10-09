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
    public override string Path => AssetManager.IPPath;

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
    }

    public override void SetupMenu()
    {
        base.SetupMenu();
        Test.GetComponent<Image>().sprite = AssetManager.Assets["IconPack"];
        Packs.ForEach(SetUpPack);
        PackTemplate.SetActive(false);
        NoPacks.SetActive(Packs.Count == 0);
    }

    // Why the hell am I not allowed to make extension methods in instance classes smhh
    private void SetUpPack(PackJson packJson)
    {
        var go = Instantiate(PackTemplate, PackTemplate.transform.parent);
        go.name = packJson.Name;
        go.transform.Find("PackName").GetComponent<TextMeshProUGUI>().SetText(packJson.Name);
        var link = go.transform.Find("RepoButton");
        link.GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(packJson.Link()));
        link.AddComponent<TooltipTrigger>().NonLocalizedString = "Open Link";
        var button = go.transform.Find("Download");
        button.GetComponent<Button>().onClick.AddListener(() => DownloadIcons(packJson.Name));
        button.gameObject.AddComponent<TooltipTrigger>().NonLocalizedString = $"Download {packJson.Name}";
        go.SetActive(true);
        PackGOs.Add(go);

        if (!StringUtils.IsNullEmptyOrWhiteSpace(packJson.Credits))
            go.AddComponent<TooltipTrigger>().NonLocalizedString = packJson.Credits;
    }

    public override void AfterGenerating()
    {
        var json = GenerateLinkAndAddToPackCount();
        Packs.Add(json);
        SetUpPack(json);
    }

    public override void OpenTestingUI()
    {
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
            Logging.LogError(www.error);
            HandlerRunning = false;
            yield break;
        }

        Packs.Clear();
        Packs.AddRange(JsonConvert.DeserializeObject<PackJson[]>(www.downloadHandler.text));
        Packs.ForEach(x => x.FromMainRepo = true);

        var others = GeneralUtils.ReadText("OtherPacks.json", AssetManager.IPPath);

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
            Logging.LogError($"{packName} download is still running");
            LoadingUI.Instance.Finish();
            yield break;
        }

        LoadingUI.Begin(Instance.gameObject, packName);
        LoadingUI.Instance.LoadingProgress.SetText("Starting Download");
        Running[packName] = true;
        var packJson = Packs.Find(x => x.Name == packName);

        if (packJson == null)
        {
            Logging.LogError($"{packName} somehow doesn't exist");
            Running[packName] = false;
            LoadingUI.Instance.Finish();
            yield break;
        }

        LoadingUI.Instance.LoadingProgress.SetText("Clearing Old Files/Creating Folders");
        var pack = System.IO.Path.Combine(AssetManager.IPPath, packName);

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
            Logging.LogError(www.error);
            Running[packName] = false;
            LoadingUI.Instance.Finish();
            yield break;
        }

        LoadingUI.Instance.LoadingProgress.SetText("Fetching Pack Data");
        var filePath = System.IO.Path.Combine(AssetManager.IPPath, $"{packName}.zip");
        using var task = File.WriteAllBytesAsync(filePath, www.downloadHandler.GetData());

        while (!task.IsCompleted)
        {
            if (task.Exception != null)
            {
                Logging.LogError(task.Exception);
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
            Logging.LogWarning("Process was aborted");

        LoadingUI.Instance.LoadingProgress.SetText("Loading Icon Pack");
        ModSettings.SetString("Selected Icon Pack", packName, "alchlcsystm.fancy.ui");
        AssetManager.TryLoadingSprites(packName, PackType.IconPacks);
        Instance.OpenDirectory();
        Running[packName] = false;
        LoadingUI.Instance.LoadingProgress.SetText("Loaded!");
        yield return new WaitForSeconds(1f);

        LoadingUI.Instance.Finish();
        yield break;
    }
}