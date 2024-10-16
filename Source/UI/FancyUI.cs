using Home.Shared;

namespace FancyUI.UI;

public class FancyUI : UIController
{
    private static TMP_FontAsset GameFont;
    private static Material GameFontMaterial;

    private GameObject ColoredWoodButton;
    private GameObject IconPacksButton;
    private GameObject SilhouetteSwapper;
    private GameObject CloseButton;
    private GameObject Settings;

    public static FancyUI Instance { get; private set; }

    public void Awake()
    {
        Instance = this;

        ColoredWoodButton = transform.Find("ColoredWood").gameObject;
        IconPacksButton = transform.Find("IconPacks").gameObject;
        SilhouetteSwapper = transform.Find("SilhouetteSwapper").gameObject;
        CloseButton = transform.Find("CloseButton").gameObject;
        Settings = transform.Find("Settings").gameObject;

        var font = ApplicationController.ApplicationContext.FontControllerSource.fonts[0];
        GameFont = font.tmp_FontAsset;
        GameFontMaterial = font.standardFontMaterial;

        SetupFonts(transform);

        SetupMenu();
    }

    public void OnDestroy()
    {
        Instance = null;
        LoadingUI.Instance?.gameObject?.Destroy();
        IconPacksUI.Instance?.gameObject?.Destroy();
    }

    private void SetupMenu()
    {
        IconPacksButton.GetComponent<Button>().onClick.AddListener(OpenIP);
        IconPacksButton.AddComponent<TooltipTrigger>().NonLocalizedString = "Open Icon Packs Menu";

        // ColoredWoodButton.GetComponent<Button>().onClick.AddListener(OpenCW);
        ColoredWoodButton.AddComponent<TooltipTrigger>().NonLocalizedString = "Open Recoloured UI Menu";

        SilhouetteSwapper.GetComponent<Button>().onClick.AddListener(OpenSS);
        SilhouetteSwapper.AddComponent<TooltipTrigger>().NonLocalizedString = "Open Silhouette Swapper Menu";

        // Settings.GetComponent<Button>().onClick.AddListener(OpenSettings);
        Settings.AddComponent<TooltipTrigger>().NonLocalizedString = "Open Settings Menu";

        CloseButton.GetComponent<Button>().onClick.AddListener(gameObject.Destroy);
        CloseButton.AddComponent<TooltipTrigger>().NonLocalizedString = "Close Menu";
    }

    public static void SetupFonts(Transform trans)
    {
        foreach (var tmp in trans.GetComponentsInChildren<TextMeshProUGUI>())
        {
            tmp.font = GameFont;
            tmp.fontMaterial = GameFontMaterial;
        }
    }

    private void OpenIP()
    {
        if (IconPacksUI.Instance)
        {
            IconPacksUI.Instance.gameObject.SetActive(true);
            gameObject.SetActive(false);
            return;
        }

        OpenMenu<IconPacksUI>();
    }

    private void OpenSS()
    {
        if (SilhouetteSwapperUI.Instance)
        {
            SilhouetteSwapperUI.Instance.gameObject.SetActive(true);
            gameObject.SetActive(false);
            return;
        }

        OpenMenu<SilhouetteSwapperUI>();
    }

    private void OpenMenu<T>() where T : BaseUI
    {
        var go = Instantiate(Fancy.Instance.Assets.GetGameObject("DownloaderUI"), transform.parent, false);
        go.transform.localPosition = new(0, 0, 0);
        go.transform.localScale = Vector3.one * 1.5f;
        go.AddComponent<T>();
        SetupFonts(go.transform);
        gameObject.SetActive(false);
    }
}