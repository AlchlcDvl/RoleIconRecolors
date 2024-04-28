using Home.Common;

namespace IconPacks;

public static class BTOS2Compatibility
{
    private static Harmony BTOS2PatchesHarmony { get; set; }
    private static Type[] BTOS2Types { get; set; }
    private static Assembly BTOS2Assembly { get; set; }

    private static Type BookPassingMenuListItemControllerType { get; set; }

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

            var awakeMethod = AccessTools.Method(BookPassingMenuListItemControllerType, "Awake");

            var voteForType = BTOS2Types.FirstOrDefault(x => x.Name == "VoteForNecroMessage");
            var handleItemVoteMethod = AccessTools.Method(BookPassingMenuListItemControllerType, "HandleVote", [ voteForType ]);

            BTOS2PatchesHarmony.Patch(awakeMethod, null, new(AccessTools.Method(typeof(BTOS2Compatibility), nameof(ItemPostfix1))));
            BTOS2PatchesHarmony.Patch(handleItemVoteMethod, null, new(AccessTools.Method(typeof(BTOS2Compatibility), nameof(ItemPostfix2))));
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
}