using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using BepInEx;
using GorillaNetworking;
using GorillaTag;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using static FishMenu.MainUtils.Utils;
using static NetworkSystem;
using UnityEngine.InputSystem;

namespace FishMenu.Main
{
    [BepInPlugin("FISHMods.GUI", "GUI", "1.0")]
    internal class MainGUI : BaseUnityPlugin
    {
        private void DrawMainTab()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            ToggleMain[1] = ToggleButton("Disconnect", ToggleMain[1]);
            ToggleMain[2] = ToggleButton("Join Random Room", ToggleMain[2]);
            ToggleMain[3] = ToggleButton("a", ToggleMain[3]);

            GUILayout.EndScrollView();
        }
        private void DrawMicModsTab()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
           
            GUILayout.EndScrollView();
        }
        private void DrawMenuTab()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            if (Main)
            {
                for (int i = 0; i < MainMenu.buttons.Length; i++)
                {
                    if (i < MainMenu.buttonsActive.Length)
                    {
                        MainMenu.buttonsActive[i] = MainGUI.Instance.ToggleButton(MainMenu.buttons[i], MainMenu.buttonsActive[i]);
                        if (i == 0 && MainMenu.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            Settings = true;
                        }
                        if (i == 1 && MainMenu.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            Basic = true;
                        }
                        if (i == 2 && MainMenu.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            Rope = true;
                        }
                        if (i == 3 && MainMenu.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            SpamRpc = true;
                        }
                        if (i == 4 && MainMenu.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            Bug = true;
                        }
                        if (i == 5 && MainMenu.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            Tag = true;
                        }
                        if (i == 6 && MainMenu.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            Mic = true;
                        }
                        if (i == 7 && MainMenu.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            OP = true;
                        }
                        if (i == 8 && MainMenu.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            Halloween = true;
                        }
                        if (i == 9 && MainMenu.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            Lava = true;
                        }
                    }
                }
            }
            if (Settings)
            {
                for (int i = 0; i < MainMenu.Settingsbuttons.Length; i++)
                {
                    if (i < MainMenu.SettingsButtonsActive.Length)
                    {
                        MainMenu.SettingsButtonsActive[i] = MainGUI.Instance.ToggleButton(MainMenu.Settingsbuttons[i], MainMenu.SettingsButtonsActive[i]);
                        if (i == 0 && MainMenu.SettingsButtonsActive[i])
                        {
                            ClearAllButtonStates();
                            Main = true;
                        }
                    }
                }
            }
            if (Basic)
            {
                for (int i = 0; i < MainMenu.Basicbuttons.Length; i++)
                {
                    if (i < MainMenu.BasicButtonsActive.Length)
                    {
                        MainMenu.BasicButtonsActive[i] = MainGUI.Instance.ToggleButton(MainMenu.Basicbuttons[i], MainMenu.BasicButtonsActive[i]);
                        if (i == 0 && MainMenu.BasicButtonsActive[i])
                        {
                            ClearAllButtonStates();
                            Main = true;
                        }
                    }
                }
            }
            if (Rope)
            {
                for (int i = 0; i < MainMenu.Ropebuttons.Length; i++)
                {
                    if (i < MainMenu.RopeButtonsActive.Length)
                    {
                        MainMenu.RopeButtonsActive[i] = MainGUI.Instance.ToggleButton(MainMenu.Ropebuttons[i], MainMenu.RopeButtonsActive[i]);
                        if (i == 0 && MainMenu.RopeButtonsActive[i])
                        {
                            ClearAllButtonStates();
                            Main = true;
                        }
                    }
                }
            }
            if (SpamRpc)
            {
                for (int i = 0; i < MainMenu.SpamRpcbuttons.Length; i++)
                {
                    if (i < MainMenu.SpamRpcButtonsActive.Length)
                    {
                        MainMenu.SpamRpcButtonsActive[i] = MainGUI.Instance.ToggleButton(MainMenu.SpamRpcbuttons[i], MainMenu.SpamRpcButtonsActive[i]);
                        if (i == 0 && MainMenu.SpamRpcButtonsActive[i])
                        {
                            ClearAllButtonStates();
                            Main = true;
                        }
                    }
                }
            }
            if (Bug)
            {
                for (int i = 0; i < MainMenu.Bugbuttons.Length; i++)
                {
                    if (i < MainMenu.BugButtonsActive.Length)
                    {
                        MainMenu.BugButtonsActive[i] = MainGUI.Instance.ToggleButton(MainMenu.Bugbuttons[i], MainMenu.BugButtonsActive[i]);
                        if (i == 0 && MainMenu.BugButtonsActive[i])
                        {
                            ClearAllButtonStates();
                            Main = true;
                        }
                    }
                }
            }
            if (Tag)
            {
                for (int i = 0; i < MainMenu.Tagbuttons.Length; i++)
                {
                    if (i < MainMenu.TagButtonsActive.Length)
                    {
                        MainMenu.TagButtonsActive[i] = MainGUI.Instance.ToggleButton(MainMenu.Tagbuttons[i], MainMenu.TagButtonsActive[i]);
                        if (i == 0 && MainMenu.TagButtonsActive[i])
                        {
                            ClearAllButtonStates();
                            Main = true;
                        }
                    }
                }
            }
            if (Mic)
            {
                for (int i = 0; i < MainMenu.Micbuttons.Length; i++)
                {
                    if (i < MainMenu.MicButtonsActive.Length)
                    {
                        MainMenu.MicButtonsActive[i] = MainGUI.Instance.ToggleButton(MainMenu.Micbuttons[i], MainMenu.MicButtonsActive[i]);
                        if (i == 0 && MainMenu.MicButtonsActive[i])
                        {
                            ClearAllButtonStates();
                            Main = true;
                        }
                    }
                }
            }
            if (OP)
            {
                for (int i = 0; i < MainMenu.OPbuttons.Length; i++)
                {
                    if (i < MainMenu.OPButtonsActive.Length)
                    {
                        MainMenu.OPButtonsActive[i] = MainGUI.Instance.ToggleButton(MainMenu.OPbuttons[i], MainMenu.OPButtonsActive[i]);
                        if (i == 0 && MainMenu.OPButtonsActive[i])
                        {
                            ClearAllButtonStates();
                            Main = true;
                        }
                    }
                }
            }
            if (Lava)
            {
                for (int i = 0; i < MainMenu.Lavabuttons.Length; i++)
                {
                    if (i < MainMenu.LavaButtonsActive.Length)
                    {
                        MainMenu.LavaButtonsActive[i] = MainGUI.Instance.ToggleButton(MainMenu.Lavabuttons[i], MainMenu.LavaButtonsActive[i]);
                        if (i == 0 && MainMenu.LavaButtonsActive[i])
                        {
                            ClearAllButtonStates();
                            Main = true;
                        }
                    }
                }
            }
            if (Halloween)
            {
                for (int i = 0; i < MainMenu.Halloweenbuttons.Length; i++)
                {
                    if (i < MainMenu.HalloweenButtonsActive.Length)
                    {
                        MainMenu.HalloweenButtonsActive[i] = MainGUI.Instance.ToggleButton(MainMenu.Halloweenbuttons[i], MainMenu.HalloweenButtonsActive[i]);
                        if (i == 0 && MainMenu.HalloweenButtonsActive[i])
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
            Lava = false;
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
                        if (HasPlayerTouchedLiquid(selectedPlayer)) { if (GUILayout.Button("UnAcid " + selectedPlayer.NickName)) { MainMenu.Mods.MainStuff.OpMods.AcidKid(selectedPlayer); } }
                        if (!HasPlayerTouchedLiquid(selectedPlayer)) { if (GUILayout.Button("Acid " + selectedPlayer.NickName)) { MainMenu.Mods.MainStuff.OpMods.AcidKid(selectedPlayer); } }
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
        private Texture2D button, windowBackground, buttonHovered, buttonActive, textArea, textAreaHovered, textAreaActive, box;
        private GameObject directionalLightClone;
        private static MainGUI _instance;
        public bool Main = true;
        private Player selectedPlayer;
        public bool Settings, Basic, Rope, SpamRpc, Bug, Tag, Mic, Halloween, OP, Lava = false;
        public KeyCode toggleKey = KeyCode.Insert;
        public static GameObject pointer;
        internal static bool playerManagerEnabled = false;
        public string guiSelectedEnemy;
        public static MainGUI Instance => _instance;
        // GUI Variables
        public static Rect GUIRect = new Rect(0, 0, 540, 240);
        private static int selectedTab = 0;
        private static readonly string[] tabNames = { "Main", "Misc", "Menu", "Player List", "Settings" };
        private bool[] TogglePlayerList, ToggleMic, ToggleMain = new bool[999];
        private bool toggled = true;
        public float toggleDelay = 0.5f;
        private float lastToggleTime;
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
    }
}
