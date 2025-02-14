namespace FancyUI.UI;

public class LoadingUI : UIController
{
    private Transform Cog { get; set; }

    private GameObject Caller { get; set; }

    private TextMeshProUGUI Title { get; set; }
    public TextMeshProUGUI LoadingProgress { get; set; }

    private bool Started { get; set; }

    public static LoadingUI Instance { get; private set; }

    public void Awake()
    {
        Instance = this;

        Cog = transform.Find("Cog");
        Title = transform.GetComponent<TextMeshProUGUI>("Title");
        LoadingProgress = transform.GetComponent<TextMeshProUGUI>("Progress");

        Started = false;

        var cancel = transform.Find("Cancel");
        cancel.AddComponent<HoverEffect>()!.LookupKey = "FANCY_CANCEL_CURRENT";
        cancel.GetComponent<Button>().onClick.AddListener(Cancel);

        FancyUI.SetupFonts(transform);
    }

    public void OnDestroy() => Instance = null;

    private void Cancel() => DownloaderUI.Instance.Abort = true;

    public void Update()
    {
        if (Started)
            Cog.Rotate(Vector3.forward * (-10 * Time.fixedDeltaTime));
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
        DownloaderUI.Instance.Abort = true;
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
        }

        Instance.BeginLoading(caller, title);
    }
}