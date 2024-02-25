using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using Color = UnityEngine.Color;
using Object = UnityEngine.Object;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using GorillaLocomotion.Gameplay;
using GorillaLocomotion.Swimming;
using System.Net;
using System.Diagnostics;
using GorillaNetworking;
using System.Collections;
using BepInEx;
using System.Collections.Specialized;
using Valve.Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Text = UnityEngine.UI.Text;
using UnityEngine.XR;
using static Fish_Menu.MainMenuPatches.Patches;
using PlayFab.ClientModels;
using PlayFab;
using System.Text.RegularExpressions;
using GorillaTag;
using Facebook.WitAi.Utilities;
using static System.Net.Mime.MediaTypeNames;
using ExitGames.Client.Photon.StructWrapping;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine.UIElements;

namespace Fish_Menu.MainMenu
{
    #region Loader
    [BepInPlugin("com.notfishvr.fishmenu", "notfishvr", "1.0.0")]
    public class Loader : BaseUnityPlugin
    {
        public void FixedUpdate()
        {
            Debug.unityLogger.logEnabled = false;
            if (!GameObject.Find("Loader") && GorillaLocomotion.Player.hasInstance)
            {
                GameObject Loader = new GameObject("Loader");
                Loader.AddComponent<MenuPatch>();
                Loader.AddComponent<ControllerInput>();
                Loader.AddComponent<RigManager>();
                Loader.AddComponent<NotifiLib>();
                Loader.AddComponent<RoomManager>();
            }
        }
    }
    #endregion
    #region HarmonyPatch
    [BepInPlugin(modGUID, modName, modVersion)]
    [Description(modVersion)]
    public class HarmonyPatch : BaseUnityPlugin
    {
        public void Awake()
        {
            Harmony harmony = new Harmony(modName);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        private const string modGUID = "FISH.Menu";
        private const string modName = "FISH.Menu";
        public const string modVersion = "1.0.0";
    }
    #endregion
    #region MainMenu
    [HarmonyLib.HarmonyPatch(typeof(GorillaLocomotion.Player), "LateUpdate", MethodType.Normal)]
    public class MenuPatch  : MonoBehaviourPunCallbacks
    {
        #region Buttons
        #region MainButtons
        public static string[] buttons = new string[]
        {
            "Settings",                  // 0
            "Basic Mods",                // 1
            "Rope Mods",                 // 2
            "Spam Rpc Mods",             // 3
            "Bug Mods",                  // 4
            "Tag Mods",                  // 5
            "Mic Mods",                  // 6
            "Halloween Mods",            // 7
            "OP Mods"                    // 8
        };
        #endregion
        #region SettingsButtons
        public static string[] Settingsbuttons = new string[]
        {
            "Back",             // 0
            "Save Settings",    // 1
            "Load Settings",    // 2
            "ESP Color:",       // 3
            "Bone ESP Color:",  // 4
            "Fly Speed:",       // 5
            "Platforms Type:",  // 6
            "Platforms Color:", // 7
            "TP Gun Speed:",    // 8
            "Slingshot = ",     // 9
            "Bug Type:"         // 10
        };
        #endregion
        #region BasicButtons
        public static string[] Basicbuttons = new string[]
        {
            "Back",                      // 0
            "Disconnect",                // 1
            "Join Random Room",          // 2
            "Fly",                       // 3
            "No Clip",                   // 4
            "Platform",                  // 5
            "Image Platform",            // 6
            "Speed Boost",               // 7
            "Ghost Monkey",              // 8
            "Invs Monkey",               // 9
            "Beacons",                   // 10
            "ESP",                       // 11
            "Bone ESP",                  // 12
            "TP Gun",                    // 13
        };
        #endregion
        #region RopeButtons
        public static string[] Ropebuttons = new string[]
        {
            "Back",                      // 0
            "Rope Up",                   // 1
            "Rope Down",                 // 2
            "Ropes To Self",             // 3
            "Rope Spaz"                  // 4
        };
        #endregion
        #region SpamRpcbuttons
        public static string[] SpamRpcbuttons = new string[]
        {
            "Back",                      // 0
            "Water Spam",                // 1
            "Water ThrowUp",             // 2
            "Water Aura",                // 3
            "Water Bending",             // 4
            "Water Gun",                 // 5
            "Water Balloon Spam",        // 6
            "Water Balloon Gun",         // 7
            "SnowBall Spam",             // 8
            "SnowBall Gun",              // 9
            "Rock Spam",                 // 10
            "Rock Spam Gun"              // 11
        };
        #endregion
        #region Bugbuttons
        public static string[] Bugbuttons = new string[]
        {
            "Back",                      // 0
            "Invis Bug",                 // 1 // can be edit on Setting Page
            "Bug Gun",                   // 2 // can be edit on Setting Page
            "Big Bug",                   // 3 // can be edit on Setting Page
            "Grab Bug",                  // 4 // can be edit on Setting Page
        };
        #endregion
        #region Tagbuttons
        public static string[] Tagbuttons = new string[]
        {
            "Back",                      // 0
            "Tag All",                   // 1
            "Tag Gun",                   // 2
            "Mat All",                   // 3
            "Mat Self"                   // 4
        };
        #endregion
        #region Micbuttons
        public static string[] Micbuttons = new string[]
        {
            "Back",                      // 0
            "Slingshot = Water Balloon", // 1 // can be edit on Setting Page
            "Span Hit Target",           // 2
            "Kick Shiba Users Gun",      // 3
            "Lag Shiba Users Gun",       // 4
            "Move Shiba Users Gun",      // 5
            "Silent Aim",                // 6
            "Hot Pepper Crash",          // 7
            "Earape Aura",               // 8
            "Fuck Game [M]"              // 9
        };
        #endregion
        #region Halloweenbuttons
        public static string[] Halloweenbuttons = new string[]
        {
            "Back",                       // 0
            "Spawn Lucy",                 // 1
            "Set Lucy Target Gun",        // 2
            "Set Lucy Grabbed Player Gun",// 3
            "Lucy TP Gun",                // 4
            "Broom Stick TP Gun",         // 5
            "Fast Broom Stick",           // 6
            "Slow Broom Stick"            // 7
        };
        #endregion
        #region OPbuttons
        public static string[] OPbuttons = new string[]
        {
            "Back",                      // 0
            "Anit ban",                  // 1
            "Acid All",                  // 2
            "Acid Gun",                  // 3
            "Acid Mat Spam",             // 4
            "Crash All",                 // 5
            "Score Board FUp",           // 6
            "Rise Lava",                 // 7
            "Drain Lava",                // 8
            "Erupt Lava",                // 9
        };
        #endregion
        #endregion
        #region buttonsActive
        public static bool[] buttonsActive = new bool[111]
{
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false
};
        public static bool[] SettingsButtonsActive = new bool[111]
    {
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false
    };
        public static bool[] BasicButtonsActive = new bool[111]
    {
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false
    };
        public static bool[] RopeButtonsActive = new bool[111]
   {
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false
   };
        public static bool[] SpamRpcButtonsActive = new bool[111]
  {
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false
  };
        public static bool[] BugButtonsActive = new bool[111]
 {
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false
 };
        public static bool[] TagButtonsActive = new bool[111]
 {
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false
 };
        public static bool[] MicButtonsActive = new bool[111]
 {
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false
 };
        public static bool[] HalloweenButtonsActive = new bool[111]
 {
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false
 };
        public static bool[] OPButtonsActive = new bool[111]
{
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false,
         false
};
        #endregion
        #region Prefix
        private static void Prefix()
        {
            try
            {
                #region Menu
                UpdateMaterialColors();
                //Instance.Thing2();
                /*gripDownL = ControllerInput.LeftGrip;
                gripDownR = ControllerInput.RightGrip;
                triggerDownL = ControllerInput.LeftTrigger;
                triggerDownR = ControllerInput.RightTrigger;
                abuttonDown = ControllerInput.RightPrimary;
                bbuttonDown = ControllerInput.RightSecondary;
                xbuttonDown = ControllerInput.LeftPrimary;
                ybuttonDown = ControllerInput.LeftSecondary; */
                if (once)
                {
                    MenuLoaded = true;
                    once = false;
                    Mods.Utils.CreateConfigFileIfNotExists("FISHModsCustomInv.txt", "https://bit.ly/NIKOModsDiscordServer");
                    Mods.Utils.CreateConfigFileIfNotExists("FISHModsNOTCustomInv.txt", "https://bit.ly/NIKOModsInv2");
                    MainMenuRef = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    MainMenuRef.transform.parent = GorillaLocomotion.Player.Instance.rightControllerTransform;
                    string folderPath = "FISH_Mods_Config";
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                }
                if (!MenuPatch.consoleStartAttempt)
                {
                    ConsoleUtility.OpenConsoleWindow();
                    if (MenuPatch.l)
                    {
                        ConsoleUtility.WriteToConsole("[INFO]: OpenConsoleWindow();", ConsoleColor.White);
                    }

                    ConsoleUtility.WriteToConsole(" ", ConsoleColor.White);
                    MenuPatch.consoleStartAttempt = true;
                    Debug.Log("Attempted to start the console, awaiting for further notice.");
                }
                if (!MenuPatch.consoleStartAttempt2)
                {
                    if (GorillaLocomotion.Player.hasInstance)
                    {
                        Task.Delay(999);
                        Task.Delay(999);
                        Task.Delay(999);
                        Task.Delay(999);
                        ConsoleUtility.WriteToConsole("  _   _ ___ _  _____  ", ConsoleColor.Blue);
                        ConsoleUtility.WriteToConsole(" | \\ | |_ _| |/ / _ \\ ", ConsoleColor.Blue);
                        ConsoleUtility.WriteToConsole(" |  \\| || || ' / | | |", ConsoleColor.Blue);
                        ConsoleUtility.WriteToConsole(" | |\\  || || . \\ |_| |", ConsoleColor.Blue);
                        ConsoleUtility.WriteToConsole(" |_| \\_|___|_|\\_\\___/ ", ConsoleColor.Blue);
                        ConsoleUtility.WriteToConsole("", ConsoleColor.Blue);
                        ConsoleUtility.WriteToConsole(" ", ConsoleColor.White);
                        ConsoleUtility.WriteToConsole("  __  __  ___  ____    __  __ _____ _   _ _   _ ", ConsoleColor.Blue);
                        ConsoleUtility.WriteToConsole(" |  \\/  |/ _ \\|  _ \\  |  \\/  | ____| \\ | | | | |", ConsoleColor.Blue);
                        ConsoleUtility.WriteToConsole(" | |\\/| | | | | | | | | |\\/| |  _| |  \\| | | | |", ConsoleColor.Blue);
                        ConsoleUtility.WriteToConsole(" | |  | | |_| | |_| | | |  | | |___| |\\  | |_| |", ConsoleColor.Blue);
                        ConsoleUtility.WriteToConsole(" |_|  |_|\\___/|____/  |_|  |_|_____|_| \\_|\\___/ ", ConsoleColor.Blue);
                        MenuPatch.consoleStartAttempt2 = true;
                    }
                }
                GorillaLocomotion.Player __instance = GorillaLocomotion.Player.Instance;
                List<UnityEngine.XR.InputDevice> list = new List<UnityEngine.XR.InputDevice>();
                if (ControllerInputPoller.instance.leftControllerSecondaryButton)
                {
                    if (menu == null)
                    {
                        Draw();
                        if (fingerButtonPresser == null)
                        {
                            fingerButtonPresser = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            fingerButtonPresser.transform.SetParent(GorillaLocomotion.Player.Instance.rightControllerTransform);
                            fingerButtonPresser.name = "MenuClicker";
                            fingerButtonPresser.transform.localPosition = new Vector3(0f, -0.1f, 0f);
                            fingerButtonPresser.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                        }
                    }
                    menu.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position;
                    menu.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
                }
                else if (menu != null)
                {
                    Object.Destroy(menu);
                    menu = null;
                    Object.Destroy(fingerButtonPresser);
                    fingerButtonPresser = null;
                }
                #endregion
                #region Main buttonsActive
                // Settings
                if (buttonsActive[0] == true)
                {
                    NumberForPage = 2;
                    buttonsActive[0] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                // Basic
                if (buttonsActive[1] == true)
                {
                    NumberForPage = 3;
                    buttonsActive[1] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                // Rope
                if (buttonsActive[2] == true)
                {
                    NumberForPage = 4;
                    buttonsActive[2] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                // Spam Rpc
                if (buttonsActive[3] == true)
                {
                    NumberForPage = 5;
                    buttonsActive[3] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                // Bug
                if (buttonsActive[4] == true)
                {
                    NumberForPage = 6;
                    buttonsActive[4] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                // Tag
                if (buttonsActive[5] == true)
                {
                    NumberForPage = 7;
                    buttonsActive[5] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                // Mic
                if (buttonsActive[6] == true)
                {
                    NumberForPage = 8;
                    buttonsActive[6] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                // Lucy
                if (buttonsActive[7] == true)
                {
                    NumberForPage = 99;
                    buttonsActive[7] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                // Owner
                if (buttonsActive[8] == true)
                {
                    NumberForPage = 69;
                    buttonsActive[8] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                #endregion
                #region Setting buttonsActive
                    #region UpdateText
                    // ESP Color
                    string[] colorNames = { "Tag", "Black", "Red", "Blue", "Yellow", "Green", "Purple", "White" };
                int[] espcolorIndices = { 0, 1, 2, 3, 4, 5, 6, 7 };
                int espnumColors = espcolorIndices.Length;
                int espcurrentColorIndex = ESpInt;
                Settingsbuttons[3] = "ESP Color: " + colorNames[espcurrentColorIndex];

                // Bone ESP Color
                string[] BonecolorNames = { "Tag", "Black", "Red", "Blue", "Yellow", "Green", "Purple", "White" };
                int[] BoneespcolorIndices = { 0, 1, 2, 3, 4, 5, 6, 7 };
                int BoneespnumColors = BoneespcolorIndices.Length;
                int BoneespcurrentColorIndex = BoneESpInt;
                Settingsbuttons[4] = "Bone ESP Color: " + BonecolorNames[BoneespcurrentColorIndex];

                // Fly Speed
                string[] flySpeedNames = { "Super Slow", "Slow", "Normal", "Fast", "Super Fast" };
                float[] flyIndices = { 0, 1, 2, 3, 4 };
                int flySpeedCount = flyIndices.Length;
                int currentFlySpeedIndex = SpeedCount;
                Settingsbuttons[5] = "Fly Speed: " + flySpeedNames[currentFlySpeedIndex];

                // Platforms Type
                string[] PlatformsTypeNames = { "Normal", "Sticky", "Capsule", "Cylinder", "Plane", "Quad" };
                float[] PlatformsTypeIndices = { 0, 1, 2, 3, 4, 5 };
                int PlatformsTypeCount = PlatformsTypeIndices.Length;
                int currentPlatformsTypeIndex = platCountType;
                Settingsbuttons[6] = "Platforms Type: " + PlatformsTypeNames[currentPlatformsTypeIndex];

                // Platforms Color
                string[] PlatformscolorNames = { "Black", "Red", "Blue", "Yellow", "Green", "Purple", "White" };
                int[] PlatformsespcolorIndices = { 0, 1, 2, 3, 4, 5, 6 };
                int PlatformsespnumColors = PlatformsespcolorIndices.Length;
                int PlatformsespcurrentColorIndex = platCountColor;
                Settingsbuttons[7] = "Platforms Color: " + PlatformscolorNames[PlatformsespcurrentColorIndex];

                // TP Gun Speed
                string[] TPSpeedNames = { "Super Slow", "Slow", "Normal", "Fast", "Super Fast" };
                float[] TPIndices = { 0, 1, 2, 3, 4 };
                int TPSpeedCount2 = TPIndices.Length;
                int currentTPSpeedIndex = TPSpeedCount;
                Settingsbuttons[8] = "TP Gun Speed: " + TPSpeedNames[currentTPSpeedIndex];

                // Slingshot = 
                string[] SlingshotTypeNames = { "Water Balloon", "Snowball", "Heart", "Leaf", "Deadshot", "Cloud", "Ice" };
                float[] SlingshotTypeIndices = { 0, 1, 2, 3, 4, 5, 6 };
                int SlingshotTypeCount = SlingshotTypeIndices.Length;
                int currentSlingshotTypeIndex = SlingshotCountType;
                Settingsbuttons[9] = "Slingshot = " + SlingshotTypeNames[currentSlingshotTypeIndex];

                // Bug Type
                string[] BugTypeNames = { "Bug", "Balloon", "BeachBall", "Monsters", "Bat" };
                float[] BugTypeIndices = { 0, 1, 2, 3, 4 };
                int BugTypeCount = BugTypeIndices.Length;
                int currentBugTypeIndex = BugCountType;
                Settingsbuttons[10] = "Bug Type: " + BugTypeNames[currentBugTypeIndex];
                #endregion
                #region SettingsButtonsActive
                if (SettingsButtonsActive[3] == true)
                {
                    espcurrentColorIndex = (espcurrentColorIndex + 1) % espnumColors;
                    ESpInt = espcurrentColorIndex;
                    Settingsbuttons[3] = "ESP Color: " + colorNames[espcurrentColorIndex];
                    Color[] colors = { Color.clear, Color.black, Color.red, Color.blue, Color.yellow, Color.green, Color.magenta, Color.white };
                    ESPColor = colors[espcurrentColorIndex];
                    SettingsButtonsActive[3] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (SettingsButtonsActive[4] == true)
                {
                    BoneespcurrentColorIndex = (BoneespcurrentColorIndex + 1) % BoneespnumColors;
                    BoneESpInt = BoneespcurrentColorIndex;
                    Settingsbuttons[4] = "Bone ESP Color: " + BonecolorNames[BoneespcurrentColorIndex];
                    Color[] Bonecolors = { Color.clear, Color.black, Color.red, Color.blue, Color.yellow, Color.green, Color.magenta, Color.white };
                    ESPColor = Bonecolors[BoneespcurrentColorIndex];
                    SettingsButtonsActive[4] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (SettingsButtonsActive[5] == true)
                {
                    currentFlySpeedIndex = (currentFlySpeedIndex + 1) % flySpeedCount;
                    SpeedCount = currentFlySpeedIndex;
                    Settingsbuttons[5] = "Fly Speed: " + flySpeedNames[currentFlySpeedIndex];
                    float[] FlySpeedf = { 0.1f, 5f, 15f, 30f, 150f };
                    FlySpeed = FlySpeedf[currentFlySpeedIndex];
                    SettingsButtonsActive[5] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (SettingsButtonsActive[6] == true)
                {
                    currentPlatformsTypeIndex = (currentPlatformsTypeIndex + 1) % PlatformsTypeCount;
                    platCountType = currentPlatformsTypeIndex;
                    Settingsbuttons[6] = "Platforms Type: " + PlatformsTypeNames[currentPlatformsTypeIndex];
                    float[] Platformsf = { 0f, 1f, 2f, 3f, 4f, 5f };
                    plattype = Platformsf[currentPlatformsTypeIndex];
                    SettingsButtonsActive[6] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (SettingsButtonsActive[7] == true)
                {
                    PlatformsespcurrentColorIndex = (PlatformsespcurrentColorIndex + 1) % PlatformsespnumColors;
                    platCountColor = PlatformsespcurrentColorIndex;
                    Settingsbuttons[7] = "Platforms Color: " + PlatformscolorNames[PlatformsespcurrentColorIndex];
                    Color[] platcolors = { Color.black, Color.red, Color.blue, Color.yellow, Color.green, Color.magenta, Color.white };
                    Platcolor = platcolors[PlatformsespcurrentColorIndex];
                    SettingsButtonsActive[7] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (SettingsButtonsActive[8] == true)
                {
                    currentTPSpeedIndex = (currentTPSpeedIndex + 1) % TPSpeedCount2;
                    TPSpeedCount = currentTPSpeedIndex;
                    Settingsbuttons[8] = "TP Gun Speed: " + TPSpeedNames[currentTPSpeedIndex];
                    float[] TPSpeedf = { 5f, 10f, 35f, 68f, 250f };
                    TPGunSpeed = TPSpeedf[currentTPSpeedIndex];
                    SettingsButtonsActive[8] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (SettingsButtonsActive[9] == true)
                {
                    currentSlingshotTypeIndex = (currentSlingshotTypeIndex + 1) % SlingshotTypeCount;
                    SlingshotCountType = currentSlingshotTypeIndex;
                    Settingsbuttons[9] = "Slingshot = " + SlingshotTypeNames[currentSlingshotTypeIndex];
                    float[] Slingshotf = { 0f, 1f, 2f, 3f, 4f, 5f, 6f };
                    SlingshotType = Slingshotf[currentSlingshotTypeIndex];
                    SettingsButtonsActive[9] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (SettingsButtonsActive[10] == true)
                {
                    currentBugTypeIndex = (currentBugTypeIndex + 1) % BugTypeCount;
                    BugCountType = currentBugTypeIndex;
                    Settingsbuttons[10] = "Bug Type: " + BugTypeNames[currentBugTypeIndex];
                    float[] Bugf = { 0f, 1f, 2f, 3f, 4f };
                    BugType = Bugf[currentBugTypeIndex];
                    SettingsButtonsActive[10] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (SettingsButtonsActive[0] == true)
                {
                    NumberForPage = 1;
                    SettingsButtonsActive[0] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (SettingsButtonsActive[1] == true)
                {
                    SaveSettings();
                    SettingsButtonsActive[1] = (bool)false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                    SettingsPageOn = true;
                }
                if (SettingsButtonsActive[2] == true)
                {
                    LoadSettings();
                    SettingsButtonsActive[2] = (bool)false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                    SettingsPageOn = true;
                }
                #endregion
                #endregion
                #region Basic buttonsActive
                if (BasicButtonsActive[0] == true)
                {
                    NumberForPage = 1;
                    BasicButtonsActive[0] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (BasicButtonsActive[1] == true)
                {
                    PhotonNetwork.Disconnect();
                    NotifiLib.SendNotification("Your Have Been Disconnected", Color.green);
                    BasicButtonsActive[1] = (bool)false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (BasicButtonsActive[2] == true)
                {
                    PhotonNetwork.JoinRandomRoom();
                    NotifiLib.SendNotification("You Have Join Random Room", Color.green);
                    BasicButtonsActive[2] = (bool)false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (BasicButtonsActive[3] == true)
                {
                    Mods.MainStuff.BasicMods.Fly();
                    NotifiLib.SendNotification("Fly Is On", Color.green);
                }
                if (BasicButtonsActive[4] == true)
                {
                    Mods.MainStuff.BasicMods.NoClip();
                    NotifiLib.SendNotification("No Clip Is On", Color.green);
                }
                if (BasicButtonsActive[5] == true)
                {
                    Mods.MainStuff.BasicMods.PlatformMonke();
                    NotifiLib.SendNotification("Platforms Is On", Color.green);
                }
                if (BasicButtonsActive[6] == true)
                {
                    Mods.MainStuff.BasicMods.ImagePlatformMonke();
                    NotifiLib.SendNotification("Image Platform Is On", Color.green);
                }
                if (BasicButtonsActive[7] == true)
                {
                    Mods.MainStuff.BasicMods.SpeedBoost();
                    NotifiLib.SendNotification("Speed Boost Is On", Color.green);
                }
                if (BasicButtonsActive[8] == true)
                {
                    Mods.MainStuff.BasicMods.GhostMonkey();
                    NotifiLib.SendNotification("Ghost Monkey Is On", Color.green);
                }
                if (BasicButtonsActive[9] == true)
                {
                    Mods.MainStuff.BasicMods.InvsMonkey();
                    NotifiLib.SendNotification("Invs Monkey Is On", Color.green);
                }
                if (BasicButtonsActive[10] == true)
                {
                    Mods.MainStuff.BasicMods.Beacons();
                    NotifiLib.SendNotification("Beacons Is On", Color.green);
                }
                if (BasicButtonsActive[11] == true)
                {
                    NotifiLib.SendNotification("ESP Is On", Color.green);
                }
                if (BasicButtonsActive[12] == true)
                {
                    Mods.MainStuff.BasicMods.BoneESP();
                    noesp = true;
                    NotifiLib.SendNotification("Bone ESP Is On", Color.green);
                }
                else if (noesp)
                {
                    foreach (VRRig vrrig4 in GorillaParent.instance.vrrigs)
                    {
                        for (int num63 = 0; num63 < Enumerable.Count<int>(bones); num63 += 2)
                        {
                            if (vrrig4 != null)
                            {
                                if (vrrig4.mainSkin.bones[bones[num63]].gameObject.GetComponent<LineRenderer>())
                                {
                                    UnityEngine.Object.Destroy(vrrig4.mainSkin.bones[bones[num63]].gameObject.GetComponent<LineRenderer>());
                                }
                                if (vrrig4.head.rigTarget.gameObject.GetComponent<LineRenderer>())
                                {
                                    UnityEngine.Object.Destroy(vrrig4.head.rigTarget.gameObject.GetComponent<LineRenderer>());
                                }
                            }
                        }
                    }
                    noesp = false;
                }
                if (BasicButtonsActive[13] == true)
                {
                    Mods.MainStuff.BasicMods.TPGun();
                    NotifiLib.SendNotification("TP Gun Is On", Color.green);
                }
                #endregion
                #region Rope buttonsActive
                if (RopeButtonsActive[0] == true)
                {
                    NumberForPage = 1;
                    RopeButtonsActive[0] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (RopeButtonsActive[1] == true)
                {
                    Mods.MainStuff.OpMods.Rope(new Vector3(0f, 999f, 0f));
                    NotifiLib.SendNotification("Rope Up Is On", Color.green);
                }
                if (RopeButtonsActive[2] == true)
                {
                    Mods.MainStuff.OpMods.Rope(new Vector3(0f, -999f, 0f));
                    NotifiLib.SendNotification("Rope Down Is On", Color.green);
                }
                if (RopeButtonsActive[3] == true)
                {
                    // Mods.MainStuff.OpMods.Rope(GorillaLocomotion.Player.Instance.bodyCollider.center);
                    Vector3 center = GorillaLocomotion.Player.Instance.bodyCollider.center;
                    Mods.MainStuff.OpMods.Rope(new Vector3(center.x, center.y, center.z));
                    NotifiLib.SendNotification("Ropes To Self Is On", Color.green);
                }
                if (RopeButtonsActive[4] == true)
                {
                    foreach (GorillaRopeSwing gorillaRopeSwing in Object.FindObjectsOfType<GorillaRopeSwing>())
                    {
                        Vector3 position5 = GorillaLocomotion.Player.Instance.transform.position;
                        Vector3 normalized3 = (position5 - gorillaRopeSwing.transform.position).normalized;
                        float d3 = 100f;
                        Vector3 volocity = normalized3 * d3;
                        Mods.MainStuff.OpMods.Rope(volocity);
                    }
                    NotifiLib.SendNotification("Rope Spaz Is On", Color.green);
                }
                #endregion
                #region Spam Rpc buttonsActive
                if (SpamRpcButtonsActive[0] == true)
                {
                    NumberForPage = 1;
                    SpamRpcButtonsActive[0] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (SpamRpcButtonsActive[1] == true)
                {
                    Mods.MainStuff.OpMods.WaterStuff.Water();
                    NotifiLib.SendNotification("Water Spam Is On", Color.green);
                }
                if (SpamRpcButtonsActive[2] == true)
                {
                    Mods.MainStuff.OpMods.WaterStuff.WaterThrowUp();
                    NotifiLib.SendNotification("Water ThrowUp Is On", Color.green);
                }
                if (SpamRpcButtonsActive[3] == true)
                {
                    Mods.MainStuff.OpMods.WaterStuff.WaterAura();
                    NotifiLib.SendNotification("Water Aura Is On", Color.green);
                }
                if (SpamRpcButtonsActive[4] == true)
                {
                    Mods.MainStuff.OpMods.WaterStuff.WaterBending();
                    NotifiLib.SendNotification("Water Bending Is On", Color.green);
                }
                if (SpamRpcButtonsActive[5] == true)
                {
                    Mods.MainStuff.OpMods.WaterStuff.WaterGun();
                    NotifiLib.SendNotification("Water Gun Is On", Color.green);
                }
                if (SpamRpcButtonsActive[6] == true)
                {
                    if (ControllerInput.RightGrip && (double)Time.time > (double)WaterBalloonTimer + 0.085)
                    {
                        Color color2 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                        Instance.Projectile(-1674517839, GorillaLocomotion.Player.Instance.currentVelocity, GorillaLocomotion.Player.Instance.rightControllerTransform.position, color2, -1);
                        WaterBalloonTimer = Time.time;
                    }
                    NotifiLib.SendNotification("Water Balloon Spam Is On", Color.green);
                }
                if (SpamRpcButtonsActive[7] == true)
                {
                    Instance.SpamGun(-1674517839, WaterBalloonTimer);
                    NotifiLib.SendNotification("Water Balloon Gun Is On", Color.green);
                }
                if (SpamRpcButtonsActive[8] == true)
                {
                    if (ControllerInput.RightGrip && (double)Time.time > (double)SnowBallTimer + 0.085)
                    {
                        Color color2 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                        Instance.Projectile(-675036877, GorillaLocomotion.Player.Instance.currentVelocity, GorillaLocomotion.Player.Instance.rightControllerTransform.position, color2, -1);
                        SnowBallTimer = Time.time;
                    }
                    NotifiLib.SendNotification("Snow Ball Spam Is On", Color.green);
                }
                if (SpamRpcButtonsActive[9] == true)
                {
                    Instance.SpamGun(-675036877, SnowBallTimer);
                    NotifiLib.SendNotification("Snow Ball Gun Is On", Color.green);
                }
                if (SpamRpcButtonsActive[10] == true)
                {
                    if (ControllerInput.RightGrip && (double)Time.time > (double)RockSpamTimer + 0.085)
                    {
                        Color color2 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                        Instance.Projectile(-622368518, GorillaLocomotion.Player.Instance.currentVelocity, GorillaLocomotion.Player.Instance.rightControllerTransform.position, color2, -1);
                        RockSpamTimer = Time.time;
                    }
                    NotifiLib.SendNotification("Rock Spam Is On", Color.green);
                }
                if (SpamRpcButtonsActive[11] == true)
                {
                    Instance.SpamGun(-622368518, RockSpamTimer);
                    NotifiLib.SendNotification("Rock Spam Gun Is On", Color.green);
                }
                #endregion
                #region Bug buttonsActive
                #region BugType
                if (BugButtonsActive[0] == true)
                {
                    NumberForPage = 1;
                    BugButtonsActive[0] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (BugType == 0f)
                {
                    string NAME = "Bug";
                    if (BugButtonsActive[1] == true)
                    {
                        Mods.MainStuff.BasicMods.BugStuff.InvisBug();
                        NotifiLib.SendNotification($"Invis {NAME} Is On", Color.green);
                        Bugbuttons[1] = $"Invis {NAME}";
                    }
                    if (BugButtonsActive[2] == true)
                    {
                        Mods.MainStuff.BasicMods.BugStuff.BugGun();
                        NotifiLib.SendNotification($"{NAME} Gun Is On", Color.green);
                        Bugbuttons[2] = $"{NAME} Gun";
                    }
                    if (BugButtonsActive[3] == true)
                    {
                        Mods.MainStuff.BasicMods.BugStuff.BigBug();
                        NotifiLib.SendNotification($"Big {NAME} Is On", Color.green);
                        Bugbuttons[3] = $"Big {NAME}";
                    }
                    if (BugButtonsActive[4] == true)
                    {
                        Mods.MainStuff.BasicMods.BugStuff.GrabBug();
                        NotifiLib.SendNotification($"Grab {NAME} Is On", Color.green);
                        Bugbuttons[4] = $"Grab {NAME}";
                    }
                }
                if (BugType == 1f)
                {
                    string NAME = "Balloon";
                    if (BugButtonsActive[1] == true)
                    {
                        Mods.MainStuff.BasicMods.BalloonStuff.InvisBalloon();
                        NotifiLib.SendNotification($"Invis {NAME} Is On", Color.green);
                        Bugbuttons[1] = $"Invis {NAME}";
                    }
                    if (BugButtonsActive[2] == true)
                    {
                        Mods.MainStuff.BasicMods.BalloonStuff.BalloonGun();
                        NotifiLib.SendNotification($"{NAME} Gun Is On", Color.green);
                        Bugbuttons[2] = $"{NAME} Gun";
                    }
                    if (BugButtonsActive[3] == true)
                    {
                        Mods.MainStuff.BasicMods.BalloonStuff.BigBalloon();
                        NotifiLib.SendNotification($"Big {NAME} Is On", Color.green);
                        Bugbuttons[3] = $"Big {NAME}";
                    }
                    if (BugButtonsActive[4] == true)
                    {
                        Mods.MainStuff.BasicMods.BalloonStuff.GrabBalloon();
                        NotifiLib.SendNotification($"Grab {NAME} Is On", Color.green);
                        Bugbuttons[4] = $"Grab {NAME}";
                    }
                }
                if (BugType == 2f)
                {
                    string NAME = "BeachBall";
                    if (BugButtonsActive[1] == true)
                    {
                        Mods.MainStuff.BasicMods.BeachBallStuff.InvisBeachBall();
                        NotifiLib.SendNotification($"Invis {NAME} Is On", Color.green);
                        Bugbuttons[1] = $"Invis {NAME}";
                    }
                    if (BugButtonsActive[2] == true)
                    {
                        Mods.MainStuff.BasicMods.BeachBallStuff.BeachBallGun();
                        NotifiLib.SendNotification($"{NAME} Gun Is On", Color.green);
                        Bugbuttons[2] = $"{NAME} Gun";
                    }
                    if (BugButtonsActive[3] == true)
                    {
                        Mods.MainStuff.BasicMods.BeachBallStuff.BigBeachBall();
                        NotifiLib.SendNotification($"Big {NAME} Is On", Color.green);
                        Bugbuttons[3] = $"Big {NAME}";
                    }
                    if (BugButtonsActive[4] == true)
                    {
                        Mods.MainStuff.BasicMods.BeachBallStuff.GrabBeachBall();
                        NotifiLib.SendNotification($"Grab {NAME} Is On", Color.green);
                        Bugbuttons[4] = $"Grab {NAME}";
                    }
                }
                if (BugType == 3f)
                {
                    string NAME = "Monsters";
                    if (BugButtonsActive[1] == true)
                    {
                        Mods.MainStuff.BasicMods.MonstersStuff.InvisMonsters();
                        NotifiLib.SendNotification($"Invis {NAME} Is On", Color.green);
                        Bugbuttons[1] = $"Invis {NAME}";
                    }
                    if (BugButtonsActive[2] == true)
                    {
                        Mods.MainStuff.BasicMods.MonstersStuff.MonstersGun();
                        NotifiLib.SendNotification($"{NAME} Gun Is On", Color.green);
                        Bugbuttons[2] = $"{NAME} Gun";
                    }
                    if (BugButtonsActive[3] == true)
                    {
                        Mods.MainStuff.BasicMods.MonstersStuff.BigMonsters();
                        NotifiLib.SendNotification($"Big {NAME} Is On", Color.green);
                        Bugbuttons[3] = $"Big {NAME}";
                    }
                    if (BugButtonsActive[4] == true)
                    {
                        Mods.MainStuff.BasicMods.MonstersStuff.GrabMonsters();
                        NotifiLib.SendNotification($"Grab {NAME} Is On", Color.green);
                        Bugbuttons[4] = $"Grab {NAME}";
                    }
                }
                if (BugType == 4f)
                {
                    string NAME = "Bat";
                    if (BugButtonsActive[1] == true)
                    {
                        Mods.MainStuff.BasicMods.BatStuff.InvisBat();
                        NotifiLib.SendNotification($"Invis {NAME} Is On", Color.green);
                        Bugbuttons[1] = $"Invis {NAME}";
                    }
                    if (BugButtonsActive[2] == true)
                    {
                        Mods.MainStuff.BasicMods.BatStuff.BatGun();
                        NotifiLib.SendNotification($"{NAME} Gun Is On", Color.green);
                        Bugbuttons[2] = $"{NAME} Gun";
                    }
                    if (BugButtonsActive[3] == true)
                    {
                        Mods.MainStuff.BasicMods.BatStuff.BigBat();
                        NotifiLib.SendNotification($"Big {NAME} Is On", Color.green);
                        Bugbuttons[3] = $"Big {NAME}";
                    }
                    if (BugButtonsActive[4] == true)
                    {
                        Mods.MainStuff.BasicMods.BatStuff.GrabBat();
                        NotifiLib.SendNotification($"Grab {NAME} Is On", Color.green);
                        Bugbuttons[4] = $"Grab {NAME}";
                    }
                }
                if (BugType == 0f)
                {
                    string NAME = "Bug";
                    Bugbuttons[1] = $"Invis {NAME}";
                    Bugbuttons[2] = $"{NAME} Gun";
                    Bugbuttons[3] = $"Big {NAME}";
                    Bugbuttons[4] = $"Grab {NAME}";
                }
                if (BugType == 1f)
                {
                    string NAME = "Balloon";
                    Bugbuttons[1] = $"Invis {NAME}";
                    Bugbuttons[2] = $"{NAME} Gun";
                    Bugbuttons[3] = $"Big {NAME}";
                    Bugbuttons[4] = $"Grab {NAME}";
                }
                if (BugType == 2f)
                {
                    string NAME = "BeachBall";
                    Bugbuttons[1] = $"Invis {NAME}";
                    Bugbuttons[2] = $"{NAME} Gun";
                    Bugbuttons[3] = $"Big {NAME}";
                    Bugbuttons[4] = $"Grab {NAME}";
                }
                if (BugType == 3f)
                {
                    string NAME = "Monsters";
                    Bugbuttons[1] = $"Invis {NAME}";
                    Bugbuttons[2] = $"{NAME} Gun";
                    Bugbuttons[3] = $"Big {NAME}";
                    Bugbuttons[4] = $"Grab {NAME}";
                }
                if (BugType == 4f)
                {
                    string NAME = "Bat";
                    Bugbuttons[1] = $"Invis {NAME}";
                    Bugbuttons[2] = $"{NAME} Gun";
                    Bugbuttons[3] = $"Big {NAME}";
                    Bugbuttons[4] = $"Grab {NAME}";
                }
                #endregion
                #endregion
                #region Tag buttonsActive
                if (TagButtonsActive[0] == true)
                {
                    NumberForPage = 1;
                    TagButtonsActive[0] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (TagButtonsActive[1] == true)
                {
                    Instance.TagAll();
                }
                else if (TagButtonsActive[1] == false)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
                if (TagButtonsActive[2] == true)
                {
                    Instance.TagGun();
                    NotifiLib.SendNotification("Tag Gun Is On", Color.green);
                }
                if (TagButtonsActive[3] == true)
                {

                    TagButtonsActive[3] = true;
                }
                else if (TagButtonsActive[3] == false)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                    TagButtonsActive[3] = false;
                }
                if (buttonsActive[4] == true)
                {

                    TagButtonsActive[4] = true;
                }
                else if (TagButtonsActive[4] == false)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                    TagButtonsActive[4] = false;
                }
                #endregion
                #region Mic buttonsActive
                if (MicButtonsActive[0] == true)
                {
                    NumberForPage = 1;
                    MicButtonsActive[0] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                #region Slingshot
                if (MicButtonsActive[1] == true)
                {
                    if (SlingshotType == 0f)
                    {
                        Mods.MainStuff.BasicMods.Slingshot("WaterBalloonProjectile");
                        NotifiLib.SendNotification("Slingshot = Water Balloon Is On", Color.green);
                        Micbuttons[1] = "Slingshot = Water Balloon";
                    }
                    if (SlingshotType == 1f)
                    {
                        Mods.MainStuff.BasicMods.Slingshot("SnowballProjectile");
                        NotifiLib.SendNotification("Slingshot = Snowball Is On", Color.green);
                        Micbuttons[1] = "Slingshot = Snowball";
                    }
                    if (SlingshotType == 2f)
                    {
                        Mods.MainStuff.BasicMods.Slingshot("CupidBow_Projectile");
                        NotifiLib.SendNotification("Slingshot = Heart Is On", Color.green);
                        Micbuttons[1] = "Slingshot = Heart";
                    }
                    if (SlingshotType == 3f)
                    {
                        Mods.MainStuff.BasicMods.Slingshot("ElfBow_Projectile");
                        NotifiLib.SendNotification("Slingshot = Leaf Is On", Color.green);
                        Micbuttons[1] = "Slingshot = Leaf";
                    }
                    if (SlingshotType == 4f)
                    {
                        Mods.MainStuff.BasicMods.Slingshot("HornsSlingshotProjectile");
                        NotifiLib.SendNotification("Slingshot = Deadshot Is On", Color.green);
                        Micbuttons[1] = "Slingshot = Deadshot";
                    }
                    if (SlingshotType == 5f)
                    {
                        Mods.MainStuff.BasicMods.Slingshot("CloudSlingshot_Projectile");
                        NotifiLib.SendNotification("Slingshot = Cloud Is On", Color.green);
                        Micbuttons[1] = "Slingshot = Cloud";
                    }
                    if (SlingshotType == 6f)
                    {
                        Mods.MainStuff.BasicMods.Slingshot("IceSlingshot_Projectile");
                        NotifiLib.SendNotification("Slingshot = Ice Is On", Color.green);
                        Micbuttons[1] = "Slingshot = Ice";
                    }
                }
                if (SlingshotType == 0f)
                {
                    Micbuttons[1] = "Slingshot = Water Balloon";
                }
                if (SlingshotType == 1f)
                {
                    Micbuttons[1] = "Slingshot = Snowball";
                }
                if (SlingshotType == 2f)
                {
                    Micbuttons[1] = "Slingshot = Heart";
                }
                if (SlingshotType == 3f)
                {
                    Micbuttons[1] = "Slingshot = Leaf";
                }
                if (SlingshotType == 4f)
                {
                    Micbuttons[1] = "Slingshot = Deadshot";
                }
                if (SlingshotType == 5f)
                {
                    Micbuttons[1] = "Slingshot = Cloud";
                }
                if (SlingshotType == 6f)
                {
                    Micbuttons[1] = "Slingshot = Ice";
                }
                #endregion
                if (MicButtonsActive[2] == true)
                {
                    Mods.MainStuff.OpMods.HitTarget();
                    NotifiLib.SendNotification("Hit Target Is On", Color.green);
                }
                if (MicButtonsActive[3] == true)
                {
                    Mods.MainStuff.OpMods.KickShibaUsersGun();
                    NotifiLib.SendNotification("Kick Shiba Users Gub Is On", Color.green);
                }
                if (MicButtonsActive[4] == true)
                {
                    Mods.MainStuff.OpMods.LagShibaUsersGun();
                    NotifiLib.SendNotification("Lag Shiba Users Gun Is On", Color.green);
                }
                if (MicButtonsActive[5] == true)
                {
                    Mods.MainStuff.OpMods.MoveShibaUsersGun();
                    NotifiLib.SendNotification("Move Shiba Users Gun Is On", Color.green);
                }
                if (MicButtonsActive[6] == true)
                {
                    Mods.MainStuff.BasicMods.SilentAim();
                    NotifiLib.SendNotification("Silent Aim Is On", Color.green);
                }
                if (MicButtonsActive[7] == true)
                {
                    if (HotPepperFace == null)
                    {
                        HotPepperFace = GorillaGameManager.instance.gameObject.GetComponent<HotPepperFace>();
                    }
                    for (int i = 0; i < 100; i++)
                    {
                        HotPepperFace.PlayFX(0.01f);
                    }
                    NotifiLib.SendNotification("Hot Pepper Crash Is On", Color.green);
                }
                if (MicButtonsActive[8] == true)
                {
                    Instance.EarapeAura();
                    NotifiLib.SendNotification("Earape Aura Is On", Color.green);
                }
                if (MicButtonsActive[11] == true)
                {
                    Mods.MainStuff.OpMods.FuckGame();
                    NotifiLib.SendNotification("Fuck Game Is On", Color.green);
                }
                #endregion
                #region Halloween buttonsActive
                if (HalloweenButtonsActive[0] == true)
                {
                    NumberForPage = 1;
                    HalloweenButtonsActive[0] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (HalloweenButtonsActive[1] == true)
                {
                    Mods.MainStuff.BasicMods.HalloweenMods.SpawnLucy();
                    NotifiLib.SendNotification("Spawn Lucy Is On", Color.green);
                }
                if (HalloweenButtonsActive[2] == true)
                {
                    Mods.MainStuff.BasicMods.HalloweenMods.SetLucyTargetGun();
                    NotifiLib.SendNotification("Set Lucy Target Gun Is On", Color.green);
                }
                if (HalloweenButtonsActive[3] == true)
                {
                    Mods.MainStuff.BasicMods.HalloweenMods.SetLucyGrabbedPlayerGun();
                    NotifiLib.SendNotification("Set Lucy Grabbed Player Gun Is On", Color.green);
                }
                if (HalloweenButtonsActive[4] == true)
                {
                    Mods.MainStuff.BasicMods.HalloweenMods.LucyTPGun();
                    NotifiLib.SendNotification("Lucy TP Gun Is On", Color.green);
                }
                if (HalloweenButtonsActive[5] == true)
                {
                    Mods.MainStuff.BasicMods.HalloweenMods.BroomStickTPGun();
                    NotifiLib.SendNotification("Broom Stick TP Gun Is On", Color.green);
                }
                if (HalloweenButtonsActive[6] == true)
                {
                    Mods.MainStuff.BasicMods.HalloweenMods.FastBroomStick();
                    NotifiLib.SendNotification("Fast Broom Stick Is On", Color.green);
                }
                else
                {
                    hasBeenGrabbed = false;
                }
                if (HalloweenButtonsActive[7] == true)
                {
                    Mods.MainStuff.BasicMods.HalloweenMods.SlowBroomStick();
                    NotifiLib.SendNotification("Slow Broom Stick Is On", Color.green);
                }
                else
                {
                    hasBeenGrabbed2 = false;
                }
                #endregion
                #region OP buttonsActive
                if (OPButtonsActive[0] == true)
                {
                     NumberForPage = 1;
                     OPButtonsActive[0] = false;
                     Object.Destroy(menu);
                     menu = null;
                     Draw();
                }
                if (OPButtonsActive[1] == true)
                {
                    Instance.StartCoroutine(Instance.AntiBan());
                }
                if (OPButtonsActive[2] == true)
                {
                    Instance.StartCoroutine(Instance.AntiBan());
                    object obj;
                    PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gameMode", out obj);
                    if (obj.ToString().Contains("MODDED"))
                    {
                        int playerCount = 10;
                        Traverse.Create(ScienceExperimentManager.instance).Field("inGamePlayerCount").SetValue(playerCount);
                        ScienceExperimentManager.PlayerGameState[] playerStates = new ScienceExperimentManager.PlayerGameState[playerCount];
                        for (int i = 0; i < playerCount; i++)
                        {
                            ScienceExperimentManager.PlayerGameState state = new ScienceExperimentManager.PlayerGameState();
                            state.touchedLiquid = true;
                            state.playerId = (PhotonNetwork.PlayerList[i] != null) ? PhotonNetwork.PlayerList[i].ActorNumber : 0;
                            playerStates[i] = state;
                        }
                        Traverse.Create(ScienceExperimentManager.instance).Field("inGamePlayerStates").SetValue(playerStates);
                        return;
                    }
                }
                if (OPButtonsActive[3] == true)
                {
                    Instance.StartCoroutine(Instance.AntiBan());
                    Mods.MainStuff.OpMods.AcidGun();
                }
                if (OPButtonsActive[4] == true)
                {
                    Instance.StartCoroutine(Instance.AntiBan());
                    object obj;
                    PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gameMode", out obj);
                    if (obj.ToString().Contains("MODDED"))
                    {
                        int playerCount = 10;
                        Traverse.Create(ScienceExperimentManager.instance).Field("inGamePlayerCount").SetValue(playerCount);
                        ScienceExperimentManager.PlayerGameState[] playerStates = new ScienceExperimentManager.PlayerGameState[playerCount];
                        for (int i = 0; i < playerCount; i++)
                        {
                            ScienceExperimentManager.PlayerGameState state = new ScienceExperimentManager.PlayerGameState();
                            Photon.Realtime.Player randomPlayer = RigManager.GetRandomPlayer(true);
                            state.touchedLiquid = true;
                            state.playerId = (randomPlayer != null) ? randomPlayer.ActorNumber : 0;
                            playerStates[i] = state;
                        }
                        Traverse.Create(ScienceExperimentManager.instance).Field("inGamePlayerStates").SetValue(playerStates);
                        return;
                    }
                }
                if (OPButtonsActive[5] == true)
                {
                    Instance.StartCoroutine(Instance.AntiBan());
                    object obj;
                    PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gameMode", out obj);
                    if (obj.ToString().Contains("MODDED"))
                    {
                        Vector2 vector2 = UnityEngine.Random.insideUnitCircle.normalized + new Vector2(0.5f, 2f);
                        double num2 = (PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time));
                        ScienceExperimentManager.instance.photonView.RPC("SpawnSodaBubbleRPC", RpcTarget.Others, vector2, -100000000f * 1, 130f, num2);
                        ScienceExperimentManager.instance.photonView.RPC("SpawnSodaBubbleRPC", RpcTarget.Others, vector2, 100000000f * 1, 130f, num2);
                        return;
                    }
                }
                if (OPButtonsActive[6] == true)
                {
                    Instance.StartCoroutine(Instance.AntiBan());
                    object obj;
                    PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gameMode", out obj);
                    if (obj.ToString().Contains("MODDED"))
                    {
                        foreach (GorillaScoreBoard sB in UnityEngine.Object.FindObjectsOfType(typeof(GorillaScoreBoard)))
                        {
                            for (int i = 0; i < sB.lines.Count; i++)
                            {
                                GorillaPlayerScoreboardLine gorillaPlayerScoreboardLine = sB.lines[i];
                                gorillaPlayerScoreboardLine.ResetData();
                            }
                        }
                        /*for (int j = 0; j < GorillaScoreboardTotalUpdater.allScoreboards.Count; j++)
                        {
                            if (string.IsNullOrEmpty(GorillaScoreboardTotalUpdater.allScoreboards[j].initialGameMode))
                            {
                                GorillaScoreboardTotalUpdater.instance.UpdateScoreboard(GorillaScoreboardTotalUpdater.allScoreboards[j]);
                            }
                        }*/
                        foreach (GorillaPlayerScoreboardLine sB in UnityEngine.Object.FindObjectsOfType(typeof(GorillaPlayerScoreboardLine)))
                        {
                            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                            {
                                sB.linePlayer = player;
                                sB.toxicityButton.SetActive(true);
                                sB.reportButton.isOn = sB.reportedToxicity;
                                sB.reportButton.UpdateColor();
                                //PlayerPrefs.SetInt(sB.linePlayer.UserId, 1);
                                sB.muteButton.UpdateColor();
                                sB.SetLineData(player);
                                sB.UpdateLine();
                                sB.parentScoreboard.RedrawPlayerLines();
                            }
                            foreach (GorillaScoreboardTotalUpdater sB2 in UnityEngine.Object.FindObjectsOfType(typeof(GorillaScoreboardTotalUpdater)))
                            {
                                GorillaScoreboardTotalUpdater.lineIndex = 11;
                                GorillaScoreboardTotalUpdater.allScoreboardLines[GorillaScoreboardTotalUpdater.lineIndex].UpdateLine();
                                sB2.UpdateLineState(sB);
                            }
                        }
                        return;
                    }
                }
                if (OPButtonsActive[7] == true) { Mods.MainStuff.OpMods.SetLavaState(InfectionLavaController.RisingLavaState.Full); }
                if (OPButtonsActive[8] == true) { Mods.MainStuff.OpMods.SetLavaState(InfectionLavaController.RisingLavaState.Drained); }
                if (OPButtonsActive[9] == true) { Mods.MainStuff.OpMods.SetLavaState(InfectionLavaController.RisingLavaState.Erupting); }
                #endregion
            }
            catch (Exception ex)
            {
                Mods.Utils.LogError(ex);
            }
        }
        #endregion
        #region Mods
        public class Mods : MonoBehaviour
        {
            public class MainStuff
            {
                public class OpMods
                {
                    public class WaterStuff
                    {
                        public static void PlaySplashEffect(Vector3 position, Quaternion rotation, float duration, float size, bool idk, bool idk2)
                        {
                            GorillaTagger.Instance.myVRRig.RPC("PlaySplashEffect", RpcTarget.All, position, rotation, duration, size, idk, idk2);
                            //PhotonView.Get(GorillaTagger.Instance.offlineVRRig).RPC("PlaySplashEffect", RpcTarget.All, position, rotation, duration, size, idk, idk2);
                        }
                        public static void Water()
                        {
                            if (ControllerInput.RightGrip)
                            {
                                WaterParameters waterParameters = ScriptableObject.CreateInstance<WaterParameters>();
                                PlaySplashEffect(GorillaTagger.Instance.headCollider.transform.position, UnityEngine.Random.rotation, 8f, waterParameters.splashEffectScale, false, false);
                            }
                        }
                        public static void WaterThrowUp()
                        {
                            if (ControllerInput.RightGrip)
                            {
                                Vector3 cameraPosition = Camera.main.transform.position;
                                Vector3 cameraForward = Camera.main.transform.forward;

                                RaycastHit raycastHit;
                                Physics.Raycast(cameraPosition, cameraForward, out raycastHit);

                                Vector3 startPosition = GorillaTagger.Instance.offlineVRRig.transform.position;
                                Vector3 targetPosition = raycastHit.point;
                                Vector3 directionToTarget = (targetPosition - startPosition).normalized;
                                if (GorillaGameManager.instance != null)
                                {
                                    //int projectileCount = GorillaGameManager.instance.IncrementLocalPlayerProjectileCount();
                                    PlaySplashEffect(directionToTarget, Quaternion.identity, 1f, 1f, false, false);
                                }
                            }
                        }
                        public static void WaterAura()
                        {
                            WaterParameters waterParameters = ScriptableObject.CreateInstance<WaterParameters>();
                            PlaySplashEffect(GorillaLocomotion.Player.Instance.headCollider.transform.position + new Vector3((float)UnityEngine.Random.Range(0, 4), (float)UnityEngine.Random.Range(0, 4), (float)UnityEngine.Random.Range(0, 4)), UnityEngine.Random.rotation, 8f, waterParameters.splashEffectScale, false, false);
                        }
                        public static void WaterBending()
                        {
                            if (ControllerInput.RightGrip && ControllerInput.LeftGrip)
                            {
                                Vector3 rightControllerPosition = GorillaLocomotion.Player.Instance.rightControllerTransform.transform.position;
                                Vector3 leftControllerPosition = GorillaLocomotion.Player.Instance.leftControllerTransform.transform.position;
                                Quaternion rightControllerRotation = GorillaLocomotion.Player.Instance.rightControllerTransform.transform.rotation;
                                Quaternion leftControllerRotation = GorillaLocomotion.Player.Instance.leftControllerTransform.transform.rotation;
                                Vector3 averagePosition = (rightControllerPosition + leftControllerPosition) / 2f;
                                Quaternion averageRotation = Quaternion.Lerp(rightControllerRotation, leftControllerRotation, 0.5f);
                                float distance = Vector3.Distance(rightControllerPosition, leftControllerPosition);
                                PlaySplashEffect(averagePosition, averageRotation, distance, distance, false, true);
                            }
                            else
                            {
                                if (ControllerInput.RightGrip)
                                {
                                    PlaySplashEffect(GorillaLocomotion.Player.Instance.rightControllerTransform.transform.position, GorillaTagger.Instance.myVRRig.transform.rotation, 0.3f, 0.3f, false, true);
                                }
                                if (ControllerInput.LeftGrip)
                                {
                                    PlaySplashEffect(GorillaLocomotion.Player.Instance.leftControllerTransform.transform.position, GorillaTagger.Instance.myVRRig.transform.rotation, 0.3f, 0.3f, false, true);
                                }
                            }

                            if (ControllerInput.RightSecondary)
                            {
                                Vector3 headColliderPosition = GorillaLocomotion.Player.Instance.headCollider.transform.position;
                                Quaternion headRotation = Quaternion.Euler(90f, 0f, 0f);
                                PlaySplashEffect(headColliderPosition + new Vector3(0f, 0.5f, 0f), headRotation, 0.3f, 0.3f, false, true);
                            }
                        }
                        public static void WaterGun()
                        {
                            if (ControllerInput.RightGrip)
                            {
                                RaycastHit raycastHit;
                                if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                                {
                                    pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                    UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                                    UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                                    pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                pointer.transform.position = raycastHit.point;
                                if (ControllerInput.RightTrigger)
                                {
                                    if (GorillaTagger.Instance.offlineVRRig.enabled)
                                    {
                                        VRRigOnDisable.Prefix(GorillaTagger.Instance.offlineVRRig);
                                    }
                                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                                    GorillaTagger.Instance.offlineVRRig.transform.position = pointer.transform.position;
                                    if (Time.time > SplashTime + 0.5f)
                                    {
                                        GorillaTagger.Instance.myVRRig.RPC("PlaySplashEffect", RpcTarget.All, pointer.transform.position, Random.rotation, 4f, 100f, false, true);
                                        SplashTime = Time.time;
                                    }
                                    else
                                    {
                                        GorillaTagger.Instance.offlineVRRig.enabled = true;
                                    }
                                }
                                else
                                {
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                return;
                            }
                            UnityEngine.GameObject.Destroy(pointer);
                        }
                    }
                    public static void Rope(Vector3 Pos)
                    {
                        if ((double)Time.time > (double)RopeTimer + 0.1)
                        {
                            foreach (GorillaRopeSwing gorillaRopeSwing in UnityEngine.Object.FindObjectsOfType(typeof(GorillaRopeSwing)))
                            {
                                HalloweenGhostChaser[] ghostChasers = UnityEngine.Object.FindObjectsOfType<HalloweenGhostChaser>();
                                foreach (HalloweenGhostChaser ghostChaser in ghostChasers)
                                {
                                    PhotonView photonView = ghostChaser.photonView;
                                    string methodName = "SetVelocity";
                                    RpcTarget target = RpcTarget.All;
                                    object[] array2 = new object[4];
                                    array2[0] = 1;
                                    array2[1] = Pos;
                                    array2[2] = true;
                                    photonView.RPC(methodName, target, array2);
                                }
                            }
                            RopeTimer = Time.time;
                        }
                    }
                    public static void CrashGun()
                    {
                        if (ControllerInput.RightGrip)
                        {
                            if (lagrig == null)
                            {
                                RaycastHit raycastHit;
                                if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                                {
                                    pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                    Object.Destroy(pointer.GetComponent<Rigidbody>());
                                    Object.Destroy(pointer.GetComponent<SphereCollider>());
                                    pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                                    pointer.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f, 0.15f);
                                    pointer.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                                }
                                pointer.transform.position = raycastHit.point;
                                if (ControllerInput.RightTrigger)
                                {
                                    if (raycastHit.collider.GetComponentInParent<VRRig>())
                                    {
                                        lagrig = raycastHit.collider.GetComponentInParent<VRRig>();
                                    }
                                }
                            }
                            if (ControllerInput.RightTrigger && lagrig != null)
                            {
                                pointer.transform.position = lagrig.transform.position;
                                PhotonView photonViewFromRig = RigManager.GetRigView(lagrig);
                                Photon.Realtime.Player owner = photonViewFromRig.Owner;
                                if (photonViewFromRig != null || owner != null)
                                {
                                    photonViewFromRig.OwnerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
                                    photonViewFromRig.ControllerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
                                    PhotonNetwork.Destroy(photonViewFromRig);
                                    PhotonNetwork.Destroy(photonViewFromRig.gameObject);
                                    pointer.GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f, 0.15f);
                                }
                            }
                            else
                            {
                                lagrig = null;
                                pointer.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f, 0.15f);
                            }
                        }
                        else
                        {
                            Object.Destroy(pointer);
                        }
                    }
                    public static void HitTarget()
                    {
                        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                        {
                            if (player.IsMasterClient)
                            {
                                Photon.Realtime.Player player2 = player;
                                if (player.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                                {
                                    GorillaGameManager.instance.currentRoom.masterClientId = PhotonNetwork.LocalPlayer.ActorNumber;
                                    GorillaGameManager.instance.currentMasterClient = PhotonNetwork.LocalPlayer;
                                    //foreach (HitTargetWithScoreCounter hitTargetWithScoreCounter in UnityEngine.Object.FindObjectsOfType<HitTargetWithScoreCounter>())
                                    {
                                        //if (!hitTargetWithScoreCounter.photonView.IsMine)
                                        //{
                                        //   hitTargetWithScoreCounter.photonView.RequestOwnership();
                                        //    GetOwnershipPhotonView(hitTargetWithScoreCounter.photonView);
                                        //}
                                        //if (hitTargetWithScoreCounter.photonView.IsMine)
                                        //{
                                        //    hitTargetWithScoreCounter.rotateSpeed = int.MaxValue;
                                        //    hitTargetWithScoreCounter.hitCooldownTime = 0;
                                        //    hitTargetWithScoreCounter.TargetHit();
                                        //}
                                    }
                                    GorillaGameManager.instance.currentRoom.masterClientId = player.ActorNumber;
                                    GorillaGameManager.instance.currentMasterClient = player2;
                                }
                            }
                        }
                    }
                    public static void KickShibaUsersGun()
                    {
                        if (ControllerInput.RightGrip)
                        {
                            RaycastHit raycastHit;
                            if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                            {
                                pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                                UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                                pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                                pointer.GetComponent<Renderer>().material.color = Color.red;
                            }
                            pointer.transform.position = raycastHit.point;
                            PhotonView Photonview = RigManager.GetRigView(raycastHit.collider.GetComponentInParent<VRRig>());
                            Photon.Realtime.Player owner = Photonview.Owner;
                            if (ControllerInput.RightTrigger)
                            {
                                if (Photonview != null || owner != null)
                                {
                                    Hashtable hash = new Hashtable();
                                    hash.Add("Kick", owner.NickName);
                                    PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                                    GorillaTagger.Instance.myVRRig.Controller.SetCustomProperties(hash);
                                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                                    raiseEventOptions.Receivers = ReceiverGroup.All;
                                    PhotonNetwork.RaiseEvent(100, null, raiseEventOptions, SendOptions.SendReliable);
                                }
                            }
                            else
                            {
                                pointer.GetComponent<Renderer>().material.color = Color.red;
                            }
                            return;
                        }
                        UnityEngine.GameObject.Destroy(pointer);
                    }
                    public static void LagShibaUsersGun()
                    {
                        if (ControllerInput.RightGrip)
                        {
                            RaycastHit raycastHit;
                            if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                            {
                                pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                                UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                                pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                                pointer.GetComponent<Renderer>().material.color = Color.red;
                            }
                            pointer.transform.position = raycastHit.point;
                            PhotonView Photonview = RigManager.GetRigView(raycastHit.collider.GetComponentInParent<VRRig>());
                            Photon.Realtime.Player owner = Photonview.Owner;
                            if (ControllerInput.RightTrigger)
                            {
                                if (Photonview != null || owner != null)
                                {
                                    Hashtable hash = new Hashtable();
                                    hash.Add("Lag", owner.NickName);
                                    PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                                    GorillaTagger.Instance.myVRRig.Controller.SetCustomProperties(hash);
                                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                                    raiseEventOptions.Receivers = ReceiverGroup.All;
                                    PhotonNetwork.RaiseEvent(101, null, raiseEventOptions, SendOptions.SendReliable);
                                }
                            }
                            else
                            {
                                pointer.GetComponent<Renderer>().material.color = Color.red;
                            }
                            return;
                        }
                        UnityEngine.GameObject.Destroy(pointer);
                    }
                    public static void MoveShibaUsersGun()
                    {
                        if (ControllerInput.RightGrip)
                        {
                            RaycastHit raycastHit;
                            if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                            {
                                pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                                UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                                pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                                pointer.GetComponent<Renderer>().material.color = Color.red;
                            }
                            pointer.transform.position = raycastHit.point;
                            PhotonView Photonview = RigManager.GetRigView(raycastHit.collider.GetComponentInParent<VRRig>());
                            Photon.Realtime.Player owner = Photonview.Owner;
                            if (ControllerInput.RightTrigger)
                            {
                                if (Photonview != null || owner != null)
                                {
                                    Hashtable hash = new Hashtable();
                                    hash.Add("Move", owner.NickName);
                                    hash.Add("MovePos", raycastHit.point);
                                    PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                                    GorillaTagger.Instance.myVRRig.Controller.SetCustomProperties(hash);
                                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                                    raiseEventOptions.Receivers = ReceiverGroup.All;
                                    PhotonNetwork.RaiseEvent(102, null, raiseEventOptions, SendOptions.SendReliable);
                                }
                            }
                            else
                            {
                                pointer.GetComponent<Renderer>().material.color = Color.red;
                            }
                            return;
                        }
                        UnityEngine.GameObject.Destroy(pointer);
                    }
                    public static void FuckGame()
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            if (smth < Time.time && PhotonNetwork.InRoom)
                            {
                                smth = Time.time + 0.5f;
                                foreach (GorillaTagManager gm in UnityEngine.Object.FindObjectsOfType(typeof(GorillaTagManager)))
                                {
                                    gm.currentInfected.Clear();
                                    gm.UpdateState();
                                    gm.UpdateTagState();
                                    gm.ClearInfectionState();
                                    gm.StartCoroutine(gm.InfectionEnd());
                                }
                                foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                                {
                                    PhotonView photonView = GorillaGameManager.instance.FindVRRigForPlayer(player);
                                    photonView.RPC("PlayTagSound", RpcTarget.All, 2, 0.25f);
                                }
                            }
                        }
                    }
                    public static void AcidGun()
                    {
                        if (ControllerInput.RightGrip)
                        {
                            RaycastHit raycastHit;
                            if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                            {
                                pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                                UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                                pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                                pointer.GetComponent<Renderer>().material.color = Color.red;
                            }
                            pointer.transform.position = raycastHit.point;
                            PhotonView Photonview = RigManager.GetRigView(raycastHit.collider.GetComponentInParent<VRRig>());
                            Photon.Realtime.Player owner = Photonview.Owner;
                            if (ControllerInput.RightTrigger)
                            {
                                pointer.GetComponent<Renderer>().material.color = Color.green;
                                ScienceExperimentManager instance = ScienceExperimentManager.instance;
                                if (instance != null)
                                {
                                    Traverse.Create(instance).Field("inGamePlayerCount").SetValue(10);
                                    if (Photonview != null && Photonview.Owner != null)
                                    {
                                        int actorNumber = Photonview.Owner.ActorNumber;
                                        ScienceExperimentManager.PlayerGameState[] array2 = new ScienceExperimentManager.PlayerGameState[10];
                                        int num3 = ((array2.Length > actorNumber) ? actorNumber : 0);
                                        array2[num3].touchedLiquid = true;
                                        array2[num3].playerId = actorNumber;
                                        Traverse.Create(instance).Field("inGamePlayerStates").SetValue(array2);
                                    }
                                }
                            }
                            else
                            {
                                pointer.GetComponent<Renderer>().material.color = Color.red;
                            }
                            return;
                        }
                        UnityEngine.GameObject.Destroy(pointer);
                    }
                    public static void AcidKid(Player player)
                    {
                        Instance.StartCoroutine(Instance.AntiBan());
                        object obj;
                        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gameMode", out obj);
                        if (obj.ToString().Contains("MODDED"))
                        {
                            bool touchedLiquid = MainGUI.Instance.HasPlayerTouchedLiquid(player);
                            Traverse.Create(ScienceExperimentManager.instance).Field("inGamePlayerCount").SetValue(10);
                            ScienceExperimentManager.PlayerGameState[] playerStates = new ScienceExperimentManager.PlayerGameState[10];

                            for (int i = 0; i < 10; i++)
                            {
                                ScienceExperimentManager.PlayerGameState state = new ScienceExperimentManager.PlayerGameState();
                                state.touchedLiquid = !touchedLiquid;
                                state.playerId = (player != null) ? player.ActorNumber : 0;
                                playerStates[i] = state;
                            }

                            Traverse.Create(ScienceExperimentManager.instance).Field("inGamePlayerStates").SetValue(playerStates);
                        }
                    }
                    public static void SetLavaState(InfectionLavaController.RisingLavaState state)
                    {
                        InfectionLavaController instance = InfectionLavaController.Instance;

                        Type type = typeof(InfectionLavaController);
                        FieldInfo reliableStateField = type.GetField("reliableState", BindingFlags.Instance | BindingFlags.NonPublic);

                        object reliableState = reliableStateField.GetValue(instance);

                        Type reliableStateType = reliableState.GetType();
                        FieldInfo stateField = reliableStateType.GetField("state");
                        FieldInfo stateStartTimeField = reliableStateType.GetField("stateStartTime");

                        stateField.SetValue(reliableState, state);
                        stateStartTimeField.SetValue(reliableState, PhotonNetwork.Time);
                        reliableStateField.SetValue(instance, reliableState);
                    }
                }
                public class BasicMods
                {
                    public class BugStuff
                    {
                        public static void InvisBug()
                        {
                            foreach (ThrowableBug throwableBug in UnityEngine.Object.FindObjectsOfType<ThrowableBug>())
                            {
                                Vector3 newPosition = GorillaTagger.Instance.headCollider.transform.position + new Vector3(0f, 9999f, 0f);
                                throwableBug.transform.position = newPosition;
                            }
                        }
                        public static void BugGun()
                        {
                            if (ControllerInput.RightGrip)
                            {
                                RaycastHit raycastHit;
                                if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                                {
                                    pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                    UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                                    UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                                    pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                pointer.transform.position = raycastHit.point;
                                if (ControllerInput.RightTrigger)
                                {
                                    foreach (ThrowableBug throwableBug in UnityEngine.Object.FindObjectsOfType<ThrowableBug>())
                                    {
                                        GameObject.Find("Floating Bug Holdable").transform.position = raycastHit.point;
                                    }
                                }
                                else
                                {
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                return;
                            }
                            UnityEngine.GameObject.Destroy(pointer);
                        }
                        public static void BigBug()
                        {
                            if (PhotonNetwork.InRoom)
                            {
                                foreach (ThrowableBug throwableBug in UnityEngine.Object.FindObjectsOfType<ThrowableBug>())
                                {
                                    GameObject.Find("Floating Bug Holdable").transform.localScale = new Vector3(5f, 5f, 5f);
                                }
                            }
                            else
                            {
                                foreach (ThrowableBug throwableBug in UnityEngine.Object.FindObjectsOfType<ThrowableBug>())
                                {
                                    GameObject.Find("Floating Bug Holdable").transform.localScale = new Vector3(1f, 1f, 1f);
                                }
                            }
                        }
                        public static void GrabBug()
                        {
                            foreach (ThrowableBug throwableBug in UnityEngine.Object.FindObjectsOfType<ThrowableBug>())
                            {
                                throwableBug.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                            }
                        }
                    }
                    public class BeachBallStuff
                    {
                        public static void InvisBeachBall()
                        {
                            foreach (TransferrableBall transferrableBall in UnityEngine.Object.FindObjectsOfType<TransferrableBall>())
                            {
                                transferrableBall.transform.transform.position = GorillaTagger.Instance.headCollider.transform.position + new Vector3(0f, 9999f, 0f);
                            }
                        }
                        public static void BeachBallGun()
                        {
                            if (ControllerInput.RightGrip)
                            {
                                RaycastHit raycastHit;
                                if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                                {
                                    pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                    UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                                    UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                                    pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                pointer.transform.position = raycastHit.point;
                                if (ControllerInput.RightTrigger)
                                {
                                    foreach (TransferrableBall transferrableBall in UnityEngine.Object.FindObjectsOfType<TransferrableBall>())
                                    {
                                        transferrableBall.transform.position = raycastHit.point;
                                    }
                                }
                                else
                                {
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                return;
                            }
                            UnityEngine.GameObject.Destroy(pointer);
                        }
                        public static void BigBeachBall()
                        {
                            if (PhotonNetwork.InRoom)
                            {
                                foreach (TransferrableBall transferrableBall in UnityEngine.Object.FindObjectsOfType<TransferrableBall>())
                                {
                                    transferrableBall.transform.localScale = new Vector3(5f, 5f, 5f);
                                }
                            }
                            else
                            {
                                foreach (TransferrableBall transferrableBall in UnityEngine.Object.FindObjectsOfType<TransferrableBall>())
                                {
                                    transferrableBall.transform.localScale = new Vector3(1f, 1f, 1f);
                                }
                            }
                        }
                        public static void GrabBeachBall()
                        {
                            foreach (TransferrableBall transferrableBall in UnityEngine.Object.FindObjectsOfType<TransferrableBall>())
                            {
                                transferrableBall.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                            }
                        }
                    }
                    public class BalloonStuff
                    {
                        public static void InvisBalloon()
                        {
                            foreach (BalloonHoldable balloonHoldable in UnityEngine.Object.FindObjectsOfType<BalloonHoldable>())
                            {
                                balloonHoldable.transform.transform.position = GorillaTagger.Instance.headCollider.transform.position + new Vector3(0f, 9999f, 0f);
                            }
                        }
                        public static void BalloonGun()
                        {
                            if (ControllerInput.RightGrip)
                            {
                                RaycastHit raycastHit;
                                if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                                {
                                    pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                    UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                                    UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                                    pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                pointer.transform.position = raycastHit.point;
                                if (ControllerInput.RightTrigger)
                                {
                                    foreach (BalloonHoldable balloonHoldable in UnityEngine.Object.FindObjectsOfType<BalloonHoldable>())
                                    {
                                        balloonHoldable.transform.transform.position = raycastHit.point + new Vector3(0, 0.3f, 0);
                                    }
                                }
                                else
                                {
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                return;
                            }
                            UnityEngine.GameObject.Destroy(pointer);
                        }
                        public static void BigBalloon()
                        {
                            if (PhotonNetwork.InRoom)
                            {
                                foreach (BalloonHoldable balloonHoldable in UnityEngine.Object.FindObjectsOfType<BalloonHoldable>())
                                {
                                    balloonHoldable.transform.localScale = new Vector3(5f, 5f, 5f);
                                }
                            }
                            else
                            {
                                foreach (BalloonHoldable balloonHoldable in UnityEngine.Object.FindObjectsOfType<BalloonHoldable>())
                                {
                                    balloonHoldable.transform.localScale = new Vector3(1f, 1f, 1f);
                                }
                            }
                        }
                        public static void GrabBalloon()
                        {
                            foreach (BalloonHoldable balloonHoldable in UnityEngine.Object.FindObjectsOfType<BalloonHoldable>())
                            {
                                balloonHoldable.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                            }
                        }
                    }
                    public class MonstersStuff
                    {
                        public static void InvisMonsters()
                        {
                            foreach (MonkeyeAI monkeyeAI in UnityEngine.Object.FindObjectsOfType<MonkeyeAI>())
                            {
                                monkeyeAI.transform.transform.position = GorillaTagger.Instance.headCollider.transform.position + new Vector3(0f, 9999f, 0f);
                            }
                        }
                        public static void MonstersGun()
                        {
                            if (ControllerInput.RightGrip)
                            {
                                RaycastHit raycastHit;
                                if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                                {
                                    pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                    UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                                    UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                                    pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                pointer.transform.position = raycastHit.point;
                                if (ControllerInput.RightTrigger)
                                {
                                    foreach (MonkeyeAI monkeyeAI in UnityEngine.Object.FindObjectsOfType<MonkeyeAI>())
                                    {
                                        monkeyeAI.transform.transform.position = raycastHit.point + new Vector3(0, 0.3f, 0);
                                    }
                                }
                                else
                                {
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                return;
                            }
                            UnityEngine.GameObject.Destroy(pointer);
                        }
                        public static void BigMonsters()
                        {
                            if (PhotonNetwork.InRoom)
                            {
                                foreach (MonkeyeAI monkeyeAI in UnityEngine.Object.FindObjectsOfType<MonkeyeAI>())
                                {
                                    monkeyeAI.transform.localScale = new Vector3(5f, 5f, 5f);
                                }
                            }
                            else
                            {
                                foreach (MonkeyeAI monkeyeAI in UnityEngine.Object.FindObjectsOfType<MonkeyeAI>())
                                {
                                    monkeyeAI.transform.localScale = new Vector3(1f, 1f, 1f);
                                }
                            }
                        }
                        public static void GrabMonsters()
                        {
                            foreach (MonkeyeAI monkeyeAI in UnityEngine.Object.FindObjectsOfType<MonkeyeAI>())
                            {
                                monkeyeAI.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                            }
                        }
                    }
                    public class BatStuff
                    {
                        public static void InvisBat()
                        {
                            GameObject.Find("Cave Bat Holdable").transform.transform.position = GorillaTagger.Instance.headCollider.transform.position + new Vector3(0f, 9999f, 0f);
                        }
                        public static void BatGun()
                        {
                            if (ControllerInput.RightGrip)
                            {
                                RaycastHit raycastHit;
                                if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                                {
                                    pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                    UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                                    UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                                    pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                pointer.transform.position = raycastHit.point;
                                if (ControllerInput.RightTrigger)
                                {
                                    GameObject.Find("Cave Bat Holdable").transform.transform.position = raycastHit.point + new Vector3(0, 0.3f, 0);
                                }
                                else
                                {
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                return;
                            }
                            UnityEngine.GameObject.Destroy(pointer);
                        }
                        public static void BigBat()
                        {
                            if (PhotonNetwork.InRoom)
                            {
                                GameObject.Find("Cave Bat Holdable").transform.localScale = new Vector3(5f, 5f, 5f);
                            }
                            else
                            {
                                GameObject.Find("Cave Bat Holdable").transform.localScale = new Vector3(1f, 1f, 1f);
                            }
                        }
                        public static void GrabBat()
                        {
                            GameObject.Find("Cave Bat Holdable").transform.position = GorillaTagger.Instance.rightHandTransform.position;
                        }
                    }
                    public class HalloweenMods
                    {
                        public static void SpawnLucy()
                        {
                            HalloweenGhostChaser[] ghostChasers = UnityEngine.Object.FindObjectsOfType<HalloweenGhostChaser>();
                            foreach (HalloweenGhostChaser ghostChaser in ghostChasers)
                            {
                                if (!ghostChaser.photonView.IsMine)
                                {
                                    ghostChaser.photonView.RequestOwnership();
                                    GetOwnershipPhotonView(ghostChaser.photonView);
                                }
                                if (ghostChaser.photonView.IsMine)
                                {
                                    ghostChaser.lastSummonCheck = Time.time + 35f;
                                    ghostChaser.currentState = HalloweenGhostChaser.ChaseState.Chasing;
                                    ghostChaser.defaultColor = ghostChaser.ghostMaterial.color;
                                    ghostChaser.summonedColor = ghostChaser.defaultColor;
                                    ghostChaser.spawnIndex = 1;
                                    ghostChaser.UpdateState();
                                }
                            }
                        }
                        public static void SetLucyTargetGun()
                        {
                            if (ControllerInput.RightGrip)
                            {
                                RaycastHit raycastHit;
                                if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                                {
                                    pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                    UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                                    UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                                    pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                pointer.transform.position = raycastHit.point;
                                PhotonView photonView = RigManager.GetRigView(raycastHit.collider.GetComponentInParent<VRRig>());
                                if (ControllerInput.RightTrigger)
                                {
                                    HalloweenGhostChaser[] ghostChasers = UnityEngine.Object.FindObjectsOfType<HalloweenGhostChaser>();
                                    foreach (HalloweenGhostChaser ghostChaser in ghostChasers)
                                    {
                                        ghostChaser.possibleTarget.Add(photonView.Owner);
                                        if (ghostChaser.possibleTarget != lastghostChaser)
                                        {
                                            lastghostChaser = ghostChaser.possibleTarget;
                                            NotifiLib.SendNotification($"Lucy Target Set To {ghostChaser.possibleTarget}", Color.green);
                                        }
                                    }
                                }
                                else
                                {
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                return;
                            }
                            UnityEngine.GameObject.Destroy(pointer);
                        }
                        public static void SetLucyGrabbedPlayerGun()
                        {
                            if (ControllerInput.RightGrip)
                            {
                                if (lucyp == null)
                                {
                                    RaycastHit raycastHit;
                                    if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                                    {
                                        pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                        UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                                        UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                                        pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                                        pointer.GetComponent<Renderer>().material.color = Color.red;
                                    }
                                    if (raycastHit.collider.GetComponentInParent<VRRig>() != null)
                                    {
                                        if (ControllerInput.RightTrigger)
                                        {
                                            lucyp = raycastHit.collider.GetComponentInParent<VRRig>();
                                        }
                                        GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tapHapticStrength, GorillaTagger.Instance.tapHapticDuration);
                                    }
                                    pointer.transform.position = raycastHit.point;
                                }
                                else
                                {
                                    if (ControllerInput.RightTrigger)
                                    {
                                        pointer.transform.position = lucyp.transform.position;
                                        Photon.Realtime.Player p = RigManager.GetRigView(lucyp).Owner;
                                        HalloweenGhostChaser[] ghostChasers = UnityEngine.Object.FindObjectsOfType<HalloweenGhostChaser>();
                                        foreach (HalloweenGhostChaser ghostChaser in ghostChasers)
                                        {
                                            if (ghostChaser.targetPlayer != p)
                                            {
                                                ghostChaser.targetPlayer = p;
                                            }
                                            if (ghostChaser.grabbedPlayer != p)
                                            {
                                                ghostChaser.grabbedPlayer = p;
                                            }
                                            ghostChaser.grabDuration = float.PositiveInfinity;
                                            ghostChaser.summonDistance = float.PositiveInfinity;
                                            ghostChaser.possibleTarget.Add(p);
                                            ghostChaser.transform.position = lucyp.transform.position;
                                            if (ghostChaser.lastSummonCheck != Time.time)
                                            {
                                                SpawnLucy();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        HalloweenGhostChaser[] ghostChasers = UnityEngine.Object.FindObjectsOfType<HalloweenGhostChaser>();
                                        foreach (HalloweenGhostChaser ghostChaser in ghostChasers)
                                        {
                                            ghostChaser.grabDuration = 0f;
                                        }
                                        lucyp = null;
                                        pointer.GetComponent<Renderer>().material.color = Color.blue;
                                    }
                                }
                                return;
                            }
                            UnityEngine.GameObject.Destroy(pointer);
                        }
                        public static void LucyTPGun()
                        {
                            if (ControllerInput.RightGrip)
                            {
                                RaycastHit raycastHit;
                                if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                                {
                                    pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                    UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                                    UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                                    pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                pointer.transform.position = raycastHit.point;
                                if (ControllerInput.RightTrigger)
                                {
                                    HalloweenGhostChaser[] ghostChasers = UnityEngine.Object.FindObjectsOfType<HalloweenGhostChaser>();
                                    foreach (HalloweenGhostChaser ghostChaser in ghostChasers)
                                    {
                                        ghostChaser.transform.position = pointer.transform.position;
                                        ghostChaser.targetPlayer = PhotonNetwork.LocalPlayer;
                                    }
                                }
                                else
                                {
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                return;
                            }
                            UnityEngine.GameObject.Destroy(pointer);
                        }
                        public static void BroomStickTPGun()
                        {
                            if (ControllerInput.RightGrip)
                            {
                                RaycastHit raycastHit;
                                if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                                {
                                    pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                    UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                                    UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                                    pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                pointer.transform.position = raycastHit.point;
                                if (ControllerInput.RightTrigger)
                                {
                                    NoncontrollableBroomstick[] Broomsticks = UnityEngine.Object.FindObjectsOfType<NoncontrollableBroomstick>();
                                    foreach (NoncontrollableBroomstick Broomstick in Broomsticks)
                                    {
                                        Broomstick.transform.position = pointer.transform.position;
                                        Broomstick.duration = float.PositiveInfinity;
                                    }
                                }
                                else
                                {
                                    pointer.GetComponent<Renderer>().material.color = Color.red;
                                }
                                return;
                            }
                            UnityEngine.GameObject.Destroy(pointer);
                        }
                        public static void FastBroomStick()
                        {
                            NoncontrollableBroomstick[] Broomsticks = UnityEngine.Object.FindObjectsOfType<NoncontrollableBroomstick>();
                            foreach (NoncontrollableBroomstick Broomstick in Broomsticks)
                            {
                                if (!hasBeenGrabbed)
                                {
                                    //Broomstick.OnGrabbed();
                                }
                                Broomstick.speedMultiplierWhileHeld = 17f;
                                Broomstick.acceleration = 17f;
                                Broomstick.deceleration = 17f;
                            }
                        }
                        public static void SlowBroomStick()
                        {
                            NoncontrollableBroomstick[] Broomsticks = UnityEngine.Object.FindObjectsOfType<NoncontrollableBroomstick>();
                            foreach (NoncontrollableBroomstick Broomstick in Broomsticks)
                            {
                                if (!hasBeenGrabbed2)
                                {
                                    //Broomstick.OnGrabbed();
                                }
                                Broomstick.speedMultiplierWhileHeld = 0.17f;
                                Broomstick.acceleration = 0.17f;
                                Broomstick.deceleration = 0.17f;
                            }
                        }
                        public static void KickOfBroomStick()
                        {
                            NoncontrollableBroomstick[] Broomsticks = UnityEngine.Object.FindObjectsOfType<NoncontrollableBroomstick>();
                            foreach (NoncontrollableBroomstick Broomstick in Broomsticks)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    //Broomstick.OnGrabReleased();
                                    //Broomstick.OnGrabbed();
                                }
                            }
                        }
                    }
                    public static void Matthing()
                    {
                        if (GorillaTagger.Instance.offlineVRRig != null && GorillaTagger.Instance.offlineVRRig.enabled)
                        {
                            if (GorillaGameManager.instance != null)
                            {
                                GorillaTagger.Instance.offlineVRRig.ChangeMaterialLocal(GorillaGameManager.instance.MyMatIndex(PhotonNetwork.LocalPlayer));
                            }
                            else
                            {
                                GorillaTagger.Instance.offlineVRRig.ChangeMaterialLocal(0);
                            }
                        }
                    }
                    public static void InvsMonkey()
                    {
                        if (ControllerInput.RightTrigger)
                        {
                            if (!ghostToggled && GorillaTagger.Instance.offlineVRRig.enabled)
                            {
                                if (GorillaTagger.Instance.offlineVRRig.enabled)
                                {
                                    VRRigOnDisable.Prefix(GorillaTagger.Instance.offlineVRRig);
                                }
                                GorillaTagger.Instance.offlineVRRig.enabled = false;
                                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(999f, 999f, 999f);
                                ghostToggled = true;
                            }
                            else
                            {
                                if (!ghostToggled && !GorillaTagger.Instance.offlineVRRig.enabled)
                                {
                                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                                    Matthing();
                                    ghostToggled = true;
                                }
                            }
                        }
                        else
                        {
                            ghostToggled = false;
                        }
                    }
                    public static void GhostMonkey()
                    {
                        if (ControllerInput.RightTrigger)
                        {
                            if (!ghostToggled && GorillaTagger.Instance.offlineVRRig.enabled)
                            {
                                if (GorillaTagger.Instance.offlineVRRig.enabled)
                                {
                                    VRRigOnDisable.Prefix(GorillaTagger.Instance.offlineVRRig);
                                }
                                GorillaTagger.Instance.offlineVRRig.enabled = false;
                                ghostToggled = true;
                            }
                            else
                            {
                                if (!ghostToggled && !GorillaTagger.Instance.offlineVRRig.enabled)
                                {
                                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                                    Matthing();
                                    ghostToggled = true;
                                }
                            }
                        }
                        else
                        {
                            ghostToggled = false;
                        }
                    }
                    public static void SpeedBoost()
                    {
                        if (PhotonNetwork.InRoom)
                        {
                            GorillaGameManager gameManager = GorillaGameManager.instance;
                            GorillaLocomotion.Player player = GorillaLocomotion.Player.Instance;


                            player.maxJumpSpeed = gameManager.fastJumpLimit;
                            player.jumpMultiplier = gameManager.fastJumpMultiplier;

                            gameManager.fastJumpLimit = 8.5f;
                            gameManager.fastJumpMultiplier = 1.2f;
                        }
                    }
                    public static void SilentAim()
                    {
                        foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                        {
                            if (Vector3.Distance(vrrig.transform.position, RigManager.GetOfflineRig().transform.position) <= 9 && RigManager.GetRigView(vrrig).Owner != PhotonNetwork.LocalPlayer)
                            {
                                foreach (SlingshotProjectile slingshot in UnityEngine.Object.FindObjectsOfType<SlingshotProjectile>())
                                {
                                    slingshot.projectileOwner = PhotonNetwork.LocalPlayer;
                                    slingshot.transform.position = vrrig.transform.position;
                                };
                            }
                        }
                    }
                    public static void PlatformMonke()
                    {
                        RaiseEventOptions safs = new RaiseEventOptions
                        {
                            Receivers = ReceiverGroup.Others
                        };
                        if (ControllerInput.RightGrip)
                        {
                            if (RightPlat == null)
                            {
                                RightPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                RightPlat.transform.position = new Vector3(0f, -0.0175f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position;
                                RightPlat.transform.rotation = GorillaLocomotion.Player.Instance.rightControllerTransform.rotation;
                                RightPlat.transform.localScale = scale;
                                RightPlat.GetComponent<Renderer>().material.color = Color.black;
                                PhotonNetwork.RaiseEvent(110, new object[] { RightPlat.transform.position, RightPlat.transform.rotation, scale }, safs, SendOptions.SendReliable);
                            }
                        }
                        else
                        {
                            if (RightPlat != null)
                            {
                                PhotonNetwork.RaiseEvent(111, null, safs, SendOptions.SendReliable);
                                UnityEngine.Object.Destroy(RightPlat);
                                RightPlat = null;
                            }
                        }
                        if (ControllerInput.LeftGrip)
                        {
                            if (LeftPlat == null)
                            {
                                LeftPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                LeftPlat.transform.localScale = scale;
                                LeftPlat.transform.position = new Vector3(0f, -0.0175f, 0f) + GorillaLocomotion.Player.Instance.leftControllerTransform.position;
                                LeftPlat.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
                                LeftPlat.GetComponent<Renderer>().material.color = Color.black;
                                PhotonNetwork.RaiseEvent(120, new object[] { LeftPlat.transform.position, LeftPlat.transform.rotation, scale }, safs, SendOptions.SendReliable);
                            }
                        }
                        else
                        {
                            if (LeftPlat != null)
                            {
                                PhotonNetwork.RaiseEvent(121, null, safs, SendOptions.SendReliable);
                                UnityEngine.Object.Destroy(LeftPlat);
                                LeftPlat = null;
                            }
                        }
                    }
                    public static void ImagePlatformMonke()
                    {
                        if (plattype == 0f)
                        {
                            rPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            lPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        }
                        if (plattype == 1f)
                        {
                            rPlat = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            lPlat = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        }
                        if (plattype == 2f)
                        {
                            rPlat = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                            lPlat = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                        }
                        if (plattype == 3f)
                        {
                            rPlat = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                            lPlat = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                        }
                        if (plattype == 4f)
                        {
                            rPlat = GameObject.CreatePrimitive(PrimitiveType.Plane);
                            lPlat = GameObject.CreatePrimitive(PrimitiveType.Plane);
                        }
                        if (plattype == 5f)
                        {
                            rPlat = GameObject.CreatePrimitive(PrimitiveType.Quad);
                            lPlat = GameObject.CreatePrimitive(PrimitiveType.Quad);
                        }
                        float rGrip = ControllerInputPoller.GripFloat(XRNode.RightHand);
                        float lGrip = ControllerInputPoller.GripFloat(XRNode.LeftHand);

                        if (rGrip > 0.6f)
                        {
                            if (!onceRightGrip)
                            {
                                if (rPlat == null)
                                {
                                    string folderPath = "FISH_Mods_Config";
                                    if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }
                                    string imagePath = Path.Combine(folderPath, "FISH_Right_Platforms_Pic.png");
                                    if (!File.Exists(imagePath))
                                    {
                                        WebClient webClient = new WebClient();
                                        byte[] imageData = webClient.DownloadData("https://notfishvr6969.github.io/Image/NIKO_Right_Platforms_Pic.png");
                                        File.WriteAllBytes(imagePath, imageData);
                                    }
                                    Texture2D texture = new Texture2D(2, 2);
                                    byte[] imageBytes = File.ReadAllBytes(imagePath);
                                    texture.LoadImage(imageBytes);
                                    Material material = new Material(Shader.Find("Standard"));
                                    material.mainTexture = texture;
                                    rPlat.GetComponent<Renderer>().material = material;
                                    rPlat.transform.localScale = new Vector3(0.0125f, 0.28f, 0.3825f);
                                }
                                rPlat.transform.position = GorillaLocomotion.Player.Instance.rightControllerTransform.position;
                                rPlat.transform.rotation = GorillaLocomotion.Player.Instance.rightControllerTransform.rotation;
                                onceRightGrip = true;
                            }
                        }
                        else
                        {
                            if (onceRightGrip)
                            {
                                onceRightGrip = false;
                                GameObject.Destroy(rPlat);
                            }
                        }
                        if (lGrip > 0.6f)
                        {
                            if (!onceLeftGrip)
                            {
                                if (lPlat == null)
                                {
                                    string folderPath = "FISH_Mods_Config";
                                    if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }
                                    string imagePath = Path.Combine(folderPath, "FISH_Left_Platforms_Pic.png");
                                    if (!File.Exists(imagePath))
                                    {
                                        WebClient webClient = new WebClient();
                                        byte[] imageData = webClient.DownloadData("https://notfishvr6969.github.io/Image/NIKO_Left_Platforms_Pic.png");
                                        File.WriteAllBytes(imagePath, imageData);
                                    }
                                    Texture2D texture = new Texture2D(2, 2);
                                    byte[] imageBytes = File.ReadAllBytes(imagePath);
                                    texture.LoadImage(imageBytes);
                                    Material material = new Material(Shader.Find("Standard"));
                                    material.mainTexture = texture;
                                    lPlat.GetComponent<Renderer>().material = material;
                                    lPlat.transform.localScale = new Vector3(0.0125f, 0.28f, 0.3825f);
                                }
                                lPlat.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position;
                                lPlat.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
                                onceLeftGrip = true;
                            }
                        }
                        else
                        {
                            if (onceLeftGrip)
                            {
                                if (onceLeftGrip)
                                {
                                    onceLeftGrip = false;
                                    GameObject.Destroy(lPlat);
                                }
                            }
                        }
                    }
                    public static void Fly()
                    {
                        GorillaLocomotion.Player player = GorillaLocomotion.Player.Instance;
                        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
                        Vector3 flyDirection = player.headCollider.transform.forward;
                        if (!ControllerInput.RightTrigger)
                        {
                            if (flying)
                            {
                                playerRigidbody.velocity = flyDirection * Time.deltaTime * FlySpeed;
                                flying = false;
                            }
                        }
                        else
                        {
                            player.transform.position += flyDirection * FlySpeed;
                            playerRigidbody.velocity = Vector3.zero;
                            if (!flying)
                            {
                                flying = true;
                            }
                        }
                    }
                    public static void NoClip()
                    {
                        if (ControllerInput.RightTrigger)
                        {
                            MeshCollider[] meshColliders = Resources.FindObjectsOfTypeAll<MeshCollider>();
                            foreach (MeshCollider collider in meshColliders)
                            {
                                collider.enabled = false;
                            }
                        }
                        else
                        {
                            MeshCollider[] meshColliders = Resources.FindObjectsOfTypeAll<MeshCollider>();
                            foreach (MeshCollider collider in meshColliders)
                            {
                                collider.enabled = true;
                            }
                        }
                    }
                    public static void Beacons()
                    {
                        if (PhotonNetwork.CurrentRoom != null)
                        {
                            foreach (Photon.Realtime.Player player3 in PhotonNetwork.PlayerListOthers)
                            {
                                PhotonView photonView3 = GorillaGameManager.instance.FindVRRigForPlayer(player3);
                                VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(player3);
                                if (!vrrig.isOfflineVRRig && !vrrig.isMyPlayer && !photonView3.IsMine)
                                {
                                    GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                                    UnityEngine.Object.Destroy(gameObject.GetComponent<BoxCollider>());
                                    UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
                                    UnityEngine.Object.Destroy(gameObject.GetComponent<Collider>());
                                    gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
                                    gameObject.transform.rotation = Quaternion.identity;
                                    gameObject.transform.localScale = new Vector3(0.04f, 200f, 0.04f);
                                    gameObject.transform.position = vrrig.transform.position;
                                    gameObject.GetComponent<MeshRenderer>().material = vrrig.mainSkin.material;
                                    UnityEngine.Object.Destroy(gameObject, Time.deltaTime);
                                }
                            }
                        }
                    }
                    public static void Slingshot(string Thing)
                    {
                        if (GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/rig/body/Slingshot Chest Snap/DropZoneAnchor/Slingshot Anchor/Slingshot").GetComponent<Slingshot>().projectilePrefab.tag != Thing)
                        {
                            GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/rig/body/Slingshot Chest Snap/DropZoneAnchor/Slingshot Anchor/Slingshot").GetComponent<Slingshot>().projectilePrefab.tag = Thing;
                        }
                    }
                    public static void BoneESP()
                    {
                        foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                        {
                            if (!vrrig.isOfflineVRRig && !vrrig.isMyPlayer)
                            {
                                if (vrrig != null)
                                {
                                    Material material2 = new Material(Shader.Find("GUI/Text Shader"));
                                    material2.color = Color.green;
                                    if (!vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>())
                                    {
                                        vrrig.head.rigTarget.gameObject.AddComponent<LineRenderer>();
                                    }
                                    vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().endWidth = 0.015f;
                                    vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().startWidth = 0.015f;
                                    vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().material = material2;
                                    vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().SetPosition(0, vrrig.head.rigTarget.transform.position + new Vector3(0f, 0.16f, 0f));
                                    vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().SetPosition(1, vrrig.head.rigTarget.transform.position - new Vector3(0f, 0.4f, 0f));
                                    for (int j = 0; j < Enumerable.Count<int>(bones); j += 2)
                                    {
                                        if (!vrrig.mainSkin.bones[bones[j]].gameObject.GetComponent<LineRenderer>())
                                        {
                                            vrrig.mainSkin.bones[bones[j]].gameObject.AddComponent<LineRenderer>();
                                        }
                                        vrrig.mainSkin.bones[bones[j]].gameObject.GetComponent<LineRenderer>().endWidth = 0.015f;
                                        vrrig.mainSkin.bones[bones[j]].gameObject.GetComponent<LineRenderer>().startWidth = 0.015f;
                                        vrrig.mainSkin.bones[bones[j]].gameObject.GetComponent<LineRenderer>().material = material2;
                                        vrrig.mainSkin.bones[bones[j]].gameObject.GetComponent<LineRenderer>().SetPosition(0, vrrig.mainSkin.bones[bones[j]].position);
                                        vrrig.mainSkin.bones[bones[j]].gameObject.GetComponent<LineRenderer>().SetPosition(1, vrrig.mainSkin.bones[bones[j + 1]].position);
                                    }
                                }
                            }
                            else
                            {
                                Material material3 = new Material(Shader.Find("GUI/Text Shader"));
                                material3.color = BoneESPColor;
                                if (!vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>())
                                {
                                    vrrig.head.rigTarget.gameObject.AddComponent<LineRenderer>();
                                }
                                vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().endWidth = 0.015f;
                                vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().startWidth = 0.015f;
                                vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().material = material3;
                                vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().SetPosition(0, vrrig.head.rigTarget.transform.position + new Vector3(0f, 0.16f, 0f));
                                vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().SetPosition(1, vrrig.head.rigTarget.transform.position - new Vector3(0f, 0.4f, 0f));
                                for (int k = 0; k < Enumerable.Count<int>(bones); k += 2)
                                {
                                    if (!vrrig.mainSkin.bones[bones[k]].gameObject.GetComponent<LineRenderer>())
                                    {
                                        vrrig.mainSkin.bones[bones[k]].gameObject.AddComponent<LineRenderer>();
                                    }
                                    vrrig.mainSkin.bones[bones[k]].gameObject.GetComponent<LineRenderer>().endWidth = 0.015f;
                                    vrrig.mainSkin.bones[bones[k]].gameObject.GetComponent<LineRenderer>().startWidth = 0.015f;
                                    vrrig.mainSkin.bones[bones[k]].gameObject.GetComponent<LineRenderer>().material = material3;
                                    vrrig.mainSkin.bones[bones[k]].gameObject.GetComponent<LineRenderer>().SetPosition(0, vrrig.mainSkin.bones[bones[k]].position);
                                    vrrig.mainSkin.bones[bones[k]].gameObject.GetComponent<LineRenderer>().SetPosition(1, vrrig.mainSkin.bones[bones[k + 1]].position);
                                }
                            }
                        }
                    }
                    public static void TPGun()
                    {
                        if (ControllerInput.RightGrip)
                        {
                            RaycastHit raycastHit;
                            if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                            {
                                pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                                UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                                pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                                pointer.GetComponent<Renderer>().material.color = Color.red;
                            }
                            pointer.transform.position = raycastHit.point;
                            if (ControllerInput.RightTrigger)
                            {
                                pointer.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().isKinematic = true;
                                float maxDistanceDelta = TPGunSpeed * Time.deltaTime;
                                GameObject.Find("GorillaPlayer").transform.position = Vector3.MoveTowards(GameObject.Find("GorillaPlayer").transform.position, pointer.transform.position, maxDistanceDelta);
                                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().isKinematic = false;
                            }
                            else
                            {
                                pointer.GetComponent<Renderer>().material.color = Color.red;
                            }
                            return;
                        }
                        UnityEngine.GameObject.Destroy(pointer);
                    }
                }
            }
            public class Utils
            {
                public static void CreateConfigFileIfNotExists(string fileName, string url)
                {
                    string configFolderPath = "FISH_Mods_Config";
                    string configFile = Path.Combine(configFolderPath, fileName);

                    if (!File.Exists(configFile))
                    {
                        if (!Directory.Exists(configFolderPath))
                        {
                            Directory.CreateDirectory(configFolderPath);
                        }

                        using (StreamWriter sw = File.CreateText(configFile))
                        {
                            sw.WriteLine("Ty For Useing FISH Mods Mod Menu, Do Not Del This, Only Del This If You Want To Get Inved Back To The Discord Server, ReOpen Your Game To Get Inved");
                        }

                        if (!string.IsNullOrEmpty(url))
                        {
                            Process.Start(Uri.EscapeUriString(url));
                        }

                        Process.Start(configFile);
                    }
                }
                public static void LogError(Exception ex)
                {
                    string logDirectory = "FISH_Mods_Error";
                    Directory.CreateDirectory(logDirectory);

                    string errorMessage = $"Error Message: {ex.Message}";
                    string stackTrace = $"Error occurred on Line: {ex.StackTrace}";
                    string logFileName = Path.Combine(logDirectory, $"FISH_Mods_error_{DateTime.Now:yyyy_MM_dd_HHmm_ss}.log");

                    File.WriteAllText(logFileName, ex.ToString());
                }
            }
        }
        #endregion
        #region Utils
        private void Awake()
        {
            _instance = this;
        }
        public static void UpdateMaterialColors()
        {
            MenuColor.mainTexture = menutexture;
            BtnDisabledColor.color = Color.black;
            BtnEnabledColor.color = Color.blue;
            Next.color = Color.grey;
            Previous.color = Color.grey;
        }
        internal static void GetOwnershipPhotonView(PhotonView view)
        {
            view.OwnershipTransfer = OwnershipOption.Takeover;
            view.RequestOwnership();
            view.TransferOwnership(PhotonNetwork.LocalPlayer);
            view.ControllerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
        }
        public static void SaveSettings()
        {
            PlayerPrefs.SetInt("ESpInt", ESpInt);
            PlayerPrefs.SetInt("BoneESpInt", BoneESpInt);
            PlayerPrefs.SetInt("SpeedCount", SpeedCount);
            PlayerPrefs.SetInt("platCountType", platCountType);
            PlayerPrefs.SetInt("platCountColor", platCountColor);
            PlayerPrefs.SetInt("TPSpeedCount", TPSpeedCount);
            PlayerPrefs.SetInt("SlingshotCountType", SlingshotCountType);
            PlayerPrefs.SetInt("BugCountType", BugCountType);
            PlayerPrefs.Save();
        }
        public static void LoadSettings()
        {
            ESpInt = PlayerPrefs.GetInt("ESpInt", ESpInt);
            BoneESpInt = PlayerPrefs.GetInt("BoneESpInt", BoneESpInt);
            SpeedCount = PlayerPrefs.GetInt("SpeedCount", SpeedCount);
            platCountType = PlayerPrefs.GetInt("platCountType", platCountType);
            platCountColor = PlayerPrefs.GetInt("platCountColor", platCountColor);
            TPSpeedCount = PlayerPrefs.GetInt("TPSpeedCount", TPSpeedCount);
            SlingshotCountType = PlayerPrefs.GetInt("SlingshotCountType", SlingshotCountType);
            BugCountType = PlayerPrefs.GetInt("BugCountType", BugCountType);
        }
        public void Projectile(int Hash, Vector3 vel, Vector3 pos, Color color, int trail = -1)
        {
            SlingshotProjectile component = ObjectPools.instance.Instantiate(Hash).GetComponent<SlingshotProjectile>();
            float num = Mathf.Abs(GorillaTagger.Instance.offlineVRRig.slingshot.projectilePrefab.transform.lossyScale.x);
            int num2 = 1;
            if (GorillaGameManager.instance != null)
            {
                GorillaGameManager.instance.returnPhotonView.RPC("LaunchSlingshotProjectile", RpcTarget.All, new object[]
                {
                    pos,
                    vel,
                    Hash,
                    trail,
                    true,
                    num2,
                    false,
                    color.r,
                    color.g,
                    color.b,
                    1f
                });
            }
            component.Launch(pos, vel, PhotonNetwork.LocalPlayer, false, false, num2, num, true, color);
            SlingshotProjectileManager.RegisterSP(component);
        }
        public void ProcessTagAura(Photon.Realtime.Player pl)
        {
            if ((double)Time.time > (double)TagAura + 0.1)
            {
                float num = Vector3.Distance(GorillaTagger.Instance.offlineVRRig.transform.position, GorillaGameManager.instance.FindPlayerVRRig(pl).transform.position);
                if (num < GorillaGameManager.instance.tagDistanceThreshold)
                {
                    PhotonView.Get(GorillaGameManager.instance.GetComponent<GorillaGameManager>()).RPC("ReportTagRPC", RpcTarget.MasterClient, pl);
                }
                TagAura = Time.time;
            }
        }
        public void TagGun()
        {
            if (ControllerInput.RightGrip)
            {
                if (Tagger == null)
                {
                    RaycastHit raycastHit;
                    if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                    {
                        pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        Object.Destroy(pointer.GetComponent<Rigidbody>());
                        Object.Destroy(pointer.GetComponent<SphereCollider>());
                        pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        pointer.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f, 0.15f);
                        pointer.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                    }
                    pointer.transform.position = raycastHit.point;
                    if (ControllerInput.RightTrigger)
                    {
                        if (raycastHit.collider.GetComponentInParent<VRRig>())
                        {
                            Tagger = raycastHit.collider.GetComponentInParent<VRRig>();
                        }
                    }
                }
                if (ControllerInput.RightTrigger && Tagger != null)
                {
                    pointer.transform.position = Tagger.transform.position;
                    pointer.GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f, 0.15f);
                    if (GorillaGameManager.instance.GameModeName().Contains("INFECTION"))
                    {
                        GorillaTagger.Instance.offlineVRRig.transform.position = Tagger.transform.position;
                        ProcessTagAura(RigManager.GetRigView(Tagger).Owner);
                    }
                }
                else
                {
                    pointer.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f, 0.15f);
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                    Tagger = null;
                }
            }
        }
        public void TagAll()
        {
            /*if (TagAllTime2 < Time.time)
            {
                TagAllTime2 = Time.time + 0.01f;
                beesPlayer = RigManager.GetRandomPlayer(false);
                VRRig rigFromPlayer = RigManager.FindRig(beesPlayer);
                if (!rigFromPlayer.mainSkin.material.name.Contains("fected"))
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = rigFromPlayer.transform.position - new Vector3(0f, 3f, 0f);
                    GorillaGameManager.instance.returnPhotonView.RPC("ReportTagRPC", RpcTarget.MasterClient, beesPlayer);
                }
            }*/
            if (GorillaGameManager.instance != null)
            {
                if (GorillaGameManager.instance.GameModeName().Contains("INFECTION"))
                {
                    if (GorillaTagManager == null)
                    {
                        GorillaTagManager = GorillaGameManager.instance.gameObject.GetComponent<GorillaTagManager>();
                    }
                }
                else if (GorillaGameManager.instance.GameModeName().Contains("HUNT"))
                {
                    if (GorillaHuntManager == null)
                    {
                        GorillaHuntManager = GorillaGameManager.instance.gameObject.GetComponent<GorillaHuntManager>();
                    }
                }
                else if (GorillaGameManager.instance.GameModeName().Contains("BATTLE"))
                {
                    if (GorillaBattleManager == null)
                    {
                        GorillaBattleManager = GorillaGameManager.instance.gameObject.GetComponent<GorillaBattleManager>();
                    }
                }
            }
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerListOthers)
            {
                if (GorillaTagManager != null)
                {
                    if (GorillaTagManager.currentInfected.Contains(PhotonNetwork.LocalPlayer))
                    {
                        if (!GorillaTagManager.currentInfected.Contains(player))
                        {
                            GorillaTagger.Instance.offlineVRRig.transform.position = GorillaGameManager.instance.FindPlayerVRRig(player).transform.position;
                            ProcessTagAura(player);
                            break;
                        }
                    }
                }
                if (GorillaHuntManager != null)
                {
                    if (GorillaTagger.Instance.offlineVRRig.huntComputer.activeSelf && GorillaTagger.Instance.offlineVRRig.huntComputer.GetComponent<GorillaHuntComputer>().myRig != null)
                    {
                        GorillaTagger.Instance.offlineVRRig.transform.position = GorillaTagger.Instance.offlineVRRig.huntComputer.GetComponent<GorillaHuntComputer>().myRig.transform.position;
                        ProcessTagAura(player);
                        break;
                    }
                }
                if (GorillaBattleManager != null)
                {
                    if (GorillaBattleManager.playerLives[player.ActorNumber] == 0 || GorillaBattleManager.playerLives[player.ActorNumber] == 1 || GorillaBattleManager.playerLives[player.ActorNumber] == 2)
                    {
                        GorillaBattleManager.playerLives[player.ActorNumber] = 3;
                        ProcessTagAura(player);
                        break;
                    }
                }
            }
        }
        public void SpamGun(int Hash, float Timer)
        {
            if (ControllerInput.RightGrip)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && pointer == null)
                {
                    pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                    UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                    pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    pointer.GetComponent<Renderer>().material.color = Color.red;
                }
                pointer.transform.position = raycastHit.point;
                if (ControllerInput.RightTrigger && Time.time > Timer + 0.07f)
                {
                    Vector3 vector = ((Vector3)raycastHit.point - (Vector3)GorillaTagger.Instance.offlineVRRig.transform.position).normalized;
                    vector *= 50f;
                    Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                    Instance.Projectile(Hash, vector, GorillaTagger.Instance.offlineVRRig.transform.position, color, -1);
                    Timer = Time.time;
                }
                UnityEngine.GameObject.Destroy(pointer);
            }
        }
        public void EarapeAura()
        {
            if (Time.time > TagAura + 0.5)
            {
                PhotonView.Get(GorillaGameManager.instance.GetComponent<GorillaGameManager>()).RPC("SpawnSlingshotPlayerImpactEffect", RpcTarget.All, RigManager.GetNearbyPosition(RigManager.GetOwnVRRig().transform.position, 20f), 0, 0, 1, 0, 1);
                TagAura = Time.time;
            }
        }
        private IEnumerator AntiBan()
        {
            if (!PhotonNetwork.CurrentRoom.CustomProperties["gameMode"].ToString().Contains("MODDED"))
            {
                ExecuteCloudScriptRequest executeCloudScriptRequest = new ExecuteCloudScriptRequest();
                executeCloudScriptRequest.FunctionName = "RoomClosed";
                executeCloudScriptRequest.FunctionParameter = new
                {
                    GameId = PhotonNetwork.CurrentRoom.Name,
                    Region = Regex.Replace(PhotonNetwork.CloudRegion, "[^a-zA-Z0-9]", "").ToUpper(),
                    UserId = PhotonNetwork.MasterClient.UserId,
                    ActorNr = 1,
                    ActorCount = 1,
                    AppVersion = PhotonNetwork.AppVersion
                };
                PlayFabClientAPI.ExecuteCloudScript(executeCloudScriptRequest, delegate (ExecuteCloudScriptResult result)
                {
                    Debug.Log(":)");
                }, null, null, null);
                yield return new WaitForSeconds(0.5f);
                string gamemode = PhotonNetwork.CurrentRoom.CustomProperties["gameMode"].ToString().Replace(GorillaComputer.instance.currentQueue, GorillaComputer.instance.currentQueue + "MODDEDMODDED");
                ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable { { "gameMode", gamemode } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(hash, null, null);
                yield return new WaitForSeconds(0.5f);
                PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                yield break;
            }
            yield break;
        }
        #endregion
        #region Main Menu
        public static void PlatformNetwork(EventData data)
        {
            if (data.Code == 110)
            {
                object[] customshit = (object[])data.CustomData;
                RightPlat_Networked[data.Sender] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                RightPlat_Networked[data.Sender].transform.position = (Vector3)customshit[0];
                RightPlat_Networked[data.Sender].transform.rotation = (Quaternion)customshit[1];
                RightPlat_Networked[data.Sender].transform.localScale = (Vector3)customshit[2];
                RightPlat_Networked[data.Sender].GetComponent<BoxCollider>().enabled = false;
            }
            if (data.Code == 120)
            {
                object[] customshit = (object[])data.CustomData;
                LeftPlat_Networked[data.Sender] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                LeftPlat_Networked[data.Sender].transform.position = (Vector3)customshit[0];
                LeftPlat_Networked[data.Sender].transform.rotation = (Quaternion)customshit[1];
                LeftPlat_Networked[data.Sender].transform.localScale = (Vector3)customshit[2];
                LeftPlat_Networked[data.Sender].GetComponent<BoxCollider>().enabled = false;
            }
            if (data.Code == 110)
            {
                UnityEngine.Object.Destroy(RightPlat_Networked[data.Sender]);
                RightPlat_Networked[data.Sender] = null;
            }
            if (data.Code == 121)
            {
                UnityEngine.Object.Destroy(LeftPlat_Networked[data.Sender]);
                LeftPlat_Networked[data.Sender] = null;
            }
        }
        #region Draw
        private static void AddButton(float offset, string text, string[] btns, bool[] btnsActive)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.transform.parent = menu.transform;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(0.09f, 0.8f, 0.08f);
            gameObject.transform.localPosition = new Vector3(0.56f, 0f, 0.58f - offset);
            gameObject.AddComponent<BtnCollider>().relatedText = text;
            gameObject.name = text;

            int num = -1;

            for (int i = 0; i < btns.Length; i++)
            {
                if (text == btns[i])
                {
                    num = i;
                    break;
                }
            }

            if (btnsActive[num] == false)
            {
                gameObject.GetComponent<Renderer>().material.color = Color.black;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = Color.red;
            }

            GameObject gameObject2 = new GameObject();
            gameObject2.transform.parent = canvasObj.transform;

            Text text2 = gameObject2.AddComponent<UnityEngine.UI.Text>();
            text2.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text2.text = text;
            text2.fontSize = 1;
            text2.fontStyle = FontStyle.Italic;
            text2.alignment = TextAnchor.MiddleCenter;
            text2.resizeTextForBestFit = true;
            text2.resizeTextMinSize = 0;

            RectTransform component = text2.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(0.2f, 0.03f);
            component.localPosition = new Vector3(0.064f, 0f, 0.231f - offset / 2.55f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }
        public static void Draw()
        {
            menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(menu.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(menu.GetComponent<BoxCollider>());
            UnityEngine.Object.Destroy(menu.GetComponent<Renderer>());
            menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.4f);
            menu.name = "Menu";

            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(gameObject.GetComponent<BoxCollider>());
            gameObject.transform.parent = menu.transform;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(0.1f, 0.94f, 1.2f);
            gameObject.name = "Menucolor";
            gameObject.transform.position = new Vector3(0.05f, 0f, -0.03f);
            canvasObj = new GameObject();
            canvasObj.transform.parent = menu.transform;
            canvasObj.name = "canvas";
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvasScaler.dynamicPixelsPerUnit = 1000f;

            GameObject gameObject2 = new GameObject();
            gameObject2.transform.parent = canvasObj.transform;
            gameObject2.name = "Title";
            Text text = gameObject2.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text.text = "Fish Menu";
            text.fontSize = 1;
            text.fontStyle = FontStyle.Italic;
            text.alignment = TextAnchor.MiddleCenter;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(0.28f, 0.05f);
            component.position = new Vector3(0.06f, 0f, 0.175f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

            if (NumberForPage == 1)
            {
                AddPageButtons(buttons);
                string[] array2 = buttons.Skip(pageNumber * pageSize).Take(pageSize).ToArray();
                for (int i = 0; i < array2.Length; i++)
                {
                    AddButton((float)i * 0.13f + 0.26f, array2[i], buttons, buttonsActive);
                }
            }
            if (NumberForPage == 2)
            {
                AddPageButtons(Settingsbuttons);
                string[] array2 = Settingsbuttons.Skip(pageNumber * pageSize).Take(pageSize).ToArray();
                for (int i = 0; i < array2.Length; i++)
                {
                    AddButton((float)i * 0.13f + 0.26f, array2[i], Settingsbuttons, SettingsButtonsActive);
                }
            }
            if (NumberForPage == 3)
            {
                AddPageButtons(Basicbuttons);
                string[] array2 = Basicbuttons.Skip(pageNumber * pageSize).Take(pageSize).ToArray();
                for (int i = 0; i < array2.Length; i++)
                {
                    AddButton((float)i * 0.13f + 0.26f, array2[i], Basicbuttons, BasicButtonsActive);
                }
            }
            if (NumberForPage == 4)
            {
                AddPageButtons(Ropebuttons);
                string[] array2 = Ropebuttons.Skip(pageNumber * pageSize).Take(pageSize).ToArray();
                for (int i = 0; i < array2.Length; i++)
                {
                    AddButton((float)i * 0.13f + 0.26f, array2[i], Ropebuttons, RopeButtonsActive);
                }
            }
            if (NumberForPage == 5)
            {
                AddPageButtons(SpamRpcbuttons);
                string[] array2 = SpamRpcbuttons.Skip(pageNumber * pageSize).Take(pageSize).ToArray();
                for (int i = 0; i < array2.Length; i++)
                {
                    AddButton((float)i * 0.13f + 0.26f, array2[i], SpamRpcbuttons, SpamRpcButtonsActive);
                }
            }
            if (NumberForPage == 6)
            {
                AddPageButtons(Bugbuttons);
                string[] array2 = Bugbuttons.Skip(pageNumber * pageSize).Take(pageSize).ToArray();
                for (int i = 0; i < array2.Length; i++)
                {
                    AddButton((float)i * 0.13f + 0.26f, array2[i], Bugbuttons, BugButtonsActive);
                }
            }
            if (NumberForPage == 7)
            {
                AddPageButtons(Tagbuttons);
                string[] array2 = Tagbuttons.Skip(pageNumber * pageSize).Take(pageSize).ToArray();
                for (int i = 0; i < array2.Length; i++)
                {
                    AddButton((float)i * 0.13f + 0.26f, array2[i], Tagbuttons, TagButtonsActive);
                }
            }
            if (NumberForPage == 8)
            {
                AddPageButtons(Micbuttons);
                string[] array2 = Micbuttons.Skip(pageNumber * pageSize).Take(pageSize).ToArray();
                for (int i = 0; i < array2.Length; i++)
                {
                    AddButton((float)i * 0.13f + 0.26f, array2[i], Micbuttons, MicButtonsActive);
                }
            }
            if (NumberForPage == 99)
            {
                AddPageButtons(Halloweenbuttons);
                string[] array2 = Halloweenbuttons.Skip(pageNumber * pageSize).Take(pageSize).ToArray();
                for (int i = 0; i < array2.Length; i++)
                {
                    AddButton((float)i * 0.13f + 0.26f, array2[i], Halloweenbuttons, HalloweenButtonsActive);
                }
            }
            if (NumberForPage == 69)
            {
                AddPageButtons(OPbuttons);
                string[] array2 = OPbuttons.Skip(pageNumber * pageSize).Take(pageSize).ToArray();
                for (int i = 0; i < array2.Length; i++)
                {
                    AddButton((float)i * 0.13f + 0.26f, array2[i], OPbuttons, OPButtonsActive);
                }
            }
        }
        private static void AddPageButtons(string[] btns)
        {
            int num = (btns.Length + pageSize - 1) / pageSize;
            int num2 = pageNumber + 1;
            int num3 = pageNumber - 1;
            if (num2 > num - 1)
            {
                num2 = 0;
            }
            if (num3 < 0)
            {
                num3 = num - 1;
            }
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.transform.parent = menu.transform;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(0.09f, 0.15f, 0.98f);
            gameObject.transform.localPosition = new Vector3(0.56f, 0.5833f, -0.13f);
            gameObject.AddComponent<BtnCollider>().relatedText = "PreviousPage";
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            gameObject.name = "back";
            gameObject.GetComponent<Renderer>().material.color = Color.black;
            GameObject gameObject2 = new GameObject();
            gameObject2.transform.parent = canvasObj.transform;
            gameObject2.name = "back";
            Text text = gameObject2.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text.text = "<";
            text.fontSize = 1;
            text.alignment = TextAnchor.MiddleCenter;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(0.2f, 0.03f);
            component.localPosition = new Vector3(0.064f, 0.175f, -0.04f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            component.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gameObject3.GetComponent<Renderer>().material.color = Color.black;
            UnityEngine.Object.Destroy(gameObject3.GetComponent<Rigidbody>());
            gameObject3.GetComponent<BoxCollider>().isTrigger = true;
            gameObject3.transform.parent = menu.transform;
            gameObject3.transform.rotation = Quaternion.identity;
            gameObject3.name = "Next";
            gameObject3.transform.localScale = new Vector3(0.09f, 0.15f, 0.98f);
            gameObject3.transform.localPosition = new Vector3(0.56f, -0.5833f, -0.13f);
            gameObject3.AddComponent<BtnCollider>().relatedText = "NextPage";
            gameObject3.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            GameObject gameObject4 = new GameObject();
            gameObject4.transform.parent = canvasObj.transform;
            gameObject4.name = "Next";
            Text text2 = gameObject4.AddComponent<Text>();
            text2.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text2.text = ">";
            text2.fontSize = 1;
            text2.alignment = TextAnchor.MiddleCenter;
            text2.resizeTextForBestFit = true;
            text2.resizeTextMinSize = 0;
            RectTransform component2 = text2.GetComponent<RectTransform>();
            component2.localPosition = Vector3.zero;
            component2.sizeDelta = new Vector2(0.2f, 0.03f);
            component2.localPosition = new Vector3(0.064f, -0.175f, -0.04f);
            component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            component2.localScale = new Vector3(1.3f, 1.3f, 1.3f);






            GameObject gameObject5 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gameObject5.GetComponent<Renderer>().material.color = Color.black;
            UnityEngine.Object.Destroy(gameObject5.GetComponent<Rigidbody>());
            gameObject5.GetComponent<BoxCollider>().isTrigger = true;
            gameObject5.transform.parent = menu.transform;
            gameObject5.transform.rotation = Quaternion.identity;
            gameObject5.name = "LeaveButton";
            gameObject5.transform.localScale = new Vector3(0.09f, 0.7682f, 0.075f);
            gameObject5.transform.localPosition = new Vector3(0.56f, -0.0076f, 0.5755f);
            gameObject5.AddComponent<BtnCollider>().relatedText = "Cum";
            gameObject5.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            GameObject gameObject6 = new GameObject();
            gameObject6.transform.parent = canvasObj.transform;
            gameObject6.name = "LeaveButton";
            Text text3 = gameObject6.AddComponent<Text>();
            text3.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text3.text = "Leave";
            text3.fontSize = 1;
            text3.alignment = TextAnchor.MiddleCenter;
            text3.resizeTextForBestFit = true;
            text3.resizeTextMinSize = 0;
            RectTransform component3 = text3.GetComponent<RectTransform>();
            component3.localPosition = Vector3.zero;
            component3.sizeDelta = new Vector2(0.2f, 0.03f);
            component3.localPosition = new Vector3(0.064f, 0, 0.23f);
            component3.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            component3.localScale = new Vector3(1f, 1f, 1f);

        }
        public static void Toggle(string relatedText, string[] btns, bool[] btnsActive)
        {
            int num = (btns.Length + pageSize - 1) / pageSize;
            if (relatedText == "Cum")
            {
                PhotonNetwork.Disconnect();
                UnityEngine.Object.Destroy(menu);
                menu = null;
                Draw();
                return;
            }
            if (relatedText == "NextPage")
            {
                if (pageNumber < num - 1)
                {
                    pageNumber++;
                }
                else
                {
                    pageNumber = 0;
                }
                UnityEngine.Object.Destroy(menu);
                menu = null;
                Draw();
                return;
            }
            if (relatedText == "PreviousPage")
            {
                if (pageNumber > 0)
                {
                    pageNumber--;
                }
                else
                {
                    pageNumber = num - 1;
                }
                UnityEngine.Object.Destroy(menu);
                menu = null;
                Draw();
                return;
            }
            int num2 = -1;
            for (int i = 0; i < btns.Length; i++)
            {
                if (relatedText == btns[i])
                {
                    num2 = i;
                    break;
                }
            }
            if (btnsActive[num2] != null)
            {
                btnsActive[num2] = !btnsActive[num2];
                UnityEngine.Object.Destroy(menu);
                menu = null;
                Draw();
            }
        }
        #endregion
        #region Field
        public static string _playFabPlayerIdCache, _sessionTicket, userToken;
        public static bool SendMsg = false;
        private bool successfullyFoundFriend;
        public static VRRig kickp;
        public static VRRig lucyp;
        private float startingToLookForFriend;
        public static bool AntiBanOn = false;
        public static string OldName;
        public static FieldInfo fi1 = typeof(InfectionLavaController).GetField("fullTime", BindingFlags.NonPublic);
        public static InfectionLavaController.RisingLavaState fi1a;
        private static readonly RaiseEventOptions KickOptions = new RaiseEventOptions
        {
            CachingOption = EventCaching.AddToRoomCacheGlobal
        };
        public static bool Patches = false;
        public static bool AntiBanStop = false;
        private float timeToSpendLookingForFriend = 15f;
        public string customRoomID;
        public static bool consoleStartAttempt = false;
        public static bool consoleStartAttempt2 = false;
        internal static bool l;
        public static Texture2D menutexture = new Texture2D(2, 3);
        public static GameObject fingerButtonPresser = null;
        public static GameObject MainMenuRef;
        private static bool hasBeenGrabbed = false;
        private static bool hasBeenGrabbed2 = false;
        public static bool Crash = true;
        public static bool OwnerMods = false;
        public static int[] bones = { 4, 3, 5, 4, 19, 18, 20, 19, 3, 18, 21, 20, 22, 21, 25, 21, 29, 21, 31, 29, 27, 25, 24, 22, 6, 5, 7, 6, 10, 6, 14, 6, 16, 14, 12, 10, 9, 7 };
        internal static Coroutine freezeallCoroutine;
        public static GorillaTagManager GorillaTagManager;
        private static GameObject RightPlat;
        private static GameObject LeftPlat;
        private static GameObject[] RightPlat_Networked = new GameObject[9999];
        private static GameObject[] LeftPlat_Networked = new GameObject[9999];
        public static bool SaveSetting = false;
        public static bool LoadSetting = false;
        public static GorillaHuntManager GorillaHuntManager;
        public static HotPepperFace HotPepperFace;
        public static List<Player> lastghostChaser;
        public static GorillaBattleManager GorillaBattleManager;
        public static bool RightToggle;
        public static Material PlatColor = new Material(Shader.Find("GorillaTag/UberShader"));
        public static bool LeftToggle;
        public static GameObject lPlat;
        public static GameObject rPlat;
        public static bool onceRightGrip = false;
        public static bool onceLeftGrip = false;
        public static GameObject MainTextCanvas;
        public static System.Random random = new System.Random();
        public static bool rightsecondarybutton = false;
        public static float orbitSpeed;
        private static float angle;
        private static bool trampolinesRestored = false;
        private static readonly RaiseEventOptions ServerCleanOptions = new RaiseEventOptions { CachingOption = EventCaching.RemoveFromRoomCache };
        private static readonly ExitGames.Client.Photon.Hashtable ServerCleanDestroyEvent = new ExitGames.Client.Photon.Hashtable();
        public static bool once = true;
        public static bool MenuLoaded = true;
        public static bool once3 = true;
        private static float SplashTime;
        private static float SnowBallTimer;
        public static float WaterBalloonTimer;
        private static float RockSpamTimer;
        public string Room;
        private static VRRig lagrig;
        private static int SpeedCount = 0;
        private static int TPSpeedCount = 0;
        private static int platCountType = 0;
        private static int SlingshotCountType = 0;
        private static int BugCountType = 0;
        private static float SlingshotType = 0;
        private static float BugType = 0;
        private static int platCountColor = 0;
        public static VRRig chosenplayer;
        public static int layers = 512;
        public static bool once2 = true;
        public static bool init = true;
        public static Vector2 rockLifetimeRange = new Vector2(5f, 10f);
        public static Vector2 rockSizeRange = new Vector2(0.5f, 2f);
        public static int framePressCooldown = 0;
        public static bool Enabled = true;
        private static float c1;
        private static bool noesp = false;
        public static GameObject C4 = null;
        private static float KickG;
        public static AnimationCurve rockMaxSizeMultiplierVsLavaProgress = AnimationCurve.Linear(0f, 1f, 1f, 1f);
        public static float aimAssistDistance = 5.0f;
        private static float TagAura;
        public static float smth = 0f;
        public static float smth2 = 0f;
        public static int NumberForPage = 1;
        private static GradientColorKey[] colorKeysPlatformMonke = new GradientColorKey[4];
        private static Vector3 scale = new Vector3(0.0125f, 0.28f, 0.3825f);
        private static Vector3? leftHandOffsetInitial = null;
        private static Vector3? rightHandOffsetInitial = null;
        private static GameObject[] jump_left_network = new GameObject[9999];
        private static GameObject[] jump_right_network = new GameObject[9999];
        public static GameObject menu = null;
        private static VRRig Tagger;
        private static bool DontDestroy = true;
        private static GameObject canvasObj = null;
        public static float a;
        private static GameObject reference = null;
        private static float ParticleSpam2;
        private static float RopeTimer;
        private static float plattype = 0f;
        private static GameObject jump_left_local = null;
        private static GameObject jump_right_local = null;
        public static GameObject pointer = null;
        private static GameObject menuObject;
        public static float reporttimer = 0f;
        public static float mastertimer = 0f;
        private static float? maxArmLengthInitial = null;
        private static float? maxJumpSpeed = null;
        private static float? jumpMultiplier = null;
        private static float Timer2;
        private static int btnCooldown = 0;
        private static int pageNumber = 0;
        private static int pageSize = 8;
        private static bool teleportGunAntiRepeat = false;
        private static bool flying = false;
        private static bool DevThing = true;
        private static bool once_left;
        private static bool once_right;
        private static bool once_left_false;
        private static bool once_right_false;
        private static bool once_networking;
        private static bool ghostToggled;
        private static float FlySpeed = 10f;
        public static string dsds;
        private static float TPGunSpeed = 32f;
        public static bool TagAllTime = false;
        public static int ESpInt = 0;
        private static int BoneESpInt = 0;
        private static Color BoneESPColor = Color.white;
        private static Color ESPColor = Color.white;
        private static bool SettingsPageOn = false;
        private static Color Platcolor = Color.clear;
        public static bool[] IsTaggedSelf = new bool[10];
        public static bool[] istagged = new bool[100000];
        private static bool MainPageOn = true;
        public static Material MenuColor = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Material Changer = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Material BtnDisabledColor = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Material BtnEnabledColor = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Material Next = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Material Previous = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Material PointerColor = new Material(Shader.Find("GorillaTag/UberShader"));
        private static MenuPatch _instance;
        public static MenuPatch Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MenuPatch>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("MenuPatch");
                        _instance = obj.AddComponent<MenuPatch>();
                    }
                }
                return _instance;
            }
        }
        #endregion
        #endregion
    }
    #endregion
    #region MainGUI
    [BepInPlugin("FISHMods.GUI", "GUI", "1.0")]
    internal class MainGUI : BaseUnityPlugin
    {
        private void DrawMainTab()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            ToggleMain[1] = ToggleButton("Disconnect", ToggleMain[1]);
            ToggleMain[2] = ToggleButton("Join Random Room", ToggleMain[2]);

            GUILayout.EndScrollView();
        }
        private void DrawMicModsTab()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            /*
            one = GUILayout.Toggle(one, "Emulate Left Trigger");
            if (one)
            {
                ControllerInput.LeftTrigger = true;
            }
            else
            {
                ControllerInput.LeftTrigger = false;
            }
            two = GUILayout.Toggle(two, "Emulate Right Trigger");
            if (two)
            {
                ControllerInput.RightTrigger = true;
            }
            else if (!nine)
            {
                ControllerInput.RightTrigger = false;
            }
            three = GUILayout.Toggle(three, "Emulate Left Grip");
            if (three)
            {
                ControllerInput.LeftGrip = true;
            }
            else
            {
                ControllerInput.LeftGrip = false;
            }
            four = GUILayout.Toggle(four, "Emulate Right Grip");
            if (four)
            {
                ControllerInput.RightGrip = true;
            }
            else if (!nine)
            {
                ControllerInput.RightGrip = false;
            }
            five = GUILayout.Toggle( five, "Emulate Left Primary");
            if (five)
            {
                ControllerInput.LeftPrimary = true;
            }
            else
            {
                ControllerInput.LeftPrimary = false;
            }
            six = GUILayout.Toggle( six, "Emulate Right Primary");
            if (six)
            {
                ControllerInput.RightPrimary = true;
            }
            else
            {
                ControllerInput.RightPrimary = false;
            }
            seven = GUILayout.Toggle(seven, "Emulate Left Secondary");
            if (seven)
            {
                ControllerInput.LeftSecondary = true;
            }
            else
            {
                ControllerInput.LeftSecondary = false;
            }
            eight = GUILayout.Toggle(eight, "Emulate Right Secondary");
            if (eight)
            {
                ControllerInput.RightSecondary = true;
            }
            else
            {
                ControllerInput.RightSecondary = false;
            }
            */
            GUILayout.EndScrollView();
        }
        private void DrawMenuTab()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            if (Main)
            {
                for (int i = 0; i < MenuPatch.buttons.Length; i++)
                {
                    if (i < MenuPatch.buttonsActive.Length)
                    {
                        MenuPatch.buttonsActive[i] = MainGUI.Instance.ToggleButton(MenuPatch.buttons[i], MenuPatch.buttonsActive[i]);
                        if (i == 0 && MenuPatch.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            Settings = true;
                        }
                        if (i == 1 && MenuPatch.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            Basic = true;
                        }
                        if (i == 2 && MenuPatch.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            Rope = true;
                        }
                        if (i == 3 && MenuPatch.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            SpamRpc = true;
                        }
                        if (i == 4 && MenuPatch.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            Bug = true;
                        }
                        if (i == 5 && MenuPatch.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            Tag = true;
                        }
                        if (i == 6 && MenuPatch.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            Mic = true;
                        }
                        if (i == 7 && MenuPatch.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            Halloween = true;
                        }
                        if (i == 8 && MenuPatch.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            OP = true;
                        }
                    }
                }
            }
            if (Settings)
            {
                for (int i = 0; i < MenuPatch.Settingsbuttons.Length; i++)
                {
                    if (i < MenuPatch.SettingsButtonsActive.Length)
                    {
                        MenuPatch.SettingsButtonsActive[i] = MainGUI.Instance.ToggleButton(MenuPatch.Settingsbuttons[i], MenuPatch.SettingsButtonsActive[i]);
                        if (i == 0 && MenuPatch.SettingsButtonsActive[i])
                        {
                            ClearAllButtonStates();
                            Main = true;
                        }
                    }
                }
            }
            if (Rope)
            {
                for (int i = 0; i < MenuPatch.Ropebuttons.Length; i++)
                {
                    if (i < MenuPatch.RopeButtonsActive.Length)
                    {
                        MenuPatch.RopeButtonsActive[i] = MainGUI.Instance.ToggleButton(MenuPatch.Ropebuttons[i], MenuPatch.RopeButtonsActive[i]);
                        if (i == 0 && MenuPatch.RopeButtonsActive[i])
                        {
                            ClearAllButtonStates();
                            Main = true;
                        }
                    }
                }
            }
            if (SpamRpc)
            {
                for (int i = 0; i < MenuPatch.SpamRpcbuttons.Length; i++)
                {
                    if (i < MenuPatch.SpamRpcButtonsActive.Length)
                    {
                        MenuPatch.SpamRpcButtonsActive[i] = MainGUI.Instance.ToggleButton(MenuPatch.SpamRpcbuttons[i], MenuPatch.SpamRpcButtonsActive[i]);
                        if (i == 0 && MenuPatch.SpamRpcButtonsActive[i])
                        {
                            ClearAllButtonStates();
                            Main = true;
                        }
                    }
                }
            }
            if (Bug)
            {
                for (int i = 0; i < MenuPatch.Bugbuttons.Length; i++)
                {
                    if (i < MenuPatch.BugButtonsActive.Length)
                    {
                        MenuPatch.BugButtonsActive[i] = MainGUI.Instance.ToggleButton(MenuPatch.Bugbuttons[i], MenuPatch.BugButtonsActive[i]);
                        if (i == 0 && MenuPatch.BugButtonsActive[i])
                        {
                            ClearAllButtonStates();
                            Main = true;
                        }
                    }
                }
            }
            if (Tag)
            {
                for (int i = 0; i < MenuPatch.Tagbuttons.Length; i++)
                {
                    if (i < MenuPatch.TagButtonsActive.Length)
                    {
                        MenuPatch.TagButtonsActive[i] = MainGUI.Instance.ToggleButton(MenuPatch.Tagbuttons[i], MenuPatch.TagButtonsActive[i]);
                        if (i == 0 && MenuPatch.TagButtonsActive[i])
                        {
                            ClearAllButtonStates();
                            Main = true;
                        }
                    }
                }
            }
            if (Mic)
            {
                for (int i = 0; i < MenuPatch.Micbuttons.Length; i++)
                {
                    if (i < MenuPatch.MicButtonsActive.Length)
                    {
                        MenuPatch.MicButtonsActive[i] = MainGUI.Instance.ToggleButton(MenuPatch.Micbuttons[i], MenuPatch.MicButtonsActive[i]);
                        if (i == 0 && MenuPatch.MicButtonsActive[i])
                        {
                            ClearAllButtonStates();
                            Main = true;
                        }
                    }
                }
            }
            if (Halloween)
            {
                for (int i = 0; i < MenuPatch.Halloweenbuttons.Length; i++)
                {
                    if (i < MenuPatch.HalloweenButtonsActive.Length)
                    {
                        MenuPatch.HalloweenButtonsActive[i] = MainGUI.Instance.ToggleButton(MenuPatch.Halloweenbuttons[i], MenuPatch.HalloweenButtonsActive[i]);
                        if (i == 0 && MenuPatch.HalloweenButtonsActive[i])
                        {
                            ClearAllButtonStates();
                            Main = true;
                        }
                    }
                }
            }
            if (OP)
            {
                for (int i = 0; i < MenuPatch.OPbuttons.Length; i++)
                {
                    if (i < MenuPatch.OPButtonsActive.Length)
                    {
                        MenuPatch.OPButtonsActive[i] = MainGUI.Instance.ToggleButton(MenuPatch.OPbuttons[i], MenuPatch.OPButtonsActive[i]);
                        if (i == 0 && MenuPatch.OPButtonsActive[i])
                        {
                            ClearAllButtonStates();
                            Main = true;
                        }
                    }
                }
            }

            GUILayout.EndScrollView();
        }
        private void ClearAllButtonStates()
        {
            Settings = false; 
            Basic = false; 
            Rope = false; 
            SpamRpc = false; 
            Bug = false; 
            Tag = false; 
            Mic = false; 
            Halloween = false; 
            OP = false;
            Main = false;
        }
        private void DrawPlayerListTab()
        {
            _instance.scrollPosition = GUILayout.BeginScrollView(_instance.scrollPosition);

            if (PhotonNetwork.InRoom)
            {
                int num = 1;

                foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Player " + num.ToString() + ": " + player.NickName);

                    if (GUILayout.Button("i", GUILayout.Width(20), GUILayout.Height(20))) { selectedPlayer = player; playerManagerEnabled = true; }

                    GUILayout.EndHorizontal();

                    if (playerManagerEnabled && selectedPlayer == player)
                    {
                        Instance.StartCoroutine(Instance.AntiBan());
                        if (HasPlayerTouchedLiquid(selectedPlayer)) { if (GUILayout.Button("UnAcid " + selectedPlayer.NickName)) { MenuPatch.Mods.MainStuff.OpMods.AcidKid(selectedPlayer); } }
                        if (!HasPlayerTouchedLiquid(selectedPlayer)) { if (GUILayout.Button("Acid " + selectedPlayer.NickName)) { MenuPatch.Mods.MainStuff.OpMods.AcidKid(selectedPlayer); } }
                        if (GUILayout.Button("Back")) { selectedPlayer = null; playerManagerEnabled = false; }
                    }

                    num++;
                }
            }
            else
            {
                ToggleMain[2] = ToggleButton("Join Random Room", ToggleMain[2]);
            }

            GUILayout.EndScrollView();
        }
        private void DrawSettingsTab()
        {
            _instance.scrollPosition = GUILayout.BeginScrollView(_instance.scrollPosition);

            GUILayout.EndScrollView();
        }
        public void Update()
        {
            GUIToggleCheck();

            // Main
            if (ToggleMain[1])
            {
                PhotonNetwork.Disconnect();
            }
            if (ToggleMain[2])
            {
                PhotonNetwork.JoinRandomRoom();
            }
            if (ToggleMain[3])
            {

            }
        }
        #region Main GUI
        private void OnGUI()
        {
            GUI.skin = GUI.skin ?? new GUISkin();
            UpdateStyles();
            Update();
            if (toggled)
            {
                GUIRect = GUI.Window(69, GUIRect, OnGUI, "Fish GUI | Toggle: " + toggleKey);
            }
        }
        public static void OnGUI(int windowId)
        {
            //Instance.Update();
            GUILayout.BeginArea(new Rect(10, 10, GUIRect.width - 20, GUIRect.height - 20));
            GUILayout.Space(10);

            DrawTabButtons();

            if (selectedTab == 0)
            {
                _instance.DrawMainTab();
            }
            else if (selectedTab == 1)
            {
                _instance.DrawMicModsTab();
            }
            else if (selectedTab == 2)
            {
                _instance.DrawMenuTab();
            }
            else if (selectedTab == 3)
            {
                _instance.DrawPlayerListTab();
            }
            else if (selectedTab == 4)
            {
                _instance.DrawSettingsTab();
            }

            GUILayout.EndArea();
            GUI.DragWindow(new Rect(0, 0, GUIRect.width, 20));
        }
        private void GUIToggleCheck()
        {
            if (UnityInput.Current.GetKey(toggleKey))
            {
                if (Time.time - lastToggleTime >= toggleDelay)
                {
                    toggled = !toggled;
                    lastToggleTime = Time.time;
                }
            }
        }
        private static void DrawTabButtons()
        {
            GUILayout.BeginHorizontal();

            for (int i = 0; i < tabNames.Length; i++)
            {
                if (selectedTab == i)
                {
                    GUIStyle selectedStyle = Instance.CreateButtonStyle(Instance.buttonActive, Instance.buttonHovered, Instance.buttonActive);
                    if (GUILayout.Button(tabNames[i], selectedStyle))
                    {
                        selectedTab = i;
                    }
                }
                else
                {
                    GUIStyle unselectedStyle = Instance.CreateButtonStyle(Instance.button, Instance.buttonHovered, Instance.buttonActive);
                    if (GUILayout.Button(tabNames[i], unselectedStyle))
                    {
                        selectedTab = i;
                    }
                }
            }

            GUILayout.EndHorizontal();
        }
        private bool ToggleButton(string text, bool toggle)
        {
            GUIStyle buttonStyle = CreateButtonStyle(toggle ? buttonActive : button, buttonHovered, buttonActive);

            if (GUILayout.Button(text, buttonStyle))
            {
                return !toggle;
            }

            return toggle;
        }
        public void Projectile(int Hash, Vector3 vel, Vector3 pos, Color color, int trail = -1)
        {
            SlingshotProjectile component = ObjectPools.instance.Instantiate(Hash).GetComponent<SlingshotProjectile>();
            float num = Mathf.Abs(GorillaTagger.Instance.offlineVRRig.slingshot.projectilePrefab.transform.lossyScale.x);
            int num2 = 1;
            if (GorillaGameManager.instance != null)
            {
                ScienceExperimentManager.instance.photonView.RPC("LaunchSlingshotProjectile", RpcTarget.All, new object[]
                {
                    pos,
                    vel,
                    Hash,
                    trail,
                    true,
                    num2,
                    false,
                    color.r,
                    color.g,
                    color.b,
                    1f
                });
            }
            component.Launch(pos, vel, PhotonNetwork.LocalPlayer, false, false, num2, num);
        }
        #region Styles
        private void Awake()
        {
            _instance = this;
            button = CreateTexture(new Color32(64, 64, 64, 255));
            buttonHovered = CreateTexture(new Color32(75, 75, 75, 255));
            buttonActive = CreateTexture(new Color32(100, 100, 100, 255));
            windowBackground = CreateTexture(new Color32(30, 30, 30, 255));
            textArea = CreateTexture(new Color32(64, 64, 64, 255));
            textAreaHovered = CreateTexture(new Color32(75, 75, 75, 255));
            textAreaActive = CreateTexture(new Color32(100, 100, 100, 255));
            box = CreateTexture(new Color32(40, 40, 40, 255));
        }
        private void UpdateStyles()
        {
            GUI.skin.button = CreateButtonStyle(button, buttonHovered, buttonActive);
            GUI.skin.window = CreateWindowStyle(windowBackground);
            GUI.skin.textArea = CreateTextFieldStyle(textArea, textAreaHovered, textAreaActive);
            GUI.skin.textField = CreateTextFieldStyle(textArea, textAreaHovered, textAreaActive);
            GUI.skin.box = CreateBoxStyle(box);
        }
        public Texture2D CreateTexture(Color32 color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
        public GUIStyle CreateButtonStyle(Texture2D normal, Texture2D hover, Texture2D active)
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.normal.background = normal;
            style.hover.background = hover;
            style.active.background = active;
            style.normal.textColor = Color.white;
            style.hover.textColor = Color.white;
            style.active.textColor = Color.white;
            return style;
        }
        public GUIStyle CreateWindowStyle(Texture2D background)
        {
            GUIStyle style = new GUIStyle(GUI.skin.window);
            style.normal.background = background;
            style.onNormal.background = background;
            style.normal.textColor = Color.white;
            style.onNormal.textColor = Color.white;
            return style;
        }
        public GUIStyle CreateTextFieldStyle(Texture2D normal, Texture2D hover, Texture2D active)
        {
            GUIStyle style = new GUIStyle(GUI.skin.textField);
            style.normal.background = normal;
            style.hover.background = hover;
            style.active.background = active;
            style.focused.background = active;
            style.normal.textColor = Color.white;
            style.hover.textColor = Color.white;
            style.active.textColor = Color.white;
            style.focused.textColor = Color.white;
            return style;
        }
        public GUIStyle CreateBoxStyle(Texture2D normal)
        {
            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.normal.background = normal;
            style.hover.background = normal;
            style.active.background = normal;
            style.normal.textColor = Color.white;
            style.hover.textColor = Color.white;
            style.active.textColor = Color.white;
            return style;
        }
        #endregion
        #endregion
        #region Field
        // Textures
        private Texture2D button, buttonHovered, buttonActive;
        private Texture2D windowBackground;
        private Texture2D textArea, textAreaHovered, textAreaActive;
        private GameObject directionalLightClone;
        private Texture2D box;
        private static MainGUI _instance;
        public bool Main = true;
        private Player selectedPlayer;
        public bool Settings,Basic,Rope,SpamRpc,Bug,Tag,Mic,Halloween,OP = false;
        public KeyCode toggleKey = KeyCode.Insert;
        public static GameObject pointer;
        internal static bool playerManagerEnabled = false;
        public string guiSelectedEnemy;
        public static MainGUI Instance => _instance;
        // GUI Variables
        public static Rect GUIRect = new Rect(0, 0, 540, 240);
        private static int selectedTab = 0;
        private static readonly string[] tabNames = { "Main", "Misc", "Menu", "Player List", "Settings" };
        private bool[] TogglePlayerList = new bool[999];
        private bool[] ToggleMain = new bool[999];
        private bool[] ToggleMic = new bool[999];
        private bool toggled = true;
        public float toggleDelay = 0.5f;
        private float lastToggleTime;
        private bool one;
        private bool two;
        private bool three;
        private bool four;
        private bool six;
        private bool eight;
        private bool seven;
        private bool nine;
        private bool five;
        private Vector2 scrollPosition = Vector2.zero;
        #endregion
        #region Mods
        public bool HasPlayerTouchedLiquid(Photon.Realtime.Player player)
        {
            ScienceExperimentManager.PlayerGameState[] playerStates = (ScienceExperimentManager.PlayerGameState[])Traverse.Create(ScienceExperimentManager.instance).Field("inGamePlayerStates").GetValue();

            if (playerStates != null)
            {
                foreach (ScienceExperimentManager.PlayerGameState state in playerStates)
                {
                    if (state.playerId == player.ActorNumber && state.touchedLiquid)
                    {
                        player = null;
                        return true;
                    }
                }
            }

            player = null;
            return false;
        }
        private IEnumerator AntiBan()
        {
            if (!PhotonNetwork.CurrentRoom.CustomProperties["gameMode"].ToString().Contains("MODDED"))
            {
                ExecuteCloudScriptRequest executeCloudScriptRequest = new ExecuteCloudScriptRequest();
                executeCloudScriptRequest.FunctionName = "RoomClosed";
                executeCloudScriptRequest.FunctionParameter = new
                {
                    GameId = PhotonNetwork.CurrentRoom.Name,
                    Region = Regex.Replace(PhotonNetwork.CloudRegion, "[^a-zA-Z0-9]", "").ToUpper(),
                    UserId = PhotonNetwork.MasterClient.UserId,
                    ActorNr = 1,
                    ActorCount = 1,
                    AppVersion = PhotonNetwork.AppVersion
                };
                PlayFabClientAPI.ExecuteCloudScript(executeCloudScriptRequest, delegate (ExecuteCloudScriptResult result)
                {
                    Debug.Log(":)");
                }, null, null, null);
                yield return new WaitForSeconds(0.5f);
                string gamemode = PhotonNetwork.CurrentRoom.CustomProperties["gameMode"].ToString().Replace(GorillaComputer.instance.currentQueue, GorillaComputer.instance.currentQueue + "MODDEDMODDED");
                ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable { { "gameMode", gamemode } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(hash, null, null);
                yield return new WaitForSeconds(0.5f);
                PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                yield break;
            }
            yield break;
        }
        private void Test()
        {
            //CosmeticsController.instance.gotMyDaily = false;
            Debug.Log($"a: {CosmeticsController.instance.lastDailyLogin}");
            Debug.Log($"aa: {CosmeticsController.instance.userDataRecord.Value}");
            CosmeticsController.instance.lastDailyLogin = "2024-02-9";
            CosmeticsController.instance.userDataRecord.Value = "2024-02-9";
            CosmeticsController.instance.secondsToWaitToCheckDaily = 0f;
            CosmeticsController.instance.checkedDaily = false;
            CosmeticsController.instance.gotMyDaily = false;
            CosmeticsController.instance.GetLastDailyLogin();
            CosmeticsController.instance.currentTime.AddDays(-1.0);
        }
        #endregion
    }
    #endregion
    #region UtilsClass
    internal class BtnCollider : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collider)
        {
            if (Time.frameCount >= MenuPatch.framePressCooldown + 10)
            {
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.1f);
                GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tagHapticStrength / 2, GorillaTagger.Instance.tagHapticDuration / 2);
                if (MenuPatch.NumberForPage == 1)
                {
                    MenuPatch.Toggle(relatedText, MenuPatch.buttons, MenuPatch.buttonsActive);
                }
                else if (MenuPatch.NumberForPage == 2)
                {
                    MenuPatch.Toggle(relatedText, MenuPatch.Settingsbuttons, MenuPatch.SettingsButtonsActive);
                }
                else if (MenuPatch.NumberForPage == 3)
                {
                    MenuPatch.Toggle(relatedText, MenuPatch.Basicbuttons, MenuPatch.BasicButtonsActive);
                }
                else if (MenuPatch.NumberForPage == 4)
                {
                    MenuPatch.Toggle(relatedText, MenuPatch.Ropebuttons, MenuPatch.RopeButtonsActive);
                }
                else if (MenuPatch.NumberForPage == 5)
                {
                    MenuPatch.Toggle(relatedText, MenuPatch.SpamRpcbuttons, MenuPatch.SpamRpcButtonsActive);
                }
                else if (MenuPatch.NumberForPage == 6)
                {
                    MenuPatch.Toggle(relatedText, MenuPatch.Bugbuttons, MenuPatch.BugButtonsActive);
                }
                else if (MenuPatch.NumberForPage == 7)
                {
                    MenuPatch.Toggle(relatedText, MenuPatch.Tagbuttons, MenuPatch.TagButtonsActive);
                }
                else if (MenuPatch.NumberForPage == 8)
                {
                    MenuPatch.Toggle(relatedText, MenuPatch.Micbuttons, MenuPatch.MicButtonsActive);
                }
                else if (MenuPatch.NumberForPage == 99)
                {
                    MenuPatch.Toggle(relatedText, MenuPatch.Halloweenbuttons, MenuPatch.HalloweenButtonsActive);
                }
                else if (MenuPatch.NumberForPage == 69)
                {
                    MenuPatch.Toggle(relatedText, MenuPatch.OPbuttons, MenuPatch.OPButtonsActive);
                }
                MenuPatch.framePressCooldown = Time.frameCount;
            }
        }
        public string relatedText;
    }
    internal class ControllerInput : MonoBehaviour
    {
        private static bool CalculateGripState(float grabValue, float grabThreshold)
        {
            return grabValue >= grabThreshold;
        }
        public void Update()
        {
            if (ControllerInputPoller.instance != null)
            {
                ControllerInputPoller instance = ControllerInputPoller.instance;
                RightSecondary = instance.rightControllerPrimaryButton;
                RightPrimary = instance.rightControllerSecondaryButton;
                RightTrigger = CalculateGripState(instance.rightControllerIndexFloat, 0.1f);
                RightGrip = CalculateGripState(instance.rightControllerGripFloat, 0.1f);
                RightJoystick = instance.rightControllerPrimary2DAxis;
                RightStickClick = SteamVR_Actions.gorillaTag_RightJoystickClick.GetState(SteamVR_Input_Sources.RightHand);
                LeftSecondary = instance.leftControllerPrimaryButton;
                LeftPrimary = instance.leftControllerSecondaryButton;
                LeftTrigger = CalculateGripState(instance.leftControllerIndexFloat, 0.1f);
                LeftGrip = CalculateGripState(instance.leftControllerGripFloat, 0.1f);
                LeftJoystick = SteamVR_Actions.gorillaTag_LeftJoystick2DAxis.GetAxis(SteamVR_Input_Sources.LeftHand);
                LeftStickClick = SteamVR_Actions.gorillaTag_LeftJoystickClick.GetState(SteamVR_Input_Sources.LeftHand);
            }
        }
        public static bool RightSecondary;
        public static bool RightPrimary;
        public static bool RightTrigger;
        public static bool RightGrip;
        public static Vector2 RightJoystick;
        public static bool RightStickClick;
        public static bool LeftSecondary;
        public static bool LeftPrimary;
        public static bool LeftGrip;
        public static bool LeftTrigger;
        public static Vector2 LeftJoystick;
        public static bool LeftStickClick;
    }
    public class NotifiLib : MonoBehaviour
    {
        private static NotifiLib _instance;
        public static NotifiLib Instance => _instance;
        private GameObject HUDObj;
        private GameObject HUDObj2;
        private Camera mainCamera;
        private Text testText;
        private static Text notifiText;
        private Material alertText;
        private int notificationDecayTime = 300;
        private int notificationDecayTimeCounter = 0;
        private string[] notifiLines;
        private string newtext;
        public static string PreviousNotification = "";
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            mainCamera = Camera.main;
            alertText = new Material(Shader.Find("GUI/Text Shader"));
            InitializeHUD();
        }
        private void InitializeHUD()
        {
            HUDObj2 = new GameObject("NOTIFICATIONLIB_HUD_OBJ");
            HUDObj = new GameObject("NOTIFICATIONLIB_HUD_OBJ");
            Canvas canvas = HUDObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = mainCamera;

            RectTransform rectTransform = HUDObj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(5, 5);
            rectTransform.position = mainCamera.transform.position;
            HUDObj2.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z - 4.6f);
            HUDObj.transform.parent = HUDObj2.transform;
            rectTransform.localPosition = new Vector3(0f, 0f, 1.6f);
            rectTransform.rotation = Quaternion.Euler(new Vector3(0, -270f, 0));

            GameObject textObject = new GameObject();
            textObject.transform.parent = HUDObj.transform;
            testText = textObject.AddComponent<Text>();
            testText.text = "";
            testText.fontSize = 10;
            testText.font = GameObject.Find("COC Text").GetComponent<Text>().font;
            testText.rectTransform.sizeDelta = new Vector2(260, 70);
            testText.alignment = TextAnchor.LowerLeft;
            testText.rectTransform.localScale = new Vector3(0.01f, 0.01f, 1f);
            testText.rectTransform.localPosition = new Vector3(-1.5f, -.9f, -.6f);
            testText.material = alertText;
            notifiText = testText;
        }
        private void Update()
        {
            if (string.IsNullOrWhiteSpace(testText.text))
            {
                notificationDecayTimeCounter = 0;
            }
            else
            {
                notificationDecayTimeCounter++;
                if (notificationDecayTimeCounter > notificationDecayTime)
                {
                    notifiLines = null;
                    newtext = "";
                    notificationDecayTimeCounter = 0;
                    notifiLines = testText.text.Split(Environment.NewLine.ToCharArray()).Skip(1).ToArray();
                    foreach (string text in notifiLines)
                    {
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            newtext += text + Environment.NewLine;
                        }
                    }
                    testText.text = newtext;
                }
            }

            HUDObj2.transform.position = mainCamera.transform.position;
            HUDObj2.transform.rotation = mainCamera.transform.rotation;
        }
        private static string PreviousNotifications = "";
        private const int MaxPreviousNotifications = 10;

        public static void SendNotification(string notificationText, Color color)
        {
            try
            {
                notificationText = ProcessNotificationText(notificationText);

                if (!PreviousNotifications.Contains(notificationText))
                {
                    ConsoleUtility.WriteToConsole($"[Notification]: {notificationText}", ConsoleColor.Green);
                    notifiText.text += notificationText + Environment.NewLine;
                    notifiText.color = color;
                    PreviousNotifications = notificationText;
                }
            }
            catch (Exception ex)
            {
                ConsoleUtility.WriteToConsole($"[ERROR]: {ex.Message}", ConsoleColor.Red);
            }
        }
        private static string ProcessNotificationText(string notificationText)
        {
            if (notificationText.Contains("Is On"))
            {
                notificationText = "<color=grey>[MENU]</color> " + notificationText;
            }
            else if (notificationText.Contains("reported"))
            {
                notificationText = "<color=grey>[REPORTED]</color> " + notificationText;
            }
            else if (notificationText.Contains("Patch"))
            {
                notificationText = "<color=grey>[PATCH]</color> " + notificationText;
            }
            else if (notificationText.Contains("Joined") || notificationText.Contains("Left") || notificationText.Contains("Room"))
            {
                notificationText = "<color=grey>[ROOM]</color> " + notificationText;
            }
            return notificationText;
        }
    }
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        private static RoomManager _instance;
        public static RoomManager Instance => _instance;
        private void Awake()
        {
            _instance = this;
        }
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            MenuPatch.Instance.Room = PhotonNetwork.CurrentRoom.Name;
            NotifiLib.SendNotification(string.Concat(new string[]
            {
                "You have joined Room: ",
                MenuPatch.Instance.Room,
                " With ",
                PhotonNetwork.CurrentRoom.PlayerCount.ToString(),
                "/10 Players!"
            }), Color.green);
        }
        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            NotifiLib.SendNotification("You have Left Room: " + MenuPatch.Instance.Room, Color.red);
            MenuPatch.Instance.Room = string.Empty;
            MenuPatch.GorillaBattleManager = null;
            MenuPatch.GorillaHuntManager = null;
            MenuPatch.GorillaTagManager = null;
        }
        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            NotifiLib.SendNotification(newPlayer.NickName + " Has Joined Room: " + MenuPatch.Instance.Room, Color.green);
        }
        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            NotifiLib.SendNotification(otherPlayer.NickName + " Has Left Room: " + MenuPatch.Instance.Room, Color.red);
        }
    }
    internal class RigManager : MonoBehaviour
    {
        internal static VRRig GetOfflineRig()
        {
            return GorillaTagger.Instance.offlineVRRig;
        }
        internal static VRRig FindRig(Photon.Realtime.Player player)
        {
            return GorillaGameManager.instance.FindPlayerVRRig(player);
        }
        internal static VRRig GetOwnVRRig()
        {
            return GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player").GetComponent<VRRig>();
        }
        internal static PhotonView GetRigView(VRRig rig)
        {
            return (PhotonView)Traverse.Create(rig).Field("photonView").GetValue();
        }
        internal static Photon.Realtime.Player GetPlayerFromVRRig(VRRig p)
        {
            return GetRigView(p).Owner;
        }
        internal static PhotonView MyView()
        {
            return GorillaTagger.Instance.myVRRig;
        }
        public static Vector3 GetNearbyPosition(Vector3 center, float radius)
        {
            Vector2 vector = UnityEngine.Random.insideUnitCircle * radius;
            Vector3 result = new Vector3(center.x + vector.x, center.y, center.z + vector.y);
            return result;
        }
        public static Photon.Realtime.Player GetPlayerFromID(string id)
        {
            Photon.Realtime.Player found = null;
            foreach (Photon.Realtime.Player target in PhotonNetwork.PlayerList)
            {
                if (target.UserId == id)
                {
                    found = target;
                    break;
                }
            }
            return found;
        }
        public static Player GetRandomPlayer(bool includeSelf)
        {
            Player player;
            if (includeSelf)
            {
                player = PhotonNetwork.PlayerList[UnityEngine.Random.Range(0, PhotonNetwork.PlayerList.Length - 1)];
            }
            else
            {
                player = PhotonNetwork.PlayerListOthers[UnityEngine.Random.Range(0, PhotonNetwork.PlayerListOthers.Length - 1)];
            }
            return player;
        }
    }
    #endregion
}