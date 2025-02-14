namespace FancyUI.Compatibility;

public static class Btos2IPCompatibility
{
    private static FieldInfo RoleIconField;

    private static FieldInfo RoleField;
    private static FieldInfo IconField;
    private static FieldInfo BannedField;

    public static bool Init()
    {
        try
        {
            if (!ModStates.EnabledMods.TryFinding(x => x.HarmonyId == "curtis.tuba.better.tos2", out var btos2Mod))
                return false;

            var btos2Assembly = Assembly.LoadFile(btos2Mod!.AssemblyPath);
            var btos2Types = AccessTools.GetTypesFromAssembly(btos2Assembly);
            var roleDeckPlusPanelControllerType = btos2Types.FirstOrDefault(x => x.Name == "RoleDeckPlusPanelController");
            var deckItemType = btos2Types.FirstOrDefault(x => x.Name.Contains("DeckItem") && !x.IsEnum);
            var menuRoleType = btos2Types.FirstOrDefault(x => x.Name.Contains("MenuRole"));

            RoleIconField = AccessTools.Field(deckItemType, "roleIcon");

            IconField = AccessTools.Field(menuRoleType, "icon");
            RoleField = AccessTools.Field(menuRoleType, "role");
            BannedField = AccessTools.Field(menuRoleType, "banned");

            var setDataMethod1 = AccessTools.Method(deckItemType, "SetData", [ typeof(Role), typeof(FactionType), typeof(bool), roleDeckPlusPanelControllerType ]);
            var refreshDataMethod = AccessTools.Method(menuRoleType, "RefreshData");
            var compatType = typeof(Btos2IPCompatibility);

            Btos2Compatibility.Btos2PatchesHarmony.Patch(setDataMethod1, null, new(AccessTools.Method(compatType, nameof(ItemPostfix1))));
            Btos2Compatibility.Btos2PatchesHarmony.Patch(refreshDataMethod, null, new(AccessTools.Method(compatType, nameof(ItemPostfix2))));
            Fancy.Instance.Message("BTOS2 compatibility was successful");
            return true;
        }
        catch (Exception ex)
        {
            Fancy.Instance.Error($"BTOS2 compatibility patch loading failed because:\n{ex}");
            return false;
        }
    }

    public static void ItemPostfix1(dynamic __instance, ref Role a_role, ref FactionType faction, ref bool isBan)
    {
        if (!Constants.EnableIcons())
            return;

        var roleIcon = (Image)RoleIconField.GetValue(__instance);

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

        var roleIcon = (Image)IconField.GetValue(__instance);

        if (!roleIcon)
            return;

        var role = (Role)RoleField.GetValue(__instance);
        var sprite = GetSprite(Utils.RoleName(role), Utils.FactionName(role.GetFactionType(ModType.BTOS2)));

        if (sprite.IsValid())
            roleIcon.sprite = sprite;

        var banned = (GameObject)BannedField.GetValue(__instance);

        if (!banned)
            return;

        var bannedIcon = banned.GetComponent<Image>();
        var bannedSprite = GetSprite("Banned");

        if (bannedIcon && bannedSprite.IsValid())
            bannedIcon.sprite = bannedSprite;
    }
}