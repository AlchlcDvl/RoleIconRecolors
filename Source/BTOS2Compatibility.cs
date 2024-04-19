using Home.Common;

namespace IconPacks;

public static class BTOS2Compatibility
{
    private static Harmony BTOS2PatchesHarmony { get; set; }
    private static Type[] BTOS2Types { get; set; }
    private static Assembly BTOS2Assembly { get; set; }

    public static void Init()
    {
        var btos2Mod = ModStates.EnabledMods.Find(x => x.HarmonyId == "curtis.tuba.better.tos2");

        if (btos2Mod == null)
            return;

        var path = Path.Combine(Path.GetDirectoryName(Application.dataPath), "SalemModLoader", "Mods", "BetterTOS2.dll");
        BTOS2Assembly = Assembly.LoadFile(path);

        if (BTOS2Assembly == null)
            return;

        BTOS2PatchesHarmony = new("alchlcsystm.recolors.btos2patches");

        BTOS2Types = AccessTools.GetTypesFromAssembly(BTOS2Assembly);

        var itemControllerType = BTOS2Types.FirstOrDefault(x => x.Name == "BookPassingMenuListItemController");

        var awakeMethod = AccessTools.Method(itemControllerType, "Awake");

        var voteForType = BTOS2Types.FirstOrDefault(x => x.Name == "VoteForNecroMessage");
        var handleItemVoteMethod = AccessTools.Method(itemControllerType, "HandleVote", [ voteForType ]);

        BTOS2PatchesHarmony.Patch(awakeMethod, null, new(AccessTools.Method(typeof(BTOS2Compatibility), nameof(ItemPostfix1))));
        BTOS2PatchesHarmony.Patch(handleItemVoteMethod, null, new(AccessTools.Method(typeof(BTOS2Compatibility), nameof(ItemPostfix2))));
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

        var image2 = controller.transform.Find("RoleIcon").GetComponentInChildren<Image>();
        var sprite = AssetManager.GetSprite(Utils.RoleName(tuple.Item1), Utils.FactionName(tuple.Item2));

        if (sprite.IsValid() && image2)
            image2.sprite = sprite;
    }

    public static void ItemPostfix2(dynamic __instance, dynamic message)
    {
        if (!Constants.EnableIcons)
            return;

        var id = (int)message.sourcePosition;

        if (!Service.Game.Sim.simulation.knownRolesAndFactions.Data.TryGetValue(id, out var tuple))
            return;

        var busts = (List<GameObject>)__instance.busts;
        var go = busts.Find(x => x.name == id.ToString());
        var sprite = AssetManager.GetSprite(Utils.RoleName(tuple.Item1), Utils.FactionName(tuple.Item2));
        var image = go.GetComponent<Image>();

        if (sprite.IsValid() && image)
            image.sprite = sprite;
    }
}