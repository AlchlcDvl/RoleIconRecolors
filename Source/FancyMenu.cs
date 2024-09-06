using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Home.Shared;
using Home.Common;
using System.Diagnostics;

namespace FancyUI;

public class FancyMenu : UIController
{
    private const string REPO = "https://raw.githubusercontent.com/AlchlcDvl/RoleIconRecolors/main";
    private static readonly Dictionary<string, bool> Running = [];
    private static bool HandlerRunning;
    public static readonly List<PackJson> Packs = [];

    public static FancyMenu Instance { get; private set; }

    private GameObject Back;
    private GameObject OpenDir;
    private GameObject Next;
    private GameObject Previous;
    private GameObject Confirm;
    private GameObject PackName;
    private GameObject RepoName;
    private GameObject RepoOwner;
    private GameObject BranchName;
    private GameObject JsonName;
    private GameObject NoPacks;
    private GameObject Pack1;
    private GameObject Pack2;
    private GameObject Pack3;
    private GameObject Pack4;
    private GameObject WaitingScreen;

    private TextMeshProUGUI CounterTMP;
    private int Page;

    private static TMP_FontAsset GameFont;
    private static Material GameFontMaterial;

    public void Start()
    {
        Instance = this;
        Back = transform.Find("Buttons/Back").gameObject;
		OpenDir = transform.Find("Buttons/Directory").gameObject;
		Confirm = transform.Find("Buttons/Confirm").gameObject;
		Next = transform.Find("Pages/Next").gameObject;
		Previous = transform.Find("Pages/Previous").gameObject;
        NoPacks = transform.Find("NoPacks").gameObject;
        Pack1 = transform.Find("Pack1").gameObject;
        Pack2 = transform.Find("Pack2").gameObject;
        Pack3 = transform.Find("Pack3").gameObject;
        Pack4 = transform.Find("Pack4").gameObject;
        PackName = transform.Find("Inputs/PackName").gameObject;
        RepoName = transform.Find("Inputs/RepoName").gameObject;
        RepoOwner = transform.Find("Inputs/RepoOwner").gameObject;
        BranchName = transform.Find("Inputs/BranchName").gameObject;
        JsonName = transform.Find("Inputs/JsonName").gameObject;

        CounterTMP = transform.Find("Pages").GetComponent<TextMeshProUGUI>();

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

        SetupMenu();
    }

    public void OnDestroy()
    {
        Instance = null;
        WaitingScreen.Destroy();
        GeneralUtils.SaveText("OtherPacks.json", JsonConvert.SerializeObject(Packs.Where(x => !x.FromMainRepo).ToList(), typeof(List<PackJson>), Formatting.Indented, new()),
            path: AssetManager.ModPath);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            gameObject.Destroy();
    }

    private void SetupMenu()
    {
        Back.GetComponent<Button>().onClick.AddListener(gameObject.Destroy);
        Back.AddComponent<TooltipTrigger>().NonLocalizedString = "Close Packs Menu";
        OpenDir.GetComponent<Button>().onClick.AddListener(OpenDirectory);
        OpenDir.AddComponent<TooltipTrigger>().NonLocalizedString = "Open Icons Folder";
        Next.AddComponent<TooltipTrigger>().NonLocalizedString = "Next Page";
        Next.GetComponent<Button>().onClick.AddListener(() => ChangePage(true));
        Previous.AddComponent<TooltipTrigger>().NonLocalizedString = "Previous Page";
        Previous.GetComponent<Button>().onClick.AddListener(() => ChangePage(false));
        CounterTMP.gameObject.AddComponent<TooltipTrigger>().NonLocalizedString = "Page Counter";
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
        SetPage();
    }

    private void ChangePage(bool increment)
    {
        Page += increment ? 1 : -1;
        var funnyMath = (Packs.Count / 3) + (Packs.Count % 3 == 0 ? 0 : 1);

        if (Page < 0)
            Page = funnyMath;
        else if (Page > funnyMath)
            Page = 0;

        SetPage();
    }

    private void SetPage()
    {
        Pack1.SetActive(Packs.Count > 0);
        Pack2.SetActive(Packs.Count < 5 || ((Page * 3) + 2) <= Packs.Count);
        Pack3.SetActive(Packs.Count < 5 || ((Page + 1) * 3) <= Packs.Count);
        Pack4.SetActive(Packs.Count == 4);
        NoPacks.SetActive(Packs.Count == 0);
        CounterTMP.gameObject.SetActive(Packs.Count > 4);
        var funnyMath = (Packs.Count / 3) + (Packs.Count % 3 == 0 ? 0 : 1);
        var unfunnyMath = Page + 1;
        CounterTMP.SetText($"{unfunnyMath}/{funnyMath}");
        SetUpPack(Pack1, Page, 1);
        SetUpPack(Pack2, Page, 2);
        SetUpPack(Pack3, Page, 3);
        SetUpPack(Pack4, Page, 4);
    }

    // Why the hell am I not allowed to make extension methods in instance classes smhh
    private static void SetUpPack(GameObject pack, int page, int index)
    {
        var packJson = Packs[Packs.Count < 5 ? (index - 1) : ((page * 3) + index)];
        pack.transform.Find("PackName").GetComponent<TextMeshProUGUI>().SetText(packJson.DisplayName);
        var link = pack.transform.Find("RepoButton");
        link.GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(packJson.Link()));
        link.gameObject.EnsureComponent<TooltipTrigger>().NonLocalizedString = "Open Link";
        var button = pack.transform.Find("Download");
        button.GetComponent<Button>().onClick.AddListener(() => DownloadIcons(packJson.Name));
        button.gameObject.EnsureComponent<TooltipTrigger>().NonLocalizedString = $"Download {packJson.Name}";

        if (!StringUtils.IsNullEmptyOrWhiteSpace(packJson.Credits))
            pack.EnsureComponent<TooltipTrigger>().NonLocalizedString = packJson.Credits;
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
        Packs.Add(packJson);
        SetPage();
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
        var pack = Path.Combine(AssetManager.ModPath, "IconPacks", packName);

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