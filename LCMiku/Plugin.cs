using System.IO;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using LCMikuJester.Patches;
using LCSoundTool;
using UnityEngine;

namespace LCMikuJester
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class MikuJesterBase : BaseUnityPlugin
    {
        private const string modGUID = "schlues.LCMikuJester";
        private const string modName = "Hatsune Miku Jester Mod";
        private const string modVersion = "1.0.0.0";

        private readonly Harmony _harmony = new Harmony(modGUID);

        public AudioClip Intro { get; private set; }
        public AudioClip Solo { get; private set; }

        public ConfigEntry<bool> IntroEnabled { get; private set; }
        public ConfigEntry<int> IntroVolume { get; private set; }
        public ConfigEntry<bool> SoloEnabled { get; private set; }
        public ConfigEntry<int> SoloVolume { get; private set; }

        public static MikuJesterBase Instance;

        internal ManualLogSource mls;


        void Awake()
        {
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            if (Instance == null)
            {
                Instance = this;
            } else
            {
                mls.LogInfo("MikuJester instance already running");
            }

            var isEnabled = Config.Bind("Mod", "EnableMod", true, "Enables the mod, otherwise doesn't load it");
            IntroEnabled = Config.Bind("Sound", "EnableCrankingIntro", true, "Enables the cranking to be replaced by Ievan Polkka intro");
            IntroVolume = Config.Bind("Sound", "IntroVolume", 50, new ConfigDescription("Sets the volume of the cranking intro (in %)", new AcceptableValueRange<int>(0, 200)));
            SoloEnabled = Config.Bind("Sound", "EnableScreamSolo", true, "Enables the scream to be replaced by Hatsune Miku's Ievan Polkka");
            SoloVolume = Config.Bind("Sound", "SoloVolume", 100, new ConfigDescription("Sets the volume of the screaming solo (in %)", new AcceptableValueRange<int>(0, 200)));

            if (!isEnabled.Value)
            {
                Logger.Log(LogLevel.Info, "MikuJester is disabled in config");
                return;
            }

            Intro = SoundTool.GetAudioClip(Path.GetDirectoryName(Info.Location), "intro.wav");
            Solo = SoundTool.GetAudioClip(Path.GetDirectoryName(Info.Location), "main.wav");

            _harmony.PatchAll(typeof(MikuJesterBase));
            _harmony.PatchAll(typeof(JesterAiPatch));
            
            mls.LogInfo("Hatsune Miku Jester has awakened :3");
        }
    }
}
