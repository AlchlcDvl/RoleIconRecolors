namespace FancyUI.UI;

public class SettingsAndTestingUI : UIController
{
    public static SettingsAndTestingUI Instance { get; private set; }

    private CustomAnimator Animator { get; set; }
    private CustomAnimator Flame { get; set; }

    private RoleCardIcon IconTemplate { get; set; }

    private SliderSetting SliderTemplate { get; set; }
    private ColorSetting ColorTemplate { get; set; }
    private DropdownSetting DropdownTemplate { get; set; }
    private ToggleSetting ToggleTemplate { get; set; }
    private StringInputSetting InputTemplate { get; set; }

    private TextMeshProUGUI RoleText { get; set; }
    private TextMeshProUGUI NameText { get; set; }

    private Image ToggleImage { get; set; }
    private Image SlotCounter { get; set; }
    private Image RoleIcon { get; set; }
    private Image Attack { get; set; }
    private Image Defense { get; set; }
    private Image SpecialButtonImage { get; set; }
    private Image EffectButtonImage { get; set; }

    private readonly List<Image> Leathers = [];
    private readonly List<Image> Papers = [];
    private readonly List<Image> Metals = [];
    private readonly List<Image> Waxes = [];
    private readonly List<Image> Fires = [];
    private readonly List<Image> Woods = [];

    private readonly Dictionary<DisplayType, GameObject> Displays = [];

    private readonly List<RoleCardIcon> Icons = [];
    public readonly List<Setting> Settings = [];

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

        var roleCard = transform.FindRecursive("RoleCard");
        var playerList = transform.FindRecursive("PlayerList");
        var roleDeck = transform.FindRecursive("RoleDeck");
        var chat = transform.FindRecursive("Chat");
        var rlPlusGy = transform.FindRecursive("RlPlusGy");
        var roleList = transform.FindRecursive("RoleList");
        var graveyard = transform.FindRecursive("Graveyard");

        Displays[DisplayType.RoleCard] = roleCard.gameObject;
        Displays[DisplayType.PlayerList] = playerList.gameObject;
        Displays[DisplayType.RoleDeck] = roleDeck.gameObject;
        Displays[DisplayType.Chat] = chat.gameObject;
        Displays[DisplayType.RlPlusGy] = rlPlusGy.gameObject;
        Displays[DisplayType.RoleList] = roleList.gameObject;
        Displays[DisplayType.Graveyard] = graveyard.gameObject;

        IconTemplate = roleCard.EnsureComponent<RoleCardIcon>("ButtonTemplate");

        Flame = roleCard.EnsureComponent<CustomAnimator>("Fire")!;
        Flame.SetAnim([ .. FancyAssetManager.Flame.Frames.Select(x => x.RenderedSprite) ], 1f);

        RoleText = roleCard.GetComponent<TextMeshProUGUI>("Role");
        NameText = transform.GetComponent<TextMeshProUGUI>("NameText");

        SlotCounter = roleCard.GetComponent<Image>("SlotCounter");
        RoleIcon = roleCard.GetComponent<Image>("RoleIcon");
        Attack = roleCard.GetComponent<Image>("Attack");
        Defense = roleCard.GetComponent<Image>("Defense");

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
        ToggleImage.gameObject.SetActive(Constants.BTOS2Exists());

        transform.GetComponent<Button>("RecolouredUI")!.onClick.AddListener(() => Page = PackType.RecoloredUI);
        transform.GetComponent<Button>("IconPacks")!.onClick.AddListener(() => Page = PackType.IconPacks);
        transform.GetComponent<Button>("SilSwapper")!.onClick.AddListener(() => Page = PackType.SilhouetteSets);
        transform.GetComponent<Button>("MRC")!.onClick.AddListener(() => Page = PackType.MiscRoleCustomisation);
        transform.GetComponent<Button>("Testing")!.onClick.AddListener(() => Page = PackType.Testing);
        ToggleImage.GetComponent<Button>()!.onClick.AddListener(() => IsBTOS2 = !IsBTOS2);

        Metals.Add(roleCard.GetComponent<Image>("Special"));
        Metals.Add(roleCard.GetComponent<Image>("Effect"));
        Metals.Add(playerList.GetComponent<Image>("Corners"));
        Metals.Add(roleDeck.GetComponent<Image>("Metal"));
        Metals.Add(chat.GetComponent<Image>("Metal"));
        Metals.Add(chat.GetComponent<Image>("Nameplate"));
        Metals.Add(rlPlusGy.GetComponent<Image>("Metal"));
        Metals.Add(roleList.GetComponent<Image>("Metal"));
        Metals.Add(graveyard.GetComponent<Image>("Metal"));

        Woods.Add(roleCard.GetComponent<Image>("Frame"));
        Woods.Add(Metals[0].transform.GetComponent<Image>("Wood"));
        Woods.Add(Metals[1].transform.GetComponent<Image>("Wood"));
        Woods.Add(SlotCounter);
        Woods.Add(chat.transform.GetComponent<Image>("WoodFrame"));
        Woods.Add(chat.transform.GetComponent<Image>("WoodDetails1"));
        Woods.Add(chat.transform.GetComponent<Image>("WoodDetails2"));
        Woods.Add(chat.transform.GetComponent<Image>("ChatBottom"));
        Woods.Add(rlPlusGy.transform.GetComponent<Image>("Wood"));
        Woods.Add(roleList.transform.GetComponent<Image>("Wood"));
        Woods.Add(graveyard.transform.GetComponent<Image>("Wood"));

        Leathers.Add(playerList.GetComponent<Image>("Leather"));
        Leathers.Add(roleDeck.GetComponent<Image>("Leather"));

        Waxes.Add(playerList.GetComponent<Image>("Tab"));
        Waxes.Add(chat.GetComponent<Image>("Wax"));

        Papers.Add(playerList.GetComponent<Image>("Paper"));
        Papers.Add(roleDeck.GetComponent<Image>("Paper"));
        Papers.Add(chat.GetComponent<Image>("SendPaper"));

        Fires.Add(Flame.Renderer);

        FancyUI.SetupFonts(transform);

        foreach (var opt in Option.All)
        {
            switch (opt)
            {
                case FloatOption slider:
                {
                    slider.Setting = Instantiate(SliderTemplate, SliderTemplate!.transform.parent);
                    slider.Setting.Option = slider;
                    break;
                }
                case IDropdown dropdown:
                {
                    dropdown.Setting = Instantiate(DropdownTemplate, DropdownTemplate!.transform.parent);
                    dropdown.Setting.Option = dropdown;
                    break;
                }
                case ColorOption color:
                {
                    color.Setting = Instantiate(ColorTemplate, ColorTemplate!.transform.parent);
                    color.Setting.Option = color;
                    break;
                }
                case StringInputOption input:
                {
                    input.Setting = Instantiate(InputTemplate, InputTemplate!.transform.parent);
                    input.Setting.Option = input;
                    break;
                }
                case ToggleOption toggle:
                {
                    toggle.Setting = Instantiate(ToggleTemplate, ToggleTemplate!.transform.parent);
                    toggle.Setting.Option = toggle;
                    break;
                }
            }
        }

        SliderTemplate!.gameObject.SetActive(false);
        DropdownTemplate!.gameObject.SetActive(false);
        ColorTemplate!.gameObject.SetActive(false);
        InputTemplate!.gameObject.SetActive(false);
        ToggleTemplate!.gameObject.SetActive(false);
    }

    public void Start() => RefreshOptions();

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
        Leathers.ForEach(x => x.SetImageColor(ColorType.Leather));
        Metals.ForEach(x => x.SetImageColor(ColorType.Metal));
        Woods.ForEach(x => x.SetImageColor(ColorType.Wood));
        Papers.ForEach(x => x.SetImageColor(ColorType.Paper));
        Waxes.ForEach(x => x.SetImageColor(ColorType.Wax));
        Fires.ForEach(x => x.SetImageColor(ColorType.Fire, 0.7f));
        Displays.ForEach((x, y) => y.SetActive(Fancy.SelectDisplay.Value == x));
        Icons.ForEach(x => x.UpdateIcon(Fancy.SelectTestingRole.Value));
        ToggleImage.sprite = Fancy.Assets.GetSprite($"{(IsBTOS2 ? "B" : "")}ToS2Icon");

        foreach (var setting in Settings)
        {
            setting.Refresh();
            setting.gameObject.SetActive(setting.SetActive());
        }
    }
}