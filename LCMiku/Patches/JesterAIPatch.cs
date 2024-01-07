using HarmonyLib;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace LCMiku.Patches
{
    [HarmonyPatch(typeof(JesterAI))]
    public static class JesterAiPatch
    {
        [HarmonyPatch(nameof(JesterAI.Start))]
        [HarmonyPrefix]
        public static void MikuJesterPatch(
            [SuppressMessage("ReSharper", "InconsistentNaming")] ref AudioClip ___popGoesTheWeaselTheme,
            [SuppressMessage("ReSharper", "InconsistentNaming")] ref AudioClip ___screamingSFX,
            [SuppressMessage("ReSharper", "InconsistentNaming")] ref AudioSource ___farAudio,
            [SuppressMessage("ReSharper", "InconsistentNaming")] ref AudioSource ___creatureVoice
            )
        {
            if (MikuJesterBase.Instance.IntroEnabled.Value)
            {
                ___popGoesTheWeaselTheme = MikuJesterBase.Instance.Intro;
                ___farAudio.volume = MikuJesterBase.Instance.IntroVolume.Value / 100.0f;
            }
            if (MikuJesterBase.Instance.SoloEnabled.Value)
            {
                ___screamingSFX = MikuJesterBase.Instance.Solo;
                ___creatureVoice.volume = MikuJesterBase.Instance.SoloVolume.Value / 100.0f;
            }
        }
    }
}
