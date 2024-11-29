namespace FancyUI.UI;

public class SettingsAndTestingUI : UIController
{
    public static SettingsAndTestingUI Instance { get; private set; }

    private CustomAnimator Animator { get; set; }

    private GameObject Back { get; set; }
    // private GameObject Confirm { get; set; }
    private GameObject Special { get; set; }
    private GameObject Effect { get; set; }
    // private GameObject Toggle { get; set; }
    private GameObject ButtonTemplate { get; set; }

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

    private bool IsBTOS2 { get; set; }

    private readonly List<GameObject> ButtonGOs = [];
    private readonly List<Image> ButtonImages = [];
    private readonly List<Sprite> ButtonSprites = [];

    private readonly List<Setting> Settings = [];

    private static readonly Vector3 SpecialOrEffectButtonPosition = new(0f, -20f, 0f);
    private static readonly Vector3[][] ButtonPositions =
    [
        [ new(0f, -182f, 0f) ],
        [ new(-24f, -190f, 0f), new(24f, -190f, 0f) ],
        [ new(-45f, -190f, 0f), new(0f, -188f, 0f), new(45f, -190f, 0f) ],
        [ new(-72f, -180f, 0f), new(-24f, -190f, 0f), new(-24f, -190f, 0f), new(72f, -180f, 0f) ]
    ];

    private const string ToReplace = "<sprite=\"PlayerNumbers\" name=\"PlayerNumbers_%num%\"><b>Giles Corey</b>\'s role is <sprite=\"%mod%RoleIcons (%type%)\" name=\"Role%roleInt%\"><b>" +
        "%roleName%</b>!";
    private const string DefaultNum = "0";
    private const string DefaultMod = "";
    private const string DefaultType = "Regular";
    private const string DefaultRoleInt = "241";
    private const string DefaultRoleName = "Hidden";

    public void Awake()
    {
        Instance = this;

        Animator = transform.EnsureComponent<CustomAnimator>("Animator");
        Back = transform.FindRecursive("Back").gameObject;

        // Confirm = transform.FindRecursive("Confirm").gameObject;
        // Toggle = transform.FindRecursive("Toggle").gameObject;
        RoleCard = transform.FindRecursive("RoleCard");

        Special = RoleCard.FindRecursive("Special").gameObject;
        Effect = RoleCard.FindRecursive("Effect").gameObject;
        ButtonTemplate = RoleCard.FindRecursive("ButtonTemplate").gameObject; // CommunityRecolors

        RoleText = RoleCard.GetComponent<TextMeshProUGUI>("Role");
        NameText = transform.GetComponent<TextMeshProUGUI>("NameText");

        SlotCounter = RoleCard.GetComponent<Image>("SlotCounter");
        RoleIcon = RoleCard.GetComponent<Image>("RoleIcon");
        Attack = RoleCard.GetComponent<Image>("Attack");
        Defense = RoleCard.GetComponent<Image>("Defense");

        // ToggleImage = Toggle.GetComponent<Image>();

        SliderTemplate = transform.EnsureComponent<SliderSetting>("SliderTemplate");
        ColorTemplate = transform.EnsureComponent<ColorSetting>("ColorTemplate");
        DropdownTemplate = transform.EnsureComponent<DropdownSetting>("DropdownTemplate");
        ToggleTemplate = transform.EnsureComponent<ToggleSetting>("ToggleTemplate");
        InputTemplate = transform.EnsureComponent<StringInputSetting>("InputTemplate");

        SetupMenu();
    }

    private void SetupMenu()
    {
        Back.GetComponent<Button>().onClick.AddListener(GoBack);
        Back.AddComponent<TooltipTrigger>().NonLocalizedString = "Close Testing Menu";

        Animator.SetAnim(Loading.Frames, Constants.AnimationDuration());
        Animator.AddComponent<TooltipTrigger>().NonLocalizedString = "This Is Your Animator";

        // Confirm.GetComponent<Button>().onClick.AddListener(SetIcons);
        // Confirm.AddComponent<TooltipTrigger>().NonLocalizedString = "Confirm";

        // Toggle.GetComponent<Button>().onClick.AddListener(ToggleVersion);
        // Toggle.AddComponent<TooltipTrigger>().NonLocalizedString = "Toggle To Choose Icons From BTOS2";
        // Toggle.SetActive(Constants.BTOS2Exists());

        foreach (var opt in Option.All)
        {
            if (opt is SliderOption slider)
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

        RefreshOptions();
    }

    public void GoBack()
    {
        gameObject.SetActive(false);
        FancyUI.Instance.gameObject.SetActive(true);
    }

    // public void ToggleVersion()
    // {
    //     IsBTOS2 = !IsBTOS2;
    //     ToggleImage.sprite = Fancy.Assets.GetSprite($"{(IsBTOS2 ? "B" : "")}ToS2Icon");
    //     Toggle.EnsureComponent<TooltipTrigger>().NonLocalizedString = $"Toggle To Choose Icons From {(IsBTOS2 ? "Vanilla" : "BTOS2")}";
    // }

    public void RefreshOptions() => Settings.ForEach(x => x.SetActive());
}