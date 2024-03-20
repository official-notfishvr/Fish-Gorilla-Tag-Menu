using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using Color = UnityEngine.Color;
using Object = UnityEngine.Object;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.IO;
using System.Linq;
using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using GorillaLocomotion.Gameplay;
using GorillaLocomotion.Swimming;
using System.Net;
using System.Diagnostics;
using GorillaNetworking;
using System.Collections;
using System.Reflection;
using System.Threading.Tasks;
using Text = UnityEngine.UI.Text;
using UnityEngine.XR;
using PlayFab.ClientModels;
using PlayFab;
using System.Text.RegularExpressions;
using GorillaTag;
using Player = Photon.Realtime.Player;
using FishMenu.MainMenuUtils;
using static FishMenu.MainMenuUtils.Patches;
using static FishMenu.MainMenuUtils.Utils;

namespace FishMenu.MainMenu
{
    [HarmonyLib.HarmonyPatch(typeof(GorillaLocomotion.Player), "LateUpdate", MethodType.Normal)]
    public class MenuPatch : MonoBehaviourPunCallbacks
    {
        #region Buttons
        public static string[] buttons = new string[]
        {
            "Settings",                  // 0
            "Basic Mods",                // 1
            "Rope Mods",                 // 2
            "Spam Rpc Mods",             // 3
            "Bug Mods",                  // 4
            "Tag Mods",                  // 5
            "Mic Mods",                  // 6
            "OP Mods",                   // 7
            "Halloween Mods",            // 8
            "Lava Mods",                 // 9
            "Save Presets",              // 10
            "Load Presets",              // 11
        };
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
        public static string[] Ropebuttons = new string[]
        {
            "Back",                      // 0
            "Rope Up",                   // 1
            "Rope Down",                 // 2
            "Ropes To Self",             // 3
            "Rope Spaz"                  // 4
        };
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
            "Rock Spam Gun" ,            // 11
            "Coal Spam",                 // 12
            "Coal Spam Gun"              // 13
        };
        public static string[] Bugbuttons = new string[]
        {
            "Back",                      // 0
            "Invis Bug",                 // 1 // can be edit on Setting Page
            "Bug Gun",                   // 2 // can be edit on Setting Page
            "Big Bug",                   // 3 // can be edit on Setting Page
            "Grab Bug",                  // 4 // can be edit on Setting Page
        };
        public static string[] Tagbuttons = new string[]
        {
            "Back",                      // 0
            "Tag All",                   // 1
            "Tag Gun",                   // 2
            "Mat All",                   // 3
            "Mat Self"                   // 4
        };
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
            "Fuck Game [M]",             // 9
        };
        public static string[] OPbuttons = new string[]
        {
            "Back",                      // 0
            "Anit ban",                  // 1
            "Acid All",                  // 2
            "Acid Gun",                  // 3
            "Acid Mat Spam",             // 4
            "Freeze All",                // 5
            "Name Change All",           // 6
            "Fling All",                 // 7
        };
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
        public static string[] Lavabuttons = new string[]
        {
            "Back",                       // 0
            "Rise Lava",                  // 1
            "Drain Lava",                 // 2
            "Erupt Lava",                 // 3
            "Full Whole Map Lava [CS]",   // 4
            "Fix Lava",                   // 5
            "Spaz Lava",                  // 6
        };
        #endregion
        #region buttonsActive
        public static bool[] buttonsActive = new bool[111];
        public static bool[] SettingsButtonsActive = new bool[111];
        public static bool[] BasicButtonsActive = new bool[111];
        public static bool[] RopeButtonsActive = new bool[111];
        public static bool[] SpamRpcButtonsActive = new bool[111];
        public static bool[] BugButtonsActive = new bool[111];
        public static bool[] TagButtonsActive = new bool[111];
        public static bool[] MicButtonsActive = new bool[111];
        public static bool[] OPButtonsActive = new bool[111];
        public static bool[] HalloweenButtonsActive = new bool[111];
        public static bool[] LavaButtonsActive = new bool[111];
        #endregion
        #region Prefix
        private static void Prefix()
        {
            try
            {
                #region Menu
                PhotonNetwork.LocalPlayer.NickName = "fishmods";
                GorillaComputer.instance.savedName = "fishmods";
                GorillaComputer.instance.currentName = "fishmods";
                UpdateMaterialColors();
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
                #region buttonsActive
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
                // op
                if (buttonsActive[7] == true)
                {
                    NumberForPage = 9;
                    buttonsActive[7] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                // Lucy
                if (buttonsActive[8] == true)
                {
                    NumberForPage = 10;
                    buttonsActive[8] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                // Lava
                if (buttonsActive[9] == true)
                {
                    NumberForPage = 11;
                    buttonsActive[9] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (buttonsActive[10] == true)
                {
                    Presets.SavePresets();
                    buttonsActive[10] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (buttonsActive[11] == true)
                {
                    Presets.LoadPresets();
                    buttonsActive[11] = false;
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
                    //Presets.SavePresets();
                    SettingsButtonsActive[1] = (bool)false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                    SettingsPageOn = true;
                }
                if (SettingsButtonsActive[2] == true)
                {
                    //Presets.LoadPresets();
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
                        Vector3 vector = ((Vector3)GorillaTagger.Instance.offlineVRRig.transform.position).normalized;
                        vector *= 0.1f;
                        Color color2 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                        Projectile("WaterBalloon", vector, GorillaTagger.Instance.offlineVRRig.transform.position + new Vector3(0, 3, 0), color2);
                        WaterBalloonTimer = Time.time;
                    }
                    NotifiLib.SendNotification("Water Balloon Spam Is On", Color.green);
                }
                if (SpamRpcButtonsActive[7] == true)
                {
                    Instance.SpamGun("WaterBalloon", WaterBalloonTimer);
                    NotifiLib.SendNotification("Water Balloon Gun Is On", Color.green);
                }
                if (SpamRpcButtonsActive[8] == true)
                {
                    if (ControllerInput.RightGrip && (double)Time.time > (double)SnowBallTimer + 0.085)
                    {
                        Vector3 vector = ((Vector3)GorillaTagger.Instance.offlineVRRig.transform.position).normalized;
                        vector *= 0.1f;
                        Color color2 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                        Projectile("Snowball", vector, GorillaTagger.Instance.offlineVRRig.transform.position + new Vector3(0, 3, 0), color2);
                        SnowBallTimer = Time.time;
                    }
                    NotifiLib.SendNotification("Snow Ball Spam Is On", Color.green);
                }
                if (SpamRpcButtonsActive[9] == true)
                {
                    Instance.SpamGun("Snowball", SnowBallTimer);
                    NotifiLib.SendNotification("Snow Ball Gun Is On", Color.green);
                }
                if (SpamRpcButtonsActive[10] == true)
                {
                    if (ControllerInput.RightGrip && (double)Time.time > (double)RockSpamTimer + 0.085)
                    {
                        Vector3 vector = ((Vector3)GorillaTagger.Instance.offlineVRRig.transform.position).normalized;
                        vector *= 0.1f;
                        Color color2 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                        Projectile("LavaRock", vector, GorillaTagger.Instance.offlineVRRig.transform.position + new Vector3(0, 3, 0), color2);
                        RockSpamTimer = Time.time;
                    }
                    NotifiLib.SendNotification("Rock Spam Is On", Color.green);
                }
                if (SpamRpcButtonsActive[11] == true)
                {
                    Instance.SpamGun("LavaRock", RockSpamTimer);
                    NotifiLib.SendNotification("Rock Spam Gun Is On", Color.green);
                }
                if (SpamRpcButtonsActive[12] == true)
                {
                    if (ControllerInput.RightGrip && (double)Time.time > (double)CoalSpamTimer + 0.085)
                    {
                        Vector3 vector = ((Vector3)GorillaTagger.Instance.offlineVRRig.transform.position).normalized;
                        vector *= 0.1f;
                        Color color2 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                        Projectile("BucketGiftCoal", vector, GorillaTagger.Instance.offlineVRRig.transform.position + new Vector3(0, 3, 0), color2);
                        CoalSpamTimer = Time.time;
                    }
                    NotifiLib.SendNotification("Rock Spam Is On", Color.green);
                }
                if (SpamRpcButtonsActive[13] == true)
                {
                    Instance.SpamGun("BucketGiftCoal", CoalSpamTimer);
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
                if (SlingshotType == 0f) { Micbuttons[1] = "Slingshot = Water Balloon"; }
                if (SlingshotType == 1f) { Micbuttons[1] = "Slingshot = Snowball"; }
                if (SlingshotType == 2f) { Micbuttons[1] = "Slingshot = Heart"; }
                if (SlingshotType == 3f) { Micbuttons[1] = "Slingshot = Leaf"; }
                if (SlingshotType == 4f) { Micbuttons[1] = "Slingshot = Deadshot"; }
                if (SlingshotType == 5f) { Micbuttons[1] = "Slingshot = Cloud"; }
                if (SlingshotType == 6f) { Micbuttons[1] = "Slingshot = Ice"; }
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
                if (MicButtonsActive[9] == true)
                {
                    Mods.MainStuff.OpMods.FuckGame();
                    NotifiLib.SendNotification("Fuck Game Is On", Color.green);
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
                if (OPButtonsActive[1] == true) { Instance.StartCoroutine(Instance.AntiBan()); }
                if (OPButtonsActive[2] == true)
                {
                    if (Instance.IsModded())
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
                    else { Instance.StartCoroutine(Instance.AntiBan()); }
                }
                if (OPButtonsActive[3] == true)
                {
                    Instance.StartCoroutine(Instance.AntiBan());
                    Mods.MainStuff.OpMods.AcidGun();
                }
                if (OPButtonsActive[4] == true)
                {
                    if (Instance.IsModded())
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
                    else { Instance.StartCoroutine(Instance.AntiBan()); }
                }
                if (OPButtonsActive[5] == true)
                {
                    if (Instance.IsModded())
                    {
                        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { CachingOption = EventCaching.RemoveFromRoomCache, Receivers = ReceiverGroup.MasterClient };
                        PhotonNetwork.NetworkingClient.OpRaiseEvent(0, null, raiseEventOptions, SendOptions.SendReliable);
                        Hashtable hashtable = new Hashtable();
                        hashtable[0] = -1;
                        PhotonNetwork.NetworkingClient.OpRaiseEvent(207, hashtable, null, SendOptions.SendReliable);
                        return;
                    }
                    else { Instance.StartCoroutine(Instance.AntiBan()); }
                }
                if (OPButtonsActive[6] == true)
                {
                    // i no skid
                    // proof 1: https://notfishvr.dev/upload/J36TrfFPRq
                    // proof 2: https://notfishvr.dev/upload/01CiNteENM
                    if (Instance.IsModded())
                    {
                        if (Time.time > pookiebear)
                        {
                            pookiebear = Time.time + 0.2f;
                            foreach (Player player in PhotonNetwork.PlayerListOthers)
                            {
                                player.NickName = PhotonNetwork.LocalPlayer.NickName;
                                Type typeFromHandle = typeof(Player);
                                typeFromHandle.Reflect().Invoke("SetPlayerNameProperty", player);
                                //MethodInfo method = typeFromHandle.GetMethod("SetPlayerNameProperty", BindingFlags.Instance | BindingFlags.NonPublic);
                                //if (method != null) { method.Invoke(player, new object[0]); }
                            }
                        }
                        return;
                    }
                    else { Instance.StartCoroutine(Instance.AntiBan()); }
                }
                if (OPButtonsActive[7] == true)
                {
                    if (Instance.IsModded())
                    {
                        foreach (BarrelCannon barrelCannon in Resources.FindObjectsOfTypeAll<BarrelCannon>())
                        {
                            barrelCannon.photonView.RPC("FireBarrelCannonRPC", RpcTarget.Others, GorillaLocomotion.Player.Instance.transform.position, GorillaLocomotion.Player.Instance.transform.position);
                        }
                        return;
                    }
                    else { Instance.StartCoroutine(Instance.AntiBan()); }
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
                //else { hasBeenGrabbed = false; }
                if (HalloweenButtonsActive[7] == true)
                {
                    Mods.MainStuff.BasicMods.HalloweenMods.SlowBroomStick();
                    NotifiLib.SendNotification("Slow Broom Stick Is On", Color.green);
                }
                //else { hasBeenGrabbed2 = false; }
                #endregion
                #region Lava buttonsActive
                if (LavaButtonsActive[0] == true)
                {
                    NumberForPage = 1;
                    LavaButtonsActive[0] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (LavaButtonsActive[1] == true) { if (Instance.IsModded()) { Mods.MainStuff.OpMods.SetLavaState(InfectionLavaController.RisingLavaState.Full); } else { Instance.StartCoroutine(Instance.AntiBan()); } }
                if (LavaButtonsActive[2] == true) { if (Instance.IsModded()) { Mods.MainStuff.OpMods.SetLavaState(InfectionLavaController.RisingLavaState.Drained); } else { Instance.StartCoroutine(Instance.AntiBan()); } }
                if (LavaButtonsActive[3] == true) { if (Instance.IsModded()) { Mods.MainStuff.OpMods.SetLavaState(InfectionLavaController.RisingLavaState.Erupting); } else { Instance.StartCoroutine(Instance.AntiBan()); } }
                if (LavaButtonsActive[4] == true) { if (Instance.IsModded()) { Mods.MainStuff.OpMods.SetLavaState(InfectionLavaController.RisingLavaState.Full, true); } else { Instance.StartCoroutine(Instance.AntiBan()); } }
                if (LavaButtonsActive[5] == true) { if (Instance.IsModded()) { Mods.MainStuff.OpMods.SetLavaState(InfectionLavaController.RisingLavaState.Drained, false); } else { Instance.StartCoroutine(Instance.AntiBan()); } }
                if (LavaButtonsActive[6] == true)
                {
                    if (Instance.IsModded())
                    {
                        InfectionLavaController instance = InfectionLavaController.Instance;

                        Type type = typeof(InfectionLavaController);
                        FieldInfo reliableStateField = type.GetField("reliableState", BindingFlags.Instance | BindingFlags.NonPublic);
                        FieldInfo lavaMeshMaxScaleField = type.GetField("lavaMeshMaxScale", BindingFlags.Instance | BindingFlags.NonPublic);
                        FieldInfo lavaMeshMinScaleField = type.GetField("lavaMeshMinScale", BindingFlags.Instance | BindingFlags.NonPublic);

                        object reliableState = reliableStateField.GetValue(instance);

                        Type reliableStateType = reliableState.GetType();
                        FieldInfo stateField = reliableStateType.GetField("state");
                        FieldInfo stateStartTimeField = reliableStateType.GetField("stateStartTime");

                        if (spazLava) { stateField.SetValue(reliableState, InfectionLavaController.RisingLavaState.Full); }
                        else { stateField.SetValue(reliableState, InfectionLavaController.RisingLavaState.Drained); }
                        spazLava = !spazLava;
                        stateStartTimeField.SetValue(reliableState, PhotonNetwork.Time + (double)Random.Range(0f, 20f));
                        reliableStateField.SetValue(instance, reliableState);
                    }
                    else { Instance.StartCoroutine(Instance.AntiBan()); }
                }
                #endregion
                #endregion
            }
            catch (Exception ex) { Mods.Utils.LogError(ex); }
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
                                RopeSwingManager.instance.SendSetVelocity_RPC(gorillaRopeSwing.ropeId, 1, Pos, true);
                            }
                            RopeTimer = Time.time;
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
                        if (Instance.IsModded())
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
                        if (Instance.IsModded())
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
                        else { Instance.StartCoroutine(Instance.AntiBan()); }
                    }
                    public static void SetLavaState(InfectionLavaController.RisingLavaState state, bool a = false)
                    {
                        InfectionLavaController instance = InfectionLavaController.Instance;

                        Type type = typeof(InfectionLavaController);
                        FieldInfo reliableStateField = type.GetField("reliableState", BindingFlags.Instance | BindingFlags.NonPublic);
                        FieldInfo lavaMeshMaxScaleField = type.GetField("lavaMeshMaxScale", BindingFlags.Instance | BindingFlags.NonPublic);
                        FieldInfo lavaMeshMinScaleField = type.GetField("lavaMeshMinScale", BindingFlags.Instance | BindingFlags.NonPublic);

                        object reliableState = reliableStateField.GetValue(instance);

                        Type reliableStateType = reliableState.GetType();
                        FieldInfo stateField = reliableStateType.GetField("state");
                        FieldInfo stateStartTimeField = reliableStateType.GetField("stateStartTime");

                        stateField.SetValue(reliableState, state);
                        stateStartTimeField.SetValue(reliableState, PhotonNetwork.Time);
                        reliableStateField.SetValue(instance, reliableState);

                        if (a)
                        {
                            lavaMeshMaxScaleField.SetValue(instance, 26.941086f);
                            lavaMeshMinScaleField.SetValue(instance, 25.941086f);
                        }
                        else
                        {
                            lavaMeshMaxScaleField.SetValue(instance, 8.941086f);
                            lavaMeshMinScaleField.SetValue(instance, 3.17f);
                        }
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
                                //if (!hasBeenGrabbed)
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
                                //if (!hasBeenGrabbed2)
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
        #region ModsNotInClass
        public void Projectileold(int Hash, Vector3 vel, Vector3 pos, Color color, int trail = -1)
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
        public static void Projectile(string projectileName, Vector3 velocity, Vector3 position, Color color, bool noDelay = false)
        {
            ControllerInputPoller.instance.leftControllerGripFloat = 1f;

            GameObject projectileObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Object.Destroy(projectileObject, 0.1f);
            projectileObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            projectileObject.transform.position = GorillaTagger.Instance.leftHandTransform.position;
            projectileObject.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;

            int[] overrideIndices = new int[] { 32, 204, 231, 240, 249 };
            int index = Array.IndexOf<string>(fullProjectileNames, projectileName);
            projectileObject.AddComponent<GorillaSurfaceOverride>().overrideIndex = overrideIndices[index];
            projectileObject.GetComponent<Renderer>().enabled = false;

            if (Time.time > projDebounce)
            {
                try
                {
                    string[] anchorPrefixes = new string[] { "LMACE.", "LMAEX.", "LMAGD.", "LMAHQ.", "LMAIE." };
                    string anchorName = fullProjectileNames[index] + "LeftAnchor";
                    Transform anchor = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L/palm.01.L/TransferrableItemLeftHand/" + anchorName)
                                        .transform.Find(anchorPrefixes[index]);
                    SnowballThrowable snowball = anchor.GetComponent<SnowballThrowable>();


                    Vector3 initialPosition = snowball.transform.position;
                    Vector3 initialVelocity = GorillaTagger.Instance.GetComponent<Rigidbody>().velocity;

                    snowball.randomizeColor = true;
                    snowball.transform.position = position;
                    GorillaTagger.Instance.GetComponent<Rigidbody>().velocity = velocity;
                    GorillaTagger.Instance.offlineVRRig.SetThrowableProjectileColor(true, color);
                    GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/EquipmentInteractor").GetComponent<EquipmentInteractor>().ReleaseLeftHand();

                    GorillaTagger.Instance.GetComponent<Rigidbody>().velocity = initialVelocity;
                    snowball.transform.position = initialPosition;
                    snowball.randomizeColor = false;
                }
                catch (Exception ex) { Debug.LogError("Error launching projectile: " + ex.Message); }
                if (projDebounceType > 0f && !noDelay) { projDebounce = Time.time + projDebounceType; }
            }
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
        public void SpamGun(string Hash, float Timer)
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
                    Projectile(Hash, vector, GorillaTagger.Instance.offlineVRRig.transform.position, color);
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
                    UserId = PhotonNetwork.PlayerList[Random.Range(0, PhotonNetwork.PlayerList.Length + 1)].UserId,
                    ActorNr = PhotonNetwork.PlayerList[Random.Range(0, PhotonNetwork.PlayerList.Length + 1)],
                    ActorCount = PhotonNetwork.ViewCount,
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
        public bool IsModded()
        {
            object obj;
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gameMode", out obj);
            if (obj.ToString().Contains("MODDED"))
            {
                return true;
            }
            return false;
        }
        #endregion
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
            if (NumberForPage == 9)
            {
                AddPageButtons(OPbuttons);
                string[] array2 = OPbuttons.Skip(pageNumber * pageSize).Take(pageSize).ToArray();
                for (int i = 0; i < array2.Length; i++)
                {
                    AddButton((float)i * 0.13f + 0.26f, array2[i], OPbuttons, OPButtonsActive);
                }
            }
            if (NumberForPage == 10)
            {
                AddPageButtons(Halloweenbuttons);
                string[] array2 = Halloweenbuttons.Skip(pageNumber * pageSize).Take(pageSize).ToArray();
                for (int i = 0; i < array2.Length; i++)
                {
                    AddButton((float)i * 0.13f + 0.26f, array2[i], Halloweenbuttons, HalloweenButtonsActive);
                }
            }
            if (NumberForPage == 11)
            {
                AddPageButtons(Lavabuttons);
                string[] array2 = Lavabuttons.Skip(pageNumber * pageSize).Take(pageSize).ToArray();
                for (int i = 0; i < array2.Length; i++)
                {
                    AddButton((float)i * 0.13f + 0.26f, array2[i], Lavabuttons, LavaButtonsActive);
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
        #region Fields

        // General fields
        public static bool consoleStartAttempt, consoleStartAttempt2, SaveSetting, LoadSetting, onceRightGrip, onceLeftGrip, rightsecondarybutton, teleportGunAntiRepeat, flying, noesp, SettingsPageOn, spazLava = false;
        public static string _playFabPlayerIdCache, _sessionTicket, userToken;
        public static bool once, MenuLoaded = true;
        public string Room;
        public static bool once_left, once_right, once_left_false, once_right_false, once_networking, LeftToggle, RightToggle, ghostToggled;

        // Player-related fields
        public static VRRig kickp, lucyp, lagrig, chosenplayer, Tagger;

        // Object references
        public static GorillaScoreBoard[] boards;
        public static GameObject LeftPlat, lPlat, MainMenuRef, MainTextCanvas, menuObject, RightPlat, rPlat;
        public static GameObject[] RightPlat_Networked, LeftPlat_Networked, jump_left_network, jump_right_network = new GameObject[9999];
        public static GameObject jump_left_local, C4, menu, canvasObj, fingerButtonPresser, reference, pointer, jump_right_local = null;

        // Managers and components
        public static GorillaTagManager GorillaTagManager;
        public static GorillaHuntManager GorillaHuntManager;
        public static HotPepperFace HotPepperFace;
        public static GorillaBattleManager GorillaBattleManager;

        // Lists and arrays
        public static List<Player> lastghostChaser;
        public static int[] bones = { 4, 3, 5, 4, 19, 18, 20, 19, 3, 18, 21, 20, 22, 21, 25, 21, 29, 21, 31, 29, 27, 25, 24, 22, 6, 5, 7, 6, 10, 6, 14, 6, 16, 14, 12, 10, 9, 7 };

        // Values and parameters
        public static string[] fullProjectileNames = new string[] { "Snowball", "WaterBalloon", "LavaRock", "ThrowableGift", "ScienceCandy", "BucketGiftCoal" };
        public static float orbitSpeed, KickG, c1, RockSpamTimer, CoalSpamTimer, WaterBalloonTimer, SnowBallTimer, SplashTime, RopeTimer, angle, TagAura;
        public static int ESpInt, platCountColor, BugCountType, SlingshotCountType, platCountType, TPSpeedCount, SpeedCount, BoneESpInt, framePressCooldown, pageNumber, btnCooldown = 0;
        public static float SlingshotType, BugType, smth, smth2, plattype, projDebounce = 0f;

        public static float projDebounceType = 0.1f;
        public static float timeToSpendLookingForFriend = 15f;
        public static float aimAssistDistance = 5.0f;
        public static float FlySpeed = 10f;
        public static float TPGunSpeed = 32f;
        public static int NumberForPage = 1;
        public static int pageSize = 8;
        public static float pookiebear = -1f;

        // Other
        public static Texture2D menutexture = new Texture2D(2, 3);
        public static Material PlatColor = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Material MenuColor = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Material Changer = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Material BtnDisabledColor = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Material BtnEnabledColor = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Material Next = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Material Previous = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Material PointerColor = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Color BoneESPColor = Color.white;
        public static Color ESPColor = Color.white;
        public static Color Platcolor = Color.clear;
        public static Vector3 scale = new Vector3(0.0125f, 0.28f, 0.3825f);
        public static Vector3? leftHandOffsetInitial = null;
        public static Vector3? rightHandOffsetInitial = null;
        public static GradientColorKey[] colorKeysPlatformMonke = new GradientColorKey[4];
        public static System.Random random = new System.Random();
        private static MenuPatch _instance;

        // Properties
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
    }
}
