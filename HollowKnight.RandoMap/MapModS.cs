using HutongGames.PlayMaker;
using Modding;
using RandoMapMod.UnityComponents;
using SereCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RandoMapMod {
	public class MapModS : Mod {
		#region Meta
		public override string GetVersion() {
			string ver = "1.0.6"; //If you update this, please also update the README.
			int minAPI = 45;

			bool apiTooLow = Convert.ToInt32(ModHooks.Instance.ModVersion.Split('-')[1]) < minAPI;
			if (apiTooLow) {
				return ver + " (Update your API)";
			}

			return ver;
		}
		#endregion
		#region Static Stuff
		private const int SAFE = 3;
		private const float MAP_MIN_X = -24.16f;
		private const float MAP_MAX_X = 17.3f;
		private const float MAP_MIN_Y = -12.58548f;
		private const float MAP_MAX_Y = 15.6913f;

		private static int _convoCheck = 0;
		private static bool _locked = false;

		public static MapModS Mm {
			get; private set;
		}

		public static bool IsRando => RandomizerMod.RandomizerMod.Instance.Settings.Randomizer;

		internal static void ToggleGroup1() {
			Mm.Settings.Group1On = !Mm.Settings.Group1On;
			Mm.PinGroupInstance.Group1.SetActive(Mm.Settings.Group1On);
		}

		internal static void ToggleGroup2() {
			Mm.Settings.Group2On = !Mm.Settings.Group2On;
			Mm.PinGroupInstance.Group2.SetActive(Mm.Settings.Group2On);
		}

		internal static void ToggleGroup3() {
			Mm.Settings.Group3On = !Mm.Settings.Group3On;
			Mm.PinGroupInstance.Group3.SetActive(Mm.Settings.Group3On);
		}

		internal static void ToggleGroup4() {
			Mm.Settings.Group4On = !Mm.Settings.Group4On;
			Mm.PinGroupInstance.Group4.SetActive(Mm.Settings.Group4On);
		}

		internal static void ToggleGroup5() {
			Mm.Settings.Group5On = !Mm.Settings.Group5On;
			Mm.PinGroupInstance.Group5.SetActive(Mm.Settings.Group5On);
		}

		internal static void ToggleGroup6() {
			Mm.Settings.Group6On = !Mm.Settings.Group6On;
			Mm.PinGroupInstance.Group6.SetActive(Mm.Settings.Group6On);
		}

		internal static void SetAllPins() {
			Mm.PinGroupInstance.Group1.SetActive(Mm.Settings.Group1On);
			Mm.PinGroupInstance.Group2.SetActive(Mm.Settings.Group2On);
			Mm.PinGroupInstance.Group3.SetActive(Mm.Settings.Group3On);
			Mm.PinGroupInstance.Group4.SetActive(Mm.Settings.Group4On);
			Mm.PinGroupInstance.Group5.SetActive(Mm.Settings.Group5On);
			Mm.PinGroupInstance.Group6.SetActive(Mm.Settings.Group6On);
		}

		internal static void ToggleAllPins() {
			if (!Mm.Settings.Group1On
				&& !Mm.Settings.Group2On
				&& !Mm.Settings.Group3On
				&& !Mm.Settings.Group4On
				&& !Mm.Settings.Group5On
				&& !Mm.Settings.Group6On) {
				Mm.Settings.Group1On = true;
				Mm.Settings.Group2On = true;
				Mm.Settings.Group3On = true;
				Mm.Settings.Group4On = true;
				Mm.Settings.Group5On = true;
				Mm.Settings.Group6On = true;
				SetAllPins();
			} else {
				Mm.Settings.Group1On = false;
				Mm.Settings.Group2On = false;
				Mm.Settings.Group3On = false;
				Mm.Settings.Group4On = false;
				Mm.Settings.Group5On = false;
				Mm.Settings.Group6On = false;
				SetAllPins();
			}

		}

		internal static void ToggleRandoPins() {
			Mm.Settings.SpoilerOn = !Mm.Settings.SpoilerOn;
			Mm.Settings.UnknownOn = false;
			Mm.PinGroupInstance.SetSpoilerSprites(Mm.Settings.SpoilerOn);
		}

		internal static void ToggleUnknownPins() {
			Mm.Settings.UnknownOn = !Mm.Settings.UnknownOn;
			if (Mm.Settings.UnknownOn) {
				Mm.PinGroupInstance.SetUnknownSprites();
			} else {
				Mm.PinGroupInstance.SetSpoilerSprites(Mm.Settings.SpoilerOn);
			}
		}

#if DEBUG
		internal static void ReloadGameMapPins() {
			try {
				GameMap gameMap = GameObject.Find("Game_Map(Clone)").GetComponent<GameMap>();
				// Change PinDataDictionary during run-time! Used for editing pin positions while the game is running
				ResourceHelper.ReloadPinData();
				Mm.PinGroupInstance.RefreshPins(gameMap);

				DebugLog.Log("Updating Pin Positions");
			} catch (Exception e) {
				DebugLog.Error($"Error: {e}");
			}
		}

		internal static void GetAllMapNames() {
			GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
			foreach (GameObject go in allObjects) {
				//DebugLog.Log($"{go.name}");
				if (go.name == "Inventory") {
					foreach (Transform invObj in go.transform) {
						DebugLog.Log($"- {invObj.gameObject.name}");
					}
				}
			}

			GameMap gameMap = GameObject.Find("Game_Map(Clone)").GetComponent<GameMap>();
			foreach (Transform areaObj in gameMap.transform) {
				DebugLog.Log($"{areaObj.gameObject.name}");
				foreach (Transform roomObj in areaObj.transform) {
					DebugLog.Log($"- {roomObj.gameObject.name}");
				}
			}
		}
#endif

		internal static void DisableVanillaMapAssets() {
			foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) {
				switch (go.name) {
					case "Game_Map(Clone)":
						foreach (Transform mapObj in go.transform) {
							if (mapObj.gameObject.name == "Dreamer Pins"
								|| mapObj.gameObject.name == "Flame Pins"
								|| mapObj.gameObject.name == "Map Markers")
								mapObj.gameObject.SetActive(false);
						}
						break;
					case "Map Markers":
						go.SetActive(false);
						break;
					case "Inventory":
						foreach (Transform invObj in go.transform) {
							if (invObj.gameObject.name == "Map Key")
								invObj.gameObject.SetActive(false);
						}
						break;
				}
			}
		}
		
		// This needs to be done once every game load when you don't have Quill, otherwise the map will be broken
		internal static void ForceMapUpdate() {
			PlayerData pd = PlayerData.instance;

			if (!pd.hasQuill) {
				try {
					// Give Quill, because it's required to...
					pd.SetBool(nameof(pd.hasQuill), true);

					// ... uncover the full map!!
					GameMap gameMap = GameObject.Find("Game_Map(Clone)").GetComponent<GameMap>();
					gameMap.SetupMap();

					// Remove Quill
					pd.SetBool(nameof(pd.hasQuill), false);
				} catch (Exception e) {
					DebugLog.Error($"Map object not found: {e}");
				}
			}
		}

		public static void GiveAllMaps(string from) {
			if (!Mm.Settings.MapsGiven) {
				DebugLog.Log($"Maps granted from {from}");

				PlayerData pd = PlayerData.instance;
				Type playerData = typeof(PlayerData);

				// Give the maps to the player
				pd.SetBool(nameof(pd.hasMap), true);

				foreach (FieldInfo field in playerData.GetFields().Where(field => field.Name.StartsWith("map") && field.FieldType == typeof(bool))) {
					pd.SetBool(field.Name, true);
				}

				ForceMapUpdate();

				// Set cornifer as having left all the areas. This could be condensed into the previous foreach for one less GetFields(), but I value the clarity more.
				foreach (FieldInfo field in playerData.GetFields().Where(field => field.Name.StartsWith("corn") && field.Name.EndsWith("Left"))) {
					pd.SetBool(field.Name, true);
				}

				// Set Cornifer as sleeping at home
				pd.SetBool(nameof(pd.corniferAtHome), true);

				SetAllPins();

				Mm.Settings.MapsGiven = true;
				GameManager.instance.SaveGame();
			}
		}
		#endregion

		#region Private Non-Methods
		private ItemPickup _itemTracker;
		#endregion

		#region Non-Private Non-Methods
		public SaveSettings Settings { get; private set; } = new SaveSettings();
		public GameObject PinGroupGO = null;
		public PinGroup PinGroupInstance => PinGroupGO?.GetComponent<PinGroup>();
		#endregion

		#region <Mod> Overrides
		public override ModSettings SaveSettings {
			get => Settings ??= new SaveSettings();
			set => Settings = value is SaveSettings saveSettings ? saveSettings : Settings;
		}

		public override void Initialize() {

			if (Mm != null) {
				DebugLog.Warn("Initialized twice... Stop that.");
				return;
			}
			Mm = this;
			DebugLog.Log("RandoMapMod Initializing...");

			On.GameMap.Start += this._GameMap_Start;                        //Set up custom pins
			On.GameMap.WorldMap += this._GameMap_WorldMap;                  //Set big map boundaries
			On.GameMap.SetupMapMarkers += this._GameMap_SetupMapMarkers;    //Enable the custom pins
			On.GameMap.DisableMarkers += this._GameMap_DisableMarkers;      //Disable the custom pins

			On.GameMap.QuickMapAncientBasin += this._GameMap_QuickMapAncientBasin;
			On.GameMap.QuickMapCity += this._GameMap_QuickMapCity;
			On.GameMap.QuickMapCliffs += this._GameMap_QuickMapCliffs;
			On.GameMap.QuickMapCrossroads += this._GameMap_QuickMapCrossroads;
			On.GameMap.QuickMapCrystalPeak += this._GameMap_QuickMapCrystalPeak;
			On.GameMap.QuickMapDeepnest += this._GameMap_QuickMapDeepnest;
			On.GameMap.QuickMapDirtmouth += this._GameMap_QuickMapDirtmouth;
			On.GameMap.QuickMapFogCanyon += this._GameMap_QuickMapFogCanyon;
			On.GameMap.QuickMapFungalWastes += this._GameMap_QuickMapFungalWastes;
			On.GameMap.QuickMapGreenpath += this._GameMap_QuickMapGreenpath;
			On.GameMap.QuickMapKingdomsEdge += this._GameMap_QuickMapKingdomsEdge;
			On.GameMap.QuickMapQueensGardens += this._GameMap_QuickMapQueensGardens;
			On.GameMap.QuickMapRestingGrounds += this._GameMap_QuickMapRestingGrounds;
			On.GameMap.QuickMapWaterways += this._GameMap_QuickMapWaterways;

			On.GrubPin.OnEnable += this._GrubPin_Enable;                    //Disable all grub pins so we can use our own. (Only if we were given maps.)

			UnityEngine.SceneManagement.SceneManager.activeSceneChanged += _HandleSceneChanges;
			ModHooks.Instance.LanguageGetHook += _HandleLanguageGet;

			_itemTracker = new ItemPickup();

			DebugLog.Log("RandoMapMod Initialize complete!");
		}
		#endregion

		#region Private Methods
		private void _GameMap_Start(On.GameMap.orig_Start orig, GameMap self) {
			DebugLog.Log("GameMap_Start");
			if (!IsRando) {
				orig(self);
				return;
			}

			try {
				DebugLog.Log("Emptying out HelperData on game start.");

				DisableVanillaMapAssets();
#if DEBUG
				GetAllMapNames();
#endif
				//Create the custom pin group, and add all the new pins
				if (this.PinGroupGO == null) {
					DebugLog.Log("First Setup. Adding Pin Group and Populating...");
					this.PinGroupGO = new GameObject("RandoMap Pins");
					this.PinGroupGO.AddComponent<PinGroup>();
					this.PinGroupGO.transform.SetParent(self.transform);

					Mm.PinGroupInstance.MakePinGroups();

					// Find the spoiler pools when On.GameMap.Start is invoked
					DebugLog.Log("Getting Spoiler Item Info");
					ResourceHelper.FindSpoilerPools();

					foreach (KeyValuePair<string, PinData> entry in ResourceHelper.PinDataDictionary) {
						string itemName = entry.Key;
						PinData pin = entry.Value;

						if (!pin.HidePin) {
							this.PinGroupInstance.AddPinToRoom(pin, self);
						}
					}

					if (Mm.Settings.UnknownOn) {
						Mm.PinGroupInstance.SetUnknownSprites();
					} else {
						Mm.PinGroupInstance.SetSpoilerSprites(Mm.Settings.SpoilerOn);
					}

					DebugLog.Log($"Settings: SpoilerOn {Mm.Settings.SpoilerOn}");
					DebugLog.Log($"Settings: UnknownOn {Mm.Settings.UnknownOn}");

					InputListener.InstantiateSingleton();
				}
			} catch (Exception e) {
				DebugLog.Error($"Error: {e}");
			}

			DebugLog.Log("Finished.");
			orig(self);
		}

		private void _GameMap_WorldMap(On.GameMap.orig_WorldMap orig, GameMap self) {
			orig(self);
			if (!IsRando)
				return;

			DebugLog.Log("WorldMap");

			// Check reachable, item picked up, etc.
			Mm.PinGroupInstance.UpdatePins();

			//Set the maximum scroll boundaries, so we can scroll the entire map, even if we don't have the maps unlocked.
			if (self.panMinX > MAP_MIN_X)
				self.panMinX = MAP_MIN_X;
			if (self.panMaxX < MAP_MAX_X)
				self.panMaxX = MAP_MAX_X;
			if (self.panMinY > MAP_MIN_Y)
				self.panMinY = MAP_MIN_Y;
			if (self.panMaxY < MAP_MAX_Y)
				self.panMaxY = MAP_MAX_Y;
		}

		private void _GameMap_SetupMapMarkers(On.GameMap.orig_SetupMapMarkers orig, GameMap self) {
			orig(self);
			this._DeleteErrantLifebloodPin(self);

			if (!IsRando) {
				return;
			}
			this.PinGroupInstance.Show();

			ForceMapUpdate();

			SetAllPins();
		}

		private void _DeleteErrantLifebloodPin(GameMap gameMap) {
			GameObject go = gameMap.transform.Find("Deepnest")?.Find("Deepnest_26")?.Find("pin_blue_health")?.gameObject;
			if (go == null) {
				DebugLog.Log("Couldn't find the pin!");
				return;
			}

			go.SetActive(false);
		}

		private void _GameMap_DisableMarkers(On.GameMap.orig_DisableMarkers orig, GameMap self) {
			if (!IsRando) {
				orig(self);
				return;
			}
			try {
				this.PinGroupInstance.Hide();
			} catch (Exception e) {
				DebugLog.Error($"Failed to DisableMarkers {e.Message}");
			}
			orig(self);
		}
		private void _GrubPin_Enable(On.GrubPin.orig_OnEnable orig, GrubPin self) {
			if (Mm.Settings.MapsGiven) {
				if (self.gameObject.activeSelf) self.gameObject.SetActive(false);
			}

			orig(self);
		}

		private void _GameMap_QuickMapAncientBasin(On.GameMap.orig_QuickMapAncientBasin orig, GameMap self) {
			// Ancient_Basin
			Mm.PinGroupInstance.UpdatePins();
			orig(self);
		}
		private void _GameMap_QuickMapCity(On.GameMap.orig_QuickMapCity orig, GameMap self) {
			// City_of_Tears
			Mm.PinGroupInstance.UpdatePins();
			orig(self);
		}
		private void _GameMap_QuickMapCliffs(On.GameMap.orig_QuickMapCliffs orig, GameMap self) {
			// Howling_Cliffs
			Mm.PinGroupInstance.UpdatePins();
			orig(self);
		}
		private void _GameMap_QuickMapCrossroads(On.GameMap.orig_QuickMapCrossroads orig, GameMap self) {
			// Forgotten_Crossroads
			Mm.PinGroupInstance.UpdatePins();
			orig(self);
		}
		private void _GameMap_QuickMapCrystalPeak(On.GameMap.orig_QuickMapCrystalPeak orig, GameMap self) {
			// Crystal_Peak
			Mm.PinGroupInstance.UpdatePins();
			orig(self);
		}
		private void _GameMap_QuickMapDeepnest(On.GameMap.orig_QuickMapDeepnest orig, GameMap self) {
			// Deepnest
			Mm.PinGroupInstance.UpdatePins();
			orig(self);
		}
		private void _GameMap_QuickMapDirtmouth(On.GameMap.orig_QuickMapDirtmouth orig, GameMap self) {
			// Dirtmouth
			Mm.PinGroupInstance.UpdatePins();
			orig(self);
		}
		private void _GameMap_QuickMapFogCanyon(On.GameMap.orig_QuickMapFogCanyon orig, GameMap self) {
			// Fog_Canyon
			Mm.PinGroupInstance.UpdatePins();
			orig(self);
		}
		private void _GameMap_QuickMapFungalWastes(On.GameMap.orig_QuickMapFungalWastes orig, GameMap self) {
			// Fungal_Wastes
			Mm.PinGroupInstance.UpdatePins();
			orig(self);
		}
		private void _GameMap_QuickMapGreenpath(On.GameMap.orig_QuickMapGreenpath orig, GameMap self) {
			// Greenpath
			Mm.PinGroupInstance.UpdatePins();
			orig(self);
		}
		private void _GameMap_QuickMapKingdomsEdge(On.GameMap.orig_QuickMapKingdomsEdge orig, GameMap self) {
			// Kingdoms_Edge
			Mm.PinGroupInstance.UpdatePins();
			orig(self);
		}
		private void _GameMap_QuickMapQueensGardens(On.GameMap.orig_QuickMapQueensGardens orig, GameMap self) {
			// Queens_Gardens
			Mm.PinGroupInstance.UpdatePins();
			orig(self);
		}
		private void _GameMap_QuickMapRestingGrounds(On.GameMap.orig_QuickMapRestingGrounds orig, GameMap self) {
			// Resting_Grounds
			Mm.PinGroupInstance.UpdatePins();
			orig(self);
		}
		private void _GameMap_QuickMapWaterways(On.GameMap.orig_QuickMapWaterways orig, GameMap self) {
			// Royal_Waterways
			Mm.PinGroupInstance.UpdatePins();
			orig(self);
		}

		private void _HandleSceneChanges(Scene from, Scene to) {
			if (!IsRando) {
				return;
			}

			if (to.name == "Town") {
				_convoCheck = 0;
				_locked = false;
				PlayMakerFSM elder = FSMUtility.LocateFSM(GameObject.Find("Elderbug"), "npc_control");
				FsmState target = null;
				foreach (FsmState state in elder.FsmStates) {
					if (state.Name == "Convo End") {
						target = state;
						break;
					}
				}

				if (target != null) {
					List<FsmStateAction> actions = target.Actions.ToList();
					actions.Add(new ElderbugIsACoolDude());
					target.Actions = actions.ToArray();
				}
			}
		}

		private string _HandleLanguageGet(string key, string sheetTitle) {
			if (IsRando && _convoCheck < SAFE && !Settings.MapsGiven) {
				if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(sheetTitle)) {
					return string.Empty;
				}

				if (sheetTitle == "Elderbug") {
					DebugLog.Log("Map Check! Safety: " + _convoCheck + ", locked: " + _locked + ", Key: " + key + ", " + Settings.MapsGiven);

					string message = string.Empty;
					if (key == "ELDERBUG_INTRO_VISITEDCROSSROAD") {
						DebugLog.Log("returning original");
						return Language.Language.GetInternal(key, sheetTitle);
					} else if (_convoCheck == 0) {
						string talk = "Welcome to Randomizer Map S! This is a fork of Randomizer Map v0.5.1 with some different features." +
							"\nA big Pin means you can reach the item. A little Pin means you are missing a key item. \"$\" indicates a Shop." +
							"\nTalk to me 2 more times, and I'll give you all the Maps.";
						talk += "<page>Hotkeys:\n" +
							"\"Ctrl + M\" - Gives you all the Maps and Pins.\n" +
							"\"Ctrl + T\" - Toggles between vanilla (non-spoiler) and spoiler item locations\n" +
							"\"Ctrl + R\" - Replace all Pins with question marks";
						talk += "<page>The following controls toggle Pins on/off by the spoiler item pools:\n" +
							"\"Ctrl + P\" - Toggles all the Pins\n" +
							"\"Ctrl + 1\" - Toggles major progression items/skills\n" +
							"\"Ctrl + 2\" - Toggles Mask Shards and Vessel Fragments\n" +
							"\"Ctrl + 3\" - Toggles Charms and Charm Notches\n" +
							"\"Ctrl + 4\" - Toggles Grubs, Essence Roots and Boss Essence\n" +
							"\"Ctrl + 5\" - Toggles Relics, Eggs, Geo Deposits and Boss Geo\n" +
							"\"Ctrl + 6\" - Toggles everything else";
						message = talk;
					} else if (_convoCheck == 1) {
						//Seriously? Trying to cover up Dirtmouth's scandal, are you? Tell you what, I'll tone it down a little bit but come on man; you can't tell me Elder Bug is 100% innocent.
						//  And besides,A) Who is Iselda longingly staring and sighing at all day if not Elder Bug
						//  and B) What else is Elder Bug going to do but "talk to" literally the only resident in town before you arrive
						//  and C) He's called "Elder Bug" because he's obviously the alpha male. ;)
						message = "I frequently *ahem* \"visit\" Cornifer's wife... " +
							"She tells me he lies to travelers to get Geo for an inferior product... " +
							"The jerk. I've taken his completed originals. Maybe once they're bankrupt she'll run off with me." +
							"<page>I'll let you have the Maps if you talk to me 1 more time, since you're new around here.";

					} else if (_convoCheck == 2) {
						string maps = "Okay, hang on";
						System.Random random = new System.Random(RandomizerMod.RandomizerMod.Instance.Settings.Seed);
						for (int i = 0; i < random.Next(3, 10); i++) {
							maps += "...\n...\n...\n...\n";
						}
						maps += "<page>...Here you go! Now, if you'd keep my personal business to yourself, I won't have to get my hands dirty. Hm, interesting how the Pale King died, don't you think...?";

						message = maps;
					} else {
						DebugLog.Error($"Elderbug conversation counter overflow");
					}

					if (_convoCheck < SAFE) {
						_convoCheck++;
					} else {
						if (!_locked) {
							GiveAllMaps("Conversation Stack");

							_locked = true;
						}
					}

					return message;
				} else if (!_locked && sheetTitle == "Titles" && key == "DIRTMOUTH_MAIN") {
					return "FREE MAPS";
				} else if (!_locked && sheetTitle == "Titles" && key == "DIRTMOUTH_SUB") {
					return "Talk to Elderbug";
				}
			}

			return Language.Language.GetInternal(key, sheetTitle);
		}

#endregion

#region Mastercard
		private class ElderbugIsACoolDude : FsmStateAction {

			public override void OnEnter() {
				//_SAFETY++;

				if (_convoCheck >= SAFE & !_locked) {
					GiveAllMaps("FSMAction");

					_locked = true;
				}

				Finish();
			}
		}
#endregion
	}
}
