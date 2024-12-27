namespace FancyUI.Compatibility;
public static class BTOS2Compatibility
{
    public static bool BTOS2Patched { get; set; }
    public static Harmony BTOS2PatchesHarmony { get; set; }
    public static bool Init()
    {
        BTOS2PatchesHarmony = new("alchlcsystm.btos2patches");
        return BTOS2IPCompatibility.BTOS2IPPatched = BTOS2IPCompatibility.Init();
    }
}