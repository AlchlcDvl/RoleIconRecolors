namespace FancyUI.UI;

public class SettingsAndTestingUI : UIController
{
    public static SettingsAndTestingUI Instance { get; private set; }

    private CustomImageAnimator Animator { get; set; }
    private CustomImageAnimator Flame { get; set; }

    // private RoleCardIcon IconTemplate { get; set; }

    private SliderSetting SliderTemplate { get; set; }
    private ColorSetting ColorTemplate { get; set; }
    private DropdownSetting DropdownTemplate { get; set; }
    private ToggleSetting ToggleTemplate { get; set; }
    private StringInputSetting InputTemplate { get; set; }

    private TextMeshProUGUI RoleText { get; set; }
    private TextMeshProUGUI NameText { get; set; }

    private Image ToggleImage { get; set; }
    private Image SlotCounter { get; set; }
    // private Image RoleIcon { get; set; }
    // private Image Attack { get; set; }
    // private Image Defense { get; set; }
    // private Image SpecialButtonImage { get; set; }
    // private Image EffectButtonImage { get; set; }

    private readonly List<Image> Leathers = [];
    private readonly List<Image> Papers = [];
    private readonly List<Image> Metals = [];
    private readonly List<Image> Waxes = [];
    private readonly List<Image> Fires = [];
    private readonly List<Image> Woods = [];

    private readonly Dictionary<DisplayType, GameObject> Displays = [];

    // private readonly List<RoleCardIcon> Icons = [];
    public readonly List<Setting> Settings = [];

    private bool isBTOS2;
    public bool IsBTOS2
    {
        get => isBTOS2;
        set
        {
            isBTOS2 = value;
            Refresh();
        }
    }

    private PackType page;
    public PackType Page
    {
        get => page;
        set
        {
            page = value;
            Refresh();
        }
    }

    // private static readonly Vector3 SpecialOrEffectButtonPosition = new(0f, -20f, 0f);
    // private static readonly Vector3[][] ButtonPositions =
    // [
    //     [ new(0f, -182f, 0f) ],
    //     [ new(-24f, -190f, 0f), new(24f, -190f, 0f) ],
    //     [ new(-45f, -190f, 0f), new(0f, -188f, 0f), new(45f, -190f, 0f) ],
    //     [ new(-72f, -180f, 0f), new(-24f, -190f, 0f), new(-24f, -190f, 0f), new(72f, -180f, 0f) ]
    // ];

    private const string DefaultRoleText = "<sprite=\"%mod%RoleIcons (%type%)\" name=\"Role%roleInt%\"><b>%roleName%</b>";
    private const string DefaultNameText = "<sprite=\"PlayerNumbers\" name=\"PlayerNumbers_%num%\"><b>Giles Corey</b>";

    public void Awake()
    {
        Instance = this;

        Animator = transform.EnsureComponent<CustomImageAnimator>("Animator")!;
        Animator.SetAnim(Loading.Frames, Constants.AnimationDuration());
        Animator.AddComponent<HoverEffect>()!.NonLocalizedString = "This Is Your Animator";

        var roleCard = transform.FindRecursive("RoleCard");
        var playerList = transform.FindRecursive("PlayerList");
        var roleDeck = transform.FindRecursive("RoleDeck");
        var chat = transform.FindRecursive("Chat");
        var rlPlusGy = transform.FindRecursive("RlPlusGy");
        var roleList = transform.FindRecursive("RoleList");
        var graveyard = transform.FindRecursive("Graveyard");
        var specialAbilityPopup = transform.FindRecursive("SpecialAbilityPopup");

        Displays[DisplayType.RoleCard] = roleCard.gameObject;
        Displays[DisplayType.PlayerList] = playerList.gameObject;
        Displays[DisplayType.RoleDeck] = roleDeck.gameObject;
        Displays[DisplayType.Chat] = chat.gameObject;
        Displays[DisplayType.RlPlusGy] = rlPlusGy.gameObject;
        Displays[DisplayType.RoleList] = roleList.gameObject;
        Displays[DisplayType.Graveyard] = graveyard.gameObject;
        Displays[DisplayType.SpecialAbilityPopup] = specialAbilityPopup.gameObject;

        // IconTemplate = roleCard.EnsureComponent<RoleCardIcon>("ButtonTemplate");

        Flame = roleCard.EnsureComponent<CustomImageAnimator>("Fire")!;
        Flame.SetAnim([.. FancyAssetManager.Flame.Frames.Select(x => x.RenderedSprite)], 1f);

        RoleText = roleCard.GetComponent<TextMeshProUGUI>("Role");
        NameText = transform.GetComponent<TextMeshProUGUI>("NameText");

        SlotCounter = roleCard.GetComponent<Image>("SlotCounter");
        // RoleIcon = roleCard.GetComponent<Image>("RoleIcon");
        // Attack = roleCard.GetComponent<Image>("Attack");
        // Defense = roleCard.GetComponent<Image>("Defense");

        SliderTemplate = transform.EnsureComponent<SliderSetting>("SliderTemplate");
        ColorTemplate = transform.EnsureComponent<ColorSetting>("ColorTemplate");
        DropdownTemplate = transform.EnsureComponent<DropdownSetting>("DropdownTemplate");
        ToggleTemplate = transform.EnsureComponent<ToggleSetting>("ToggleTemplate");
        InputTemplate = transform.EnsureComponent<StringInputSetting>("InputTemplate");

        var back = transform.FindRecursive("Back").gameObject;
        back.GetComponent<Button>().onClick.AddListener(GoBack);
        var hover = back.AddComponent<HoverEffect>();
        hover.LookupKey = "FANCY_CLOSE_MENU";
        hover.FillInKeys = [("%type%", "Testing")];

        ToggleImage = transform.GetComponent<Image>("Toggle")!;
        ToggleImage.gameObject.SetActive(Constants.BTOS2Exists());

        transform.GetComponent<Button>("RecolouredUI")!.onClick.AddListener(() => Page = PackType.RecoloredUI);
        transform.GetComponent<Button>("IconPacks")!.onClick.AddListener(() => Page = PackType.IconPacks);
        transform.GetComponent<Button>("SilSwapper")!.onClick.AddListener(() => Page = PackType.SilhouetteSets);
        transform.GetComponent<Button>("MRC")!.onClick.AddListener(() => Page = PackType.MiscRoleCustomisation);
        var testing = transform.GetComponent<Button>("Testing");
        testing!.onClick.AddListener(() => Page = PackType.Testing);
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
        Metals.Add(back.GetComponent<Image>());
        Metals.Add(testing.targetGraphic as Image);
        Metals.Add(transform.GetComponent<Image>("Fill"));
        Metals.Add(specialAbilityPopup.GetComponent<Image>("Ring"));

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
        Woods.Add(specialAbilityPopup.GetComponent<Image>("Frame"));

        Leathers.Add(playerList.GetComponent<Image>("Leather"));
        Leathers.Add(roleDeck.GetComponent<Image>("Leather"));

        Waxes.Add(playerList.GetComponent<Image>("Tab"));
        Waxes.Add(chat.GetComponent<Image>("Wax"));
        Waxes.Add(transform.GetComponent<Image>("Wax"));

        Papers.Add(playerList.GetComponent<Image>("Paper"));
        Papers.Add(roleDeck.GetComponent<Image>("Paper"));
        Papers.Add(chat.GetComponent<Image>("SendPaper"));

        Fires.Add(Flame.Renderer);
        Fires.Add(specialAbilityPopup.GetComponent<Image>("Fire"));

        FancyUI.SetupFonts(transform);

        foreach (var opt in Option.All)
        {
            var setting = opt.BoxedSetting = Instantiate((Setting)(opt switch
            {
                FloatOption => SliderTemplate,
                IDropdown => DropdownTemplate,
                ColorOption => ColorTemplate,
                StringInputOption => InputTemplate,
                _ => ToggleTemplate,
            }), SliderTemplate!.transform.parent);
            setting.BoxedOption = opt;
            Metals.Add(setting.Background);

            switch (opt)
            {
                case FloatOption slider:
                {
                    Metals.Add(slider.Setting.Input.GetComponent<Image>());
                    Metals.Add(slider.Setting.Slider.targetGraphic as Image);
                    break;
                }
                case IDropdown dropdown:
                {
                    Metals.Add(dropdown.Setting.Dropdown.GetComponent<Image>());
                    Metals.Add(dropdown.Setting.Dropdown.transform.GetComponent<Image>("Arrow"));
                    Woods.Add(dropdown.Setting.Dropdown.transform.FindRecursive("Template").Find("Background").GetComponent<Image>());
                    break;
                }
                case StringInputOption input:
                {
                    Metals.Add(input.Setting.Input.GetComponent<Image>());
                    break;
                }
            }
        }

        SliderTemplate!.gameObject.SetActive(false);
        DropdownTemplate!.gameObject.SetActive(false);
        ColorTemplate!.gameObject.SetActive(false);
        InputTemplate!.gameObject.SetActive(false);
        ToggleTemplate!.gameObject.SetActive(false);

        Leathers.ForEach(x => x.SetImageColor(ColorType.Leather));
        Metals.ForEach(x => x.SetImageColor(ColorType.Metal));
        Woods.ForEach(x => x.SetImageColor(ColorType.Wood));
        Papers.ForEach(x => x.SetImageColor(ColorType.Paper));
        Waxes.ForEach(x => x.SetImageColor(ColorType.Wax));
        Fires.ForEach(x => x.SetImageColor(ColorType.Fire));

        Refresh();
    }

    public void OnDestroy() => Instance = null;

    public void GoBack()
    {
        gameObject.SetActive(false);
        FancyUI.Instance.gameObject.SetActive(true);
        Utils.UpdateMaterials(skipFactionCheck: true);
    }

    public void Refresh()
    {
        Animator.SetDuration(Constants.AnimationDuration());
        NameText.SetText(DefaultNameText.Replace("%num%", $"{Constants.PlayerNumber()}"));
        NameText.SetGraphicColor(ColorType.Paper);
        RoleText.SetText(DefaultRoleText.Replace("%type%", $"{Utils.FactionName(Constants.GetSelectedFaction(), IsBTOS2 ? GameModType.BTOS2 : GameModType.Vanilla)}").Replace("%mod%", IsBTOS2 ?
            "BTOS" : "").Replace("%roleName%", "Admirer").Replace("%roleInt%", "1"));
        Displays.ForEach((x, y) => y.SetActive(Fancy.SelectDisplay.Value == x));
        // Icons.ForEach(x => x.UpdateIcon(Fancy.SelectTestingRole.Value));
        ToggleImage.sprite = Fancy.Instance.Assets.GetSprite($"{(IsBTOS2 ? "B" : "")}ToS2Icon");

        foreach (var setting in Settings)
        {
            setting.gameObject.SetActive(false);

            if (setting.SetActive())
            {
                setting.Refresh();
                setting.gameObject.SetActive(true);
            }
        }

        Utils.UpdateMaterials();
    }
}