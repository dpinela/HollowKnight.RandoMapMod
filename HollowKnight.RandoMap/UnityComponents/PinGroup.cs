//using RandoMapMod.BoringInternals;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandoMapMod {
	[DebugName(nameof(PinGroup))]
	public class PinGroup : MonoBehaviour {
		#region Private Non-Methods
		private readonly List<Pin> _pins = new List<Pin>();
		#endregion

		#region Private Methods
		private void _SetPinGroup(GameObject newPin, PinData pinData) {
			switch (pinData.SpoilerPool) {
				case "Skill":
				case "Dreamer":
				case "Cursed":
				case "Spell":
				case "SplitCloak":
				case "SplitClaw":
				case "SplitCloakLocation":
				case "CursedNail":
				case "Ore":
				case "Key":
					newPin.transform.SetParent(Group1.transform);
					break;
				case "Mask":
				case "Vessel":
					newPin.transform.SetParent(Group2.transform);
					break;
				case "Charm":
				case "Notch":
					newPin.transform.SetParent(Group3.transform);
					break;
				case "Grub":
				case "Root":
				case "Essence_Boss":
					newPin.transform.SetParent(Group4.transform);
					break;
				case "Relic":
				case "Egg":
				case "Geo":
				case "Rock":
				case "Boss_Geo":
					newPin.transform.SetParent(Group5.transform);
					break;
				default:
					newPin.transform.SetParent(Group6.transform);
					break;
			}
		}
		#endregion

		#region Non-Private Non-Methods
		public GameObject Group1 { get; private set; } = null;
		public GameObject Group2 { get; private set; } = null;
		public GameObject Group3 { get; private set; } = null;
		public GameObject Group4 { get; private set; } = null;
		public GameObject Group5 { get; private set; } = null;
		public GameObject Group6 { get; private set; } = null;
		public bool Hidden { get; private set; } = false;
		#endregion

		#region MonoBehaviour "Overrides"
		protected void Start() {
			this.Hide();
		}
		#endregion

		#region Non-Private Methods
		public void Hide(bool force = false) {
			if (force) Hidden = true;

			this.gameObject.SetActive(false);
		}

		public void Show(bool force = false) {
			if (force) Hidden = false;

			if (!Hidden) {
				this.gameObject.SetActive(true);
			}
		}

		public void MakePinGroups () {
			if (Group1 == null) {
				Group1 = new GameObject("Group 1");
				Group1.transform.SetParent(this.transform);
				Group1.SetActive(MapModS.Mm.Settings.Group1On);
			}
			if (Group2 == null) {
				Group2 = new GameObject("Group 2");
				Group2.transform.SetParent(this.transform);
				Group2.SetActive(MapModS.Mm.Settings.Group2On);
			}
			if (Group3 == null) {
				Group3 = new GameObject("Group 3");
				Group3.transform.SetParent(this.transform);
				Group3.SetActive(MapModS.Mm.Settings.Group3On);
			}
			if (Group4 == null) {
				Group4 = new GameObject("Group 4");
				Group4.transform.SetParent(this.transform);
				Group4.SetActive(MapModS.Mm.Settings.Group4On);
			}
			if (Group5 == null) {
				Group5 = new GameObject("Group 5");
				Group5.transform.SetParent(this.transform);
				Group5.SetActive(MapModS.Mm.Settings.Group5On);
			}
			if (Group6 == null) {
				Group6 = new GameObject("Group 6");
				Group6.transform.SetParent(this.transform);
				Group6.SetActive(MapModS.Mm.Settings.Group6On);
			}
		}

		public void SetSpoilerSprites(bool IsSpoiler) {
			foreach (Pin pin in _pins) {
				if (IsSpoiler) {
					pin.SetSprite("Spoiler");
				} else {
					pin.SetSprite("Vanilla");
				}
				
			}
		}
		public void SetUnknownSprites() {
			foreach (Pin pin in _pins) {
				pin.SetSprite("Unknown");
			}
		}

		public void UpdatePins() {
			foreach (Pin pin in _pins) {
				pin.UpdateState();
			}
		}

		public void AddPinToRoom(PinData pinData, GameMap gameMap) {
			if (_pins.Any(pin => pin.PinData.ID == pinData.ID)) {
				//Already in the list... Probably shouldn't add them.
				DebugLog.Warn($"Duplicate pin found for group: {pinData.ID} - Skipped.");
				return;
			}

			string roomName = pinData.PinScene ?? ResourceHelper.PinDataDictionary[pinData.ID].SceneName;

			Sprite pinSprite;

			if (pinData.IsShop) {
				pinSprite = ResourceHelper.FetchSprite(ResourceHelper.Sprites.Shop);
			} else {
				pinSprite = ResourceHelper.FetchSpriteByPool(pinData.VanillaPool);
			}

			GameObject newPin = new GameObject($"pin_rando_{pinData.ID}");
			newPin.layer = 30;
			newPin.transform.localScale *= 1.2f;

			SpriteRenderer sr = newPin.AddComponent<SpriteRenderer>();
			sr.sprite = pinSprite;
			sr.sortingLayerName = "HUD";
			sr.size = new Vector2(1f, 1f);

			Vector3 vec = _GetRoomPos(roomName, gameMap);
			if (vec == new Vector3(0,0,0)) {
				DebugLog.Warn($"{pinData.ID} doesn't have a valid room name!");
			}
			vec.Scale(new Vector3(1.46f, 1.46f, 1));
			vec += ResourceHelper.PinDataDictionary[pinData.ID].Offset;

			newPin.transform.localPosition = new Vector3(vec.x, vec.y, vec.z - 1f);

			//Disable to avoid the Pin component's OnEnable before setting the pindata
			newPin.SetActive(false);

			Pin pinC = newPin.AddComponent<Pin>();

			pinC.SetPinData(pinData);

			newPin.SetActive(true);

			_pins.Add(pinC);

			_SetPinGroup(newPin, pinData);
		}
		private Vector3 _GetRoomPos(string roomName, GameMap gameMap) {
			// Should be indexed or hard-coded but it runs once per game session. Small gain.

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

#if DEBUG
		public void RefreshPins(GameMap gameMap) {
			foreach (KeyValuePair<string, PinData> entry in ResourceHelper.PinDataDictionary) {
				if (GameObject.Find($"pin_rando_{entry.Key}")) {
					Pin pin = GameObject.Find($"pin_rando_{entry.Key}").GetComponent<Pin>();
					try {
						string roomName = pin.PinData.PinScene ?? pin.PinData.SceneName;
						Vector3 vec = _GetRoomPos(roomName, gameMap);
						vec.Scale(new Vector3(1.46f, 1.46f, 1));
						vec += entry.Value.Offset;

						pin.transform.localPosition = new Vector3(vec.x, vec.y, vec.z - 5f);
					} catch (Exception e) {
						DebugLog.Error($"Error: UpdatePins {e}");
					}
				}
			}
		}
#endif
#endregion
	}
}