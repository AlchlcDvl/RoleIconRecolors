using UnityEngine.Events;
using UnityEngine.Networking;
using Newtonsoft.Json;
using FancyUI.Assets.IconPacks;
using System.IO.Compression;

namespace FancyUI.UI;

public class DownloaderUI : UIController
{
    private const string Repo = "https://raw.githubusercontent.com/AlchlcDvl/RoleIconRecolors/main";
    private static readonly Dictionary<string, bool> Running = [];
    private static bool HandlerRunning  { get; set; }

    private HoverEffect Back { get; set; }
    private HoverEffect OpenDir { get; set; }

    private GameObject NoPacks { get; set; }
    private GameObject PackTemplate { get; set; }

    private TextMeshProUGUI Title { get; set; }

    private TMP_InputField PackName { get; set; }
    private TMP_InputField RepoName { get; set; }
    private TMP_InputField RepoOwner { get; set; }
    private TMP_InputField BranchName { get; set; }

    private HoverEffect PackNameHover { get; set; }
    private HoverEffect RepoNameHover { get; set; }
    private HoverEffect RepoOwnerHover { get; set; }
    private HoverEffect BranchNameHover { get; set; }

    public bool Abort { get; set; }

    private static string Type => FancyUI.Instance.Page == PackType.IconPacks ? "Icon Pack" : "Silhouette Set";
    private static string FolderPath => Path.Combine(Fancy.Instance.ModPath, $"{FancyUI.Instance.Page}");

    private static readonly List<PackJson> Packs = [];
    private readonly List<GameObject> PackGOs = [];

    public static DownloaderUI Instance { get; private set; }

    public void Awake()
    {
        Instance = this;

        PackName = transform.GetComponent<TMP_InputField>("PackName")!;
        RepoName = transform.GetComponent<TMP_InputField>("RepoName")!;
        RepoOwner = transform.GetComponent<TMP_InputField>("RepoOwner")!;
        BranchName = transform.GetComponent<TMP_InputField>("BranchName")!;

        PackNameHover = PackName.EnsureComponent<HoverEffect>()!;
        PackNameHover.LookupKey = "FANCY_PACK_NAME";

        RepoNameHover = RepoName.EnsureComponent<HoverEffect>()!;
        RepoNameHover.LookupKey = "FANCY_REPO_NAME";

        RepoOwnerHover = RepoOwner.EnsureComponent<HoverEffect>()!;
        RepoOwnerHover.LookupKey = "FANCY_REPO_OWNER";

        BranchNameHover = BranchName.EnsureComponent<HoverEffect>()!;
        BranchNameHover.LookupKey = "FANCY_BRANCH_NAME";

        NoPacks = transform.FindRecursive("NoPacks").gameObject;
        PackTemplate = transform.FindRecursive("PackTemplate").gameObject;

        Title = transform.GetComponent<TextMeshProUGUI>("Title");

        Back = transform.EnsureComponent<HoverEffect>("Back")!;
        Back.GetComponent<Button>().onClick.AddListener(GoBack);
        Back.LookupKey = "FANCY_CLOSE_MENU";

        OpenDir = transform.EnsureComponent<HoverEffect>("Directory")!;
        OpenDir.LookupKey = "FANCY_OPEN_MENU";
        OpenDir.GetComponent<Button>().onClick.AddListener(OpenDirectory);

        var rend = OpenDir.GetComponent<Image>();
        OpenDir.AddOnOverListener(() => rend.sprite = Fancy.Assets.GetSprite("OpenChest"));
        OpenDir.AddOnOutListener(() => rend.sprite = Fancy.Assets.GetSprite("ClosedChest"));

        var confirm = transform.FindRecursive("Confirm");
        confirm.GetComponent<Button>().onClick.AddListener(AfterGenerating);
        confirm.EnsureComponent<HoverEffect>()!.LookupKey = "FANCY_CONFIRM_INPUT";

        PackTemplate.SetActive(false);

        FancyUI.SetupFonts(transform);
    }

    public void OnEnable()
    {
        Title.SetText(l10n("FANCY_DOWNLOADER_TITLE").Replace("%type%", Type));

        OpenDir.FillInKeys = Back.FillInKeys = PackNameHover.FillInKeys = RepoNameHover.FillInKeys = RepoOwnerHover.FillInKeys = BranchNameHover.FillInKeys = [ ( "%type%", Type ) ];

        Packs.ForEach(x => SetUpPack(x, () => DownloadIcons(x.Name)));

        NoPacks.SetActive(Packs.Count(x => x.Type == FancyUI.Instance.Page.ToString()) == 0);
    }

    public void OnDestroy()
    {
        Instance = null;

        var enumerable = Packs.Where(x => !x.FromMainRepo && x.Type == "IconPacks");
        GeneralUtils.SaveText("OtherPacks.json", JsonConvert.SerializeObject(enumerable, enumerable.GetType(), Formatting.Indented, new()), path: IPPath);

        var enumerable2 = Packs.Where(x => !x.FromMainRepo && x.Type == "SilhouetteSets");
        GeneralUtils.SaveText("OtherSets.json", JsonConvert.SerializeObject(enumerable2, enumerable2.GetType(), Formatting.Indented, new()), path: SSPath);
    }

    public void AfterGenerating()
    {
        var json = GenerateLinkAndAddToPackCount();
        json.Type = FancyUI.Instance.Page.ToString();
        SetUpPack(json, () => DownloadIcons(json.Name));
    }

    public void GoBack()
    {
        gameObject.SetActive(false);
        FancyUI.Instance.Page = PackType.Testing;
        FancyUI.Instance.gameObject.SetActive(true);
    }

    private PackJson GenerateLinkAndAddToPackCount()
    {
        var packNameText = PackName.text;

        if (StringUtils.IsNullEmptyOrWhiteSpace(packNameText))
        {
            Fancy.Instance.Error("Tried to generate pack link with no pack name");
            return null;
        }

        var packJson = new PackJson()
        {
            Name = packNameText,
            RepoName = RepoName.text,
            RepoOwner = RepoOwner.text,
            Branch = BranchName.text,
        };
        packJson.SetDefaults();
        return packJson;
    }

    public void OpenDirectory() => GeneralUtils.OpenDirectory(FolderPath);

    // Why the hell am I not allowed to make extension methods in instance classes smh
    public void SetUpPack(PackJson packJson, UnityAction download)
    {
        if (!Packs.Contains(packJson))
            Packs.Add(packJson);

        if (!PackGOs.TryFinding(x => x.name == packJson.Name, out var go))
        {
            go = Instantiate(PackTemplate, PackTemplate.transform.parent);
            go.name = packJson.Name;
            go.transform.GetComponent<TextMeshProUGUI>("PackName")!.SetText(packJson.Name);
            var link = go.transform.Find("RepoButton");
            link.GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(packJson.Link()));
            link.EnsureComponent<HoverEffect>()!.LookupKey = "FANCY_OPEN_LINK";
            var button = go.transform.Find("Download");
            button.GetComponent<Button>().onClick.AddListener(download);
            var hover = button.EnsureComponent<HoverEffect>()!;
            hover.LookupKey = "FANCY_DOWNLOAD_PACK";
            hover.FillInKeys = [ ( "%pack%", packJson.Name ) ];

            if (!StringUtils.IsNullEmptyOrWhiteSpace(packJson.Credits))
                go.EnsureComponent<HoverEffect>()!.NonLocalizedString = packJson.Credits;

            PackGOs.Add(go);
        }

        go!.SetActive(packJson.Type == FancyUI.Instance.Page.ToString());
    }

    public static void HandlePackData() => Coroutines.Start(CoHandlePackData());

    private static IEnumerator CoHandlePackData()
    {
        if (HandlerRunning)
            yield break;

        HandlerRunning = true;
        using var www = UnityWebRequest.Get($"{Repo}/Packs.json");
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
        {
            var array = JsonConvert.DeserializeObject<PackJson[]>(others);
            array.ForEach(x => x.Type = "IconPacks");
            Packs.AddRange(array);
        }

        var others2 = GeneralUtils.ReadText("OtherSets.json", SSPath);

        if (!StringUtils.IsNullEmptyOrWhiteSpace(others2))
        {
            var array = JsonConvert.DeserializeObject<PackJson[]>(others2);
            array.ForEach(x => x.Type = "SilhouetteSets");
            Packs.AddRange(array);
        }

        Packs.ForEach(x => x.SetDefaults());
        HandlerRunning = false;
    }

    private static void DownloadIcons(string packName) => Instance.StartCoroutine(CoDownloadIcons(packName));

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

        if (!Packs.TryFinding(x => x.Name == packName, out var packJson))
        {
            Fancy.Instance.Error($"{packName} somehow doesn't exist");
            Running[packName] = false;
            LoadingUI.Instance.Finish();
            yield break;
        }

        LoadingUI.Instance.LoadingProgress.SetText("Clearing Old Files/Creating Folders");
        var pack = Path.Combine(IPPath, packName);

        if (!Directory.Exists(pack))
            Directory.CreateDirectory(pack);
        else
            Directory.EnumerateFiles(pack, "*.png", SearchOption.AllDirectories).ForEach(File.Delete);

        IconPack.PopulateDirectory(pack);
        LoadingUI.Instance.LoadingProgress.SetText("Retrieving GitHub Data");
        using var www = UnityWebRequest.Get(packJson!.ApiLink());
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
        var filePath = Path.Combine(IPPath, $"{packName}.zip");
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
        ZipFile.ExtractToDirectory(filePath, FolderPath, true);

        var dir = Directory.EnumerateDirectories(FolderPath, $"{packJson.RepoOwner}-{packJson.RepoName}*").FirstOrDefault();
        var time = 0f;

        foreach (var file in Directory.EnumerateFiles(dir!, "*.png", SearchOption.AllDirectories).Where(x => x.ContainsAny(packName, packJson.RepoName)))
        {
            if (Instance.Abort)
                break;

            File.Move(file, file.Replace(dir, FolderPath));
            time += Time.deltaTime;

            if (time < 0.1f)
                continue;

            time -= 0.1f;
            yield return new WaitForEndOfFrame();
        }

        LoadingUI.Instance.LoadingProgress.SetText("Cleaning Up");
        File.Delete(filePath);
        Directory.Delete(dir, true);

        if (Instance.Abort)
            Fancy.Instance.Warning("Process was aborted");

        LoadingUI.Instance.LoadingProgress.SetText("Loading Icon Pack");
        Fancy.SelectedIconPack.Value = packName;
        TryLoadingSprites(packName, FancyUI.Instance.Page);
        Instance.OpenDirectory();
        Running[packName] = false;
        LoadingUI.Instance.LoadingProgress.SetText("Loaded!");
        yield return new WaitForSeconds(1f);

        LoadingUI.Instance.Finish();
    }
}