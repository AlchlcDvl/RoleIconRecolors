namespace FancyUI.Compatibility;

public static class BTOS2IPCompatibility
{
    private static Harmony BTOS2PatchesHarmony { get; set; }
    private static Type[] BTOS2Types { get; set; }
    private static Assembly BTOS2Assembly { get; set; }

    private static Type RoleDeckPlusPanelControllerType { get; set; }
    private static Type DeckItemType { get; set; }

    private static Type MenuRoleType { get; set; }

    private static Type OracleMenuControllerListItemType { get; set; }

    public static bool BTOS2IPPatched { get; set; }

    public static bool Init()
    {
        try
        {
            var btos2Mod = ModStates.EnabledMods.Find(x => x.HarmonyId == "curtis.tuba.better.tos2");

            BTOS2Assembly = Assembly.LoadFile(btos2Mod.AssemblyPath);

            BTOS2PatchesHarmony = new("alchlcsystm.fancy.ui.icon.packs.btos2patches");

            BTOS2Types = AccessTools.GetTypesFromAssembly(BTOS2Assembly);

            RoleDeckPlusPanelControllerType = BTOS2Types.FirstOrDefault(x => x.Name == "RoleDeckPlusPanelController");
            DeckItemType = BTOS2Types.FirstOrDefault(x => x.Name.Contains("DeckItem") && !x.IsEnum);

            MenuRoleType = BTOS2Types.FirstOrDefault(x => x.Name.Contains("MenuRole"));

            OracleMenuControllerListItemType = BTOS2Types.FirstOrDefault(x => x.Name.Contains("OracleMenuControllerListItem"));

            var setDataMethod1 = AccessTools.Method(DeckItemType, "SetData", [ typeof(Role), typeof(FactionType), typeof(bool), RoleDeckPlusPanelControllerType ]);

            var refreshDataMethod = AccessTools.Method(MenuRoleType, "RefreshData");

            var setDataMethod2 = AccessTools.Method(OracleMenuControllerListItemType, "SetData");

            var compatType = typeof(BTOS2IPCompatibility);

            BTOS2PatchesHarmony.Patch(setDataMethod1, null, new(AccessTools.Method(compatType, nameof(ItemPostfix1))));
            BTOS2PatchesHarmony.Patch(refreshDataMethod, null, new(AccessTools.Method(compatType, nameof(ItemPostfix2))));
            BTOS2PatchesHarmony.Patch(setDataMethod2, null, new(AccessTools.Method(compatType, nameof(ItemPostfix3))));
            Logging.LogMessage("BTOS2 compatibility was successful");
            return true;
        }
        catch (Exception ex)
        {
            Logging.LogError($"BTOS2 compatbility patch loading failed because:\n{ex}");
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
            var sprite2 = AssetManager.GetSprite("Banned");

            if (sprite2.IsValid())
                roleIcon.sprite = sprite2;

            return;
        }

        var ogfaction = a_role.GetFactionType(ModType.BTOS2);
        var reg = ogfaction != faction;
        var name = Utils.RoleName(a_role);
        var sprite = AssetManager.GetSprite(reg, name, Utils.FactionName(faction));

        if (!sprite.IsValid() && reg)
            sprite = AssetManager.GetSprite(name, Utils.FactionName(ogfaction));

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
        var sprite = AssetManager.GetSprite(Utils.RoleName(role), Utils.FactionName(role.GetFactionType()));

        if (sprite.IsValid())
            roleIcon.sprite = sprite;

        var banned = (GameObject)AccessTools.Field(MenuRoleType, "banned").GetValue(__instance);

        if (!banned)
            return;

        var bannedIcon = banned.GetComponent<Image>();
        var bannedSprite = AssetManager.GetSprite("Banned");

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
        var sprite = AssetManager.GetSprite(Utils.RoleName(role), Utils.FactionName(role.GetFactionType()));

        if (sprite.IsValid())
            roleIcon.sprite = sprite;
    }
}