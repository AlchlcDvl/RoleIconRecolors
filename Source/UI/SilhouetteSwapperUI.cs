namespace FancyUI.UI;

public class SilhouetteSwapperUI : BaseUI
{
    public static readonly List<PackJson> Packs = [];

    public override string Type => "Silhouette Set";
    public override string Path => SSPath;

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
        Test.GetComponent<Image>().sprite = Fancy.Assets.GetSprite("SilSwapper");
        Packs.ForEach(x => SetUpPack(x, () => {}));
        PackTemplate.SetActive(false);
        NoPacks.SetActive(Packs.Count == 0);
    }

    public void OnDestroy()
    {
        Instance = null;
        SilhouetteSetTestingUI.Instance?.gameObject?.Destroy();
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

        var go = Instantiate(Fancy.Assets.GetGameObject("SilhouetteSetTestingUI"), transform.parent);
        go.transform.localPosition = new(0, 0, 0);
        go.transform.localScale = Vector3.one * 1.5f;
        go.AddComponent<SilhouetteSetTestingUI>();
        FancyUI.SetupFonts(go.transform);
        gameObject.SetActive(false);
    }
}