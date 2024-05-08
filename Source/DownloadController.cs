using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Home.Shared;
using Home.Common;
using Home.Common.Tooltips;
using System.Diagnostics;

namespace IconPacks;

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
    private Scrollbar Scroll;
    private float Value;

    private readonly List<GameObject> PackGOs = [];

    private static TMP_FontAsset GameFont;
    private static Material GameFontMaterial;

    public void Start()
    {
        CacheObjects();
        StartCoroutine(SetupMenu());
    }

    public void OnDestroy() => StopCoroutine(SetupMenu());

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
        Scroll = transform.Find("ScrollView/ScrollbarVertical").GetComponent<Scrollbar>();
        Value = Scroll.value;

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
            go.transform.Find("PackName").GetComponent<TextMeshProUGUI>().SetText(packName);
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

        Scroll.value = Value * (PackGOs.Count == 0 ? 1 : (PackGOs.Count / 3));
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
        Packs[packJson.Name] = packJson;
        var go = Instantiate(PackTemplate, PackTemplate.transform.parent);
        go.name = packJson.Name;
        go.transform.Find("PackName").GetComponent<TextMeshProUGUI>().SetText(packJson.Name);
        var link = go.transform.Find("RepoLink");
        var linkText = packJson.Link();
        link.GetComponentInChildren<TextMeshProUGUI>().SetText(linkText);
        link.GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(linkText));
        link.gameObject.AddComponent<TooltipTrigger>().NonLocalizedString = "Open Link";
        go.transform.Find("PackJSONLink").GetComponent<TextMeshProUGUI>().SetText(packJson.JsonLink());
        var button = go.transform.Find("Download");
        button.GetComponent<Button>().onClick.AddListener(() => DownloadIcons(packJson.Name));
        button.gameObject.AddComponent<TooltipTrigger>().NonLocalizedString = $"Download {packJson.Name}";
        var pos = go.transform.localPosition;
        pos.y -= 98.3551f * PackGOs.Count;
        go.transform.localPosition = pos;
        go.SetActive(true);
        PackGOs.Add(go);
        Scroll.value = Value * (PackGOs.Count == 0 ? 1 : (PackGOs.Count / 3));
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
        else
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
                else
                {
                    foreach (var folder in mod.Folders)
                    {
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

            AssetManager.TryLoadingSprites(packName);
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