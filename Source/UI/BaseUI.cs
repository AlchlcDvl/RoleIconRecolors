using System.Diagnostics;
using UnityEngine.Events;

namespace FancyUI.UI;

public abstract class BaseUI : UIController
{
    public const string REPO = "https://raw.githubusercontent.com/AlchlcDvl/RoleIconRecolors/main";
    public static readonly Dictionary<string, bool> Running = [];
    public static bool HandlerRunning  { get; set; }

    private GameObject Back { get; set; }
    private GameObject OpenDir { get; set; }
    private GameObject Confirm { get; set; }
    public GameObject Test { get; set; }
    private TMP_InputField PackName { get; set; }
    private TMP_InputField RepoName { get; set; }
    private TMP_InputField RepoOwner { get; set; }
    private TMP_InputField BranchName { get; set; }
    public GameObject NoPacks { get; set; }
    public GameObject PackTemplate { get; set; }

    public bool Abort { get; set; }

    public abstract string Type { get; }
    public abstract string Path { get; }

    public readonly List<GameObject> PackGOs = [];

    public virtual void Awake()
    {
        Back = transform.Find("Buttons/Back").gameObject;
        OpenDir = transform.Find("Buttons/Directory").gameObject;
        Test = transform.Find("Buttons/Test").gameObject;
        Confirm = transform.Find("Inputs/Confirm").gameObject;
        PackName = transform.Find("Inputs/PackName").GetComponent<TMP_InputField>();
        RepoName = transform.Find("Inputs/RepoName").GetComponent<TMP_InputField>();
        RepoOwner = transform.Find("Inputs/RepoOwner").GetComponent<TMP_InputField>();
        BranchName = transform.Find("Inputs/BranchName").GetComponent<TMP_InputField>();
        NoPacks = transform.Find("ScrollView/NoPacks").gameObject;
        PackTemplate = transform.Find("ScrollView/Viewport/Content/PackTemplate").gameObject;

        transform.Find("Title").GetComponent<TextMeshProUGUI>().SetText($"{Type}s");
    }

    public virtual void SetupMenu()
    {
        Back.GetComponent<Button>().onClick.AddListener(GoBack);
        Back.AddComponent<TooltipTrigger>().NonLocalizedString = $"Close {Type} Menu";

        OpenDir.GetComponent<Button>().onClick.AddListener(OpenDirectory);
        OpenDir.AddComponent<TooltipTrigger>().NonLocalizedString = $"Open {Type} Folder";

        Test.GetComponent<Button>().onClick.AddListener(OpenTestingUI);
        Test.AddComponent<TooltipTrigger>().NonLocalizedString = $"Open {Type} Testing Menu";

        var dirButton = OpenDir.AddComponent<HoverEffect>();
        dirButton.OnMouseOver.AddListener(() => OpenDir.GetComponent<Image>().sprite = Fancy.Assets.GetSprite("OpenChest"));
        dirButton.OnMouseOut.AddListener(() => OpenDir.GetComponent<Image>().sprite = Fancy.Assets.GetSprite("ClosedChest"));

        Confirm.GetComponent<Button>().onClick.AddListener(AfterGenerating);
        Confirm.AddComponent<TooltipTrigger>().NonLocalizedString = "Confirm Link Parameters And Generate Link";

        PackName.AddComponent<TooltipTrigger>().NonLocalizedString = $"Name Of The {Type} (REQUIRED)";
        RepoName.AddComponent<TooltipTrigger>().NonLocalizedString = $"Name Of The {Type} GitHub Repository (Defaults To: RoleIconRecolors)";
        RepoOwner.AddComponent<TooltipTrigger>().NonLocalizedString = $"Name Of The {Type} GitHub Repository Owner (Defaults To: AlchlcDvl)";
        BranchName.AddComponent<TooltipTrigger>().NonLocalizedString = $"Name Of The {Type} GitHub Repository Branch It Is In (Defaults To: main)";
    }

    public void GoBack()
    {
        gameObject.SetActive(false);
        FancyUI.Instance.gameObject.SetActive(true);
    }

    public abstract void AfterGenerating();

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

    public void OpenDirectory()
    {
        // code stolen from jan who stole from tuba
        if (Environment.OSVersion.Platform is PlatformID.MacOSX or PlatformID.Unix)
            Process.Start("open", $"\"{Path}\"");
        else
            Application.OpenURL($"file://{Path}");
    }

    public abstract void OpenTestingUI();

    // Why the hell am I not allowed to make extension methods in instance classes smhh
    public void SetUpPack(PackJson packJson, UnityAction download)
    {
        var go = Instantiate(PackTemplate, PackTemplate.transform.parent);
        go.name = packJson.Name;
        go.transform.Find("PackName").GetComponent<TextMeshProUGUI>().SetText(packJson.Name);
        var link = go.transform.Find("RepoButton");
        link.GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(packJson.Link()));
        link.AddComponent<TooltipTrigger>().NonLocalizedString = "Open Link";
        var button = go.transform.Find("Download");
        button.GetComponent<Button>().onClick.AddListener(download);
        button.gameObject.AddComponent<TooltipTrigger>().NonLocalizedString = $"Download {packJson.Name}";
        go.SetActive(true);
        PackGOs.Add(go);

        if (!StringUtils.IsNullEmptyOrWhiteSpace(packJson.Credits))
            go.AddComponent<TooltipTrigger>().NonLocalizedString = packJson.Credits;
    }
}