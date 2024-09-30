using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Home.Shared;
using System.Diagnostics;

namespace FancyUI.IconPacks;

public class IconPacksUI : UIController
{
    private const string REPO = "https://raw.githubusercontent.com/AlchlcDvl/RoleIconRecolors/main";
    private static readonly Dictionary<string, bool> Running = [];
    private static bool HandlerRunning;
    public static readonly List<PackJson> Packs = [];

    public static IconPacksUI Instance { get; private set; }

    private GameObject Back;
    private GameObject OpenDir;
    private GameObject Confirm;
    private GameObject PackName;
    private GameObject RepoName;
    private GameObject RepoOwner;
    private GameObject BranchName;
    private GameObject JsonName;
    private GameObject NoPacks;
    private GameObject PackTemplate;

    public bool Abort { get; set; }

    private readonly List<GameObject> PackGOs = [];

    public void Start()
    {
        Instance = this;

        Back = transform.Find("Buttons/Back").gameObject;
		OpenDir = transform.Find("Buttons/Directory").gameObject;
		Confirm = transform.Find("Inputs/Confirm").gameObject;
        NoPacks = transform.Find("ScrollView/NoPacks").gameObject;
        PackName = transform.Find("Inputs/PackName").gameObject;
        RepoName = transform.Find("Inputs/RepoName").gameObject;
        RepoOwner = transform.Find("Inputs/RepoOwner").gameObject;
        BranchName = transform.Find("Inputs/BranchName").gameObject;
        JsonName = transform.Find("Inputs/JsonName").gameObject;
        PackTemplate = transform.Find("ScrollView/Viewport/Content/PackTemplate").gameObject;

        SetupMenu();
    }

    public void OnDestroy()
    {
        Instance = null;
        GeneralUtils.SaveText("OtherPacks.json", JsonConvert.SerializeObject(Packs.Where(x => !x.FromMainRepo).ToList(), typeof(List<PackJson>), Formatting.Indented, new()),
            path: AssetManager.IPPath);
    }

    private void SetupMenu()
    {
        Back.GetComponent<Button>().onClick.AddListener(GoBack);
        Back.AddComponent<TooltipTrigger>().NonLocalizedString = "Close Packs Menu";

        OpenDir.GetComponent<Button>().onClick.AddListener(OpenDirectory);
        OpenDir.AddComponent<TooltipTrigger>().NonLocalizedString = "Open Icons Folder";

        var dirButton = OpenDir.AddComponent<HoverEffect>();
        dirButton.OnMouseOver.AddListener(() => OpenDir.GetComponent<Image>().sprite = AssetManager.Assets["OpenChest"]);
        dirButton.OnMouseOut.AddListener(() => OpenDir.GetComponent<Image>().sprite = AssetManager.Assets["ClosedChest"]);

        Confirm.GetComponent<Button>().onClick.AddListener(GenerateLinkAndAddToPackCount);
        Confirm.AddComponent<TooltipTrigger>().NonLocalizedString = "Confirm Link Parameters And Generate Link";

        PackName.AddComponent<TooltipTrigger>().NonLocalizedString = "Name Of The Icon Pack (REQUIRED)";
        RepoName.AddComponent<TooltipTrigger>().NonLocalizedString = "Name Of The Icon Pack GitHub Repository (Defaults To: RoleIconRecolors)";
        RepoOwner.AddComponent<TooltipTrigger>().NonLocalizedString = "Name Of The Icon Pack GitHub Repository Owner (Defaults To: AlchlcDvl)";
        JsonName.AddComponent<TooltipTrigger>().NonLocalizedString = "Name Of The Icon Pack GitHub Json File (Defaults To: Name Of Your Pack)";
        BranchName.AddComponent<TooltipTrigger>().NonLocalizedString = "Name Of The Icon Pack GitHub Repository Branch The Pack Is In (Defaults To: main)";

        Packs.ForEach(SetUpPack);

        PackTemplate.SetActive(false);
        NoPacks.SetActive(Packs.Count == 0);
    }

    public void GoBack()
    {
        gameObject.SetActive(false);
        FancyUI.Instance.gameObject.SetActive(true);
    }

    // Why the hell am I not allowed to make extension methods in instance classes smhh
    private void SetUpPack(PackJson packJson)
    {
        var go = Instantiate(PackTemplate, PackTemplate.transform.parent);
        go.name = packJson.Name;
        go.transform.Find("PackName").GetComponent<TextMeshProUGUI>().SetText(packJson.DisplayName);
        var link = go.transform.Find("RepoButton");
        link.GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(packJson.Link()));
        link.AddComponent<TooltipTrigger>().NonLocalizedString = "Open Link";
        var button = go.transform.Find("Download");
        button.GetComponent<Button>().onClick.AddListener(() => DownloadIcons(packJson.Name));
        button.gameObject.AddComponent<TooltipTrigger>().NonLocalizedString = $"Download {packJson.DisplayName}";
        go.SetActive(true);
        PackGOs.Add(go);

        if (!StringUtils.IsNullEmptyOrWhiteSpace(packJson.Credits))
            go.AddComponent<TooltipTrigger>().NonLocalizedString = packJson.Credits;
    }

    private void GenerateLinkAndAddToPackCount()
    {
        var name = PackName.GetComponent<TMP_InputField>().text;

        if (StringUtils.IsNullEmptyOrWhiteSpace(name))
        {
            Logging.LogError("Tried to generate pack link with no pack name");
            return;
        }

        var packJson = new PackJson()
        {
            Name = name,
            RepoName = RepoName.GetComponent<TMP_InputField>().text,
            RepoOwner = RepoOwner.GetComponent<TMP_InputField>().text,
            Branch = BranchName.GetComponent<TMP_InputField>().text,
            JsonName = JsonName.GetComponent<TMP_InputField>().text,
        };
        packJson.SetDefaults();
        Packs.Add(packJson);
        SetUpPack(packJson);
    }

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

        Packs.Clear();
        Packs.AddRange(JsonConvert.DeserializeObject<List<PackJson>>(www.downloadHandler.text));
        Packs.ForEach(x => x.FromMainRepo = true);

        var others = GeneralUtils.ReadText("OtherPacks.json", AssetManager.ModPath);

        if (!StringUtils.IsNullEmptyOrWhiteSpace(others))
            Packs.AddRange(JsonConvert.DeserializeObject<List<PackJson>>(others));

        Packs.ForEach(x => x.SetDefaults());
        HandlerRunning = false;
        yield break;
    }

    public static void DownloadIcons(string packName) => ApplicationController.ApplicationContext.StartCoroutine(CoDownloadIcons(packName));

    private static IEnumerator CoDownloadIcons(string packName)
    {
        packName = packName.Replace(" ", "");

        if (Running.TryGetValue(packName, out var running) && running)
        {
            Logging.LogError($"{packName} download is still running");
            yield break;
        }

        ApplicationController.ApplicationContext.EnableTransitionOverlay(true, true, $"Downloading {packName}");
        Running[packName] = true;
        var pack = Path.Combine(AssetManager.IPPath, packName);

        if (!Directory.Exists(pack))
            Directory.CreateDirectory(pack);

        var packJson = Packs.Find(x => x.Name == packName);

        if (packJson == null)
        {
            Logging.LogError($"{packName} somehow doesn't exist");
            yield break;
        }

        var www = UnityWebRequest.Get(packJson.JsonLink());
        yield return www.SendWebRequest();

        while (!www.isDone)
            yield return new WaitForEndOfFrame();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Logging.LogError(www.error);
            Running[packName] = false;
            ApplicationController.ApplicationContext.EnableTransitionOverlay(false, false, "");
            yield break;
        }

        var json = JsonConvert.DeserializeObject<JsonItem>(www.downloadHandler.text);
        LoadingUI.Begin(Instance.gameObject, packName, json.Count());

        if (json.Assets != null)
        {
            foreach (var asset in json.Assets)
            {
                if (Instance.Abort)
                    break;

                asset.FileType ??= "png";

                var www2 = UnityWebRequest.Get($"{packJson.RawLink()}/{asset.FileName()}");
                yield return www2.SendWebRequest();

                while (!www2.isDone)
                    yield return new WaitForEndOfFrame();

                if (www2.result != UnityWebRequest.Result.Success)
                {
                    Logging.LogError(www2.error);
                    continue;
                }

                var path = Path.Combine(pack, asset.FileName());

                if (File.Exists(path))
                    File.Delete(path);

                var persistTask = File.WriteAllBytesAsync(path, www2.downloadHandler.data);

                while (!persistTask.IsCompleted)
                {
                    if (persistTask.Exception != null)
                    {
                        Logging.LogError(persistTask.Exception);
                        break;
                    }

                    yield return new WaitForEndOfFrame();
                }

                LoadingUI.Instance.UpdateProgress();
            }
        }

        if (json.ModAssets != null)
        {
            foreach (var mod in json.ModAssets)
            {
                if (Instance.Abort)
                    break;

                if (mod.Assets != null)
                {
                    foreach (var asset in mod.Assets)
                    {
                        if (Instance.Abort)
                            break;

                        asset.FileType ??= "png";

                        var www2 = UnityWebRequest.Get($"{packJson.RawLink()}/{mod.Name}/{asset.FileName()}");
                        yield return www2.SendWebRequest();

                        while (!www2.isDone)
                            yield return new WaitForEndOfFrame();

                        if (www2.result != UnityWebRequest.Result.Success)
                        {
                            Logging.LogError(www2.error);
                            continue;
                        }

                        var path = Path.Combine(pack, mod.Name, asset.FileName());

                        if (File.Exists(path))
                            File.Delete(path);

                        var persistTask = File.WriteAllBytesAsync(path, www2.downloadHandler.data);

                        while (!persistTask.IsCompleted)
                        {
                            if (persistTask.Exception != null)
                            {
                                Logging.LogError(persistTask.Exception);
                                break;
                            }

                            yield return new WaitForEndOfFrame();
                        }

                        LoadingUI.Instance.UpdateProgress();
                    }
                }

                if (mod.Folders != null)
                {
                    foreach (var folder in mod.Folders)
                    {
                        if (Instance.Abort)
                            break;

                        if (folder.Assets == null)
                            continue;

                        foreach (var asset in folder.Assets)
                        {
                            if (Instance.Abort)
                                break;

                            asset.FileType ??= "png";

                            var www2 = UnityWebRequest.Get($"{packJson.RawLink()}/{mod.Name}/{folder.Name}/{asset.FileName()}");
                            yield return www2.SendWebRequest();

                            while (!www2.isDone)
                                yield return new WaitForEndOfFrame();

                            if (www2.result != UnityWebRequest.Result.Success)
                            {
                                Logging.LogError(www2.error);
                                continue;
                            }

                            var path = Path.Combine(pack, mod.Name, folder.Name, asset.FileName());

                            if (File.Exists(path))
                                File.Delete(path);

                            var persistTask = File.WriteAllBytesAsync(path, www2.downloadHandler.data);

                            while (!persistTask.IsCompleted)
                            {
                                if (persistTask.Exception != null)
                                {
                                    Logging.LogError(persistTask.Exception);
                                    break;
                                }

                                yield return new WaitForEndOfFrame();
                            }

                            LoadingUI.Instance.UpdateProgress();
                        }
                    }
                }
            }

            AssetManager.TryLoadingSprites(packName, PackType.IconPacks);
        }

        OpenDirectory();
        Running[packName] = false;
        ApplicationController.ApplicationContext.EnableTransitionOverlay(false, false, "");
        LoadingUI.Instance.Finish();
        yield break;
    }

    public static void OpenDirectory()
    {
        // code stolen from jan who stole from tuba
        if (Environment.OSVersion.Platform is PlatformID.MacOSX or PlatformID.Unix)
            Process.Start("open", $"\"{AssetManager.ModPath}\"");
        else
            Application.OpenURL($"file://{AssetManager.ModPath}");
    }
}