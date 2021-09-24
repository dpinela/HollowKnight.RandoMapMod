using RandoMapMod.CanvasUtil;
using System.Linq;
using UnityEngine;

namespace RandoMapMod {

	internal class MapText {
		public static GameObject Canvas;

		private static CanvasPanel _mapDisplayPanel;

		private static string _textSpoilers = "ERROR";
		private static string _textStyle = "ERROR";
		private static string _textRandomized = "ERROR";
		private static string _textOthers = "ERROR";

		public static void BuildText(GameObject _canvas) {
			Canvas = _canvas;
			_mapDisplayPanel = new CanvasPanel
				(_canvas, GUIController.Instance.Images["ButtonsMenuBG"], new Vector2(10f, 1040f), new Vector2(1346f, 0f), new Rect(0f, 0f, 0f, 0f));
			_mapDisplayPanel.AddText("Spoilers (ctrl-1): ", _textSpoilers, new Vector2(410f, 0f), Vector2.zero, GUIController.Instance.TrajanNormal, 16);
			_mapDisplayPanel.AddText("Style (ctrl-2): ", _textStyle, new Vector2(710f, 0f), Vector2.zero, GUIController.Instance.TrajanNormal, 16);
			_mapDisplayPanel.AddText("Randomized (ctrl-3): ", _textRandomized, new Vector2(1010f, 0f), Vector2.zero, GUIController.Instance.TrajanNormal, 16);
			_mapDisplayPanel.AddText("Others (ctrl-4): ", _textOthers, new Vector2(1310f, 0f), Vector2.zero, GUIController.Instance.TrajanNormal, 16);
			SetTexts();
		}

		public static void RebuildText() {
			_mapDisplayPanel.Destroy();
			BuildText(Canvas);
			_mapDisplayPanel.SetActive(false, true); // collapse all subpanels
			_mapDisplayPanel.SetActive(true, false);
		}

		public static void Update() {
			if (_mapDisplayPanel == null || GameManager.instance == null
				|| !RandomizerMod.RandomizerMod.Instance.Settings.Randomizer
				|| !MapModS.Instance.Settings.MapsGiven) {
				return;
			}

			if (HeroController.instance == null || !GameManager.instance.IsGameplayScene() || GameManager.instance.IsGamePaused()) {
				if (_mapDisplayPanel.Active) _mapDisplayPanel.SetActive(false, true);
				return;
			} else {
				if (!_mapDisplayPanel.Active) {
					RebuildText();
				}
			}
		}

		private static void _SetSpoilers() {
			_mapDisplayPanel.GetText("Spoilers (ctrl-1): ").UpdateText
				(
					MapModS.Instance.Settings.SpoilerOn ? "Spoilers (ctrl-1): on" : "Spoilers (ctrl-1): off"
				);
			_mapDisplayPanel.GetText("Spoilers (ctrl-1): ").SetTextColor
				(
					MapModS.Instance.Settings.SpoilerOn ? Color.green : Color.white
				);
		}

		private static void _SetStyle() {
			string buttonName = "Style (ctrl-2): ";
			switch (MapModS.Instance.Settings.PinStyle) {
				case PinGroup.PinStyles.Normal:
					_mapDisplayPanel.GetText(buttonName).UpdateText(buttonName + "normal");
					break;

				case PinGroup.PinStyles.Q_Marks:
					_mapDisplayPanel.GetText(buttonName).UpdateText(buttonName + "q marks");
					break;

				case PinGroup.PinStyles.Old_1:
					_mapDisplayPanel.GetText(buttonName).UpdateText(buttonName + "old 1");
					break;

				case PinGroup.PinStyles.Old_2:
					_mapDisplayPanel.GetText(buttonName).UpdateText(buttonName + "old 2");
					break;
			}
		}

		private static void _SetRandomized() {
			string buttonName = "Randomized (ctrl-3): ";
			if (MapModS.Instance.PinGroupInstance.RandomizedGroups.Any(MapModS.Instance.Settings.GetBoolFromGroup)
				&& !MapModS.Instance.PinGroupInstance.RandomizedGroups.All(MapModS.Instance.Settings.GetBoolFromGroup)) {
				_mapDisplayPanel.GetText(buttonName).SetTextColor(Color.yellow);
				_mapDisplayPanel.GetText(buttonName).UpdateText(buttonName + "custom");
			} else if (MapModS.Instance.Settings.RandomizedOn) {
				_mapDisplayPanel.GetText(buttonName).SetTextColor(Color.green);
				_mapDisplayPanel.GetText(buttonName).UpdateText(buttonName + "on");
			} else {
				_mapDisplayPanel.GetText(buttonName).SetTextColor(Color.white);
				_mapDisplayPanel.GetText(buttonName).UpdateText(buttonName + "off");
			}
		}

		private static void _SetOthers() {
			string buttonName = "Others (ctrl-4): ";
			if (MapModS.Instance.PinGroupInstance.OthersGroups.Any(MapModS.Instance.Settings.GetBoolFromGroup)
				&& !MapModS.Instance.PinGroupInstance.OthersGroups.All(MapModS.Instance.Settings.GetBoolFromGroup)) {
				_mapDisplayPanel.GetText(buttonName).SetTextColor(Color.yellow);
				_mapDisplayPanel.GetText(buttonName).UpdateText(buttonName + "custom");
			} else if (MapModS.Instance.Settings.OthersOn) {
				_mapDisplayPanel.GetText(buttonName).SetTextColor(Color.green);
				_mapDisplayPanel.GetText(buttonName).UpdateText(buttonName + "on");
			} else {
				_mapDisplayPanel.GetText(buttonName).SetTextColor(Color.white);
				_mapDisplayPanel.GetText(buttonName).UpdateText(buttonName + "off");
			}
		}

		public static void SetTexts() {
			_SetSpoilers();
			_SetStyle();
			_SetRandomized();
			_SetOthers();
		}
	}
}