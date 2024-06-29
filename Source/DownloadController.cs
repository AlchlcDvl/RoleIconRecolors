using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Home.Shared;
using Home.Common;
using Home.Common.Tooltips;
using System.Diagnostics;

namespace FancyUI;

public class DownloadController : UIController
{
    private const string REPO = "https://raw.githubusercontent.com/AlchlcDvl/RoleIconRecolors/main";
    private static readonly Dictionary<string, bool> Running = [];
    private static bool HandlerRunning;
    private static readonly Dictionary<string, PackJson> Packs = [];

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
    private GameObject ScrollView;
    private GameObject WaitingScreen;

    private readonly List<GameObject> PackGOs = [];

    private static TMP_FontAsset GameFont;
    private static Material GameFontMaterial;

    public void Start()
    {
        CacheObjects();
        StartCoroutine(SetupMenu());
    }

    public void OnDestroy()
    {
        WaitingScreen.Destroy();
        StopCoroutine(SetupMenu());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            gameObject.Destroy();
    }

    public void CacheObjects()
    {
        Back = transform.Find("Buttons/Back").gameObject;
		OpenDir = transform.Find("Buttons/Directory").gameObject;
		Confirm = transform.Find("Buttons/Confirm").gameObject;
        NoPacks = transform.Find("NoPacks").gameObject;
        ScrollView = transform.Find("ScrollView").gameObject;
        PackName = transform.Find("Inputs/PackName").gameObject;
        RepoName = transform.Find("Inputs/RepoName").gameObject;
        RepoOwner = transform.Find("Inputs/RepoOwner").gameObject;
        BranchName = transform.Find("Inputs/BranchName").gameObject;
        JsonName = transform.Find("Inputs/JsonName").gameObject;
        PackTemplate = transform.Find("ScrollView/Viewport/Content/PackTemplate").gameObject;
        WaitingScreen = Instantiate(AssetManager.AssetGOs["WaitingScreen"], transform);
        WaitingScreen.transform.localPosition = new(0, 0, -1f);
        WaitingScreen.SetActive(false);

        GameFont = ApplicationController.ApplicationContext.FontControllerSource.fonts[0].tmp_FontAsset;
        GameFontMaterial = ApplicationController.ApplicationContext.FontControllerSource.fonts[0].standardFontMaterial;

        foreach (var tmp in transform.GetComponentsInChildren<TextMeshProUGUI>())
        {
            tmp.font = GameFont;
            tmp.fontMaterial = GameFontMaterial;
        }
    }

    private IEnumerator SetupMenu()
    {
        Back.GetComponent<Button>().onClick.AddListener(gameObject.Destroy);
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
        NoPacks.SetActive(Packs.Count == 0);
        ScrollView.SetActive(Packs.Count > 0);
        PackTemplate.SetActive(false);
        var time = 0f;

        foreach (var (packName, packJson) in Packs)
        {
            var go = Instantiate(PackTemplate, PackTemplate.transform.parent);
            go.name = packName;
            go.transform.Find("PackName").GetComponent<TextMeshProUGUI>().SetText(packJson.DisplayName);
            var link = go.transform.Find("RepoLink");
            var linkText = packJson.Link();
            link.GetComponentInChildren<TextMeshProUGUI>().SetText(linkText);
            link.GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(linkText));
            link.gameObject.AddComponent<TooltipTrigger>().NonLocalizedString = "Open Link";
            go.transform.Find("PackJSONLink").GetComponent<TextMeshProUGUI>().SetText(packJson.JsonLink());
            var button = go.transform.Find("Download");
            button.GetComponent<Button>().onClick.AddListener(() => DownloadIcons(packName));
            button.gameObject.AddComponent<TooltipTrigger>().NonLocalizedString = $"Download {packName}";
            var pos = go.transform.localPosition;
            pos.y -= 98.3551f * PackGOs.Count;
            go.transform.localPosition = pos;
            go.SetActive(true);
            PackGOs.Add(go);

            if (!StringUtils.IsNullEmptyOrWhiteSpace(packJson.Credits))
                go.AddComponent<TooltipTrigger>().NonLocalizedString = packJson.Credits;

            time += Time.deltaTime;

            if (time > 0.1f)
            {
                time = 0f;
                yield return new WaitForEndOfFrame();
            }
        }

        yield break;
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
            RepoName = RepoName.GetComponent<TMP_InputField>().text ?? "RoleIconRecolors",
            RepoOwner = RepoOwner.GetComponent<TMP_InputField>().text ?? "AlchlcDvl",
            Branch = BranchName.GetComponent<TMP_InputField>().text ?? "main",
            JsonName = JsonName.GetComponent<TMP_InputField>().text ?? name,
        };
        packJson.SetDefaults();
        Packs[name] = packJson;
        var go = Instantiate(PackTemplate, PackTemplate.transform.parent);
        go.name = name;
        go.transform.Find("PackName").GetComponent<TextMeshProUGUI>().SetText(packJson.DisplayName);
        var link = go.transform.Find("RepoLink");
        var linkText = packJson.Link();
        link.GetComponentInChildren<TextMeshProUGUI>().SetText(linkText);
        link.GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(linkText));
        link.gameObject.AddComponent<TooltipTrigger>().NonLocalizedString = "Open Link";
        go.transform.Find("PackJSONLink").GetComponent<TextMeshProUGUI>().SetText(packJson.JsonLink());
        var button = go.transform.Find("Download");
        button.GetComponent<Button>().onClick.AddListener(() => DownloadIcons(name));
        button.gameObject.AddComponent<TooltipTrigger>().NonLocalizedString = $"Download {name}";
        var pos = go.transform.localPosition;
        pos.y -= 98.3551f * PackGOs.Count;
        go.transform.localPosition = pos;
        go.SetActive(true);
        PackGOs.Add(go);

        if (!StringUtils.IsNullEmptyOrWhiteSpace(packJson.Credits))
            go.AddComponent<TooltipTrigger>().NonLocalizedString = packJson.Credits;
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

        var json = JsonConvert.DeserializeObject<List<PackJson>>(www.downloadHandler.text);

        foreach (var jsonPack in json)
        {
            jsonPack.SetDefaults();
            Packs[jsonPack.Name] = jsonPack;
        }

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
        var pack = Path.Combine(AssetManager.ModPath, "IconPacks", packName);

        if (!Directory.Exists(pack))
            Directory.CreateDirectory(pack);

        var packJson = Packs[packName];
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

        if (json.Assets != null)
        {
            foreach (var asset in json.Assets)
            {
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
            }
        }

        if (json.ModAssets != null)
        {
            foreach (var mod in json.ModAssets)
            {
                if (mod.Assets != null)
                {
                    foreach (var asset in mod.Assets)
                    {
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
                    }
                }

                if (mod.Folders != null)
                {
                    foreach (var folder in mod.Folders)
                    {
                        if (folder.Assets == null)
                            continue;

                        foreach (var asset in folder.Assets)
                        {
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
                        }
                    }
                }
            }

            AssetManager.TryLoadingSprites(packName, PackType.IconPacks);
        }

        OpenDirectory();
        Running[packName] = false;
        ApplicationController.ApplicationContext.EnableTransitionOverlay(false, false, "");
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