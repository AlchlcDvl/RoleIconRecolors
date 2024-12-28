namespace FancyUI.Compatibility;
public static class BTOS2IPCompatibility
{
    private static Type DeckItemType;
    private static Type MenuRoleType;
    private static Type OracleMenuControllerListItemType;
    public static bool BTOS2IPPatched { get; set; }
    public static bool Init()
    {
        try
        {
            if (!ModStates.EnabledMods.TryFinding(x => x.HarmonyId == "curtis.tuba.better.tos2", out var btos2Mod))
                return false;
            var bTOS2Assembly = Assembly.LoadFile(btos2Mod.AssemblyPath);
            var bTOS2Types = AccessTools.GetTypesFromAssembly(bTOS2Assembly);
            var roleDeckPlusPanelControllerType = bTOS2Types.FirstOrDefault(x => x.Name == "RoleDeckPlusPanelController");
            DeckItemType = bTOS2Types.FirstOrDefault(x => x.Name.Contains("DeckItem") && !x.IsEnum);
            MenuRoleType = bTOS2Types.FirstOrDefault(x => x.Name.Contains("MenuRole"));
            OracleMenuControllerListItemType = bTOS2Types.FirstOrDefault(x => x.Name.Contains("OracleMenuControllerListItem"));
            var setDataMethod1 = AccessTools.Method(DeckItemType, "SetData", [ typeof(Role), typeof(FactionType), typeof(bool), roleDeckPlusPanelControllerType ]);
            var refreshDataMethod = AccessTools.Method(MenuRoleType, "RefreshData");
            var compatType = typeof(BTOS2IPCompatibility);
            BTOS2Compatibility.BTOS2PatchesHarmony.Patch(setDataMethod1, null, new(AccessTools.Method(compatType, nameof(ItemPostfix1))));
            BTOS2Compatibility.BTOS2PatchesHarmony.Patch(refreshDataMethod, null, new(AccessTools.Method(compatType, nameof(ItemPostfix2))));
            Fancy.Instance.Message("BTOS2 compatibility was successful");
            return true;
        }
        catch (Exception ex)
        {
            Fancy.Instance.Error($"BTOS2 compatbility patch loading failed because:\n{ex}");
            return false;
        }
    }
    public static void ItemPostfix1(dynamic __instance, ref Role a_role, ref FactionType faction, ref bool isBan)
    {
        if (!Constants.EnableIcons())
            return;
        var roleIcon = (Image)AccessTools.Field(DeckItemType, "roleIcon").GetValue(__instance);
        if (!roleIcon)
            return;
        if (isBan)
        {
            var sprite2 = GetSprite("Banned");
            if (sprite2.IsValid())
                roleIcon.sprite = sprite2;
            return;
        }
        var ogfaction = a_role.GetFactionType(ModType.BTOS2);
        var reg = ogfaction != faction;
        var name = Utils.RoleName(a_role);
        var sprite = GetSprite(reg, name, Utils.FactionName(faction));
        if (!sprite.IsValid() && reg)
            sprite = GetSprite(name, Utils.FactionName(ogfaction));
        if (sprite.IsValid())
            roleIcon.sprite = sprite;
    }
    public static void ItemPostfix2(dynamic __instance)
    {
        if (!Constants.EnableIcons())
            return;
        var roleIcon = (Image)AccessTools.Field(MenuRoleType, "icon").GetValue(__instance);
        if (!roleIcon)
            return;
        var role = (Role)AccessTools.Field(MenuRoleType, "role").GetValue(__instance);
        var sprite = GetSprite(Utils.RoleName(role), Utils.FactionName(role.GetFactionType()));
        if (sprite.IsValid())
            roleIcon.sprite = sprite;
        var banned = (GameObject)AccessTools.Field(MenuRoleType, "banned").GetValue(__instance);
        if (!banned)
            return;
        var bannedIcon = banned.GetComponent<Image>();
        var bannedSprite = GetSprite("Banned");
        if (bannedIcon && bannedSprite.IsValid())
            bannedIcon.sprite = bannedSprite;
    }
    public static void ItemPostfix3(dynamic __instance)
    {
        if (!Constants.EnableIcons())
            return;
        var roleIcon = (Image)AccessTools.Field(OracleMenuControllerListItemType, "RoleIcon").GetValue(__instance);
        if (!roleIcon)
            return;
        var role = (Role)AccessTools.Field(OracleMenuControllerListItemType, "role").GetValue(__instance);
        var sprite = GetSprite(Utils.RoleName(role), Utils.FactionName(role.GetFactionType()));
        if (sprite.IsValid())
            roleIcon.sprite = sprite;
    }
}