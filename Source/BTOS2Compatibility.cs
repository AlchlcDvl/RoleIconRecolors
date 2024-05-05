using Home.Common;

namespace IconPacks;

public static class BTOS2Compatibility
{
    private static Harmony BTOS2PatchesHarmony { get; set; }
    private static Type[] BTOS2Types { get; set; }
    private static Assembly BTOS2Assembly { get; set; }

    private static Type BookPassingMenuListItemControllerType { get; set; }
    private static Type VoteForType { get; set; }

    private static Type RoleDeckPlusPanelControllerType { get; set; }
    private static Type DeckItemType { get; set; }

    private static Type MenuRoleType { get; set; }

    private static Type OracleMenuControllerListItemType { get; set; }

    public static bool BTOS2Patched { get; set; }

    public static bool Init()
    {
        try
        {
            var btos2Mod = ModStates.EnabledMods.Find(x => x.HarmonyId == "curtis.tuba.better.tos2");

            BTOS2Assembly = Assembly.LoadFile(btos2Mod.AssemblyPath);

            BTOS2PatchesHarmony = new("alchlcsystm.recolors.btos2patches");

            BTOS2Types = AccessTools.GetTypesFromAssembly(BTOS2Assembly);

            BookPassingMenuListItemControllerType = BTOS2Types.FirstOrDefault(x => x.Name == "BookPassingMenuListItemController");
            VoteForType = BTOS2Types.FirstOrDefault(x => x.Name == "VoteForNecroMessage");

            RoleDeckPlusPanelControllerType = BTOS2Types.FirstOrDefault(x => x.Name == "RoleDeckPlusPanelController");
            DeckItemType = BTOS2Types.FirstOrDefault(x => x.Name.Contains("DeckItem") && !x.Name.Contains("ItemType"));

            MenuRoleType = BTOS2Types.FirstOrDefault(x => x.Name.Contains("MenuRole"));

            OracleMenuControllerListItemType = BTOS2Types.FirstOrDefault(x => x.Name.Contains("OracleMenuControllerListItem"));

            var awakeMethod = AccessTools.Method(BookPassingMenuListItemControllerType, "Awake");

            var handleItemVoteMethod = AccessTools.Method(BookPassingMenuListItemControllerType, "HandleVote", [ VoteForType ]);

            var setDataMethod1 = AccessTools.Method(DeckItemType, "SetData", [ typeof(Role), typeof(FactionType), typeof(bool), RoleDeckPlusPanelControllerType ]);

            var refreshDataMethod = AccessTools.Method(MenuRoleType, "RefreshData");

            var setDataMethod2 = AccessTools.Method(OracleMenuControllerListItemType, "SetData");

            var compatType = typeof(BTOS2Compatibility);

            BTOS2PatchesHarmony.Patch(awakeMethod, null, new(AccessTools.Method(compatType, nameof(ItemPostfix1))));
            BTOS2PatchesHarmony.Patch(handleItemVoteMethod, null, new(AccessTools.Method(compatType, nameof(ItemPostfix2))));
            BTOS2PatchesHarmony.Patch(setDataMethod1, null, new(AccessTools.Method(compatType, nameof(ItemPostfix3))));
            BTOS2PatchesHarmony.Patch(refreshDataMethod, null, new(AccessTools.Method(compatType, nameof(ItemPostfix4))));
            BTOS2PatchesHarmony.Patch(setDataMethod2, null, new(AccessTools.Method(compatType, nameof(ItemPostfix5))));
            return true;
        }
        catch (Exception ex)
        {
            Logging.LogError($"BTOS2 compatbility patch loading failed because:\n{ex}");
            return false;
        }
    }

    public static void ItemPostfix1(dynamic __instance)
    {
        if (!Constants.EnableIcons)
            return;

        var controller = (UIController)__instance;
        var image = controller.transform.Find("Vote/Icon").GetComponent<Image>();
        var nommy = AssetManager.GetSprite("Necronomicon");

        if (nommy.IsValid() && image)
            image.sprite = nommy;

        var id = (int)__instance.data.Item1;

        if (!Service.Game.Sim.simulation.knownRolesAndFactions.Data.TryGetValue(id, out var tuple))
            return;

        if (Pepper.GetMyPosition() == id)
            tuple = new(Pepper.GetMyRole(), Pepper.GetMyFaction());

        var image2 = controller.transform.Find("RoleIcon").GetComponentInChildren<Image>();
        var ogfaction = tuple.Item1.GetFactionType(ModType.BTOS2);
        var reg = ogfaction != tuple.Item2;
        var role = Utils.RoleName(tuple.Item1);
        var sprite = AssetManager.GetSprite(reg, role, Utils.FactionName(tuple.Item2));

        if (!sprite.IsValid() && reg)
            sprite = AssetManager.GetSprite(role, Utils.FactionName(ogfaction));

        if (sprite.IsValid() && image2)
            image2.sprite = sprite;
    }

    public static void ItemPostfix2(dynamic __instance, ref dynamic message)
    {
        if (!Constants.EnableIcons)
            return;

        var id = (int)message.sourcePosition;

        if (!Service.Game.Sim.simulation.knownRolesAndFactions.Data.TryGetValue(id, out var tuple))
            return;

        if (Pepper.GetMyPosition() == id)
            tuple = new(Pepper.GetMyRole(), Pepper.GetMyFaction());

        var busts = (List<GameObject>)AccessTools.Field(BookPassingMenuListItemControllerType, "busts").GetValue(__instance);
        var go = busts.Find(x => x.name == id.ToString());
        var ogfaction = tuple.Item1.GetFactionType(ModType.BTOS2);
        var reg = ogfaction != tuple.Item2;
        var role = Utils.RoleName(tuple.Item1);
        var sprite = AssetManager.GetSprite(reg, role, Utils.FactionName(tuple.Item2));

        if (!sprite.IsValid() && reg)
            sprite = AssetManager.GetSprite(role, Utils.FactionName(ogfaction));

        var image = go.GetComponent<Image>();

        if (sprite.IsValid() && image)
            image.sprite = sprite;
    }

    public static void ItemPostfix3(dynamic __instance, Role a_role, FactionType faction, bool isBan)
    {
        if (!Constants.EnableIcons)
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

    public static void ItemPostfix4(dynamic __instance)
    {
        if (!Constants.EnableIcons)
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

    public static void ItemPostfix5(dynamic __instance)
    {
        if (!Constants.EnableIcons)
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