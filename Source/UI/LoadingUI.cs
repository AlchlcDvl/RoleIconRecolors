namespace FancyUI.UI;

public class LoadingUI : UIController
{
    private Image Cog { get; set; }

    private GameObject Caller { get; set; }

    private TextMeshProUGUI Title { get; set; }
    public TextMeshProUGUI LoadingProgress { get; set; }

    private bool Started { get; set; }

    private Image Frame { get; set; }

    public static LoadingUI Instance { get; private set; }

    public void Awake()
    {
        Instance = this;

        Cog = transform.Find("Cog").GetComponent<Image>();
        Title = transform.GetComponent<TextMeshProUGUI>("Title");
        LoadingProgress = transform.GetComponent<TextMeshProUGUI>("Progress");

        Started = false;

        var cancel = transform.Find("Cancel");
        cancel.AddComponent<HoverEffect>()!.LookupKey = "FANCY_CANCEL_CURRENT";
        cancel.GetComponent<Button>().onClick.AddListener(Cancel);

        Frame = transform.Find("Panel").GetComponent<Image>();

        FancyUI.SetupFonts(transform);
    }

    public void OnEnable()
    {
        Frame.SetImageColor(ColorType.Metal);
        Cog.SetImageColor(ColorType.Metal);
    }

    public void OnDestroy() => Instance = null;

    private static void Cancel() => Coroutines.Stop(DownloaderUI.Instance.InProgress);

    public void Update()
    {
        if (Started)
            Cog.transform.Rotate(Vector3.forward * (-10 * Time.fixedDeltaTime));
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
        Instance.gameObject.SetActive(false);
        Started = false;
        DownloaderUI.Instance.InProgress = null;
    }

    public static void Begin(GameObject caller, string title)
    {
        if (!Instance)
        {
            var go = Instantiate(Fancy.Assets.GetGameObject("LoadingUI"), FancyUI.Instance.transform.parent, false);
            go.transform.localPosition = new(0, 0, 0);
            go.transform.localScale = Vector3.one * 1.5f;
            go.AddComponent<LoadingUI>();
        }

        Instance.BeginLoading(caller, title);
    }
}