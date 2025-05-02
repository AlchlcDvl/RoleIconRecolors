using System.IO.Compression;
using FancyUI.Assets.IconPacks;
using Newtonsoft.Json;
using UnityEngine.Networking;

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

    private readonly List<TextMeshProUGUI> WaxTexts = [];

    public IEnumerator InProgress { get; set; }

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

        (PackName.targetGraphic as Image).SetImageColor(ColorType.Metal);
        (RepoName.targetGraphic as Image).SetImageColor(ColorType.Metal);
        (RepoOwner.targetGraphic as Image).SetImageColor(ColorType.Metal);
        (BranchName.targetGraphic as Image).SetImageColor(ColorType.Metal);

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
        Back.GetComponent<Image>().SetImageColor(ColorType.Metal);

        OpenDir = transform.EnsureComponent<HoverEffect>("Directory")!;
        OpenDir.LookupKey = "FANCY_OPEN_MENU";
        OpenDir.GetComponent<Button>().onClick.AddListener(OpenDirectory);

        var chest = OpenDir.GetComponent<Image>();
        chest.SetImageColor(ColorType.Metal);
        var open = Fancy.Instance.Assets.GetSprite("OpenChest");
        var closed = Fancy.Instance.Assets.GetSprite("ClosedChest");
        OpenDir.AddOnOverListener(() => chest.sprite = open);
        OpenDir.AddOnOutListener(() => chest.sprite = closed);

        var confirm = transform.FindRecursive("Confirm");
        confirm.GetComponent<Button>().onClick.AddListener(AfterGenerating);
        confirm.EnsureComponent<HoverEffect>()!.LookupKey = "FANCY_CONFIRM_INPUT";
        confirm.GetComponent<Image>().SetImageColor(ColorType.Metal);

        transform.GetComponent<Image>("Fill").SetImageColor(ColorType.Metal);
        transform.GetComponent<Image>("Wax").SetImageColor(ColorType.Wax);

        PackTemplate.SetActive(false);

        FancyUI.SetupFonts(transform);
    }

    private void Refresh() => WaxTexts.ForEach(x => x.SetGraphicColor(ColorType.Wax));

    public void OnEnable()
    {
        Title.SetText(l10n("FANCY_DOWNLOADER_TITLE").Replace("%type%", Type));

        OpenDir.FillInKeys = Back.FillInKeys = PackNameHover.FillInKeys = RepoNameHover.FillInKeys = RepoOwnerHover.FillInKeys = BranchNameHover.FillInKeys = [("%type%", Type)];

        Packs.ForEach(SetUpPack);

        NoPacks.SetActive(Packs.Count(x => x.Type == FancyUI.Instance.Page.ToString()) == 0);
        Refresh();
    }

    public void OnDestroy()
    {
        Instance = null;

        if (FancyUI.Instance.Page == PackType.IconPacks)
        {
            var enumerable = Packs.Where(x => !x.FromMainRepo && x.Type == "IconPacks");
            NewModLoading.Utils.SaveText("OtherPacks.json", JsonConvert.SerializeObject(enumerable, enumerable.GetType(), Formatting.Indented, new()), path: IPPath);
        }

        if (FancyUI.Instance.Page == PackType.SilhouetteSets)
        {
            var enumerable = Packs.Where(x => !x.FromMainRepo && x.Type == "SilhouetteSets");
            NewModLoading.Utils.SaveText("OtherSets.json", JsonConvert.SerializeObject(enumerable, enumerable.GetType(), Formatting.Indented, new()), path: SSPath);
        }
    }

    public void AfterGenerating()
    {
        var json = GenerateLinkAndAddToPackCount();
        json.Type = FancyUI.Instance.Page.ToString();
        SetUpPack(json);

        if (!Packs.Contains(json))
            Packs.Add(json);

        Refresh();
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

        if (NewModLoading.Utils.IsNullEmptyOrWhiteSpace(packNameText))
        {
            Fancy.Instance.Error("Tried to generate pack link with no pack name");
            return null;
        }

        var packJson = new PackJson
        {
            Name = packNameText,
            RepoName = RepoName.text,
            RepoOwner = RepoOwner.text,
            Branch = BranchName.text,
        };
        packJson.SetDefaults();
        return packJson;
    }

    public void OpenDirectory() => NewModLoading.Utils.OpenDirectory(FolderPath);

    // Why the hell am I not allowed to make extension methods in instance classes smh
    public void SetUpPack(PackJson packJson)
    {
        if (!PackGOs.TryFinding(x => x.name == packJson.Name, out var go))
        {
            go = Instantiate(PackTemplate, PackTemplate.transform.parent);
            go.name = packJson.Name;
            go.transform.GetComponent<TextMeshProUGUI>("PackName")!.SetText(packJson.Name);
            var link = go.transform.Find("RepoButton");
            link.GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(packJson.Link()));
            link.EnsureComponent<HoverEffect>()!.LookupKey = "FANCY_OPEN_LINK";
            var button = go.transform.Find("Download");
            button.GetComponent<Button>().onClick.AddListener(() => DownloadIcons(packJson.Name));
            var hover = button.EnsureComponent<HoverEffect>()!;
            hover.LookupKey = "FANCY_DOWNLOAD_PACK";
            hover.FillInKeys = [("%pack%", packJson.Name)];

            if (!NewModLoading.Utils.IsNullEmptyOrWhiteSpace(packJson.Credits))
                go.EnsureComponent<HoverEffect>()!.NonLocalizedString = packJson.Credits;

            PackGOs.Add(go);
            go.transform.GetComponent<Image>("Background").SetImageColor(ColorType.Metal);
            button.GetComponent<Image>().SetImageColor(ColorType.Wax);
            link.GetComponent<Image>().SetImageColor(ColorType.Wax);
            WaxTexts.Add(link.GetComponent<TextMeshProUGUI>("Text"));
            WaxTexts.Add(button.GetComponent<TextMeshProUGUI>("Text"));
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

        if (www.result != UnityWebRequest.Result.Success)
        {
            Fancy.Instance.Error(www.error);
            HandlerRunning = false;
            yield break;
        }

        Packs.Clear();
        Packs.AddRange(JsonConvert.DeserializeObject<PackJson[]>(www.downloadHandler.text));
        Packs.ForEach(x => x.FromMainRepo = true);

        var others = NewModLoading.Utils.ReadText("OtherPacks.json", IPPath);

        if (!NewModLoading.Utils.IsNullEmptyOrWhiteSpace(others))
        {
            var array = JsonConvert.DeserializeObject<PackJson[]>(others);
            array.Do(x => x.Type = "IconPacks");
            Packs.AddRange(array);
        }

        var others2 = NewModLoading.Utils.ReadText("OtherSets.json", SSPath);

        if (!NewModLoading.Utils.IsNullEmptyOrWhiteSpace(others2))
        {
            var array = JsonConvert.DeserializeObject<PackJson[]>(others2);
            array.Do(x => x.Type = "SilhouetteSets");
            Packs.AddRange(array);
        }

        Packs.ForEach(x => x.SetDefaults());
        HandlerRunning = false;
    }

    private static void DownloadIcons(string packName) => Instance.InProgress = Coroutines.Start(CoDownloadIcons(packName));

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
        var pack = Path.Combine(FolderPath, packName);

        if (!Directory.Exists(pack))
            Directory.CreateDirectory(pack);
        else
            Directory.EnumerateFiles(pack, "*.png", SearchOption.AllDirectories).Do(File.Delete);

        IconPack.PopulateDirectory(pack);
        LoadingUI.Instance.LoadingProgress.SetText("Retrieving GitHub Data");
        using var www = UnityWebRequest.Get(packJson!.ApiLink());
        var op = www.SendWebRequest();

        while (!op.isDone)
        {
            LoadingUI.Instance.LoadingProgress.SetText(op.progress <= 1f ? $"Downloading Pack: {Mathf.RoundToInt(Mathf.Clamp(op.progress, 0f, 1f) * 100f)}%" : "Unity is shitting itself, please wait...");
            yield return null;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Fancy.Instance.Error(www.error);
            Running[packName] = false;
            LoadingUI.Instance.Finish();
            yield break;
        }

        LoadingUI.Instance.LoadingProgress.SetText("Fetching Pack Data");
        var filePath = Path.Combine(FolderPath, $"{packName}.zip");
        using var task = File.WriteAllBytesAsync(filePath, www.downloadHandler.GetData());

        while (!task.IsCompleted)
        {
            if (task.Exception != null)
            {
                Fancy.Instance.Error(task.Exception);
                break;
            }

            yield return null;
        }

        LoadingUI.Instance.LoadingProgress.SetText("Extracting Icons");
        ZipFile.ExtractToDirectory(filePath, FolderPath, true);

        var dir = Directory.EnumerateDirectories(FolderPath, $"{packJson.RepoOwner}-{packJson.RepoName}*").FirstOrDefault();
        var time = 0f;
        var theDir = dir;
        var pngAmount = 0;

        foreach (var tempPath in Directory.GetDirectories(dir!))
        {
            var tempLength = Directory.GetFiles(tempPath, "*.png", SearchOption.AllDirectories).Length;

            if (tempLength <= pngAmount)
                continue;

            theDir = tempPath;
            pngAmount = tempLength;
        }

        foreach (var file in Directory.EnumerateFiles(theDir!, "*.png", SearchOption.AllDirectories).Where(x => x.ContainsAny(packName, packJson.RepoName)))
        {
            try
            {
                File.Move(file, file.Replace(theDir, pack));
            } catch {}

            time += Time.deltaTime;

            if (time < 0.1f)
                continue;

            time -= 0.1f;
            yield return null;
        }

        LoadingUI.Instance.LoadingProgress.SetText("Cleaning Up");
        File.Delete(filePath);
        Directory.Delete(dir, true);

        LoadingUI.Instance.LoadingProgress.SetText("Loading Icon Pack");
        Fancy.SelectedIconPack.Value = packName;
        TryLoadingSprites(packName, FancyUI.Instance.Page);
        Instance.OpenDirectory();
        Running[packName] = false;
        LoadingUI.Instance.LoadingProgress.SetText("Loaded!");
        yield return Coroutines.Wait(1f);

        LoadingUI.Instance.Finish();
    }
}