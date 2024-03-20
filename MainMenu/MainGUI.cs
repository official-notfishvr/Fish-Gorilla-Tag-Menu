using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using BepInEx;
using FishMenu.MainMenu;
using GorillaNetworking;
using GorillaTag;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace FishMenu.MainMenu
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
                            OP = true;
                        }
                        if (i == 8 && MenuPatch.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            Halloween = true;
                        }
                        if (i == 9 && MenuPatch.buttonsActive[i])
                        {
                            ClearAllButtonStates();
                            Lava = true;
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
            if (Basic)
            {
                for (int i = 0; i < MenuPatch.Basicbuttons.Length; i++)
                {
                    if (i < MenuPatch.BasicButtonsActive.Length)
                    {
                        MenuPatch.BasicButtonsActive[i] = MainGUI.Instance.ToggleButton(MenuPatch.Basicbuttons[i], MenuPatch.BasicButtonsActive[i]);
                        if (i == 0 && MenuPatch.BasicButtonsActive[i])
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
            if (Lava)
            {
                for (int i = 0; i < MenuPatch.Lavabuttons.Length; i++)
                {
                    if (i < MenuPatch.LavaButtonsActive.Length)
                    {
                        MenuPatch.LavaButtonsActive[i] = MainGUI.Instance.ToggleButton(MenuPatch.Lavabuttons[i], MenuPatch.LavaButtonsActive[i]);
                        if (i == 0 && MenuPatch.LavaButtonsActive[i])
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
