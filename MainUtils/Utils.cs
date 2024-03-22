using System;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using UnityEngine;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Linq;
using Valve.VR;
using BepInEx;
using static FishMenu.Main.MainMenu;
using static FishMenu.MainUtils.Utils;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using FishMenu.Main;
using System.Collections.Generic;

namespace FishMenu.MainUtils
{
    public static class ReflectorExtensions { public static Reflection Reflect(this object obj) => Reflection.GetReflection(obj); }
    [BepInPlugin("com.notfishvr.fishmenu", "notfishvr", "1.0.0")]
    public class Loader : BaseUnityPlugin
    {
        public void FixedUpdate()
        {
            Awake();
            if (!GameObject.Find("Loader") && GorillaLocomotion.Player.hasInstance)
            {
                GameObject Loader = new GameObject("Loader");
                Loader.AddComponent<Local>();
                Loader.AddComponent<LocalPlayer>();
                Loader.AddComponent<MainMenu>();
                Loader.AddComponent<MainGUI>();
                Loader.AddComponent<ControllerInput>();
                Loader.AddComponent<RigManager>();
                Loader.AddComponent<NotifiLib>();
                Loader.AddComponent<RoomManager>();
            }
        }
        private void Awake()
        {
            try
            {
                string text = Paths.ConfigPath + "/BepInEx.cfg";
                string text2 = File.ReadAllText(text);
                text2 = Regex.Replace(text2, "HideManagerGameObject = .+", "HideManagerGameObject = true");
                File.WriteAllText(text, text2);
            }
            catch (Exception ex)
            {
                Mods.Utils.LogError(ex);
            }
        }
    }

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
    public class Utils
    {
        [Serializable]
        public class Presets
        {
            public static void SavePresets()
            {
                Presets presets = new Presets();
                presets.SettingsButtonsActive = MainMenu.SettingsButtonsActive;
                presets.BasicButtonsActive = MainMenu.BasicButtonsActive;
                presets.RopeButtonsActive = MainMenu.RopeButtonsActive;
                presets.SpamRpcButtonsActive = MainMenu.SpamRpcButtonsActive;
                presets.BugButtonsActive = MainMenu.BugButtonsActive;
                presets.TagButtonsActive = MainMenu.TagButtonsActive;
                presets.MicButtonsActive = MainMenu.MicButtonsActive;
                presets.OPButtonsActive = MainMenu.OPButtonsActive;
                presets.HalloweenButtonsActive = MainMenu.HalloweenButtonsActive;
                presets.LavaButtonsActive = MainMenu.LavaButtonsActive;
                XmlSerializer serializer = new XmlSerializer(typeof(Presets));
                using (StreamWriter writer = new StreamWriter("presets.xml")) { serializer.Serialize(writer, presets); }
                ConsoleUtility.WriteLine("Presets saved successfully.");
            }
            public static void LoadPresets()
            {
                if (File.Exists("presets.xml"))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Presets));
                    using (StreamReader reader = new StreamReader("presets.xml"))
                    {
                        Presets presets = (Presets)serializer.Deserialize(reader);
                        MainMenu.SettingsButtonsActive = presets.SettingsButtonsActive;
                        MainMenu.BasicButtonsActive = presets.BasicButtonsActive;
                        MainMenu.RopeButtonsActive = presets.RopeButtonsActive;
                        MainMenu.SpamRpcButtonsActive = presets.SpamRpcButtonsActive;
                        MainMenu.BugButtonsActive = presets.BugButtonsActive;
                        MainMenu.TagButtonsActive = presets.TagButtonsActive;
                        MainMenu.MicButtonsActive = presets.MicButtonsActive;
                        MainMenu.OPButtonsActive = presets.OPButtonsActive;
                        MainMenu.HalloweenButtonsActive = presets.HalloweenButtonsActive;
                        MainMenu.LavaButtonsActive = presets.LavaButtonsActive;
                    }
                    ConsoleUtility.WriteLine("Presets loaded successfully.");
                }
                else { ConsoleUtility.WriteLine("No saved presets found."); }
            }
            public bool[] SettingsButtonsActive { get; set; }
            public bool[] BasicButtonsActive { get; set; }
            public bool[] RopeButtonsActive { get; set; }
            public bool[] SpamRpcButtonsActive { get; set; }
            public bool[] BugButtonsActive { get; set; }
            public bool[] TagButtonsActive { get; set; }
            public bool[] MicButtonsActive { get; set; }
            public bool[] OPButtonsActive { get; set; }
            public bool[] HalloweenButtonsActive { get; set; }
            public bool[] LavaButtonsActive { get; set; }
        }
        public class ConsoleUtility : MonoBehaviour
        {
            [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
            private static extern int AllocConsole();

            [DllImport("user32.dll", SetLastError = true)]
            private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool SetConsoleTitle(string lpConsoleTitle);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool WriteConsole(IntPtr hConsoleOutput, string lpBuffer, uint nNumberOfCharsToWrite, out uint lpNumberOfCharsWritten, IntPtr lpReserved);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr GetStdHandle(int nStdHandle);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, uint wAttributes);

            public static void OpenConsoleWindow()
            {
                AllocConsole();
                SetConsoleTitle("Console [Initializing]");
                WriteToConsole("Debug log started!", ConsoleColor.Green);
                Debug.Log("Console was started!");
                SetConsoleTitle("Console [Ready]");
            }
            public static void WriteToConsole(string message, ConsoleColor color)
            {
                IntPtr stdHandle = GetStdHandle(-11);
                message += "\n";
                SetConsoleTextAttribute(stdHandle, (uint)color);
                uint num;
                WriteConsole(stdHandle, message, (uint)message.Length, out num, IntPtr.Zero);
                SetConsoleTextAttribute(stdHandle, 15U);
            }
            public static void WriteLine(string text)
            {
                string text2 = "[INFO]: " + text;
                IntPtr stdHandle = GetStdHandle(-11);
                text2 += "\n";
                SetConsoleTextAttribute(stdHandle, 15U);
                uint num;
                WriteConsole(stdHandle, text2, (uint)text2.Length, out num, IntPtr.Zero);
                SetConsoleTextAttribute(stdHandle, 15U);
            }
            public static void LCE(string message)
            {
                IntPtr stdHandle = GetStdHandle(-11);
                message += "\n";
                SetConsoleTextAttribute(stdHandle, 12U);
                uint num;
                WriteConsole(stdHandle, message, (uint)message.Length, out num, IntPtr.Zero);
                SetConsoleTextAttribute(stdHandle, 15U);
            }
            public static IntPtr FindConsoleWindow(string windowName)
            {
                return FindWindow(null, windowName);
            }
            public static IntPtr GetConsole()
            {
                return FindWindow("cmd", null);
            }
            private static int co;
        }
        public class RigManager : MonoBehaviour
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
            private static string PreviousNotifications = "";
            private const int MaxPreviousNotifications = 10;
            private void Awake()
            {
                if (_instance == null) { _instance = this; }
                else { Destroy(gameObject); }
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
        public class ControllerInput : MonoBehaviour
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
                MainMenu.Instance.Room = PhotonNetwork.CurrentRoom.Name;
                NotifiLib.SendNotification(string.Concat(new string[]
                {
                "You have joined Room: ",
                MainMenu.Instance.Room,
                " With ",
                PhotonNetwork.CurrentRoom.PlayerCount.ToString(),
                "/10 Players!"
                }), Color.green);
            }
            public override void OnLeftRoom()
            {
                base.OnLeftRoom();
                NotifiLib.SendNotification("You have Left Room: " + MainMenu.Instance.Room, Color.red);
                MainMenu.Instance.Room = string.Empty;
                MainMenu.GorillaBattleManager = null;
                MainMenu.GorillaHuntManager = null;
                MainMenu.GorillaTagManager = null;
            }
            public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
            {
                base.OnPlayerEnteredRoom(newPlayer);
                NotifiLib.SendNotification(newPlayer.NickName + " Has Joined Room: " + MainMenu.Instance.Room, Color.green);
            }
            public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
            {
                base.OnPlayerLeftRoom(otherPlayer);
                NotifiLib.SendNotification(otherPlayer.NickName + " Has Left Room: " + MainMenu.Instance.Room, Color.red);
            }
        }
        public class BtnCollider : MonoBehaviour
        {
            private void OnTriggerEnter(Collider collider)
            {
                if (Time.frameCount >= MainMenu.framePressCooldown + 10)
                {
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.1f);
                    GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tagHapticStrength / 2, GorillaTagger.Instance.tagHapticDuration / 2);
                    if (MainMenu.NumberForPage == 1)
                    {
                        MainMenu.Toggle(relatedText, MainMenu.buttons, MainMenu.buttonsActive);
                    }
                    else if (MainMenu.NumberForPage == 2)
                    {
                        MainMenu.Toggle(relatedText, MainMenu.Settingsbuttons, MainMenu.SettingsButtonsActive);
                    }
                    else if (MainMenu.NumberForPage == 3)
                    {
                        MainMenu.Toggle(relatedText, MainMenu.Basicbuttons, MainMenu.BasicButtonsActive);
                    }
                    else if (MainMenu.NumberForPage == 4)
                    {
                        MainMenu.Toggle(relatedText, MainMenu.Ropebuttons, MainMenu.RopeButtonsActive);
                    }
                    else if (MainMenu.NumberForPage == 5)
                    {
                        MainMenu.Toggle(relatedText, MainMenu.SpamRpcbuttons, MainMenu.SpamRpcButtonsActive);
                    }
                    else if (MainMenu.NumberForPage == 6)
                    {
                        MainMenu.Toggle(relatedText, MainMenu.Bugbuttons, MainMenu.BugButtonsActive);
                    }
                    else if (MainMenu.NumberForPage == 7)
                    {
                        MainMenu.Toggle(relatedText, MainMenu.Tagbuttons, MainMenu.TagButtonsActive);
                    }
                    else if (MainMenu.NumberForPage == 8)
                    {
                        MainMenu.Toggle(relatedText, MainMenu.Micbuttons, MainMenu.MicButtonsActive);
                    }
                    else if (MainMenu.NumberForPage == 9)
                    {
                        MainMenu.Toggle(relatedText, MainMenu.OPbuttons, MainMenu.OPButtonsActive);
                    }
                    else if (MainMenu.NumberForPage == 10)
                    {
                        MainMenu.Toggle(relatedText, MainMenu.Halloweenbuttons, MainMenu.HalloweenButtonsActive);
                    }
                    else if (MainMenu.NumberForPage == 11)
                    {
                        MainMenu.Toggle(relatedText, MainMenu.Lavabuttons, MainMenu.LavaButtonsActive);
                    }
                    MainMenu.framePressCooldown = Time.frameCount;
                }
            }
            public string relatedText;
        }
        public class Reflection
        {
            private const BindingFlags privateInst = BindingFlags.NonPublic | BindingFlags.Instance;
            private const BindingFlags privateStatic = BindingFlags.NonPublic | BindingFlags.Static;
            private const BindingFlags privateField = privateInst | BindingFlags.GetField;
            private const BindingFlags privateProp = privateInst | BindingFlags.GetProperty;
            private const BindingFlags privateMethod = privateInst | BindingFlags.InvokeMethod;
            private const BindingFlags staticField = privateStatic | BindingFlags.GetField;
            private const BindingFlags staticProp = privateStatic | BindingFlags.GetProperty;
            private const BindingFlags staticMethod = privateStatic | BindingFlags.InvokeMethod;
            private object @object { get; }
            private Type type { get; }

            Reflection(object obj)
            {
                @object = obj;
                type = obj.GetType();
            }
            private T? GetValue<T>(string fieldName, bool isStatic = false, bool isProperty = false)
            {
                BindingFlags flags = isProperty ? isStatic ? staticProp : privateProp : isStatic ? staticField : privateField;
                return isProperty ? GetProperty<T>(fieldName, flags) : GetValue<T>(fieldName, flags);
            }
            public Reflection SetValue(string fieldName, object value, bool isStatic = false, bool isProperty = false)
            {
                BindingFlags flags = isProperty ? isStatic ? staticProp : privateProp : isStatic ? staticField : privateField;
                return isProperty ? SetProperty(fieldName, value, flags) : SetValue(fieldName, value, flags);
            }
            private T? GetValue<T>(string variableName, BindingFlags flags) { try { return (T)type.GetField(variableName, flags).GetValue(@object); } catch (InvalidCastException) { return default; } }
            private T? GetProperty<T>(string propertyName, BindingFlags flags) { try { return (T)type.GetProperty(propertyName, flags).GetValue(@object); } catch (InvalidCastException) { return default; } }
            private Reflection SetValue(string variableName, object value, BindingFlags flags) { try { type.GetField(variableName, flags).SetValue(@object, value); return this; } catch (Exception) { return null; } }
            private Reflection SetProperty(string propertyName, object value, BindingFlags flags) { try { type.GetProperty(propertyName, flags).SetValue(@object, value); return this; } catch (Exception) { return null; } }
            private T? Invoke<T>(string methodName, BindingFlags flags, params object[] args) { try { return (T)type.GetMethod(methodName, flags).Invoke(@object, args); } catch (InvalidCastException) { return default; } }
            private T? Invoke<T>(string methodName, bool isStatic = false, params object[] args) => Invoke<T>(methodName, isStatic ? staticMethod : privateMethod, args);
            public object GetValue(string fieldName, bool isStatic = false, bool isProperty = false) => GetValue<object>(fieldName, isStatic, isProperty);
            public Reflection Invoke(string methodName, bool isStatic = false, params object[] args) => Invoke<object>(methodName, isStatic, args)?.Reflect();
            public Reflection Invoke(string methodName, params object[] args) => Invoke<object>(methodName, args: args)?.Reflect();
            public static Reflection GetReflection(object obj) => new(obj);
        }
        public class Num
        {
            public static bool ContainsNumber(string input) { return Enumerable.Any<char>(input, new Func<char, bool>(char.IsDigit)); }
            public static bool ContainsLetter(string input) { return Enumerable.Any<char>(input, new Func<char, bool>(char.IsLetter)); }
            public static string GenerateRandomString(int length)
            {
                System.Random random = new System.Random();
                const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                return new string(Enumerable.Repeat(letters, length).Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
    }
    // Player 
    public class Local : MonoBehaviour
    {
        public static LocalPlayer GetLocalPlayer()
        {
            var player = new LocalPlayer
            {
                headPosition = GorillaLocomotion.Player.Instance.headCollider.transform.position,
                leftHandPosition = GorillaLocomotion.Player.Instance.leftControllerTransform.position,
                rightHandPosition = GorillaLocomotion.Player.Instance.rightControllerTransform.position,
                player = GorillaLocomotion.Player.Instance,
                offlineVRRig = GorillaTagger.Instance,
            };

            return player;
        }
        public static float GetDistanceFromPlayer(GameObject obj) { return Vector3.Distance(GetLocalPlayer().player.transform.position, obj.transform.position); }
        public static void VibrateController(bool isLeftController, float strength, float duration) { GorillaTagger.Instance.StartVibration(isLeftController, strength, duration); }
    }
    public class LocalPlayer : MonoBehaviour
    {
        public Vector3 headPosition;
        public Vector3 leftHandPosition;
        public Vector3 rightHandPosition;
        public GorillaLocomotion.Player player;
        public GorillaTagger offlineVRRig;
    }
    // Online
    public class Online
    {
        public static OnlinePlayer GetOnlinePlayerFromName(string name)
        {
            var player = new OnlinePlayer();

            VRRig[] vrRigArray = (VRRig[])GameObject.FindObjectsOfType(typeof(VRRig));
            foreach (VRRig rig in vrRigArray)
            {
                foreach (Photon.Realtime.Player otherPlayer in PhotonNetwork.PlayerList)
                {
                    if (otherPlayer.NickName == name)
                    {
                        player.position = rig.transform.position;
                        player.distanceFromPlayer = Vector3.Distance(Local.GetLocalPlayer().headPosition, rig.transform.position);
                        //player.viewId = rig.photonView.ViewID;
                        return player;
                    }
                }
            }
            return null;
        }
        public static OnlinePlayer GetOnlinePlayerFromViewID(string id)
        {
            if (Num.ContainsLetter(id)) { return null; }

            var player = new OnlinePlayer();

            VRRig[] vrRigArray = (VRRig[])GameObject.FindObjectsOfType(typeof(VRRig));
            foreach (VRRig rig in vrRigArray)
            {
                foreach (Photon.Realtime.Player otherPlayer in PhotonNetwork.PlayerList)
                {
                    //if (otherPlayer.UserId == Int32.Parse(id))
                    if (otherPlayer.UserId == id)
                    {
                        player.position = rig.transform.position;
                        player.distanceFromPlayer = Vector3.Distance(Local.GetLocalPlayer().headPosition, rig.transform.position);
                        return player;
                    }
                }
            }

            return null;
        }
        public Dictionary<int, Photon.Realtime.Player> GetAllPlayers() { return PhotonNetwork.CurrentRoom.Players; }
    }
    public class OnlinePlayer
    {
        public Vector3 position;
        public float distanceFromPlayer;
    }
}
