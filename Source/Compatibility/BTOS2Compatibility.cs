namespace FancyUI.Compatibility;

public static class Btos2Compatibility
{
    public static bool Btos2Patched { get; set; }
    public static Harmony Btos2PatchesHarmony { get; private set; }

    public static bool Init()
    {
        Btos2PatchesHarmony = new("alchlcsystm.btos2patches");
        return Btos2IPCompatibility.Init();
    }
}