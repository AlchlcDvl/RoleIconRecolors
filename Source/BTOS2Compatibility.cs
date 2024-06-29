namespace FancyUI;

public static class BTOS2Compatibility
{
    public static bool BTOS2Patched { get; set; }

    public static bool Init() => BTOS2IPCompatibility.BTOS2IPPatched = BTOS2IPCompatibility.Init();
}