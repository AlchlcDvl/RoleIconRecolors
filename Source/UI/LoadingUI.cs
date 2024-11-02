namespace FancyUI.UI;

public class LoadingUI : UIController
{
    private Transform Cog { get; set; }

    private GameObject Caller { get; set; }
    private GameObject CancelButton { get; set; }

    private TextMeshProUGUI Title { get; set; }
    public TextMeshProUGUI LoadingProgress { get; set; }

    private bool Started { get; set; }

    public static LoadingUI Instance { get; private set; }

    public void Awake()
    {
        Instance = this;

        Cog = transform.Find("Cog");
        CancelButton = transform.Find("Cancel").gameObject;
        Title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        LoadingProgress = transform.Find("Progress").GetComponent<TextMeshProUGUI>();

        Started = false;

        SetupMenu();
    }

    public void OnDestroy() => Instance = null;

    private void SetupMenu()
    {
        CancelButton.AddComponent<TooltipTrigger>().NonLocalizedString = "Cancel The Current Process";
        CancelButton.GetComponent<Button>().onClick.AddListener(Cancel);
    }

    private void Cancel()
    {
        SilhouetteSwapperUI.Instance.Abort = true;
        IconPacksUI.Instance.Abort = true;
    }

    public void Update()
    {
        if (Started)
            Cog.Rotate(Vector3.forward * -10 * Time.fixedDeltaTime);
    }

    public void BeginLoading(GameObject caller, string title)
    {
        Instance.gameObject.SetActive(true);
        Title.SetText($"Downloading {title}");
        Caller = caller;
        Caller.SetActive(false);
        Started = true;
    }

    public void Finish()
    {
        Caller.SetActive(true);
        Caller = null;
        IconPacksUI.Instance.Abort = false;
        Instance.gameObject.SetActive(false);
        Started = false;
    }

    public static void Begin(GameObject caller, string title)
    {
        if (!Instance)
        {
            var go = Instantiate(Fancy.Assets.GetGameObject("LoadingUI"), FancyUI.Instance.transform.parent, false);
            go.transform.localPosition = new(0, 0, 0);
            go.transform.localScale = Vector3.one * 1.5f;
            go.AddComponent<LoadingUI>();
            FancyUI.SetupFonts(go.transform);
        }

        Instance.BeginLoading(caller, title);
    }
}