namespace FancyUI.UI;

public class IconPackTestingUI : UIController
{
    public static IconPackTestingUI Instance { get; private set; }

    private GameObject Back { get; set; }
    private GameObject Confirm { get; set; }
    private GameObject Special { get; set; }
    private GameObject Effect { get; set; }
    private GameObject Toggle { get; set; }
    private GameObject RoleCard { get; set; }
    private GameObject ButtonTemplate { get; set; }

    private TextMeshProUGUI RoleText { get; set; }
    private TextMeshProUGUI ButtonText { get; set; }

    private TMP_InputField RoleName { get; set; }
    private TMP_InputField FactionName { get; set; }
    private TMP_InputField EffectName { get; set; }
    private TMP_InputField PlayerNumber { get; set; }

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

        Back = transform.Find("Back").gameObject;
        Confirm = transform.Find("Inputs/Confirm").gameObject;
        Toggle = transform.Find("Toggle").gameObject;
        RoleCard = transform.Find("RoleCard").gameObject;

        RoleName = transform.Find("Inputs/RoleName").GetComponent<TMP_InputField>();
        FactionName = transform.Find("Inputs/FactionName").GetComponent<TMP_InputField>();
        EffectName = transform.Find("Inputs/Effect").GetComponent<TMP_InputField>();
        PlayerNumber = transform.Find("Inputs/PlayerNumber").GetComponent<TMP_InputField>();

        Special = RoleCard.transform.Find("Special").gameObject;
        Effect = RoleCard.transform.Find("Effect").gameObject;
        ButtonTemplate = RoleCard.transform.Find("ButtonTemplate").gameObject;

        RoleText = RoleCard.transform.Find("RoleText").GetComponent<TextMeshProUGUI>();

        SlotCounter = RoleCard.transform.Find("SlotCounter").GetComponent<Image>();
        RoleIcon = RoleCard.transform.Find("RoleIcon").GetComponent<Image>();
        Attack = RoleCard.transform.Find("Attack").GetComponent<Image>();
        Defense = RoleCard.transform.Find("Defense").GetComponent<Image>();

        ButtonText = Toggle.transform.Find("Text").GetComponent<TextMeshProUGUI>();

        ToggleImage = Toggle.GetComponent<Image>();

        SetupMenu();
    }

    private void SetupMenu()
    {
        Back.GetComponent<Button>().onClick.AddListener(GoBack);
        Back.AddComponent<TooltipTrigger>().NonLocalizedString = "Close Testing Menu";

        Confirm.GetComponent<Button>().onClick.AddListener(SetIcons);
        Confirm.AddComponent<TooltipTrigger>().NonLocalizedString = "Confirm";

        Toggle.GetComponent<Button>().onClick.AddListener(ToggleVersion);
        Toggle.AddComponent<TooltipTrigger>().NonLocalizedString = "Toggle To Choose Icons From BTOS2";
        Toggle.SetActive(Constants.BTOS2Exists());
    }

    public void GoBack()
    {
        gameObject.SetActive(false);
        IconPacksUI.Instance.gameObject.SetActive(true);
    }

    public void ToggleVersion()
    {
        IsBTOS2 = !IsBTOS2;
        ToggleImage.sprite = Fancy.Assets.GetSprite($"Button{(IsBTOS2 ? "Red" : "Blue")}");
        ButtonText.SetText($"Toggle {(IsBTOS2 ? "Vanilla" : "BTOS2")}");
        Toggle.EnsureComponent<TooltipTrigger>().NonLocalizedString = $"Toggle To Choose Icons From {(IsBTOS2 ? "Vanilla" : "BTOS2")}";
    }

    public void SetIcons()
    {
        ButtonSprites.Clear();
        var input = RoleName.text;
        var roleName = StringUtils.IsNullEmptyOrWhiteSpace(input) ? DefaultRoleName : input;
        input = FactionName.text;
        var factionName = StringUtils.IsNullEmptyOrWhiteSpace(input) ? DefaultType : input;
        input = PlayerNumber.text;
        var playerNumber = StringUtils.IsNullEmptyOrWhiteSpace(input) ? DefaultNum : input;
        var mod = IsBTOS2 ? "BTOS" : DefaultMod;
        var roleInt = DefaultRoleInt;

        RoleText.SetText(ToReplace.Replace("%num%", playerNumber).Replace("%mod%", mod).Replace("%type%", factionName).Replace("%roleInt%", roleInt).Replace("%roleName%", roleName));
    }
}