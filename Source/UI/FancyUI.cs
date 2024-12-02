using Home.Shared;

namespace FancyUI.UI;

public class FancyUI : UIController
{
    private static TMP_FontAsset GameFont { get; set; }
    private static Material GameFontMaterial { get; set; }

    private GameObject IconPacksButton { get; set; }
    private GameObject SilhouetteSwapper { get; set; }
    private GameObject CloseButton { get; set; }
    private GameObject Settings { get; set; }

    public PackType Page { get; set; }

    public static FancyUI Instance { get; private set; }

    public void Awake()
    {
        Instance = this;

        IconPacksButton = transform.Find("IconPacks").gameObject;
        SilhouetteSwapper = transform.Find("SilhouetteSwapper").gameObject;
        CloseButton = transform.Find("CloseButton").gameObject;
        Settings = transform.Find("Settings").gameObject;

        var font = ApplicationController.ApplicationContext.FontControllerSource.fonts[0];
        GameFont = font.tmp_FontAsset;
        GameFontMaterial = font.standardFontMaterial;

        IconPacksButton.GetComponent<Button>().onClick.AddListener(OpenIP);
        IconPacksButton.AddComponent<TooltipTrigger>().NonLocalizedString = "Open Icon Packs Menu";

        SilhouetteSwapper.GetComponent<Button>().onClick.AddListener(OpenSS);
        SilhouetteSwapper.AddComponent<TooltipTrigger>().NonLocalizedString = "Open Silhouette Swapper Menu";

        Settings.GetComponent<Button>().onClick.AddListener(OpenSettings);
        Settings.AddComponent<TooltipTrigger>().NonLocalizedString = "Open Settings And Setting Menu";

        CloseButton.GetComponent<Button>().onClick.AddListener(gameObject.Destroy);
        CloseButton.AddComponent<TooltipTrigger>().NonLocalizedString = "Close Menu";

        SetupFonts(transform);
    }

    public void OnDestroy()
    {
        Instance = null;
        LoadingUI.Instance?.gameObject?.Destroy();
        DownloaderUI.Instance?.gameObject?.Destroy();
        SettingsAndTestingUI.Instance?.gameObject?.Destroy();
    }

    public static void SetupFonts(Transform trans)
    {
        foreach (var tmp in trans.GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            tmp.font = GameFont;
            tmp.fontMaterial = GameFontMaterial;
        }
    }

    private void OpenIP()
    {
        Page = PackType.IconPacks;

        if (DownloaderUI.Instance)
        {
            DownloaderUI.Instance.gameObject.SetActive(true);
            gameObject.SetActive(false);
            return;
        }

        OpenMenu();
    }

    private void OpenSS()
    {
        Page = PackType.SilhouetteSets;

        if (DownloaderUI.Instance)
        {
            DownloaderUI.Instance.gameObject.SetActive(true);
            gameObject.SetActive(false);
            return;
        }

        OpenMenu();
    }

    private void OpenSettings()
    {
        Page = PackType.Settings;

        if (SettingsAndTestingUI.Instance)
        {
            SettingsAndTestingUI.Instance.gameObject.SetActive(true);
            gameObject.SetActive(false);
            return;
        }

        var go = Instantiate(Fancy.Assets.GetGameObject("SettingsAndTestingUI"), transform.parent, false);
        go.transform.localPosition = new(0, 0, 0);
        go.transform.localScale = Vector3.one * 1.5f;
        go.transform.SetAsLastSibling();
        go.AddComponent<SettingsAndTestingUI>();
        gameObject.SetActive(false);
    }

    private void OpenMenu()
    {
        var go = Instantiate(Fancy.Assets.GetGameObject("DownloaderUI"), transform.parent, false);
        go.transform.localPosition = new(0, 0, 0);
        go.transform.localScale = Vector3.one * 1.5f;
        go.transform.SetAsLastSibling();
        go.AddComponent<DownloaderUI>();
        gameObject.SetActive(false);
    }
}