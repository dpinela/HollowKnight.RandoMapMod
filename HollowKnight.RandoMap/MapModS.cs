using HutongGames.PlayMaker;
using Modding;
using RandoMapMod.UnityComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RandoMapMod {

	public class MapModS : Mod {
		private const float MAP_MAX_X = 17.3f;

		private const float MAP_MAX_Y = 15.6913f;

		private const float MAP_MIN_X = -24.16f;

		private const float MAP_MIN_Y = -12.58548f;

		private const int MAPS_TRIGGER = 3;

		private static int _convoCounter = 0;

		public static MapModS Instance {
			get; private set;
		}

		public override ModSettings SaveSettings {
			get => Settings ??= new SaveSettings();
			set => Settings = value is SaveSettings saveSettings ? saveSettings : Settings;
		}

		public SaveSettings Settings { get; private set; } = new SaveSettings();

		internal static bool IsRando => RandomizerMod.RandomizerMod.Instance.Settings.Randomizer;

		public override string GetVersion() {
			string ver = "1.1.0"; //If you update this, please also update the README.
			int minAPI = 45;

			bool apiTooLow = Convert.ToInt32(ModHooks.Instance.ModVersion.Split('-')[1]) < minAPI;
			if (apiTooLow) {
				return ver + " (Update your API)";
			}

			return ver;
		}

		public override void Initialize() {
			if (Instance != null) {
				Instance.LogWarn("Initialized twice... Stop that.");
				return;
			}
			Instance = this;
			Instance.Log("RandoMapMod Initializing...");

			On.GameMap.Start += this._GameMap_Start;                //Set up custom pins
			On.GameMap.SetupMapMarkers += this._GameMap_SetupMapMarkers;    //Enable the custom pins
			On.GameMap.DisableMarkers += this._GameMap_DisableMarkers;    //Disable the custom pins

			On.GameMap.WorldMap += this._GameMap_WorldMap;
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

			On.GrubPin.OnEnable += this._GrubPin_Enable;
			On.FlamePin.OnEnable += this._FlamePin_Enable;
			On.BrummFlamePin.OnEnable += this._BrummFlamePin_Enable;

			UnityEngine.SceneManagement.SceneManager.activeSceneChanged += _HandleSceneChanges;
			ModHooks.Instance.LanguageGetHook += _HandleLanguageGet;

			Instance.Log("RandoMapMod Initialize complete!");
		}

		private List<GameObject> _objectsToDisable = new List<GameObject>();

		private void _FindObjectsToDisable() {
			_objectsToDisable.Clear();

			// We want to disable these objects every time we open the map
			foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) {
				switch (go.name) {
					case "Pin_Teacher":
					case "Pin_Watcher":
					case "Pin_Beast":
					case "Pin_BlackEgg":
					case "Flame Pins":
					case "Map Markers":
					case "Map Key":
						_objectsToDisable.Add(go);
						break;
				}
			}

			// Find a particular buggy Lifeblood pin
			GameMap gameMap = GameObject.Find("Game_Map(Clone)").GetComponent<GameMap>();
			if (gameMap != null) {
				GameObject lifebloodPin = gameMap.transform.Find("Deepnest")?.Find("Deepnest_26")?.Find("pin_blue_health")?.gameObject;
				if (lifebloodPin != null) {
					_objectsToDisable.Add(lifebloodPin);
				}
			}
		}

		private void _DisableVanillaMapAssets() {
			foreach (GameObject go in _objectsToDisable) {
				go.transform.localPosition = new Vector3(0, 0, 10);
				go.SetActive(false);
			}
		}

		private void _BrummFlamePin_Enable(On.BrummFlamePin.orig_OnEnable orig, BrummFlamePin self) {
			orig(self);
			if (Instance.Settings.MapsGiven) {
				if (self.gameObject.activeSelf) self.gameObject.SetActive(false);
			}
		}

		private void _FlamePin_Enable(On.FlamePin.orig_OnEnable orig, FlamePin self) {
			orig(self);
			if (Instance.Settings.MapsGiven) {
				if (self.gameObject.activeSelf) self.gameObject.SetActive(false);
			}
		}

		private void _GrubPin_Enable(On.GrubPin.orig_OnEnable orig, GrubPin self) {
			orig(self);
			if (Instance.Settings.MapsGiven) {
				if (self.gameObject.activeSelf) self.gameObject.SetActive(false);
			}
		}

		private void _GameMap_SetupMapMarkers(On.GameMap.orig_SetupMapMarkers orig, GameMap self) {
			orig(self);

			if (!IsRando) {
				return;
			}

			this.PinGroupInstance.Show();
			ForceMapUpdate();
			Instance.PinGroupInstance.SetPinGroups();
		}

		private void _GameMap_DisableMarkers(On.GameMap.orig_DisableMarkers orig, GameMap self) {
			if (!IsRando) {
				orig(self);
				return;
			}
			try {
				this.PinGroupInstance.Hide();
			} catch (Exception e) {
				Instance.Log($"Failed to DisableMarkers {e.Message}");
			}
			orig(self);
		}

		internal GameObject PinGroupGO = null;
		internal PinGroup PinGroupInstance => PinGroupGO?.GetComponent<PinGroup>();

		// Everything for initializing stuff *per game load* is here
		private void _GameMap_Start(On.GameMap.orig_Start orig, GameMap self) {
			Instance.Log("GameMap_Start");
			if (!IsRando) {
				orig(self);
				return;
			}

			try {
				Instance.Log("Emptying out HelperData on game start.");

				_FindObjectsToDisable();

				_DisableVanillaMapAssets();

				//GetAllMapNames();

				// This will refresh the spoiler pools if a different game is loaded
				ResourceLoader.FindSpoilerPools();

				// Create the custom pin group, and add all the new pins
				// The pins are preserved between loading different games
				// ie. one PinGroup every time Hollow Knight is opened
				if (this.PinGroupGO == null) {
					Instance.Log("First Setup. Adding Pin Group and Populating...");
					this.PinGroupGO = new GameObject($"Map Mod Pin Group");
					this.PinGroupGO.AddComponent<PinGroup>();
					this.PinGroupGO.transform.SetParent(self.transform);

					foreach (KeyValuePair<string, PinData> entry in ResourceLoader.PinDataDictionary) {
						string itemName = entry.Key;
						PinData pin = entry.Value;

						if (!pin.NotPin) {
							this.PinGroupInstance.AddPinToRoom(pin, self);
						}
					}
				}

				PinGroupInstance.InitializePinGroups();
				PinGroupInstance.SetSprites();
				PinGroupInstance.FindRandomizedGroups();

				// Set up UI and hotkeys
				InputListener.InstantiateSingleton();

				if (Instance.Settings.MapsGiven) {
					GUIController.Setup();
					GUIController.Instance.BuildMenus();
				}
			} catch (Exception e) {
				Instance.LogError($"Error: {e}");
			}

			Instance.Log("Finished.");
			orig(self);
		}

		private void _GameMap_WorldMap(On.GameMap.orig_WorldMap orig, GameMap self) {
			orig(self);

			if (!IsRando) {
				return;
			}

			_DisableVanillaMapAssets();

			// Check reachable, item picked up, etc.
			Instance.PinGroupInstance.SetPinStates("WorldMap");

			// Set the maximum scroll boundaries, so we can scroll the entire map, even if we don't have the maps unlocked.
			if (self.panMinX > MAP_MIN_X)
				self.panMinX = MAP_MIN_X;
			if (self.panMaxX < MAP_MAX_X)
				self.panMaxX = MAP_MAX_X;
			if (self.panMinY > MAP_MIN_Y)
				self.panMinY = MAP_MIN_Y;
			if (self.panMaxY < MAP_MAX_Y)
				self.panMaxY = MAP_MAX_Y;
		}

		private void _GameMap_QuickMapAncientBasin(On.GameMap.orig_QuickMapAncientBasin orig, GameMap self) {
			orig(self);
			Instance.PinGroupInstance.SetPinStates("Ancient_Basin");
			_DisableVanillaMapAssets();
		}

		private void _GameMap_QuickMapCity(On.GameMap.orig_QuickMapCity orig, GameMap self) {
			orig(self);
			Instance.PinGroupInstance.SetPinStates("City_of_Tears");
			_DisableVanillaMapAssets();
		}

		private void _GameMap_QuickMapCliffs(On.GameMap.orig_QuickMapCliffs orig, GameMap self) {
			orig(self);
			Instance.PinGroupInstance.SetPinStates("Howling_Cliffs");
			_DisableVanillaMapAssets();
		}

		private void _GameMap_QuickMapCrossroads(On.GameMap.orig_QuickMapCrossroads orig, GameMap self) {
			orig(self);
			Instance.PinGroupInstance.SetPinStates("Forgotten_Crossroads");
			_DisableVanillaMapAssets();
		}

		private void _GameMap_QuickMapCrystalPeak(On.GameMap.orig_QuickMapCrystalPeak orig, GameMap self) {
			orig(self);
			Instance.PinGroupInstance.SetPinStates("Crystal_Peak");
			_DisableVanillaMapAssets();
		}

		private void _GameMap_QuickMapDeepnest(On.GameMap.orig_QuickMapDeepnest orig, GameMap self) {
			orig(self);
			Instance.PinGroupInstance.SetPinStates("Deepnest");
			_DisableVanillaMapAssets();
		}

		private void _GameMap_QuickMapDirtmouth(On.GameMap.orig_QuickMapDirtmouth orig, GameMap self) {
			orig(self);
			Instance.PinGroupInstance.SetPinStates("Dirtmouth");
			_DisableVanillaMapAssets();
		}

		private void _GameMap_QuickMapFogCanyon(On.GameMap.orig_QuickMapFogCanyon orig, GameMap self) {
			orig(self);
			Instance.PinGroupInstance.SetPinStates("Fog_Canyon");
			_DisableVanillaMapAssets();
		}

		private void _GameMap_QuickMapFungalWastes(On.GameMap.orig_QuickMapFungalWastes orig, GameMap self) {
			orig(self);
			Instance.PinGroupInstance.SetPinStates("Fungal_Wastes");
			_DisableVanillaMapAssets();
		}

		private void _GameMap_QuickMapGreenpath(On.GameMap.orig_QuickMapGreenpath orig, GameMap self) {
			orig(self);
			Instance.PinGroupInstance.SetPinStates("Greenpath");
			_DisableVanillaMapAssets();
		}

		private void _GameMap_QuickMapKingdomsEdge(On.GameMap.orig_QuickMapKingdomsEdge orig, GameMap self) {
			orig(self);
			Instance.PinGroupInstance.SetPinStates("Kingdoms_Edge");
			_DisableVanillaMapAssets();
		}

		private void _GameMap_QuickMapQueensGardens(On.GameMap.orig_QuickMapQueensGardens orig, GameMap self) {
			orig(self);
			Instance.PinGroupInstance.SetPinStates("Queens_Gardens");
			_DisableVanillaMapAssets();
		}

		private void _GameMap_QuickMapRestingGrounds(On.GameMap.orig_QuickMapRestingGrounds orig, GameMap self) {
			orig(self);
			Instance.PinGroupInstance.SetPinStates("Resting_Grounds");
			_DisableVanillaMapAssets();
		}

		private void _GameMap_QuickMapWaterways(On.GameMap.orig_QuickMapWaterways orig, GameMap self) {
			orig(self);
			Instance.PinGroupInstance.SetPinStates("Royal_Waterways");
			_DisableVanillaMapAssets();
		}

		// This needs to be done once every game load when you don't have Quill, otherwise the map will be broken
		internal static void ForceMapUpdate() {
			PlayerData pd = PlayerData.instance;

			if (!pd.hasQuill) {
				try {
					// Give Quill, because it's required to...
					pd.SetBool(nameof(pd.hasQuill), true);

					// ... uncover the full map
					GameMap gameMap = GameObject.Find("Game_Map(Clone)").GetComponent<GameMap>();
					gameMap.SetupMap();

					// Remove Quill
					pd.SetBool(nameof(pd.hasQuill), false);
				} catch (Exception e) {
					Instance.LogError($"Map object not found: {e}");
				}
			}
		}

		internal static void GiveAllMaps(string from) {
			if (!Instance.Settings.MapsGiven) {
				Instance.Log($"Maps granted from {from}");

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

				Instance.Settings.MapsGiven = true;

				GUIController.Setup();
				GUIController.Instance.BuildMenus();

				GameManager.instance.SaveGame();
			}
		}

		private string _HandleLanguageGet(string key, string sheetTitle) {
			if (IsRando && !Settings.MapsGiven) {
				if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(sheetTitle)) {
					return string.Empty;
				}

				if (sheetTitle == "Elderbug") {
					string message = string.Empty;

					if (key == "ELDERBUG_INTRO_VISITEDCROSSROAD") {
						return Language.Language.GetInternal(key, sheetTitle);
					} else if (_convoCounter == 0) {
						string talk = "Welcome to Randomizer Map S!" +
							" Talk to me two more times, and I'll give you all the Maps. Once enabled, you can use the" +
							" UI in the Pause Menu to adjust the Map Pins to your liking.\n" +
							"<page>A big Pin means you can reach the location. A little Pin means you are missing a key item to be able to reach there.";
						talk += "<page>You can also use the hotkey CTRL-M to get all the Maps.";
						message = talk;
						_convoCounter++;
					} else if (_convoCounter == 1) {
						//Seriously? Trying to cover up Dirtmouth's scandal, are you? Tell you what, I'll tone it down a little bit but come on man; you can't tell me Elder Bug is 100% innocent.
						//  And besides,A) Who is Iselda longingly staring and sighing at all day if not Elder Bug
						//  and B) What else is Elder Bug going to do but "talk to" literally the only resident in town before you arrive
						//  and C) He's called "Elder Bug" because he's obviously the alpha male. ;)
						message = "I frequently *ahem* \"visit\" Cornifer's wife... " +
							"She tells me he lies to travelers to get Geo for an inferior product... " +
							"The jerk. I've taken his completed originals. Maybe once they're bankrupt she'll run off with me." +
							"<page>I'll let you have the Maps if you talk to me again, since you're new around here.";
						_convoCounter++;
					} else if (_convoCounter == 2) {
						string maps = "Okay, hang on";
						System.Random random = new System.Random(RandomizerMod.RandomizerMod.Instance.Settings.Seed);
						for (int i = 0; i < random.Next(3, 10); i++) {
							maps += "...\n...\n...\n...\n";
						}
						maps += "<page>...Here you go! Now, if you'd keep my personal business to yourself, I won't have to get my hands dirty. Hm, interesting how the Pale King died, don't you think...?";

						message = maps;
						_convoCounter++;
					}

					if (_convoCounter >= MAPS_TRIGGER) {
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

					return message;
				} else if (sheetTitle == "Titles" && key == "DIRTMOUTH_MAIN") {
					return "FREE MAPS";
				} else if (sheetTitle == "Titles" && key == "DIRTMOUTH_SUB") {
					return "Talk to Elderbug";
				}
			}

			return Language.Language.GetInternal(key, sheetTitle);
		}

		private void _HandleSceneChanges(Scene from, Scene to) {
			if (!IsRando) {
				return;
			}

			if (to.name == "Town") {
				_convoCounter = 0;
			}

			if (to.name == "Quit_To_Menu") {
				InputListener.DestroySingleton();
				GUIController.Unload();
			}
		}

		private class ElderbugIsACoolDude : FsmStateAction {

			public override void OnEnter() {
				if (_convoCounter >= MAPS_TRIGGER & !Instance.Settings.MapsGiven) {
					GiveAllMaps("FSMAction");
				}

				Finish();
			}
		}
	}

	//internal static void ReloadGameMapPins() {
	//	try {
	//		GameMap gameMap = GameObject.Find("Game_Map(Clone)").GetComponent<GameMap>();
	//		// Change PinDataDictionary during run-time! Used for editing pin positions while the game is running
	//		ResourceLoader.ReloadPinData();
	//		Instance.PinGroupInstance.RefreshPins(gameMap);

	//	} catch (Exception e) {
	//		Instance.LogError(e);
	//	}
	//}

	//internal static void GetAllMapNames() {
	//	GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
	//	foreach (GameObject go in allObjects) {
	//		if (go.name == "Inventory") {
	//			foreach (Transform invObj in go.transform) {
	//				Instance.Log($"- {invObj.gameObject.name}");
	//				foreach (Transform invObj1 in invObj.transform) {
	//					Instance.Log($"- - {invObj1.gameObject.name}");
	//					if (invObj1.gameObject.name == "World Map") {
	//						foreach (Transform invObj2 in invObj1.transform) {
	//							Instance.Log($"- - - {invObj2.gameObject.name}");
	//							foreach (Transform invObj3 in invObj2.transform) {
	//								Instance.Log($"- - - - {invObj3.gameObject.name}");
	//							}
	//						}
	//					}
	//				}
	//			}
	//		}
	//	}
	//}

	//GameMap gameMap = GameObject.Find("Game_Map(Clone)").GetComponent<GameMap>();
	//foreach (Transform areaObj in gameMap.transform) {
	//	DebugLog.Log($"{areaObj.gameObject.name}");
	//	foreach (Transform roomObj in areaObj.transform) {
	//		DebugLog.Log($"- {roomObj.gameObject.name}");
	//	}
	//}
	//}

	//	internal static void GetAllActiveObjects() {
	//	GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
	//	foreach (GameObject go in allObjects) {
	//		if (go.activeInHierarchy) {
	//			Instance.Log(go.name + " is an active object");
	//		}
	//	}
	//}
}