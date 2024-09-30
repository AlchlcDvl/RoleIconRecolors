using System.Xml;
using Home.Services;

namespace FancyUI.Patches;

[HarmonyPatch(typeof(HomeLocalizationService), nameof(HomeLocalizationService.RebuildStringTables))]
public static class DumpStringTables
{
    public static void Postfix(HomeLocalizationService __instance)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(FromResources.LoadString("FancyUI.Resources.StringTable.xml"));
        var stringTable = XMLStringTable.Load(xmlDocument);

        foreach (var textEntry in stringTable.entries)
        {
            if (!__instance.stringTable_.ContainsKey(textEntry.key))
                __instance.stringTable_.Add(textEntry.key, textEntry.value);
            else
                Logging.LogWarning("Duplicate String Table Key \"" + textEntry.key + "\"!");

            if (!string.IsNullOrEmpty(textEntry.style))
            {
                if (!__instance.styleTable_.ContainsKey(textEntry.key))
                    __instance.styleTable_.Add(textEntry.key, textEntry.style);
                else
                    Logging.LogWarning($"Duplicate Style Table Key \"{textEntry.key}\"!");
            }
        }
    }
}