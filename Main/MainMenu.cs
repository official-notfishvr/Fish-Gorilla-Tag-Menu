﻿using System;
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
using FishMenu.MainUtils;
using static FishMenu.MainUtils.Patches;
using static FishMenu.MainUtils.Utils;
using static UnityEngine.UI.GridLayoutGroup;
using GorillaGameModes;
using System.Runtime.InteropServices;
using static Mono.Security.X509.X520;
using System.ComponentModel;
using Component = UnityEngine.Component;
using GorillaTag.Cosmetics;
using UnityEngine.SocialPlatforms;
using Local = FishMenu.MainUtils.Local;
using Steamworks;
using TMPro;

namespace FishMenu.Main
{
    [HarmonyLib.HarmonyPatch(typeof(GorillaLocomotion.Player), "LateUpdate", MethodType.Normal)]
    public class MainMenu : MonoBehaviourPunCallbacks
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
            "Coal Spam Gun",             // 13
            "Honeycomb Spam",            // 14
            "Honeycomb Spam Gun",        // 15
        };
        public static string[] Bugbuttons = new string[]
        {
            "Back",                      // 0
            "Invis Bug",                 // 1
            "Bug Gun",                   // 2
            "Big Bug",                   // 3
            "Grab Bug",                  // 4
            "Seizure Bug",               // 5

            "Invis Balloon",             // 6
            "Balloon Gun",               // 7
            "Big Balloon",               // 8
            "Grab Balloon",              // 9
            "Seizure Balloon",           // 10

            "Invis BeachBall",           // 11
            "BeachBall Gun",             // 12
            "Big BeachBall",             // 13
            "Grab BeachBall",            // 14
            "Seizure BeachBall",         // 15

            "Invis Monsters",            // 16
            "Monsters Gun",              // 17
            "Big Monsters",              // 18
            "Grab Monsters",             // 19
            "Seizure Monsters",          // 20

            "Invis Bat",                 // 21
            "Bat Gun",                   // 22
            "Big Bat",                   // 23
            "Grab Bat",                  // 24
            "Seizure Bat",               // 25

        };
        public static string[] Tagbuttons = new string[]
        {
            "Back",                      // 0
            "Tag All",                   // 1
            "Tag Gun",                   // 2
            "Mat All",                   // 3
            "Mat Self",                  // 4
            "Auto Brawl"                 // 5
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
            "Freeze/Crash All",          // 5
            "Name Change All",           // 6
            "Fling All",                 // 7
            "Float All Player",          // 8
            "Float Player Gun",          // 9
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
                UpdateMaterialColors();
                if (!consoleStartAttempt)
                {
                    try
                    {
                        if (!Directory.Exists("FishMods-Config"))
                        {
                            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://pastebin.com/raw/ZcGAVd1C");
                            httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                            using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                            {
                                using (Stream responseStream = httpWebResponse.GetResponseStream())
                                {
                                    using (StreamReader streamReader = new StreamReader(responseStream))
                                    {
                                        if (!Directory.Exists("FishMods-Config")) { Directory.CreateDirectory("FishMods-Config"); }
                                        Mods.Utils.CreateConfigFileIfNotExists("FishModsInv.txt", streamReader.ReadToEnd());
                                    }
                                }
                            }
                        }
                        MainMenuRef = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        MainMenuRef.transform.parent = GorillaLocomotion.Player.Instance.rightControllerTransform;
                        FishMenu.MainUtils.Local.GetLocalPlayer();
                    }
                    catch (Exception ex) { Mods.Utils.LogError(ex); }
                    ConsoleUtility.OpenConsoleWindow();
                    ConsoleUtility.WriteToConsole(" ", ConsoleColor.White);
                    consoleStartAttempt = true;
                    Debug.Log("Attempted to start the console, awaiting for further notice.");
                }
                if (!consoleStartAttempt2)
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
                        consoleStartAttempt2 = true;
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
                    Destroy(menu);
                    menu = null;
                    Destroy(reference);
                    reference = null;
                    Destroy(fingerButtonPresser);
                    fingerButtonPresser = null;
                    /*
                    Rigidbody menuRigidbody = menu.GetComponent<Rigidbody>();
                    if (menuRigidbody == null) { menuRigidbody = menu.AddComponent<Rigidbody>(); }
                    menuRigidbody.isKinematic = false;
                    menuRigidbody.useGravity = true;
                    Vector3 additionalVelocity = -GorillaLocomotion.Player.Instance.leftControllerTransform.up * Time.deltaTime * 65f;
                    Vector3 averageVelocity = GorillaLocomotion.Player.Instance.leftHandCenterVelocityTracker.GetAverageVelocity(true, 0f, false) + additionalVelocity;
                    menuRigidbody.velocity = averageVelocity;
                    Vector3 angularVelocity = new Vector3(0f, 0f, 10f);
                    menuRigidbody.angularVelocity = angularVelocity;
                    Collider menuCollider = menu.GetComponent<Collider>();
                    Bounds menuBounds = menuCollider.bounds;
                    Vector3 extents = menuBounds.extents;
                    Vector3 center = menuBounds.center;
                    Collider[] colliders = Physics.OverlapBox(center, extents, Quaternion.identity);
                    foreach (Collider collider in colliders)
                    {
                        if (collider != menuCollider)
                        {
                            Debug.Log("test");
                            Destroy(menu);
                            menu = null;
                            menuRigidbody.isKinematic = true;
                            menuRigidbody.velocity = Vector3.zero;
                            menuRigidbody.angularVelocity = Vector3.zero;
                            Destroy(menu);
                            menu = null;
                            break;
                        }
                    }
                    */
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
                        Vector3 vector = ((Vector3)Local.GetLocalPlayer().offlineVRRig.transform.position).normalized;
                        vector *= 0.1f;
                        Color color2 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                        Projectile("WaterBalloon", vector, Local.GetLocalPlayer().offlineVRRig.transform.position + new Vector3(0, 3, 0), color2);
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
                        Vector3 vector = ((Vector3)Local.GetLocalPlayer().offlineVRRig.transform.position).normalized;
                        vector *= 0.1f;
                        Color color2 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                        Projectile("Snowball", vector, Local.GetLocalPlayer().offlineVRRig.transform.position + new Vector3(0, 3, 0), color2);
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
                        Vector3 vector = ((Vector3)Local.GetLocalPlayer().offlineVRRig.transform.position).normalized;
                        vector *= 0.1f;
                        Color color2 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                        Projectile("LavaRock", vector, Local.GetLocalPlayer().offlineVRRig.transform.position + new Vector3(0, 3, 0), color2);
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
                        Vector3 vector = ((Vector3)Local.GetLocalPlayer().offlineVRRig.transform.position).normalized;
                        vector *= 0.1f;
                        Color color2 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                        Projectile("BucketGiftCoal", vector, Local.GetLocalPlayer().offlineVRRig.transform.position + new Vector3(0, 3, 0), color2);
                        CoalSpamTimer = Time.time;
                    }
                    NotifiLib.SendNotification("Rock Spam Is On", Color.green);
                }
                if (SpamRpcButtonsActive[13] == true)
                {
                    Instance.SpamGun("BucketGiftCoal", CoalSpamTimer);
                    NotifiLib.SendNotification("Rock Spam Gun Is On", Color.green);
                }
                if (SpamRpcButtonsActive[14] == true)
                {
                    //if (ControllerInput.RightGrip && (double)Time.time > (double)CoalSpamTimer + 0.085)
                    {
                        foreach (Player player in PhotonNetwork.PlayerListOthers)
                        {
                            GorillaGameManager.instance.FindVRRigForPlayer(player).RPC("EnableNonCosmeticHandItemRPC", RpcTarget.All, true, false);
                            EdibleWearable component = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/EdibleHoney_right/HoneyComb").GetComponent<EdibleWearable>();
                            component.biteCooldown = 0.0f;
                            component.biteDistance = 2f;
                            CoalSpamTimer = Time.time;
                        }
                    }
                    NotifiLib.SendNotification("SpiderBow Is On", Color.green);
                }
                if (SpamRpcButtonsActive[15] == true)
                {
                    Instance.SpamGun("SpiderBow", CoalSpamTimer);
                    NotifiLib.SendNotification("SpiderBow Gun Is On", Color.green);
                }
                #endregion
                #region Bug buttonsActive
                if (BugButtonsActive[0] == true)
                {
                    NumberForPage = 1;
                    BugButtonsActive[0] = false;
                    Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (BugButtonsActive[1] == true)
                {
                    Mods.Utils.HideObjects<ThrowableBug>(new Vector3(0f, 9999f, 0f));
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[2] == true)
                {
                    Gun(false, pointer => Mods.Utils.AlignObjects<ThrowableBug>(pointer));
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[3] == true)
                {
                    Vector3 scale = PhotonNetwork.InRoom ? new Vector3(5f, 5f, 5f) : new Vector3(1f, 1f, 1f);
                    Mods.Utils.ResizeObjects<ThrowableBug>(scale);
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[4] == true)
                {
                    Mods.Utils.MoveObjects<ThrowableBug>(GorillaTagger.Instance.rightHandTransform);
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[5] == true)
                {
                    Mods.Utils.SeizureObjects<ThrowableBug>();
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[6] == true)
                {
                    Mods.Utils.HideObjects<BalloonHoldable>(new Vector3(0f, 9999f, 0f));
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[7] == true)
                {
                    Gun(false, pointer => Mods.Utils.AlignObjects<BalloonHoldable>(pointer));
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[8] == true)
                {
                    Vector3 scale = PhotonNetwork.InRoom ? new Vector3(5f, 5f, 5f) : new Vector3(1f, 1f, 1f);
                    Mods.Utils.ResizeObjects<BalloonHoldable>(scale);
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[9] == true)
                {
                    Mods.Utils.MoveObjects<BalloonHoldable>(GorillaTagger.Instance.rightHandTransform);
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[10] == true)
                {
                    Mods.Utils.SeizureObjects<BalloonHoldable>();
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[11] == true)
                {
                    Mods.Utils.HideObjects<TransferrableBall>(new Vector3(0f, 9999f, 0f));
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[12] == true)
                {
                    Gun(false, pointer => Mods.Utils.AlignObjects<TransferrableBall>(pointer));
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[13] == true)
                {
                    Vector3 scale = PhotonNetwork.InRoom ? new Vector3(5f, 5f, 5f) : new Vector3(1f, 1f, 1f);
                    Mods.Utils.ResizeObjects<TransferrableBall>(scale);
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[14] == true)
                {
                    Mods.Utils.MoveObjects<TransferrableBall>(GorillaTagger.Instance.rightHandTransform);
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[15] == true)
                {
                    Mods.Utils.SeizureObjects<TransferrableBall>();
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[16] == true)
                {
                    Mods.Utils.HideObjects<MonkeyeAI>(new Vector3(0f, 9999f, 0f));
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[17] == true)
                {
                    Gun(false, pointer => Mods.Utils.AlignObjects<MonkeyeAI>(pointer));
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[18] == true)
                {
                    Vector3 scale = PhotonNetwork.InRoom ? new Vector3(5f, 5f, 5f) : new Vector3(1f, 1f, 1f);
                    Mods.Utils.ResizeObjects<MonkeyeAI>(scale);
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[19] == true)
                {
                    Mods.Utils.MoveObjects<MonkeyeAI>(GorillaTagger.Instance.rightHandTransform);
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[20] == true)
                {
                    Mods.Utils.SeizureObjects<MonkeyeAI>();
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[21] == true)
                {
                    GameObject.Find("Cave Bat Holdable").transform.position = GorillaTagger.Instance.headCollider.transform.position + new Vector3(0f, 9999f, 0f);
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[22] == true)
                {
                    Gun(false, pointer => GameObject.Find("Cave Bat Holdable").transform.position = pointer.transform.position);
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[23] == true)
                {
                    Vector3 scale = PhotonNetwork.InRoom ? new Vector3(5f, 5f, 5f) : new Vector3(1f, 1f, 1f);
                    GameObject.Find("Cave Bat Holdable").transform.localScale = scale;
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[24] == true)
                {
                    GameObject.Find("Cave Bat Holdable").transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
                if (BugButtonsActive[25] == true)
                {
                    NotifiLib.SendNotification($"Thing Is On", Color.green);
                }
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
                if (TagButtonsActive[1] == true) { Instance.TagAll(); }
                else if (TagButtonsActive[1] == false) { Local.GetLocalPlayer().offlineVRRig.enabled = true; GorillaTagger.Instance.offlineVRRig.enabled = true; }
                if (TagButtonsActive[2] == true)
                {
                    Instance.TagGun();
                    NotifiLib.SendNotification("Tag Gun Is On", Color.green);
                }
                if (TagButtonsActive[3] == false)
                {
                    Local.GetLocalPlayer().offlineVRRig.enabled = true;
                    TagButtonsActive[3] = false;
                }
                if (TagButtonsActive[4] == false)
                {
                    Local.GetLocalPlayer().offlineVRRig.enabled = true;
                    TagButtonsActive[4] = false;
                }
                if (TagButtonsActive[5] == false)
                {

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
                                Type type = typeof(Player);
                                FieldInfo reliableStateField = type.GetField("nickName", BindingFlags.Instance | BindingFlags.NonPublic);
                                FieldInfo reliableStateField2 = type.GetField("ActorNumber", BindingFlags.Instance | BindingFlags.NonPublic);
                                FieldInfo reliableStateField3 = type.GetField("RoomReference", BindingFlags.Instance | BindingFlags.NonPublic);
                                player.NickName = PhotonNetwork.LocalPlayer.NickName;
                                type.Reflect().Invoke("SetPlayerNameProperty", player);
                                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                                hashtable[byte.MaxValue] = "NIGGER";
                                Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
                                dictionary.Add(251, hashtable);
                                dictionary.Add(254, player.ActorNumber);
                                dictionary.Add(250, true);
                                PhotonNetwork.CurrentRoom.LoadBalancingClient.LoadBalancingPeer.SendOperation(252, dictionary, SendOptions.SendUnreliable);
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
                        foreach (Player player in PhotonNetwork.PlayerListOthers)
                        {
                            Mods.MainStuff.OpMods.RigContainer(player);
                        }
                    }
                    else { Instance.StartCoroutine(Instance.AntiBan()); }
                }
                if (OPButtonsActive[8] == true)
                {
                    if (Instance.IsModded())
                    {
                        foreach (Player p in PhotonNetwork.PlayerListOthers)
                        {
                            VRRig rig = GorillaGameManager.instance.FindPlayerVRRig(p);
                            AngryBeeSwarm.instance.Emerge(rig.rightHandTransform.position, rig.transform.position);
                            AngryBeeSwarm.instance.targetPlayer = p;
                            AngryBeeSwarm.instance.grabbedPlayer = p;
                        }
                    }
                    else { Instance.StartCoroutine(Instance.AntiBan()); }
                }
                if (OPButtonsActive[9] == true)
                {
                    if (Instance.IsModded())
                    {
                        foreach (Player p in PhotonNetwork.PlayerListOthers)
                        {
                            Gun(true, pointer =>
                            {
                                if (p.NickName == GunPlayer.playerName)
                                {
                                    AngryBeeSwarm.instance.Emerge(GunPlayer.rightHandTransform.position, GunPlayer.transform.position);
                                    AngryBeeSwarm.instance.targetPlayer = p;
                                    AngryBeeSwarm.instance.grabbedPlayer = p;
                                }
                            });
                        }
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
                            //PhotonView.Get(Local.GetLocalPlayer().offlineVRRig).RPC("PlaySplashEffect", RpcTarget.All, position, rotation, duration, size, idk, idk2);
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

                                Vector3 startPosition = Local.GetLocalPlayer().offlineVRRig.transform.position;
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
                            Gun(true, pointer =>
                            {
                                if (ControllerInput.RightTrigger)
                                {
                                    if (Local.GetLocalPlayer().offlineVRRig.enabled)
                                    {
                                        VRRigOnDisable.Prefix(GorillaTagger.Instance.offlineVRRig);
                                    }
                                    Local.GetLocalPlayer().offlineVRRig.enabled = false;
                                    Local.GetLocalPlayer().offlineVRRig.transform.position = pointer.transform.position;
                                    if (Time.time > SplashTime + 0.5f)
                                    {
                                        GorillaTagger.Instance.myVRRig.RPC("PlaySplashEffect", RpcTarget.All, pointer.transform.position, Random.rotation, 4f, 100f, false, true);
                                        SplashTime = Time.time;
                                    }
                                    else
                                    {
                                        Local.GetLocalPlayer().offlineVRRig.enabled = true;
                                    }
                                }
                            });
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
                        Gun(true, pointer =>
                        {
                            PhotonView Photonview = RigManager.GetRigView(pointer.GetComponentInParent<VRRig>());
                            if (Photonview != null || Photonview.Owner != null)
                            {
                                Hashtable hash = new Hashtable();
                                hash.Add("Kick", Photonview.Owner.NickName);
                                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                                GorillaTagger.Instance.myVRRig.Controller.SetCustomProperties(hash);
                                RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                                raiseEventOptions.Receivers = ReceiverGroup.All;
                                PhotonNetwork.RaiseEvent(100, null, raiseEventOptions, SendOptions.SendReliable);
                            }
                        });
                    }
                    public static void LagShibaUsersGun()
                    {
                        Gun(true, pointer =>
                        {
                            PhotonView Photonview = RigManager.GetRigView(pointer.GetComponentInParent<VRRig>());
                            if (Photonview != null || Photonview.Owner != null)
                            {
                                Hashtable hash = new Hashtable();
                                hash.Add("Lag", Photonview.Owner.NickName);
                                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                                GorillaTagger.Instance.myVRRig.Controller.SetCustomProperties(hash);
                                RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                                raiseEventOptions.Receivers = ReceiverGroup.All;
                                PhotonNetwork.RaiseEvent(101, null, raiseEventOptions, SendOptions.SendReliable);
                            }
                        });
                    }
                    public static void MoveShibaUsersGun()
                    {
                        Gun(true, pointer =>
                        {
                            PhotonView Photonview = RigManager.GetRigView(pointer.GetComponentInParent<VRRig>());
                            if (Photonview != null || Photonview.Owner != null)
                            {
                                Hashtable hash = new Hashtable();
                                hash.Add("Move", Photonview.Owner.NickName);
                                hash.Add("MovePos", pointer.transform.position);
                                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                                GorillaTagger.Instance.myVRRig.Controller.SetCustomProperties(hash);
                                RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                                raiseEventOptions.Receivers = ReceiverGroup.All;
                                PhotonNetwork.RaiseEvent(102, null, raiseEventOptions, SendOptions.SendReliable);
                            }
                        });
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
                        Gun(true, pointer =>
                        {
                            PhotonView Photonview = RigManager.GetRigView(pointer.GetComponentInParent<VRRig>());
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
                        });
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
                            type.Reflect().Invoke("UpdateLava", 26.941086f);
                        }
                        else
                        {
                            lavaMeshMaxScaleField.SetValue(instance, 8.941086f);
                            lavaMeshMinScaleField.SetValue(instance, 3.17f);
                            type.Reflect().Invoke("UpdateLava", 8.941086f);
                        }
                    }
                    public static void RigContainer(Player player)
                    {
                        GorillaGameManager instance = GorillaGameManager.instance;
                        Type type = typeof(GorillaGameManager);
                        FieldInfo outContainerField = type.GetField("outContainer", BindingFlags.Instance | BindingFlags.NonPublic);
                        object outContainer = outContainerField.GetValue(instance);
                        Type outContainerType = outContainer.GetType();
                        outContainerType.Reflect().Invoke("ReceiveAutomuteSettings", player, "none");
                        //outContainerType.Reflect().Invoke("RefreshAllRigVoices");
                    }
                }
                public class BasicMods
                {
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
                    public static void BeeCrasher()
                    {
                        int power = 9999;
                        AngryBeeAnimator beeAnimatorInstance = new AngryBeeAnimator();
                        Type beesType = beeAnimatorInstance.GetType();
                        FieldInfo womp = beesType.GetField("numBees", BindingFlags.NonPublic | BindingFlags.Instance);
                        womp.SetValue(beeAnimatorInstance, power);
                    }
                    public static void FreezeAll()
                    {
                        foreach (Photon.Realtime.Player owner in PhotonNetwork.PlayerListOthers)
                        {
                            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                            hashtable[0] = owner.ActorNumber;
                            PhotonNetwork.NetworkingClient.OpRaiseEvent(207, hashtable, null, SendOptions.SendUnreliable);
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
                        else { ghostToggled = false; }
                    }
                    public static void GhostMonkey()
                    {
                        if (ControllerInput.RightTrigger)
                        {
                            if (!ghostToggled && GorillaTagger.Instance.offlineVRRig.enabled)
                            {
                                if (Local.GetLocalPlayer().player.enabled)
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
                        Gun(false, pointer =>
                        {
                            pointer.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                            GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().isKinematic = true;
                            float maxDistanceDelta = TPGunSpeed * Time.deltaTime;
                            GameObject.Find("GorillaPlayer").transform.position = Vector3.MoveTowards(GameObject.Find("GorillaPlayer").transform.position, pointer.transform.position, maxDistanceDelta);
                            GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().isKinematic = false;
                        });
                    }
                }
            }
            public static class Utils
            {
                public static void CreateConfigFileIfNotExists(string fileName, string url)
                {
                    string configFolderPath = "FishMods-Config";
                    string configFile = Path.Combine(configFolderPath, fileName);

                    if (!File.Exists(configFile))
                    {
                        if (!Directory.Exists(configFolderPath)) { Directory.CreateDirectory(configFolderPath); }
                        using (StreamWriter sw = File.CreateText(configFile)) { sw.WriteLine("Ty For Useing FISH Mods Mod Menu, Do Not Del This, Only Del This If You Want To Get Inved Back To The Discord Server, ReOpen Your Game To Get Inved"); }
                        if (!string.IsNullOrEmpty(url)) { Process.Start(Uri.EscapeUriString(url)); }
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
                public static void HideObjects<T>(Vector3 offset) where T : Component
                {
                    //ToggleOwnership();
                    foreach (T obj in Object.FindObjectsOfType<T>())
                    {
                        obj.transform.position = GorillaTagger.Instance.headCollider.transform.position + offset;
                    }
                }
                public static void AlignObjects<T>(GameObject pointer) where T : Component
                {
                    //ToggleOwnership();
                    foreach (T obj in Object.FindObjectsOfType<T>())
                    {
                        obj.transform.position = pointer.transform.position;
                    }
                }
                public static void ResizeObjects<T>(Vector3 scale) where T : Component
                {
                   // ToggleOwnership();
                    foreach (T obj in Object.FindObjectsOfType<T>())
                    {
                        obj.transform.localScale = scale;
                    }
                }
                public static void MoveObjects<T>(Transform target) where T : Component
                {
                    //ToggleOwnership();
                    foreach (T obj in Object.FindObjectsOfType<T>())
                    {
                        obj.transform.position = target.position;
                    }
                }
                public static void SeizureObjects<T>() where T : Component
                {
                    //ToggleOwnership();
                    if (ControllerInput.RightGrip)
                    {
                        foreach (T obj in Object.FindObjectsOfType<T>())
                        {
                            float rotationSpeed = 500.0f;
                            obj.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                            obj.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
                            obj.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
                        }
                    }
                }
            }
        }
        #region ModsNotInClass
        public static void Gun(bool Player, Action<GameObject> gunAction)
        {
            if (!Player)
            {
                if (ControllerInput.RightGrip)
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
                        pointer.GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f, 0.15f);
                        gunAction(pointer);
                    }
                    else
                    {
                        pointer.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f, 0.15f);
                    }
                    return;
                }
                UnityEngine.GameObject.Destroy(pointer);
            }
            else if (Player)
            {
                if (ControllerInput.RightGrip)
                {
                    if (GunPlayer == null)
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
                                GunPlayer = raycastHit.collider.GetComponentInParent<VRRig>();
                            }
                        }
                    }
                    if (ControllerInput.RightTrigger && GunPlayer != null)
                    {
                        pointer.GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f, 0.15f);
                        gunAction(pointer);
                    }
                    else
                    {
                        pointer.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f, 0.15f);
                        GunPlayer = null;
                    }
                    return;
                }
                UnityEngine.GameObject.Destroy(pointer);
            }
            else { FishMenu.MainUtils.Utils.ConsoleUtility.WriteLine("Theres A Error With the guns"); }
        }
        public static void Projectile(string projectileName, Vector3 velocity, Vector3 position, Color color, bool noDelay = false)
        {
            /*
            ControllerInputPoller.instance.leftControllerGripFloat = 1f;

            GameObject projectileObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Object.Destroy(projectileObject, 0.1f);
            projectileObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            projectileObject.transform.position = GorillaTagger.Instance.leftHandTransform.position;
            projectileObject.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
            */
            int[] overrideIndices = new int[] { 32, 204, 231, 240, 249 };
            int index = Array.IndexOf<string>(fullProjectileNames, projectileName);
            //projectileObject.AddComponent<GorillaSurfaceOverride>().overrideIndex = overrideIndices[index];
            //projectileObject.GetComponent<Renderer>().enabled = false;
            /*

            if (Time.time > projDebounce)
            {
                try
                {
                    */
                    string[] anchorPrefixes = new string[] { "LMACE.", "LMAEX.", "LMAGD.", "LMAHQ.", "LMAIE." };
                    string anchorName = fullProjectileNames[index] + "LeftAnchor";
                    Transform anchor = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L/palm.01.L/TransferrableItemLeftHand/" + anchorName)
                                        .transform.Find(anchorPrefixes[index]);
                    //Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                    int anchorValue;
                    if (int.TryParse(anchorPrefixes[index], out anchorValue))
                    {
                        Rpc.SendLaunchProjectile(position, velocity, anchorValue, 163790326, false, false, color);
                    }
                    /*
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
            }*/
        }
        public void ProcessTagAura(Photon.Realtime.Player pl)
        {
            if ((double)Time.time > (double)TagAura + 0.1)
            {
                float num = Vector3.Distance(Local.GetLocalPlayer().offlineVRRig.transform.position, GorillaGameManager.instance.FindPlayerVRRig(pl).transform.position);
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
                        Local.GetLocalPlayer().offlineVRRig.transform.position = Tagger.transform.position;
                        ProcessTagAura(RigManager.GetRigView(Tagger).Owner);
                    }
                }
                else
                {
                    pointer.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f, 0.15f);
                    Local.GetLocalPlayer().offlineVRRig.enabled = true;
                    Tagger = null;
                }
            }
        }
        public void TagAll()
        {
            if (Instance.IsModded())
            {
                foreach (Player player in PhotonNetwork.PlayerListOthers)
                {
                    foreach (GorillaTagManager gorillaTagManager in Object.FindObjectsOfType<GorillaTagManager>())
                    {
                        if (!gorillaTagManager.currentInfected.Contains(player))
                        {
                            GorillaTagger.Instance.offlineVRRig.enabled = false;
                            GorillaTagger.Instance.offlineVRRig.transform.position = GorillaGameManager.instance.FindPlayerVRRig(player).transform.position;
                            GorillaLocomotion.Player.Instance.rightControllerTransform.position = GorillaGameManager.instance.FindPlayerVRRig(player).transform.position;
                        }
                    }
                }
            }
            else { Instance.StartCoroutine(Instance.AntiBan()); }
        }
        public void SpamGun(string Hash, float Timer)
        {
            Gun(false, pointer =>
            {
                PhotonView Photonview = RigManager.GetRigView(pointer.GetComponentInParent<VRRig>());
                if (ControllerInput.RightTrigger && Time.time > Timer + 0.07f)
                {
                    Vector3 vector = ((Vector3)pointer.transform.position - (Vector3)Local.GetLocalPlayer().offlineVRRig.transform.position).normalized;
                    vector *= 50f;
                    Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                    Projectile(Hash, vector, Local.GetLocalPlayer().offlineVRRig.transform.position, color);
                    Timer = Time.time;
                }
                UnityEngine.GameObject.Destroy(pointer);
            });
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
        public static void ToggleOwnership()
        {
            foreach (ThrowableBug BugRequest in UnityEngine.Object.FindObjectsOfType(typeof(ThrowableBug)))
            {
                foreach (TransferrableBall BeachBallRequest in UnityEngine.Object.FindObjectsOfType(typeof(TransferrableBall)))
                {
                    foreach (BalloonHoldable BalloonHoldableRequest in UnityEngine.Object.FindObjectsOfType(typeof(BalloonHoldable)))
                    {
                        foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                        {
                            if (vrrig.isMyPlayer)
                            {
                                if (!BugRequest.IsMyItem())
                                {
                                    BugRequest.WorldShareableRequestOwnership();
                                    BugRequest.currentState = (TransferrableObject.PositionState)128;
                                    BugRequest.GetType().GetField("locked", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(BugRequest, false);
                                    BugRequest.targetRig = vrrig;
                                }
                                else if (!BeachBallRequest.IsMyItem())
                                {
                                    BeachBallRequest.WorldShareableRequestOwnership();
                                    BeachBallRequest.currentState = (TransferrableObject.PositionState)128;
                                    BeachBallRequest.GetType().GetField("locked", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(BeachBallRequest, false);
                                    BeachBallRequest.targetRig = vrrig;
                                }
                                else if (!BalloonHoldableRequest.IsMyItem())
                                {
                                    BalloonHoldableRequest.WorldShareableRequestOwnership();
                                    BalloonHoldableRequest.currentState = (TransferrableObject.PositionState)128;
                                    BalloonHoldableRequest.GetType().GetField("locked", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(BalloonHoldableRequest, false);
                                    BalloonHoldableRequest.targetRig = vrrig;
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void BansINTheBin()
        {
            PlayFabAuthenticationAPI.ForgetAllCredentials();
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            GorillaComputer.instance.internetFailure = true;
            bool revoking = false;

            if (!revoking)
            {
                foreach (GorillaNetworking.PlayFabAuthenticator n in UnityEngine.GameObject.FindObjectsOfType<GorillaNetworking.PlayFabAuthenticator>())
                {
                    n.AuthenticateWithPlayFab();
                    n.gameObject.GetComponent<GorillaNetworking.PlayFabAuthenticator.BanInfo>().BanExpirationTime = "0";
                    n.gameObject.GetComponent<GorillaNetworking.PlayFabAuthenticator.BanInfo>().BanMessage = null;
                    n.oculusID = SteamUser.GetSteamID().ToString();
                    n.gorillaComputer.UpdateScreen();
                    n.debugText.text = "ban revoked";
                }
                revoking = true;
            }
            bool JoinLobbyAttemp = false;
            if (revoking)
            {
                PlayFab.PlayFabError playFab = new PlayFabError();
                playFab.ErrorMessage = "";
                playFab.ErrorMessage = null;
                PlayFabAuthenticator.BanInfo banInfo = JsonUtility.FromJson<PlayFabAuthenticator.BanInfo>("No ban lmao");
                WebClient webClient = new WebClient();
                webClient.DownloadString("https://auth-prod.gtag-cf.com/api/PlayFabAuthentication").Remove(0, int.MaxValue);
                GorillaNetworking.GorillaComputer.ComputerState s;
                s = GorillaComputer.ComputerState.NameWarning;
                JoinLobbyAttemp = true;
            }
            bool Connect = false;
            if (!JoinLobbyAttemp)
            {
                foreach (GorillaNetworking.GorillaComputer gorilla in UnityEngine.GameObject.FindObjectsOfType<GorillaNetworking.GorillaComputer>())
                {
                    gorilla.GeneralFailureMessage("oxy on top Lol.");
                    gorilla.screenText.Text = "卐卐卐卐卐卐卐卐卐卐卐卐卐oxy on top卐卐卐卐卐卐卐卐卐卐卐卐卐卐";
                    gorilla.updateCooldown = 0;
                    gorilla.UpdateScreen();
                    gorilla.screenChanged = true;
                    JoinLobbyAttemp = true;
                    Connect = true;

                }
            }

            if (Connect)
            {
                GorillaComputer.instance.internetFailure = false;
                PhotonNetwork.JoinRandomRoom();
            }

        }
        #endregion
        #endregion
        #region MainMenu
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
        #region Fields

        // General fields
        public static bool consoleStartAttempt, IfDisabled = false, IfAnimalEnabled = false, consoleStartAttempt2, SaveSetting, LoadSetting, onceRightGrip, onceLeftGrip, rightsecondarybutton, teleportGunAntiRepeat, flying, noesp, SettingsPageOn, spazLava = false;
        public static string _playFabPlayerIdCache, _sessionTicket, userToken;
        public static bool MenuLoaded = true;
        private static bool once;
        public string Room;
        public static bool once_left, once_right, once_left_false, once_right_false, once_networking, LeftToggle, RightToggle, ghostToggled;

        // Player-related fields
        public static VRRig kickp, lucyp, lagrig, chosenplayer, Tagger, GunPlayer;

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
        public static string[] fullProjectileNames = new string[] { "Snowball", "WaterBalloon", "LavaRock", "ThrowableGift", "ScienceCandy", "BucketGiftCoal", "MoltenRock", "SpiderBow", "CandyCane", "RollPresent", "RoundPresent", "Square Present" };
        public static float orbitSpeed, KickG, c1, RockSpamTimer, CoalSpamTimer, WaterBalloonTimer, SnowBallTimer, SplashTime, RopeTimer, angle, TagAura;
        public static int ESpInt, platCountColor, SlingshotCountType, platCountType, TPSpeedCount, SpeedCount, BoneESpInt, framePressCooldown, pageNumber, btnCooldown = 0;
        public static float SlingshotType, smth, smth2, plattype, projDebounce = 0f;

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
        private static MainMenu _instance;

        // Properties
        public static MainMenu Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MainMenu>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("MainMenu");
                        _instance = obj.AddComponent<MainMenu>();
                    }
                }
                return _instance;
            }
        }
        #endregion
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
    }
}
