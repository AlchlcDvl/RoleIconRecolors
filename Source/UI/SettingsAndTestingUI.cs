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
    private Transform BookPanel { get; set; }

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

    private Image BookPaper { get; set; }
    private Image BookLeather { get; set; }
    private Image BookCorners { get; set; }
    private Image PlayerPanelButton { get; set; }

    private Image SwapIcon { get; set; }

    private bool isBTOS2;
    public bool IsBTOS2
    {
        get => isBTOS2;
        set
        {
            isBTOS2 = value;
            RefreshOptions();
        }
    }

    private bool otherUI;
    private bool OtherUI
    {
        get => otherUI;
        set
        {
            otherUI = value;
            RefreshOptions();
        }
    }

    private PackType page;
    public PackType Page
    {
        get => page;
        set
        {
            page = value;
            RefreshOptions();
        }
    }

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

    public void Awake()
    {
        Instance = this;

        Animator = transform.EnsureComponent<CustomAnimator>("Animator")!;
        Animator.SetAnim(Loading.Frames, Constants.AnimationDuration());
        Animator.AddComponent<HoverEffect>()!.NonLocalizedString = "This Is Your Animator";

        RoleCard = transform.FindRecursive("RoleCard");
        BookPanel = transform.FindRecursive("Book");

        ButtonTemplate = RoleCard.EnsureComponent<RoleCardIcon>("ButtonTemplate");

        SpecialMetal = RoleCard.GetComponent<Image>("Special")!;
        EffectMetal = RoleCard.GetComponent<Image>("Effect")!;
        SpecialWood = SpecialMetal.transform.GetComponent<Image>("Wood");
        EffectWood = EffectMetal.transform.GetComponent<Image>("Wood");

        BookLeather = BookPanel.GetComponent<Image>("Leather")!;
        BookCorners = BookPanel.GetComponent<Image>("Corners")!;
        PlayerPanelButton = BookPanel.GetComponent<Image>("Tab")!;
        BookPaper = BookPanel.GetComponent<Image>()!;

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

        transform.GetComponent<Button>("RecolouredUI")!.onClick.AddListener(() => Page = PackType.RecoloredUI);
        transform.GetComponent<Button>("IconPacks")!.onClick.AddListener(() => Page = PackType.IconPacks);
        transform.GetComponent<Button>("SilSwapper")!.onClick.AddListener(() => Page = PackType.SilhouetteSets);
        transform.GetComponent<Button>("MRC")!.onClick.AddListener(() => Page = PackType.MiscRoleCustomisation);
        transform.GetComponent<Button>("Testing")!.onClick.AddListener(() => Page = PackType.Testing);
        transform.GetComponent<Button>("Toggle")!.onClick.AddListener(() => IsBTOS2 = !IsBTOS2);

        var swap =  transform.GetComponent<Button>("Swap")!;
        swap.onClick.AddListener(() => OtherUI = !OtherUI);
        SwapIcon = swap.GetComponent<Image>();

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
        Animator.SetDuration(Constants.AnimationDuration());
        NameText.SetText(DefaultNameText.Replace("%num%", $"{Constants.PlayerNumber()}"));
        NameText.SetGraphicColor(ColorType.Paper);
        RoleText.SetText(DefaultRoleText.Replace("%type%", $"{Utils.FactionName(Constants.GetSelectedFaction(), IsBTOS2 ? ModType.BTOS2 : ModType.Vanilla)}").Replace("%mod%", IsBTOS2 ? "BTOS" :
            "").Replace("%roleName%", "Admirer").Replace("%roleInt%", "1"));
        SpecialMetal.SetImageColor(ColorType.Metal);
        EffectMetal.SetImageColor(ColorType.Metal);
        SpecialWood.SetImageColor(ColorType.Wood);
        EffectWood.SetImageColor(ColorType.Wood);
        SlotCounter.SetImageColor(ColorType.Wood);
        Frame.SetImageColor(ColorType.Wood);
        BookCorners.SetImageColor(ColorType.Metal);
        BookLeather.SetImageColor(ColorType.Leather);
        BookPaper.SetImageColor(ColorType.Paper);
        PlayerPanelButton.SetImageColor(ColorType.Wax);
        Flame.Renderer.SetImageColor(ColorType.Fire, a: 0.7f);
        ToggleImage.sprite = Fancy.Assets.GetSprite($"{(IsBTOS2 ? "B" : "")}ToS2Icon");
        SwapIcon.material.SetFloat("_FlipX", OtherUI ? 1 : 0);
        BookPanel.gameObject.SetActive(OtherUI);
        RoleCard.gameObject.SetActive(!OtherUI);

        foreach (var setting in Settings)
        {
            setting.Refresh();
            setting.gameObject.SetActive(setting.SetActive());
        }
    }
}