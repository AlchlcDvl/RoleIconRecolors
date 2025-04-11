using Home.Shared;

namespace FancyUI.UI;

public class FancyUI : UIController
{
    private static TMP_FontAsset GameFont { get; set; }
    private static Material GameFontMaterial { get; set; }

    public PackType Page { get; set; }

    public static FancyUI Instance { get; private set; }

    public void Awake()
    {
        Instance = this;

        var font = ApplicationController.ApplicationContext.FontControllerSource.fonts[0];
        GameFont = font.tmp_FontAsset;
        GameFontMaterial = font.standardFontMaterial;

        var ipButton = transform.Find("IconPacks");
        ipButton.GetComponent<Button>().onClick.AddListener(OpenIP);
        ipButton.AddComponent<HoverEffect>()!.LookupKey = "FANCY_OPEN_ICONPACKS";

        var ssButton = transform.Find("SilhouetteSwapper");
        ssButton.GetComponent<Button>().onClick.AddListener(OpenSS);
        ssButton.AddComponent<HoverEffect>()!.LookupKey = "FANCY_OPEN_SILHOUETTESETS";

        var settingsButton = transform.Find("Settings");
        settingsButton.GetComponent<Button>().onClick.AddListener(OpenSettings);
        settingsButton.AddComponent<HoverEffect>()!.LookupKey = "FANCY_OPEN_SETTINGS";

        var close = transform.Find("CloseButton");
        close.GetComponent<Button>().onClick.AddListener(gameObject.Destroy);
        close.AddComponent<HoverEffect>()!.LookupKey = "FANCY_CLOSE_FANCY";

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
        foreach (var tmp in trans.GetComponentsInChildren<TMP_Text>(true))
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
        Page = PackType.Testing;

        if (SettingsAndTestingUI.Instance)
        {
            SettingsAndTestingUI.Instance.gameObject.SetActive(true);
            gameObject.SetActive(false);
            return;
        }

        var go = Instantiate(Fancy.Assets.GetGameObject("SettingsAndTestingUI"), transform.parent, false);
        go.transform.localPosition = new(0, 0, 0);
        go.transform.localScale = Vector3.one * 1.5f;
        go.AddComponent<SettingsAndTestingUI>();
        gameObject.SetActive(false);
    }

    private void OpenMenu()
    {
        var go = Instantiate(Fancy.Assets.GetGameObject("DownloaderUI"), transform.parent, false);
        go.transform.localPosition = new(0, 0, 0);
        go.transform.localScale = Vector3.one * 1.5f;
        go.AddComponent<DownloaderUI>();
        gameObject.SetActive(false);
    }
}