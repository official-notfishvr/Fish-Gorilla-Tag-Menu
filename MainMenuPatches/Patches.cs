using BepInEx;
using ExitGames.Client.Photon;
using Fish_Menu.MainMenu;
using HarmonyLib;
using Photon.Pun;
using System;
using System.Linq;
using UnityEngine;
using HarmonyPatch = HarmonyLib.HarmonyPatch;
using Player = Photon.Realtime.Player;

namespace Fish_Menu.MainMenuPatches
{
    [BepInPlugin("FISHMods.Patches", "Patches", "1.0.0")]
    public class Patches : BaseUnityPlugin
    {
        public static bool aacEnabled = true;
        public static bool allAACConsoleLogs = true;
        public static bool noInstantiate = false;
        internal static bool fakeLagEnabled2 = false;
        private void Awake()
        {
            ConsoleUtility.WriteToConsole($"Patches Activated!", ConsoleColor.Green);
        }

        [HarmonyPatch(typeof(GameObject))]
        [HarmonyPatch("CreatePrimitive", MethodType.Normal)]
        public class GameObjectPatch
        {
            private static void Postfix(GameObject __result)
            {
                __result.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                __result.GetComponent<Renderer>().material.color = Color.black;
            }
        }


        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("AntiTeleportTechnology", MethodType.Normal)]
        public class AntiTeleportTechnologyPatch
        {
            private static bool Prefix()
            {
                GorillaLocomotion.Player.Instance.teleportThresholdNoVel = int.MaxValue;
                return false;
            }
        }

        [HarmonyPatch(typeof(GorillaGameManager))]
        [HarmonyPatch("LaunchSlingshotProjectile", MethodType.Normal)]
        public class CrashPatch
        {
            private static bool Prefix(Vector3 slingshotLaunchLocation, Vector3 slingshotLaunchVelocity, int projHash, int trailHash, bool forLeftHand, int projectileCount, bool shouldOverrideColor, float colorR, float colorG, float colorB, float colorA, PhotonMessageInfo info)
            {
                if (info.Sender != PhotonNetwork.LocalPlayer)
                {
                    if (info.Sender.CustomProperties["mods"] != null)
                    {
                        if (trailHash != -1 && trailHash != 163790326 && trailHash != 1432124712)
                        {
                            return false;
                        }
                        return true;
                    }
                    return true;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(VRRig))]
        [HarmonyPatch("IncrementRPC", MethodType.Normal)]
        public class VRRigInrementRPC
        {
            private static bool Prefix(PhotonMessageInfo info, string sourceCall)
            {
                return false;
            }
        }

        [HarmonyPatch(typeof(VRRig))]
        [HarmonyPatch("OnDisable", MethodType.Normal)]
        public class VRRigOnDisable : MonoBehaviour
        {
            public static bool Prefix(VRRig __instance)
            {
                if (__instance == GorillaTagger.Instance.offlineVRRig)
                {
                    Traverse.Create(__instance).Field("initialized").SetValue(false);
                    __instance.muted = false;
                    Traverse.Create(__instance).Field("voiceAudio").SetValue(null);
                    Traverse.Create(__instance).Field("tempRig").SetValue(null);
                    Traverse.Create(__instance).Field("timeSpawned").SetValue(0f);
                    __instance.initializedCosmetics = false;
                    Traverse.Create(__instance).Field("tempMatIndex").SetValue(0);
                    __instance.setMatIndex = 0;
                    Traverse.Create(__instance).Field("creator").SetValue(null);
                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(PhotonNetwork))]
        [HarmonyPatch("OnEvent", MethodType.Normal)]
        internal class AntiKickPatch
        {
            private static bool Prefix(EventData photonEvent)
            {
                bool flag = photonEvent.Code == 203;
                bool flag2 = photonEvent.Code == 8;
                byte code = photonEvent.Code;
                bool flag3;
                if (flag || flag2)
                {
                    int senderId = photonEvent.Sender;
                    Player player = PhotonNetwork.PlayerList.FirstOrDefault((Player p) => p.ActorNumber == senderId);
                    string text = ((player != null) ? player.NickName : "Unknown");
                    flag3 = false;
                    if (flag)
                    {

                    }
                    if (flag2)
                    {

                    }
                }
                else
                {
                    flag3 = true;
                }
                return flag3;
            }
        }
        [HarmonyPatch(typeof(PhotonNetwork))]
        [HarmonyPatch("OnEvent", MethodType.Normal)]
        internal class AutoBlockedCodes
        {
            private static bool Prefix(EventData photonEvent)
            {
                bool flag = photonEvent.Code == byte.MaxValue;
                bool flag2 = photonEvent.Code == 224;
                if (flag2)
                {
                    int senderId = photonEvent.Sender;
                    Player player = PhotonNetwork.PlayerList.FirstOrDefault((Player p) => p.ActorNumber == senderId);
                    string text = ((player != null) ? player.NickName : "Unknown");
                    flag2 = false;
                    ConsoleUtility.WriteToConsole($"blocked an event! Someone (" + text + ") attempted to send unreliable data to you. This is NOT normal and was blocked!", ConsoleColor.Green);
                }
                else
                {
                    flag2 = true;
                }
                return flag2;
            }
        }
        [HarmonyPatch(typeof(GorillaNot), "CheckReports")]
        [HarmonyPrefix]
        private static bool OnGorillaNotEventUpdate()
        {
            return aacEnabled && false;
        }
        [HarmonyPatch(typeof(GorillaNot), "LogErrorCount")]
        [HarmonyPrefix]
        private static bool OnGorillaNotEventUpdate2()
        {
            return aacEnabled && false;
        }
        [HarmonyPatch(typeof(GorillaNot), "SendReport")]
        [HarmonyPrefix]
        private static bool OnGorillaNotEventUpdate3()
        {
            return aacEnabled && false;
        }
        [HarmonyPatch(typeof(GorillaNot), "IncrementRPCCall")]
        [HarmonyPrefix]
        private static bool OnGorillaNotEventUpdate4()
        {
            return aacEnabled && false;
        }
        [HarmonyPatch(typeof(GorillaNot), "IncrementRPCCallLocal")]
        [HarmonyPrefix]
        private static bool OnGorillaNotEventUpdate5()
        {
            return aacEnabled && false;
        }
        [HarmonyPatch(typeof(GorillaNot), "IncrementRPCTracker")]
        [HarmonyPrefix]
        private static bool OnGorillaNotEventUpdate7()
        {
            return aacEnabled && false;
        }
        [HarmonyPatch]
        internal class NetworkPatch
        {
            [HarmonyPatch(typeof(GorillaNetworkPublicTestsJoin), "GracePeriod")]
            [HarmonyPatch(typeof(GorillaNetworkPublicTestJoin2), "GracePeriod")]
            [HarmonyPrefix]
            private static bool Prefix()
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(PhotonNetwork))]
        [HarmonyPatch("OnEvent", MethodType.Normal)]
        internal class NoInstantiate
        {
            private static bool Prefix(EventData photonEvent)
            {
                bool flag4;
                if (photonEvent.Code == 202 && noInstantiate)
                {
                    int senderId = photonEvent.Sender;
                    Player player = PhotonNetwork.PlayerList.FirstOrDefault((Player p) => p.ActorNumber == senderId);
                    if (player != null)
                    {
                        string nickName = player.NickName;
                    }
                    if (player == null)
                    {
                        return true;
                    }
                    flag4 = false;
                }
                else
                {
                    flag4 = true;
                }
                return flag4;
            }
        }
        [HarmonyPatch]
        public class NormalizePatch
        {
            [HarmonyPatch(typeof(GorillaPlayerScoreboardLine), "NormalizeName")]
            [HarmonyPatch(typeof(GorillaPlayerScoreboardLine), "UpdateLine")]
            [HarmonyPatch(typeof(GorillaPlayerScoreboardLine), "GetUserLevel")]
            [HarmonyPrefix]
            private static bool Prefix()
            {
                bool fakeLagEnabled = fakeLagEnabled2;
                return !fakeLagEnabled || true;
            }
        }
        [HarmonyPatch(typeof(LegalAgreements))]
        [HarmonyPatch("Update", MethodType.Normal)]
        internal class NoTOSPatch
        {
            public static bool Prefix()
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(LegalAgreements))]
        [HarmonyPatch("Start", MethodType.Normal)]
        internal class NoTOSPatch2
        {
            public static bool Prefix()
            {
                return false;
            }
        }
    }
}