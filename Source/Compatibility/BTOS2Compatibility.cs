using SalemModLoader;

namespace FancyUI.Compatibility;

public static class Btos2Compatibility
{
    public static bool Btos2Patched { get; set; }
    public static Harmony Btos2PatchesHarmony { get; private set; }

    public static bool Init()
    {
        try
        {
            if (!Launch.ModAssemblies.TryGetValue("curtis.tuba.better.tos2", out var btos2Assembly))
                return false;

            var btos2Types = AccessTools.GetTypesFromAssembly(btos2Assembly);
            Btos2PatchesHarmony = new("alchlcsystm.btos2patches");
            var result = Btos2IpCompatibility.Init(btos2Types) && Btos2CwCompatibility.Init(btos2Types);
            Fancy.Instance.Message("BTOS2 compatibility was successful");
            return result;
        }
        catch (Exception ex)
        {
            Fancy.Instance.Error($"BTOS2 compatibility patch loading failed because:\n{ex}");
            return false;
        }
    }
}