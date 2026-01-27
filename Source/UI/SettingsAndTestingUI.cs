namespace FancyUI.UI;

public sealed class SettingsAndTestingUI : UISingleton<SettingsAndTestingUI>
{
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

    private SlotCounterHandler SlotCounter { get; set; }

    private Image ToggleImage { get; set; }
    // private Image RoleIcon { get; set; }
    // private Image Attack { get; set; }
    // private Image Defense { get; set; }
    // private Image SpecialButtonImage { get; set; }
    // private Image EffectButtonImage { get; set; }

    private Image FilterIcon { get; set; }
    private Image FilterArrow { get; set; }
    private GameObject FilterDropdown { get; set; }
    private Material FilterMaterialDefault { get; set; }

    private readonly List<Image> Leathers = [];
    private readonly List<Image> Papers = [];
    private readonly List<Image> Metals = [];
    private readonly List<Image> Waxes = [];
    private readonly List<Image> Fires = [];
    private readonly List<Image> Woods = [];

    private readonly Dictionary<DisplayType, GameObject> Displays = [];

    // private readonly List<RoleCardIcon> Icons = [];

    private bool isBTOS2;
    public bool IsBTOS2
    {
        get => isBTOS2;
        private set
        {
            isBTOS2 = value;
            Refresh();
        }
    }

    private PackType page;
    public PackType Page
    {
        get => page;
        private set
        {
            page = value;
            Refresh();
        }
    }

    private const string DefaultRoleText = "<sprite=\"%mod%RoleIcons (%type%)\" name=\"Role%roleInt%\"><b>%roleName%</b>";
    private const string DefaultNameText = "<sprite=\"PlayerNumbers\" name=\"PlayerNumbers_%num%\"><b>Giles Corey %roleName%</b>";

    public void Awake()
    {
        _instance = this;

        Animator = transform.EnsureComponent<CustomImageAnimator>("Animator")!;
        Animator.SetAnim(Loading.Frames, Fancy.AnimationDuration.Value);
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

        SlotCounter = roleCard.EnsureComponent<SlotCounterHandler>("SlotCounters");
        // RoleIcon = roleCard.GetComponent<Image>("RoleIcon");
        // Attack = roleCard.GetComponent<Image>("Attack");
        // Defense = roleCard.GetComponent<Image>("Defense");

        SliderTemplate = transform.EnsureComponent<SliderSetting>("SliderTemplate");
        ColorTemplate = transform.EnsureComponent<ColorSetting>("ColorTemplate");
        DropdownTemplate = transform.EnsureComponent<DropdownSetting>("DropdownTemplate");
        ToggleTemplate = transform.EnsureComponent<ToggleSetting>("ToggleTemplate");
        InputTemplate = transform.EnsureComponent<StringInputSetting>("InputTemplate");
        var optionParent = SliderTemplate!.transform.parent;

        var back = transform.FindRecursive("Back").gameObject;
        back.GetComponent<Button>().onClick.AddListener(GoBack);
        var hover = back.AddComponent<HoverEffect>();
        hover.LookupKey = "FANCY_CLOSE_MENU";
        hover.FillInKeys = [("%type%", "Testing")];

        var toggleButton = transform.GetComponent<Button>("Toggle")!;
        ToggleImage = toggleButton.targetGraphic as Image;
        ToggleImage!.gameObject.SetActive(Constants.BTOS2Exists());
        toggleButton.onClick.AddListener(() => IsBTOS2 = !IsBTOS2);

        var filter = transform.FindRecursive("PageFilter");

        FilterArrow = filter.GetComponent<Image>("FilterArrow");
        FilterDropdown = filter.FindRecursive("ButtonDropdown").gameObject;
        FilterIcon = filter.GetComponent<Image>();
        FilterMaterialDefault = FilterIcon.material;
        filter.GetComponent<Button>().onClick.AddListener(() =>
        {
            FilterArrow.sprite = Fancy.Instance.Assets.GetSprite("DropDown_ArrowUp");
            FilterDropdown.SetActive(!FilterDropdown.activeSelf);
        });

        FilterDropdown.transform.GetComponent<Button>("RecolouredUI")!.onClick.AddListener(() => Page = PackType.RecoloredUI);
        FilterDropdown.transform.GetComponent<Button>("IconPacks")!.onClick.AddListener(() => Page = PackType.IconPacks);
        FilterDropdown.transform.GetComponent<Button>("SilSwapper")!.onClick.AddListener(() => Page = PackType.SilhouetteSets);
        FilterDropdown.transform.GetComponent<Button>("MRC")!.onClick.AddListener(() => Page = PackType.MiscRoleCustomisation);
        FilterDropdown.transform.GetComponent<Button>("CineSwapper")!.onClick.AddListener(() => Page = PackType.CinematicSwapper);
        var testing = FilterDropdown.transform.GetComponent<Button>("Testing");
        testing!.onClick.AddListener(() => Page = PackType.Testing);

        var special = roleCard.GetComponent<Image>("Special");
        var drop = transform.FindRecursive("Drop");

        Metals.Add(special);
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
        Metals.Add(FilterArrow);
        Metals.Add(special.transform.GetComponent<Image>("Icon"));
        Metals.Add(transform.GetComponent<Image>("Chains1"));
        Metals.Add(transform.GetComponent<Image>("Chains2"));
        Metals.Add(drop.GetComponent<Image>("Icon1"));
        Metals.Add(drop.GetComponent<Image>("Icon2"));
        Metals.Add(transform.Find("Frame").GetComponent<Image>());
        Metals.AddRange(SlotCounter.GetIconBGs());

        Woods.Add(roleCard.GetComponent<Image>("Frame"));
        Woods.Add(Metals[0].transform.GetComponent<Image>("Wood"));
        Woods.Add(chat.transform.GetComponent<Image>("WoodFrame"));
        Woods.Add(chat.transform.GetComponent<Image>("WoodDetails1"));
        Woods.Add(chat.transform.GetComponent<Image>("WoodDetails2"));
        Woods.Add(chat.transform.GetComponent<Image>("ChatBottom"));
        Woods.Add(rlPlusGy.transform.GetComponent<Image>("Wood"));
        Woods.Add(roleList.transform.GetComponent<Image>("Wood"));
        Woods.Add(graveyard.transform.GetComponent<Image>("Wood"));
        Woods.Add(specialAbilityPopup.GetComponent<Image>("Frame"));
        Woods.Add(FilterDropdown.GetComponent<Image>());
        Woods.Add(drop.GetComponent<Image>("Wood1"));
        Woods.Add(drop.GetComponent<Image>("Wood2"));
        Woods.AddRange(SlotCounter.Counters.Select(x => x.GetComponent<Image>()));

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
            }), optionParent);
            setting.Refresh();
            Metals.Add(setting.Background);

            switch (setting)
            {
                case DropdownSetting dropdown:
                {
                    Metals.Add(dropdown.Arrow);
                    Metals.Add(dropdown.Dropdown.GetComponent<Image>());
                    break;
                }
                case BaseInputSetting input:
                {
                    if (input is not ColorSetting)
                        Metals.Add(input.Input.GetComponent<Image>());

                    if (input is SliderSetting slider)
                    {
                        Metals.Add(slider.Slider.targetGraphic as Image);
                        Metals.Add(slider.Slider.transform.GetComponent<Image>("Background"));
                    }

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

        Leathers.Clear();
        Metals.Clear();
        Woods.Clear();
        Fires.Clear();
        Waxes.Clear();
        Papers.Clear();

        Refresh();
    }

    public void GoBack()
    {
        gameObject.SetActive(false);
        FancyUI.Instance.gameObject.SetActive(true);
        Utils.UpdateMaterials(skipFactionCheck: true);
    }

    public void Refresh()
    {
        Animator.SetDuration(Fancy.AnimationDuration.Value);
        NameText.SetText(DefaultNameText
            .Replace("%num%", $"{Constants.PlayerNumber()}")
            .Replace("%roleName%", Utils.ApplyGradient($"({(Fancy.FactionalRoleNames.Value ? Utils.GetRoleName(Fancy.SelectTestingRole.Value, Fancy.SelectTestingFaction.Value, true) : Utils.GetString($"{(IsBTOS2 ? "BTOS_" : "GUI_")}ROLENAME_{(int)Fancy.SelectTestingRole.Value}"))})", Fancy.SelectTestingFaction.Value.GetChangedGradient(Fancy.SelectTestingRole.Value))));
        NameText.SetGraphicColor(ColorType.Paper);
        RoleText.SetText(DefaultRoleText
            .Replace("%type%", $"{Utils.FactionName(Fancy.SelectTestingFaction.Value, IsBTOS2 ? GameModType.BTOS2 : GameModType.Vanilla, stoned: true)}")
            .Replace("%mod%", IsBTOS2 ? "BTOS" : "")
            .Replace("%roleName%", Utils.ApplyGradient($"{(Fancy.FactionalRoleNames.Value ? Utils.GetRoleName(Fancy.SelectTestingRole.Value, Fancy.SelectTestingFaction.Value) : Utils.GetString($"{(IsBTOS2 ? "BTOS_" : "GUI_")}ROLENAME_{(int)Fancy.SelectTestingRole.Value}"))}", Fancy.SelectTestingFaction.Value.GetChangedGradient(Fancy.SelectTestingRole.Value)))
            .Replace("%roleInt%", $"{(int)Fancy.SelectTestingRole.Value}"));
        Displays.ForEach((x, y) => y.SetActive(Fancy.SelectDisplay.Value == x));
        // Icons.ForEach(x => x.UpdateIcon(Fancy.SelectTestingRole.Value));
        ToggleImage.sprite = Fancy.Instance.Assets.GetSprite($"{(IsBTOS2 ? "B" : "")}ToS2Icon");
        FilterArrow.sprite = Fancy.Instance.Assets.GetSprite("DropDown_ArrowDown");
        FilterIcon.sprite = Fancy.Instance.Assets.GetSprite(page switch
        {
            PackType.RecoloredUI => "ColoredWood",
            PackType.IconPacks => "IconPack",
            PackType.SilhouetteSets => "SilSwapper",
            PackType.MiscRoleCustomisation => "MRC",
            PackType.CinematicSwapper => "Swap",
            _ => "Settings",
        });
        FilterIcon.material = page == PackType.Testing ? Constants.AllMaterials[true][ColorType.Metal] : FilterMaterialDefault;
        FilterDropdown.SetActive(false);

        foreach (var option in Option.All)
        {
            option.BoxedSetting.gameObject.SetActive(false);

            if (!option.SetActive() || !option.Page.IsAny(page, PackType.None))
                continue;

            option.BoxedSetting.Refresh();
            option.BoxedSetting.gameObject.SetActive(true);
        }

        Utils.UpdateMaterials();
    }
}