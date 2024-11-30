using UnityEngine.Events;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using FancyUI.Assets.IconPacks;
using System.IO.Compression;

namespace FancyUI.UI;

public class DownloaderUI : UIController
{
    public const string REPO = "https://raw.githubusercontent.com/AlchlcDvl/RoleIconRecolors/main";
    public static readonly Dictionary<string, bool> Running = [];
    public static bool HandlerRunning  { get; set; }

    private GameObject Back { get; set; }
    private GameObject OpenDir { get; set; }
    private GameObject Confirm { get; set; }
    private TMP_InputField PackName { get; set; }
    private TMP_InputField RepoName { get; set; }
    private TMP_InputField RepoOwner { get; set; }
    private TMP_InputField BranchName { get; set; }
    private GameObject NoPacks { get; set; }
    private GameObject PackTemplate { get; set; }

    public bool Abort { get; set; }

    private static string Type => FancyUI.Instance.Page == PackType.IconPacks ? "Icon Pack" : "Silhouette Set";
    private static string FolderPath => Path.Combine(Fancy.Instance.ModPath, $"{FancyUI.Instance.Page}");

    private static readonly List<PackJson> Packs = [];
    private readonly List<GameObject> PackGOs = [];

    public static DownloaderUI Instance { get; private set; }

    public void Awake()
    {
        Instance = this;

        Back = transform.FindRecursive("Back").gameObject;
        OpenDir = transform.FindRecursive("Directory").gameObject;
        Confirm = transform.FindRecursive("Confirm").gameObject;
        PackName = transform.GetComponent<TMP_InputField>("PackName");
        RepoName = transform.GetComponent<TMP_InputField>("RepoName");
        RepoOwner = transform.GetComponent<TMP_InputField>("RepoOwner");
        BranchName = transform.GetComponent<TMP_InputField>("BranchName");
        NoPacks = transform.FindRecursive("NoPacks").gameObject;
        PackTemplate = transform.FindRecursive("PackTemplate").gameObject;

        transform.Find("Title").GetComponent<TextMeshProUGUI>().SetText($"{Type}s");

        Back.GetComponent<Button>().onClick.AddListener(GoBack);

        OpenDir.GetComponent<Button>().onClick.AddListener(OpenDirectory);

        var dirButton = OpenDir.EnsureComponent<HoverEffect>();
        var rend = OpenDir.GetComponent<Image>();
        dirButton.OnMouseOver.AddListener(() => rend.sprite = Fancy.Assets.GetSprite("OpenChest"));
        dirButton.OnMouseOut.AddListener(() => rend.sprite = Fancy.Assets.GetSprite("ClosedChest"));

        Confirm.GetComponent<Button>().onClick.AddListener(AfterGenerating);

        PackTemplate.SetActive(false);
    }

    public void Start()
    {
        Back.EnsureComponent<TooltipTrigger>().NonLocalizedString = $"Close {Type} Menu";
        OpenDir.EnsureComponent<TooltipTrigger>().NonLocalizedString = $"Open {Type} Folder";
        Confirm.EnsureComponent<TooltipTrigger>().NonLocalizedString = "Confirm Link Parameters And Generate Link";
        PackName.EnsureComponent<TooltipTrigger>().NonLocalizedString = $"Name Of The {Type} (REQUIRED)";
        RepoName.EnsureComponent<TooltipTrigger>().NonLocalizedString = $"Name Of The {Type} GitHub Repository (Defaults To: RoleIconRecolors)";
        RepoOwner.EnsureComponent<TooltipTrigger>().NonLocalizedString = $"Name Of The {Type} GitHub Repository Owner (Defaults To: AlchlcDvl)";
        BranchName.EnsureComponent<TooltipTrigger>().NonLocalizedString = $"Name Of The {Type} GitHub Repository Branch It Is In (Defaults To: main)";

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
        FancyUI.Instance.gameObject.SetActive(true);
    }

    public PackJson GenerateLinkAndAddToPackCount()
    {
        var name = PackName.text;

        if (StringUtils.IsNullEmptyOrWhiteSpace(name))
        {
            Fancy.Instance.Error("Tried to generate pack link with no pack name");
            return null;
        }

        var packJson = new PackJson()
        {
            Name = name,
            RepoName = RepoName.text,
            RepoOwner = RepoOwner.text,
            Branch = BranchName.text,
        };
        packJson.SetDefaults();
        return packJson;
    }

    public void OpenDirectory() => GeneralUtils.OpenDirectory(FolderPath);

    // Why the hell am I not allowed to make extension methods in instance classes smhh
    public void SetUpPack(PackJson packJson, UnityAction download)
    {
        if (!Packs.Contains(packJson))
            Packs.Add(packJson);

        if (!PackGOs.TryFinding(x => x.name == packJson.Name, out var go))
        {
            go = Instantiate(PackTemplate, PackTemplate.transform.parent);
            go.name = packJson.Name;
            go.transform.Find("PackName").GetComponent<TextMeshProUGUI>().SetText(packJson.Name);
            var link = go.transform.Find("RepoButton");
            link.GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(packJson.Link()));
            link.AddComponent<TooltipTrigger>().NonLocalizedString = "Open Link";
            var button = go.transform.Find("Download");
            button.GetComponent<Button>().onClick.AddListener(download);
            button.gameObject.AddComponent<TooltipTrigger>().NonLocalizedString = $"Download {packJson.Name}";

            if (!StringUtils.IsNullEmptyOrWhiteSpace(packJson.Credits))
                go.AddComponent<TooltipTrigger>().NonLocalizedString = packJson.Credits;

            PackGOs.Add(go);
        }

        go.SetActive(packJson.Type == FancyUI.Instance.Page.ToString());
    }

    public static void HandlePackData() => Coroutines.Start(CoHandlePackData());

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
        {
            var array = JsonConvert.DeserializeObject<PackJson[]>(others);
            array.ForEach(x => x.Type = "IconPacks");
            Packs.AddRange(array);
        }

        var others2 = GeneralUtils.ReadText("OtherSets.json", SSPath);

        if (!StringUtils.IsNullEmptyOrWhiteSpace(others2))
        {
            var array = JsonConvert.DeserializeObject<PackJson[]>(others2);
            array.ForEach(x => x.Type = "IconPacks");
            Packs.AddRange(array);
        }

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

        foreach (var file in Directory.EnumerateFiles(dir, $"*.png", SearchOption.AllDirectories).Where(x => x.Contains(packName) || x.Contains(packJson.RepoName)))
        {
            if (Instance.Abort)
                break;

            File.Move(file, file.Replace(dir, FolderPath));
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
        // ModSettings.SetString("Selected Icon Pack", packName, "alchlcsystm.fancy.ui");
        TryLoadingSprites(packName, FancyUI.Instance.Page);
        Instance.OpenDirectory();
        Running[packName] = false;
        LoadingUI.Instance.LoadingProgress.SetText("Loaded!");
        yield return new WaitForSeconds(1f);

        LoadingUI.Instance.Finish();
        yield break;
    }
}