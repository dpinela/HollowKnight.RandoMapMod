using RandoMapMod.BoringInternals;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandoMapMod {
	[DebugName(nameof(PinGroup))]
	class PinGroup : MonoBehaviour {
		#region Private Non-Methods
		private readonly List<Pin> _pins = new List<Pin>();
		private MapTextOverlay _mapTextOverlay = null;
		private MapTextOverlay _MapTextOverlay {
			get {
				if (_mapTextOverlay == null) _mapTextOverlay = gameObject.GetComponent<MapTextOverlay>();
				return _mapTextOverlay;
			}
		}
		#endregion

		#region Private Methods
		private void _SetPinGroup(GameObject newPin, PinData pinData) {
			switch (pinData.RandoPool) {
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
		//public GameObject MainGroup { get; private set; } = null;
		//public GameObject HelperGroup { get; private set; } = null;
		public GameObject Group1 { get; private set; } = null;
		public GameObject Group2 { get; private set; } = null;
		public GameObject Group3 { get; private set; } = null;
		public GameObject Group4 { get; private set; } = null;
		public GameObject Group5 { get; private set; } = null;
		public GameObject Group6 { get; private set; } = null;
		//public GameObject RandoPoolOn { get; private set; } = null;
		//public GameObject UnknownOn { get; private set; } = null;
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
			this._MapTextOverlay.Hide();
		}

		public void Show(bool force = false) {
			if (force) Hidden = false;

			if (!Hidden) {
				this.gameObject.SetActive(true);
				this._MapTextOverlay.Show();
			}
		}

		public void MakePinGroups () {
			if (Group1 == null) {
				Group1 = new GameObject("Group 1");
				Group1.transform.SetParent(this.transform);
				//default to off
				Group1.SetActive(MapModS.Instance.Settings.Group1On);
			}
			if (Group2 == null) {
				Group2 = new GameObject("Group 2");
				Group2.transform.SetParent(this.transform);
				//default to off
				Group2.SetActive(MapModS.Instance.Settings.Group2On);
			}
			if (Group3 == null) {
				Group3 = new GameObject("Group 3");
				Group3.transform.SetParent(this.transform);
				//default to off
				Group3.SetActive(MapModS.Instance.Settings.Group3On);
			}
			if (Group4 == null) {
				Group4 = new GameObject("Group 4");
				Group4.transform.SetParent(this.transform);
				//default to off
				Group4.SetActive(MapModS.Instance.Settings.Group4On);
			}
			if (Group5 == null) {
				Group5 = new GameObject("Group 5");
				Group5.transform.SetParent(this.transform);
				//default to off
				Group5.SetActive(MapModS.Instance.Settings.Group5On);
			}
			if (Group6 == null) {
				Group6 = new GameObject("Group 6");
				Group6.transform.SetParent(this.transform);
				//default to off
				Group6.SetActive(MapModS.Instance.Settings.Group6On);
			}
		}

		public void SetRandoSprites(bool IsRando) {
			foreach (Pin pin in _pins) {
				pin.SetVanillaRandoSprite(IsRando);
			}
		}
		public void SetUnknownSprites() {
			foreach (Pin pin in _pins) {
				pin.SetUnknownSprite();
			}
		}

		public void AddPinToRoom(PinData pinData, GameMap gameMap) {
			if (_pins.Any(pin => pin.PinData.ID == pinData.ID)) {
				//Already in the list... Probably shouldn't add them.
				DebugLog.Warn($"Duplicate pin found for group: {pinData.ID} - Skipped.");
				return;
			}

			string roomName = pinData.PinScene ?? ResourceHelper.PinDataDictionary[pinData.ID].SceneName;
			//Sprite pinSprite = pinData.IsShop ?
			//	pinSprite = ResourceHelper.FetchSprite(ResourceHelper.Sprites.Shop) :
			//	pinSprite = ResourceHelper.FetchSpriteByPool(pinData.Pool);

			Sprite pinSprite;

			if (pinData.IsShop) {
				pinSprite = ResourceHelper.FetchSprite(ResourceHelper.Sprites.Shop);
			} else {
				pinSprite = ResourceHelper.FetchSpriteByPool(pinData.VanillaPool);
			}

			GameObject newPin = new GameObject("pin_rando");
			//if (pinSprite.name.StartsWith("req")) {
			//	if (HelperGroup == null) {
			//		HelperGroup = new GameObject("Resource Helpers");
			//		HelperGroup.transform.SetParent(this.transform);
			//		//default to off
			//		HelperGroup.SetActive(false);
			//	}

			//	newPin.transform.SetParent(HelperGroup.transform);
			//} else {
			//	if (MainGroup == null) {
			//		MainGroup = new GameObject("Main Group");
			//		MainGroup.transform.SetParent(this.transform);
			//		//default to off
			//		MainGroup.SetActive(false);
			//	}

			//	newPin.transform.SetParent(MainGroup.transform);
			//}

			newPin.layer = 30;
			newPin.transform.localScale *= 1.2f;

			SpriteRenderer sr = newPin.AddComponent<SpriteRenderer>();
			sr.sprite = pinSprite;
			sr.sortingLayerName = "HUD";
			sr.size = new Vector2(1f, 1f);

			Vector3 vec = __GetRoomPos() + pinData.Offset;
			newPin.transform.localPosition = new Vector3(vec.x, vec.y, vec.z - 1f + (vec.y / 100) + (vec.x / 100));

			//Disable to avoid the Pin component's OnEnable before setting the pindata...
			//   Yay Constructorless Components...
			newPin.SetActive(false);

			Pin pinC = newPin.AddComponent<Pin>();
			pinC.SetPinData(pinData);

			//Don't worry about this one. It just does some totally normal non-spoilery things.
			newPin.AddComponent<BoringPinThing>();

			newPin.SetActive(true);

			_pins.Add(pinC);

			_SetPinGroup(newPin, pinData);

			Vector3 __GetRoomPos() {
				//@@OPTIMIZE: Should be indexed or hard-coded but it runs once per game session. Small gain.
				Vector3 pos = new Vector3(-30f, -30f, -0.5f);
				bool exitLoop = false;

				for (int index1 = 0; index1 < gameMap.transform.childCount; ++index1) {
					GameObject areaObj = gameMap.transform.GetChild(index1).gameObject;
					for (int index2 = 0; index2 < areaObj.transform.childCount; ++index2) {
						GameObject roomObj = areaObj.transform.GetChild(index2).gameObject;
						if (roomObj.name == roomName) {
							pos = roomObj.transform.position;
							exitLoop = true;
							break;
						}
					}
					if (exitLoop) {
						break;
					}
				}

				return pos;
			}
		}
		#endregion
	}
}