//using RandoMapMod.BoringInternals;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandoMapMod {

	public class PinGroup : MonoBehaviour {

		public enum GroupName {
			Dreamer,
			Skill,
			Charm,
			Key,
			Geo,
			Junk,
			Mask,
			Vessel,
			Notch,
			Ore,
			Egg,
			Relic,
			Map,
			Stag,
			Grub,
			Mimic,
			Root,
			Rock,
			BossGeo,
			Soul,
			Lore,
			PalaceSoul,
			PalaceLore,
			PalaceJournal,
			Cocoon,
			Flame,
			EssenceBoss,
			Journal,
			Shop,
		}

		public enum PinStyles {
			Normal,
			Q_Marks,
			Old_1,
			Old_2,
		}

		#region Private Non-Methods

		private readonly List<Pin> _pins = new List<Pin>();

		#endregion Private Non-Methods

		#region Private Methods

		// Set each Pin to the correct Parent Group
		private void _SetPinGroup(GameObject newPin, PinData pinData) {
			switch (pinData.SpoilerPool) {
				case "Dreamer":
					newPin.transform.SetParent(GroupDictionary[GroupName.Dreamer].transform);
					break;

				case "Skill":
				case "Focus":
				case "Swim":
				case "SplitCloak":
				case "SplitClaw":
				case "SplitCloakLocation":
				case "CursedNail":
					newPin.transform.SetParent(GroupDictionary[GroupName.Skill].transform);
					break;

				case "Charm":
					newPin.transform.SetParent(GroupDictionary[GroupName.Charm].transform);
					break;

				case "Key":
				case "ElevatorPass":
					newPin.transform.SetParent(GroupDictionary[GroupName.Key].transform);
					break;

				case "Geo":
				case "EggShopLocation":
					newPin.transform.SetParent(GroupDictionary[GroupName.Geo].transform);
					break;

				case "JunkPitChest":
					newPin.transform.SetParent(GroupDictionary[GroupName.Junk].transform);
					break;

				case "CursedMask":
				case "Mask":
					newPin.transform.SetParent(GroupDictionary[GroupName.Mask].transform);
					break;

				case "Vessel":
					newPin.transform.SetParent(GroupDictionary[GroupName.Vessel].transform);
					break;

				case "Notch":
				case "CursedNotch":
					newPin.transform.SetParent(GroupDictionary[GroupName.Notch].transform);
					break;

				case "Ore":
					newPin.transform.SetParent(GroupDictionary[GroupName.Ore].transform);
					break;

				case "Egg":
				case "EggShopItem":
					newPin.transform.SetParent(GroupDictionary[GroupName.Egg].transform);
					break;

				case "Relic":
					newPin.transform.SetParent(GroupDictionary[GroupName.Relic].transform);
					break;

				case "Map":
					newPin.transform.SetParent(GroupDictionary[GroupName.Map].transform);
					break;

				case "Stag":
					newPin.transform.SetParent(GroupDictionary[GroupName.Stag].transform);
					break;

				case "Grub":
				case "GrubItem":
					newPin.transform.SetParent(GroupDictionary[GroupName.Grub].transform);
					break;

				case "Mimic":
				case "MimicItem":
					newPin.transform.SetParent(GroupDictionary[GroupName.Mimic].transform);
					break;

				case "Root":
					newPin.transform.SetParent(GroupDictionary[GroupName.Root].transform);
					break;

				case "Rock":
					newPin.transform.SetParent(GroupDictionary[GroupName.Rock].transform);
					break;

				case "Boss_Geo":
					newPin.transform.SetParent(GroupDictionary[GroupName.BossGeo].transform);
					break;

				case "Soul":
					newPin.transform.SetParent(GroupDictionary[GroupName.Soul].transform);
					break;

				case "Lore":
					newPin.transform.SetParent(GroupDictionary[GroupName.Lore].transform);
					break;

				case "PalaceSoul":
					newPin.transform.SetParent(GroupDictionary[GroupName.PalaceSoul].transform);
					break;

				case "PalaceLore":
					newPin.transform.SetParent(GroupDictionary[GroupName.PalaceLore].transform);
					break;

				case "PalaceJournal":
					newPin.transform.SetParent(GroupDictionary[GroupName.PalaceJournal].transform);
					break;

				case "Cocoon":
					newPin.transform.SetParent(GroupDictionary[GroupName.Cocoon].transform);
					break;

				case "Flame":
					newPin.transform.SetParent(GroupDictionary[GroupName.Flame].transform);
					break;

				case "Essence_Boss":
					newPin.transform.SetParent(GroupDictionary[GroupName.EssenceBoss].transform);
					break;

				case "Journal":
					newPin.transform.SetParent(GroupDictionary[GroupName.Journal].transform);
					break;

				case "Shop":
					newPin.transform.SetParent(GroupDictionary[GroupName.Shop].transform);
					break;

				default:
					MapModS.Instance.LogWarn($"Unrecognized item pool: {pinData.SpoilerPool}");
					break;
			}
		}

		#endregion Private Methods

		#region Non-Private Non-Methods

		public Dictionary<GroupName, GameObject> GroupDictionary = new Dictionary<GroupName, GameObject> {
			{GroupName.Dreamer, new GameObject("Group Dreamer") },
			{GroupName.Skill, new GameObject("Group Skill") },
			{GroupName.Charm, new GameObject("Group Charm") },
			{GroupName.Key, new GameObject("Group Key") },
			{GroupName.Geo, new GameObject("Group Geo") },
			{GroupName.Junk, new GameObject("Group Junk") },
			{GroupName.Mask, new GameObject("Group Mask") },
			{GroupName.Vessel, new GameObject("Group Vessel") },
			{GroupName.Notch, new GameObject("Group Notch") },
			{GroupName.Ore, new GameObject("Group Ore") },
			{GroupName.Egg, new GameObject("Group Egg") },
			{GroupName.Relic, new GameObject("Group Relic") },
			{GroupName.Map, new GameObject("Group Map") },
			{GroupName.Stag, new GameObject("Group Stag") },
			{GroupName.Grub, new GameObject("Group Grub") },
			{GroupName.Mimic, new GameObject("Group Mimic") },
			{GroupName.Root, new GameObject("Group Root") },
			{GroupName.Rock, new GameObject("Group Rock") },
			{GroupName.BossGeo, new GameObject("Group BossGeo") },
			{GroupName.Soul, new GameObject("Group Soul") },
			{GroupName.Lore, new GameObject("Group Lore") },
			{GroupName.PalaceSoul, new GameObject("Group PalaceSoul") },
			{GroupName.PalaceLore, new GameObject("Group PalaceLore") },
			{GroupName.PalaceJournal, new GameObject("Group PalaceJournal") },
			{GroupName.Cocoon, new GameObject("Group Cocoon") },
			{GroupName.Flame, new GameObject("Group Flame") },
			{GroupName.EssenceBoss, new GameObject("Group EssenceBoss") },
			{GroupName.Journal, new GameObject("Group Journal") },
			{GroupName.Shop, new GameObject("Group Shop") },
		};

		// Used for updating button states
		public List<GroupName> RandomizedGroups = new List<GroupName>();

		public List<GroupName> UnrandomizedGroups = new List<GroupName>();

		public bool Hidden { get; private set; } = false;

		#endregion Non-Private Non-Methods

		#region MonoBehaviour "Overrides"

		protected void Start() {
			this.Hide();
		}

		#endregion MonoBehaviour "Overrides"

		#region Non-Private Methods

		public void AddPinToRoom(PinData pinData, GameMap gameMap) {
			if (_pins.Any(pin => pin.PinData.ID == pinData.ID)) {
				MapModS.Instance.LogWarn($"Duplicate pin found for group: {pinData.ID} - Skipped.");
				return;
			}

			// Get room name from pindata's overwrite, otherwise from RandomizerMod
			string roomName = pinData.PinScene ?? ResourceLoader.PinDataDictionary[pinData.ID].SceneName;

			Sprite pinSprite;

			pinSprite = ResourceLoader.GetSpriteFromPool(pinData.VanillaPool);

			GameObject pinObject = new GameObject($"pin_rando_{pinData.ID}") {
				layer = 30
			};

			pinObject.transform.localScale *= 1.2f;

			SpriteRenderer sr = pinObject.AddComponent<SpriteRenderer>();
			sr.sprite = pinSprite;
			sr.sortingLayerName = "HUD";
			sr.size = new Vector2(1f, 1f);

			Vector3 vec = _GetRoomPos(roomName, gameMap);

			if (vec == new Vector3(0, 0, 0)) {
				MapModS.Instance.LogWarn($"{pinData.ID} doesn't have a valid room name!");
			}

			vec.Scale(new Vector3(1.46f, 1.46f, 1));
			vec += ResourceLoader.PinDataDictionary[pinData.ID].Offset;

			pinObject.transform.localPosition = new Vector3(vec.x, vec.y, vec.z - 1f);

			Pin pinComponent = pinObject.AddComponent<Pin>();

			pinComponent.SetPinData(pinData);

			_pins.Add(pinComponent);

			_SetPinGroup(pinObject, pinData);
		}

		public void DestroyPins() {
			foreach (Pin pin in _pins) {
				Destroy(pin.gameObject);
			}
			_pins.Clear();
		}

		public void Show(bool force = false) {
			if (force) Hidden = false;

			if (!Hidden) {
				this.gameObject.SetActive(true);
			}
		}

		public void Hide(bool force = false) {
			if (force) Hidden = true;
			this.gameObject.SetActive(false);
		}

		public void SetPinStates(string mapName) {
			foreach (Pin pin in _pins) {
				pin.SetPinState(mapName);
			}
		}

		public void SetSprites() {
			foreach (Pin pin in _pins) {
				if (MapModS.Instance.Settings.SpoilerOn) {
					pin.SetSprite(ResourceLoader.GetSpriteFromPool(pin.PinData.SpoilerPool));
				} else {
					pin.SetSprite(ResourceLoader.GetSpriteFromPool(pin.PinData.VanillaPool));
				}
			}
		}

		internal void FindRandomizedGroups() {
			foreach (GroupName group in Enum.GetValues(typeof(GroupName))) {
				if (group == GroupName.Shop) {
					continue;
				} else if (Dictionaries.GetRandomizerSetting(group)) {
					RandomizedGroups.Add(group);
				} else {
					UnrandomizedGroups.Add(group);
				}
			}
		}

		internal void InitializePinGroups() {
			foreach (KeyValuePair<GroupName, GameObject> entry in GroupDictionary) {
				entry.Value.transform.SetParent(this.transform);
				entry.Value.SetActive(false);
			}
		}

		internal void SetPinGroups() {
			foreach (KeyValuePair<GroupName, GameObject> entry in GroupDictionary) {
				entry.Value.SetActive(MapModS.Instance.Settings.GetBoolFromGroup(entry.Key));
			}
		}

		internal void ToggleGroup(GroupName group) {
			MapModS.Instance.Settings.SetBoolFromGroup(group, !MapModS.Instance.Settings.GetBoolFromGroup(group));
			GroupDictionary[group].SetActive(MapModS.Instance.Settings.GetBoolFromGroup(group));
			PauseGUI.SetButtons();
		}

		internal void ToggleSpoilers() {
			MapModS.Instance.Settings.SpoilerOn = !MapModS.Instance.Settings.SpoilerOn;
			SetSprites();
			PauseGUI.SetButtons();
		}

		internal void TogglePinStyle() {
			switch (MapModS.Instance.Settings.PinStyle) {
				case PinStyles.Normal:
					MapModS.Instance.Settings.PinStyle = PinStyles.Q_Marks;
					break;

				case PinStyles.Q_Marks:
					MapModS.Instance.Settings.PinStyle = PinStyles.Old_1;
					break;

				case PinStyles.Old_1:
					MapModS.Instance.Settings.PinStyle = PinStyles.Old_2;
					break;

				case PinStyles.Old_2:
					MapModS.Instance.Settings.PinStyle = PinStyles.Normal;
					break;
			}

			SetSprites();
			PauseGUI.SetButtons();
		}

		internal void ToggleRandomized() {
			MapModS.Instance.Settings.RandomizedOn = !MapModS.Instance.Settings.RandomizedOn;
			foreach (GroupName group in RandomizedGroups) {
				MapModS.Instance.Settings.SetBoolFromGroup(group, MapModS.Instance.Settings.RandomizedOn);
				GroupDictionary[group].SetActive(MapModS.Instance.Settings.RandomizedOn);
			}
			PauseGUI.SetButtons();
		}

		internal void ToggleUnrandomized() {
			MapModS.Instance.Settings.UnrandomizedOn = !MapModS.Instance.Settings.UnrandomizedOn;
			foreach (GroupName group in UnrandomizedGroups) {
				MapModS.Instance.Settings.SetBoolFromGroup(group, MapModS.Instance.Settings.UnrandomizedOn);
				GroupDictionary[group].SetActive(MapModS.Instance.Settings.UnrandomizedOn);
			}
			PauseGUI.SetButtons();
		}

		private Vector3 _GetRoomPos(string roomName, GameMap gameMap) {
			foreach (Transform areaObj in gameMap.transform) {
				foreach (Transform roomObj in areaObj.transform) {
					if (roomObj.gameObject.name == roomName) {
						Vector3 roomVec = roomObj.transform.localPosition;
						roomVec.Scale(areaObj.transform.localScale);
						return areaObj.transform.localPosition + roomVec;
					}
				}
			}
			return new Vector3(0, 0, 0);
		}

		//public void RefreshPins(GameMap gameMap) {
		//	foreach (KeyValuePair<string, PinData> entry in ResourceLoader.PinDataDictionary) {
		//		if (GameObject.Find($"pin_rando_{entry.Key}")) {
		//			Pin pin = GameObject.Find($"pin_rando_{entry.Key}").GetComponent<Pin>();
		//			try {
		//				string roomName = pin.PinData.PinScene ?? pin.PinData.SceneName;
		//				Vector3 vec = _GetRoomPos(roomName, gameMap);
		//				vec.Scale(new Vector3(1.46f, 1.46f, 1));
		//				vec += entry.Value.Offset;

		//				pin.transform.localPosition = new Vector3(vec.x, vec.y, vec.z - 5f);
		//			} catch (Exception e) {
		//				MapModS.Instance.LogError($"Error: RefeshPins {e}");
		//			}
		//		}
		//	}
		//}

		#endregion Non-Private Methods
	}
}