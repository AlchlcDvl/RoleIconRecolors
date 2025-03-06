namespace FancyUI.UI;

public class SettingsAndTestingUI : UIController
{
    public static SettingsAndTestingUI Instance { get; private set; }

    private CustomAnimator Animator { get; set; }
    private CustomAnimator Flame { get; set; }

    private RoleCardIcon ButtonTemplate { get; set; }

    private SliderSetting SliderTemplate { get; set; }
    private ColorSetting ColorTemplate { get; set; }
    private DropdownSetting DropdownTemplate { get; set; }
    private ToggleSetting ToggleTemplate { get; set; }
    private StringInputSetting InputTemplate { get; set; }

    private Transform RoleCard { get; set; }

    private TextMeshProUGUI RoleText { get; set; }
    private TextMeshProUGUI NameText { get; set; }

    private Image ToggleImage { get; set; }
    private Image SlotCounter { get; set; }
    private Image RoleIcon { get; set; }
    private Image Attack { get; set; }
    private Image Defense { get; set; }
    private Image SpecialButtonImage { get; set; }
    private Image EffectButtonImage { get; set; }
    private Image Frame { get; set; }
    private Image SpecialWood { get; set; }
    private Image SpecialMetal { get; set; }
    private Image EffectWood { get; set; }
    private Image EffectMetal { get; set; }

    private bool IsBTOS2 { get; set; }

    public PackType Page { get; set; }

    private readonly List<GameObject> ButtonGOs = [];
    private readonly List<Image> ButtonImages = [];
    private readonly List<Sprite> ButtonSprites = [];

    public readonly List<Setting> Settings = [];

    private static readonly Vector3 SpecialOrEffectButtonPosition = new(0f, -20f, 0f);
    private static readonly Vector3[][] ButtonPositions =
    [
        [ new(0f, -182f, 0f) ],
        [ new(-24f, -190f, 0f), new(24f, -190f, 0f) ],
        [ new(-45f, -190f, 0f), new(0f, -188f, 0f), new(45f, -190f, 0f) ],
        [ new(-72f, -180f, 0f), new(-24f, -190f, 0f), new(-24f, -190f, 0f), new(72f, -180f, 0f) ]
    ];

    private const string DefaultRoleText = "<sprite=\"%mod%RoleIcons (%type%)\" name=\"Role%roleInt%\"><b>%roleName%</b>";
    private const string DefaultNameText = "<sprite=\"PlayerNumbers\" name=\"PlayerNumbers_%num%\"><b>Giles Corey</b>";
    private const string DefaultType = "Regular";
    private const string DefaultRoleInt = "241";
    private const string DefaultRoleName = "Hidden";

    public void Awake()
    {
        Instance = this;

        Animator = transform.EnsureComponent<CustomAnimator>("Animator")!;
        Animator.SetAnim(Loading.Frames, Constants.AnimationDuration());
        Animator.AddComponent<HoverEffect>()!.NonLocalizedString = "This Is Your Animator";

        RoleCard = transform.FindRecursive("RoleCard");

        ButtonTemplate = RoleCard.EnsureComponent<RoleCardIcon>("ButtonTemplate");

        SpecialMetal = RoleCard.GetComponent<Image>("Special")!;
        EffectMetal = RoleCard.GetComponent<Image>("Effect")!;
        SpecialWood = SpecialMetal.transform.GetComponent<Image>("Wood");
        EffectWood = EffectMetal.transform.GetComponent<Image>("Wood");

        Flame = SpecialMetal.transform.EnsureComponent<CustomAnimator>("Fire")!;
        Flame.SetAnim([ .. FancyAssetManager.Flame.Frames.Select(x => x.RenderedSprite) ], 1f);

        RoleText = RoleCard.GetComponent<TextMeshProUGUI>("Role");
        NameText = transform.GetComponent<TextMeshProUGUI>("NameText");

        SlotCounter = RoleCard.GetComponent<Image>("SlotCounter");
        RoleIcon = RoleCard.GetComponent<Image>("RoleIcon");
        Attack = RoleCard.GetComponent<Image>("Attack");
        Defense = RoleCard.GetComponent<Image>("Defense");
        Frame = RoleCard.GetComponent<Image>("Frame");

        SliderTemplate = transform.EnsureComponent<SliderSetting>("SliderTemplate");
        ColorTemplate = transform.EnsureComponent<ColorSetting>("ColorTemplate");
        DropdownTemplate = transform.EnsureComponent<DropdownSetting>("DropdownTemplate");
        ToggleTemplate = transform.EnsureComponent<ToggleSetting>("ToggleTemplate");
        InputTemplate = transform.EnsureComponent<StringInputSetting>("InputTemplate");

        var back = transform.FindRecursive("Back").gameObject;
        back.GetComponent<Button>().onClick.AddListener(GoBack);
        var hover = back.AddComponent<HoverEffect>();
        hover.LookupKey = "FANCY_CLOSE_MENU";
        hover.FillInKeys = [ ("%type%", "Testing") ];

        ToggleImage = transform.GetComponent<Image>("Toggle");

        transform.GetComponent<Button>("RecolouredUI")!.onClick.AddListener(OpenRecoloredUI);
        transform.GetComponent<Button>("IconPacks")!.onClick.AddListener(OpenIconPacks);
        transform.GetComponent<Button>("SilSwapper")!.onClick.AddListener(OpenSilSwapper);
        transform.GetComponent<Button>("MRC")!.onClick.AddListener(OpenMrc);
        transform.GetComponent<Button>("Toggle")!.onClick.AddListener(OpenTesting);
        transform.GetComponent<Button>("Testing")!.onClick.AddListener(DoTheToggle);

        FancyUI.SetupFonts(transform);

        foreach (var opt in Option.All)
        {
            if (opt is FloatOption slider)
            {
                slider.Setting = Instantiate(SliderTemplate, SliderTemplate.transform.parent);
                slider.Setting.Option = slider;
            }
            else if (opt is IDropdown dropdown)
            {
                dropdown.Setting = Instantiate(DropdownTemplate, DropdownTemplate.transform.parent);
                dropdown.Setting.Option = dropdown;
            }
            else if (opt is ColorOption color)
            {
                color.Setting = Instantiate(ColorTemplate, ColorTemplate.transform.parent);
                color.Setting.Option = color;
            }
            else if (opt is StringInputOption input)
            {
                input.Setting = Instantiate(InputTemplate, InputTemplate.transform.parent);
                input.Setting.Option = input;
            }
            else if (opt is ToggleOption toggle)
            {
                toggle.Setting = Instantiate(ToggleTemplate, ToggleTemplate.transform.parent);
                toggle.Setting.Option = toggle;
            }
        }

        SliderTemplate.gameObject.SetActive(false);
        DropdownTemplate.gameObject.SetActive(false);
        ColorTemplate.gameObject.SetActive(false);
        InputTemplate.gameObject.SetActive(false);
        ToggleTemplate.gameObject.SetActive(false);

        Page = PackType.RecoloredUI;

        RefreshOptions();
    }

    public void OnEnable() => RefreshOptions();

    public void OnDestroy() => Instance = null;

    public void GoBack()
    {
        gameObject.SetActive(false);
        FancyUI.Instance.gameObject.SetActive(true);
    }

    public void RefreshOptions()
    {
        Settings.ForEach(x => x.gameObject.SetActive(x.SetActive()));
        Animator.SetDuration(Constants.AnimationDuration());
        NameText.SetText(DefaultNameText.Replace("%num%", $"{Constants.PlayerNumber()}"));
        SpecialMetal.SetImageColor(ColorType.Metal);
        EffectMetal.SetImageColor(ColorType.Metal);
        SpecialWood.SetImageColor(ColorType.Wood);
        EffectWood.SetImageColor(ColorType.Wood);
        SlotCounter.SetImageColor(ColorType.Wood);
        Frame.SetImageColor(ColorType.Wood);
        Flame.Renderer.SetImageColor(ColorType.Flame, a: 0.75f);
    }

    private void OpenRecoloredUI()
    {
        Page = PackType.RecoloredUI;
        RefreshOptions();
    }

    private void OpenIconPacks()
    {
        Page = PackType.IconPacks;
        RefreshOptions();
    }

    private void OpenSilSwapper()
    {
        Page = PackType.SilhouetteSets;
        RefreshOptions();
    }

    private void OpenTesting()
    {
        Page = PackType.Settings;
        RefreshOptions();
    }

    private void OpenMrc()
    {
        Page = PackType.MiscRoleCustomisation;
        RefreshOptions();
    }

    private void DoTheToggle()
    {
        IsBTOS2 = !IsBTOS2;
        ToggleImage.sprite = Fancy.Assets.GetSprite($"{(IsBTOS2 ? "B" : "")}ToS2Icon");
    }
}