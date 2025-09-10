using System;
using Game.Chat;
using Game.Chat.Decoders;
using HarmonyLib;
using Mentions.UI;
using Server.Shared.Messages;
using Shared.Chat;
using SML;
using UnityEngine;
using UnityEngine.UI;

// hey tuba why do you like "Postfiex" so much
namespace FancyUI.Patches;

[HarmonyPatch(typeof(PooledChatController), nameof(PooledChatController.Start))]
public static class ChatBGOpacity
{
    public static void Postfix()
    {
        var gameObject = GameObject.Find("Hud/PooledChatElementsUI(Clone)/Background/ChatContents/ChatUpperContents/Backing");

        if (!gameObject)
            return;

        var componentInChildren = gameObject.GetComponentInChildren<Image>();

        if (!componentInChildren)
            return;

        var color = componentInChildren.color;
        color.a = Constants.ChatBackgroundOpacity();
        componentInChildren.color = color;
    }
}

[HarmonyPatch(typeof(PlayerIsWhisperingDecoder), nameof(PlayerIsWhisperingDecoder.Encode))]
public static class FancyChatWhispers
{
    public static void Postfix(PlayerIsWhisperingDecoder __instance, ref string __result)
    {
        var name1 = $"[[@{__instance.whispererId + 1}]]";
        var name2 = $"[[@{__instance.whispereeId + 1}]]";
        var myPosition = Pepper.GetMyPosition();
        var myRole = Pepper.GetMyRole();
        var isWildling = myRole == Role.WILDLING;
        var isSerialKiller = myRole == Role.SERIALKILLER && Constants.IsBTOS2();

        if (!(myPosition == __instance.whispererId || myPosition == __instance.whispereeId) && (isWildling || isSerialKiller) && Pepper.AmIAlive())
        {
            string xToY;
            Color color;

            if (isWildling)
            {
                xToY = Utils.GetString("FANCY_WILDLING_XIS_WHISPERING_TO_Y");
                color = Fancy.WildlingWhisperColor.Value.ToColor();
            }
            else
            {
                xToY = Utils.GetString("FANCY_SERIALKILLER_XIS_WHISPERING_TO_Y");
                color = Fancy.SerialKillerWhisperColor.Value.ToColor();
            }

            xToY = xToY.Replace("%name1%", name1).Replace("%name2%", name2);
            __result = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{xToY}</color>";
        }
        else
        {
            var defaultColor = Fancy.WhisperColor.Value.ToColor();
            __result = __result.Replace("%name1%", name1).Replace("%name2%", name2);
            __result = $"<color=#{ColorUtility.ToHtmlStringRGB(defaultColor)}>{__result}</color>";
        }
    }
}

[HarmonyPatch(typeof(PlayerWhisperDecoder), nameof(PlayerWhisperDecoder.Encode))]
public static class FancyChatWhispers2
{
    public static void Postfix(PlayerWhisperDecoder __instance, ref string __result)
    {
        var myPos = Pepper.GetMyPosition();

        if (__instance.whispererId == myPos || __instance.whispereeId == myPos)
        {
            __result = string.Concat(
            [
                "<color=#",
                ColorUtility.ToHtmlStringRGB(Fancy.WhisperColor.Value.ToColor()),
                ">",
                __result,
                "</color>"
            ]);
        }
        else
        {
            var isWildling = Pepper.GetMyRole() == Role.WILDLING;
            var isSerialKiller = (Pepper.GetMyRole() == Role.SERIALKILLER) && Constants.IsBTOS2();

            if (isWildling) __result = __result.Replace("<color=#FF0000", "<color=#" + ColorUtility.ToHtmlStringRGB(Fancy.WildlingWhisperColor.Value.ToColor()));
            else if (isSerialKiller) __result = __result.Replace("<color=#FF0000", "<color=#" + ColorUtility.ToHtmlStringRGB(Fancy.SerialKillerWhisperColor.Value.ToColor()));
            else __result = __result.Replace("<color=#FF0000", "<color=#" + ColorUtility.ToHtmlStringRGB(Utils.GetPlayerRoleColor(myPos)));
        }
    }
}