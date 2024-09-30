namespace FancyUI;

public class LoadingUI : UIController
{
    private Transform Cog;
    private GameObject Caller;
    private GameObject CancelButton;
    private TextMeshProUGUI Title;
    private TextMeshProUGUI LoadingProgress;

    private bool Started;
    private int Max;
    private int Current;

    public static LoadingUI Instance { get; private set; }

    public void Start()
    {
        Instance = this;

        Cog = transform.Find("Cog");
        CancelButton = transform.Find("Cancel").gameObject;
        Title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        LoadingProgress = transform.Find("Progress").GetComponent<TextMeshProUGUI>();

        Started = false;
        Max = Current = 0;

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
        IconPacksUI.Instance.Abort = true;
    }

    public void Update()
    {
        if (Started)
            return;

        Cog.Rotate(Vector3.forward * 10 * Time.fixedDeltaTime);
        LoadingProgress.SetText($"Progress: {Current * 100 / Max}%");
    }

    public void UpdateProgress() => Current++;

    public void SetCurrent(int current) => Current = current;

    public void BeginLoading(GameObject caller, string title, int max)
    {
        Title.SetText($"Downloading {title}");
        Max = max;
        Caller = caller;
        Caller.SetActive(false);
    }

    public void Finish()
    {
        Max = Current = 0;
        Caller.SetActive(true);
        Caller = null;
        IconPacksUI.Instance.Abort = false;
    }

    public static void Begin(GameObject caller, string title, int max)
    {
        if (!Instance)
        {
            var go = Instantiate(AssetManager.AssetGOs["LoadingUI"], FancyUI.Instance.transform.parent, false);
            go.transform.localPosition = new(0, 0, 0);
            go.transform.localScale = Vector3.one * 1.5f;
            go.AddComponent<LoadingUI>();
            FancyUI.SetupFonts(go.transform);
        }

        Instance.BeginLoading(caller, title, max);
    }
}