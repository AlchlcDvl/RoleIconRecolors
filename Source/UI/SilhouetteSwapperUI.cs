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
        Packs.ForEach(x => SetUpPack(x, () => {}));
        PackTemplate.SetActive(false);
        NoPacks.SetActive(Packs.Count == 0);
    }

    public void OnDestroy() => Instance = null;

    public override void AfterGenerating()
    {
    }
}