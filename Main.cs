using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Utilla;

namespace GOOMPS
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.7")]
    [BepInPlugin("buzzbb.GOOMPS", "GOOMPS", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        SphereCollider coll;
        GOOMPSCollision mngr;

        void Start()
        {
            Events.GameInitialized += OnGameInitialized;
            coll = new GameObject("GOOMPS Collision").AddComponent<SphereCollider>();
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            if (mngr) mngr.enabled = true;
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
            if (mngr) mngr.enabled = false;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            Cfg.LoadConfig();

            mngr = coll.gameObject.AddComponent<GOOMPSCollision>();
            coll.gameObject.layer = 10;
            coll.gameObject.AddComponent<TransformFollow>().transformToFollow = GorillaTagger.Instance.offlineVRRig.transform;
            coll.isTrigger = true;
            coll.radius = Cfg.radius.Value;
        }
    }

    class Cfg
    {
        public static ConfigEntry<float> radius;
        public static ConfigEntry<bool> mute;
        public static ConfigEntry<bool> audio;

        public static void LoadConfig()
        {
            var File = new ConfigFile(Path.Combine(Paths.ConfigPath, "GOOMPS.cfg"), true);

            radius = File.Bind("Settings", "Detection Radius", 0.65f);
            mute = File.Bind("Settings", "Mute Intruder", false);
            audio = File.Bind("Settings", "Hide Noise", true);
        }
    }

    public class HarmonyPatches
    {
        private static Harmony instance;

        public static bool IsPatched { get; private set; }
        public const string InstanceId = "buzzbb.GOOMPS";

        internal static void ApplyHarmonyPatches()
        {
            if (!IsPatched)
            {
                if (instance == null)
                {
                    instance = new Harmony(InstanceId);
                }

                instance.PatchAll(Assembly.GetExecutingAssembly());
                IsPatched = true;
            }
        }

        internal static void RemoveHarmonyPatches()
        {
            if (instance != null && IsPatched)
            {
                instance.UnpatchSelf();
                IsPatched = false;
            }
        }
    }
}