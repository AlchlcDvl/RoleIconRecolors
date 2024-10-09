namespace FancyUI.UI;

public class SilhouetteSwapperUI : BaseUI
{
    public static readonly List<PackJson> Packs = [];

    public override string Type => "Silhouette Set";
    public override string Path => AssetManager.SSPath;

    public static SilhouetteSwapperUI Instance { get; private set; }

    public override void Awake()
    {
        base.Awake();
        Instance = this;
        SetupMenu();
    }

    public override void SetupMenu()
    {
        base.SetupMenu();
        Test.GetComponent<Image>().sprite = AssetManager.Assets["SilSwapper"];
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
        // button.GetComponent<Button>().onClick.AddListener(() => DownloadIcons(packJson.Name));
        button.gameObject.AddComponent<TooltipTrigger>().NonLocalizedString = $"Download {packJson.Name}";
        go.SetActive(true);
        PackGOs.Add(go);

        if (!StringUtils.IsNullEmptyOrWhiteSpace(packJson.Credits))
            go.AddComponent<TooltipTrigger>().NonLocalizedString = packJson.Credits;
    }

    public override void AfterGenerating()
    {
    }

    public override void OpenTestingUI()
    {
        if (SilhouetteSetTestingUI.Instance)
        {
            SilhouetteSetTestingUI.Instance.gameObject.SetActive(true);
            gameObject.SetActive(false);
            return;
        }

        var go = Instantiate(AssetManager.AssetGOs["SilhouetteSetTestingUI"], transform.parent);
        go.transform.localPosition = new(0, 0, 0);
        go.transform.localScale = Vector3.one * 1.5f;
        go.AddComponent<SilhouetteSetTestingUI>();
        FancyUI.SetupFonts(go.transform);
        gameObject.SetActive(false);
    }
}