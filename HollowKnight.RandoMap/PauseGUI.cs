﻿using RandoMapMod.CanvasUtil;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RandoMapMod
{
	// All the following was modified from the GUI implementation of BenchwarpMod by homothetyhk
	internal class PauseGUI
	{
		public static GameObject Canvas;

		private static readonly Dictionary<string, (UnityAction<string>, Vector2)> _buttons = new Dictionary<string, (UnityAction<string>, Vector2)>
		{
			["Spoilers"] = (_SpoilersClicked, new Vector2(0f, 0f)),
			["Style"] = (_StyleClicked, new Vector2(100f, 0f)),
			["Randomized"] = (_RandomizedClicked, new Vector2(200f, 0f)),
			["Others"] = (_OthersClicked, new Vector2(300f, 0f)),
		};

		private static readonly Dictionary<string, (UnityAction<string>, Vector2)> _groupButtons = new Dictionary<string, (UnityAction<string>, Vector2)>
		{
			["Dreamers"] = (_DreamerClicked, new Vector2(100f, 0f)),
			["Skills"] = (_SkillClicked, new Vector2(200f, 0f)),
			["Charms"] = (_CharmClicked, new Vector2(300f, 0f)),
			["Keys"] = (_KeyClicked, new Vector2(400f, 0f)),
			["Mask\nShards"] = (_MaskClicked, new Vector2(500f, 0f)),
			["Vessel\nFragments"] = (_VesselClicked, new Vector2(600f, 0f)),
			["Charm\nNotches"] = (_NotchClicked, new Vector2(700f, 0f)),
			["Pale Ore"] = (_OreClicked, new Vector2(-400f, 30f)),
			["Rancid\nEggs"] = (_EggClicked, new Vector2(-300f, 30f)),
			["Relics"] = (_RelicClicked, new Vector2(-200f, 30f)),
			["Maps"] = (_MapClicked, new Vector2(-100f, 30f)),
			["Stags"] = (_StagClicked, new Vector2(0f, 30f)),
			["Grubs"] = (_GrubClicked, new Vector2(100f, 30f)),
			["Mimics"] = (_MimicClicked, new Vector2(200f, 30f)),
			["Whispering\nRoots"] = (_RootClicked, new Vector2(300f, 30f)),
			["Boss\nEssence"] = (_EssenceBossClicked, new Vector2(400f, 30f)),
			["Geo Rocks"] = (_RockClicked, new Vector2(500f, 30f)),
			["Geo Chests"] = (_GeoClicked, new Vector2(600f, 30f)),
			["Junk Pit\nChests"] = (_JunkClicked, new Vector2(700f, 30f)),
			["Boss Geo"] = (_BossGeoClicked, new Vector2(-400f, 60f)),
			["Soul\nTotems"] = (_SoulClicked, new Vector2(-300f, 60f)),
			["Lore\nTablets"] = (_LoreClicked, new Vector2(-200f, 60f)),
			["Journal\nEntries"] = (_JournalClicked, new Vector2(-100f, 60f)),
			["Palace\nTotems"] = (_PalaceSoulClicked, new Vector2(0f, 60f)),
			["Palace\nLore"] = (_PalaceLoreClicked, new Vector2(100f, 60f)),
			["Palace\nEntries"] = (_PalaceJournalClicked, new Vector2(200f, 60f)),
			["Lifeblood\nCocoons"] = (_CocoonClicked, new Vector2(300f, 60f)),
			["Grimmkin\nFlames"] = (_FlameClicked, new Vector2(400f, 60f)),
			["Shops"] = (_ShopClicked, new Vector2(500f, 60f)),
		};

		private static CanvasPanel _mapControlPanel;
		private static int _mapRevealCounter = 0;

		public static void BuildMenu(GameObject _canvas)
		{
			Canvas = _canvas;

			_mapControlPanel = new CanvasPanel
				(_canvas, GUIController.Instance.Images["ButtonsMenuBG"], new Vector2(10f, 870f), new Vector2(1346f, 0f), new Rect(0f, 0f, 0f, 0f));
			_mapControlPanel.AddText("MapModLabel", "MapMod S", new Vector2(0f, -25f), Vector2.zero, GUIController.Instance.TrajanNormal, 18);

			Rect buttonRect = new Rect(0, 0, GUIController.Instance.Images["ButtonRect"].width, GUIController.Instance.Images["ButtonRect"].height);

			// Main settings
			foreach (KeyValuePair<string, (UnityAction<string>, Vector2)> pair in _buttons)
			{
				_mapControlPanel.AddButton
				(
					pair.Key,
					GUIController.Instance.Images["ButtonRect"],
					pair.Value.Item2,
					Vector2.zero,
					pair.Value.Item1,
					buttonRect,
					GUIController.Instance.TrajanBold,
					pair.Key,
					fontSize: 10
				);
			}

			// These buttons only appear if the full map hasn't been revealed yet
			if (!MapModS.Instance.Settings.RevealedMap)
			{
				_mapControlPanel.AddButton
				(
					"Reveal\nFull Map",
					GUIController.Instance.Images["ButtonRect"],
					new Vector2(400, -30f),
					Vector2.zero,
					_RevealFullMapClicked,
					buttonRect,
					GUIController.Instance.TrajanBold,
					"Reveal\nFull Map",
					fontSize: 10
				);
				_SetRevealFullMap();

				_mapControlPanel.AddButton
				(
					"Show Pins",
					GUIController.Instance.Images["ButtonRect"],
					new Vector2(300, -30f),
					Vector2.zero,
					_ShowPinsClicked,
					buttonRect,
					GUIController.Instance.TrajanBold,
					"Show Pins",
					fontSize: 10
				);
				_SetShowPins();
			}

			// New panel for pool buttons
			CanvasPanel pools = _mapControlPanel.AddPanel
			(
				"Pools",
				GUIController.Instance.Images["ButtonRectEmpty"],
				new Vector2(400f, 0f),
				Vector2.zero,
				new Rect(0f, 0f, GUIController.Instance.Images["DropdownBG"].width, 270f)
			);
			_mapControlPanel.AddButton
			(
				"Pools",
				GUIController.Instance.Images["ButtonRect"],
				new Vector2(400f, 0f),
				Vector2.zero,
				s => _TogglePoolPanel(),
				buttonRect,
				GUIController.Instance.TrajanBold,
				"Pools",
				fontSize: 10
			);
			pools.SetActive(false, true);

			// Pool buttons
			foreach (KeyValuePair<string, (UnityAction<string>, Vector2)> pair in _groupButtons)
			{
				pools.AddButton
				(
					pair.Key,
					GUIController.Instance.Images["ButtonRect"],
					pair.Value.Item2,
					Vector2.zero,
					pair.Value.Item1,
					buttonRect,
					GUIController.Instance.TrajanBold,
					pair.Key,
					fontSize: 10
				);
			}

			_SetGUI();
			_mapControlPanel.SetActive(false, true); // collapse all subpanels
			if (GameManager.instance.IsGamePaused())
			{
				_mapControlPanel.SetActive(true, false);
			}
		}

		public static void RebuildMenu()
		{
			_mapControlPanel.Destroy();
			BuildMenu(Canvas);
		}

		public static void Update()
		{
			if (_mapControlPanel == null || GameManager.instance == null
				|| !RandomizerMod.RandomizerMod.Instance.Settings.Randomizer
				|| !MapModS.Instance.Settings.MapsGiven)
			{
				return;
			}

			if (HeroController.instance == null || !GameManager.instance.IsGameplayScene() || !GameManager.instance.IsGamePaused())
			{
				if (_mapControlPanel.Active) _mapControlPanel.SetActive(false, true);
				_mapRevealCounter = 0;
				return;
			}
			else
			{
				if (!_mapControlPanel.Active)
				{
					RebuildMenu();
				}
			}
		}

		public static void _SetGUI()
		{
			_SetPoolButton("Dreamers", MapModS.Instance.Settings.DreamerOn);
			_SetPoolButton("Skills", MapModS.Instance.Settings.SkillOn);
			_SetPoolButton("Charms", MapModS.Instance.Settings.CharmOn);
			_SetPoolButton("Keys", MapModS.Instance.Settings.KeyOn);
			_SetPoolButton("Mask\nShards", MapModS.Instance.Settings.MaskOn);
			_SetPoolButton("Vessel\nFragments", MapModS.Instance.Settings.VesselOn);
			_SetPoolButton("Charm\nNotches", MapModS.Instance.Settings.NotchOn);
			_SetPoolButton("Pale Ore", MapModS.Instance.Settings.OreOn);
			_SetPoolButton("Rancid\nEggs", MapModS.Instance.Settings.EggOn);
			_SetPoolButton("Relics", MapModS.Instance.Settings.RelicOn);
			_SetPoolButton("Maps", MapModS.Instance.Settings.MapOn);
			_SetPoolButton("Stags", MapModS.Instance.Settings.StagOn);
			_SetPoolButton("Grubs", MapModS.Instance.Settings.GrubOn);
			_SetPoolButton("Mimics", MapModS.Instance.Settings.MimicOn);
			_SetPoolButton("Whispering\nRoots", MapModS.Instance.Settings.RootOn);
			_SetPoolButton("Boss\nEssence", MapModS.Instance.Settings.EssenceBossOn);
			_SetPoolButton("Geo Rocks", MapModS.Instance.Settings.RockOn);
			_SetPoolButton("Geo Chests", MapModS.Instance.Settings.GeoOn);
			_SetPoolButton("Junk Pit\nChests", MapModS.Instance.Settings.JunkOn);
			_SetPoolButton("Boss Geo", MapModS.Instance.Settings.BossGeoOn);
			_SetPoolButton("Soul\nTotems", MapModS.Instance.Settings.SoulOn);
			_SetPoolButton("Lore\nTablets", MapModS.Instance.Settings.LoreOn);
			_SetPoolButton("Journal\nEntries", MapModS.Instance.Settings.JournalOn);
			_SetPoolButton("Palace\nTotems", MapModS.Instance.Settings.PalaceSoulOn);
			_SetPoolButton("Palace\nLore", MapModS.Instance.Settings.PalaceLoreOn);
			_SetPoolButton("Palace\nEntries", MapModS.Instance.Settings.PalaceJournalOn);
			_SetPoolButton("Lifeblood\nCocoons", MapModS.Instance.Settings.CocoonOn);
			_SetPoolButton("Grimmkin\nFlames", MapModS.Instance.Settings.FlameOn);
			_SetPoolButton("Shops", MapModS.Instance.Settings.ShopOn);

			_SetSpoilers();
			_SetRandomized();
			_SetOthers();
			_SetStyle();
		}

		private static void _SpoilersClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleSpoilers();
		}

		private static void _SetSpoilers()
		{
			_mapControlPanel.GetButton("Spoilers").SetTextColor
				(
					MapModS.Instance.Settings.SpoilerOn ? Color.green : Color.white
				);
			_mapControlPanel.GetButton("Spoilers").UpdateText
				(
					MapModS.Instance.Settings.SpoilerOn ? ("Spoilers\non") : ("Spoilers\noff")
				);
		}

		private static void _StyleClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.TogglePinStyle();
		}

		private static void _SetStyle()
		{
			switch (MapModS.Instance.Settings.PinStyle)
			{
				case PinGroup.PinStyles.Normal:
					_mapControlPanel.GetButton("Style").UpdateText("Style\nnormal");
					break;

				case PinGroup.PinStyles.Q_Marks_1:
					_mapControlPanel.GetButton("Style").UpdateText("Style\nq marks 1");
					break;

				case PinGroup.PinStyles.Q_Marks_2:
					_mapControlPanel.GetButton("Style").UpdateText("Style\nq marks 2");
					break;

				case PinGroup.PinStyles.Q_Marks_3:
					_mapControlPanel.GetButton("Style").UpdateText("Style\nq marks 3");
					break;
			}
		}

		private static void _RandomizedClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleRandomized();
		}

		private static void _SetRandomized()
		{
			if (!MapModS.Instance.PinGroupInstance.RandomizedGroups.Any(MapModS.Instance.Settings.GetBoolFromGroup))
			{
				_mapControlPanel.GetButton("Randomized").SetTextColor(Color.white);
				_mapControlPanel.GetButton("Randomized").UpdateText("Randomized\noff");
				MapModS.Instance.Settings.RandomizedOn = false;
			}
			else if (MapModS.Instance.PinGroupInstance.RandomizedGroups.All(MapModS.Instance.Settings.GetBoolFromGroup))
			{
				_mapControlPanel.GetButton("Randomized").SetTextColor(Color.green);
				_mapControlPanel.GetButton("Randomized").UpdateText("Randomized\non");
				MapModS.Instance.Settings.RandomizedOn = true;
			}
			else
			{
				_mapControlPanel.GetButton("Randomized").SetTextColor(Color.yellow);
				_mapControlPanel.GetButton("Randomized").UpdateText("Randomized\ncustom");
				MapModS.Instance.Settings.RandomizedOn = true;
			}
		}

		private static void _OthersClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleOthers();
		}

		private static void _SetOthers()
		{
			if (!MapModS.Instance.PinGroupInstance.OthersGroups.Any(MapModS.Instance.Settings.GetBoolFromGroup))
			{
				_mapControlPanel.GetButton("Others").SetTextColor(Color.white);
				_mapControlPanel.GetButton("Others").UpdateText("Others\noff");
				MapModS.Instance.Settings.OthersOn = false;
			}
			else if (MapModS.Instance.PinGroupInstance.OthersGroups.All(MapModS.Instance.Settings.GetBoolFromGroup))
			{
				_mapControlPanel.GetButton("Others").SetTextColor(Color.green);
				_mapControlPanel.GetButton("Others").UpdateText("Others\non");
				MapModS.Instance.Settings.OthersOn = true;
			}
			else
			{
				_mapControlPanel.GetButton("Others").SetTextColor(Color.yellow);
				_mapControlPanel.GetButton("Others").UpdateText("Others\ncustom");
				MapModS.Instance.Settings.OthersOn = true;
			}
		}

		private static void _RevealFullMapClicked(string buttonName)
		{
			_mapRevealCounter++;

			if (_mapRevealCounter > 1)
			{
				MapModS.RevealFullMap();
				MapModS.Instance.Settings.ShowAllPins = true;
				_mapControlPanel.GetButton("Reveal\nFull Map").SetActive(false);
				_mapControlPanel.GetButton("Show Pins").SetActive(false);
			}

			_SetRevealFullMap();
		}

		// This one is independent of the ShowButtons function as the button may or may not exist
		private static void _SetRevealFullMap()
		{
			if (_mapRevealCounter == 1)
			{
				_mapControlPanel.GetButton("Reveal\nFull Map").SetTextColor(Color.yellow);
				_mapControlPanel.GetButton("Reveal\nFull Map").UpdateText("Are you sure?");
			}
			else
			{
				_mapControlPanel.GetButton("Reveal\nFull Map").SetTextColor(Color.green);
			}
		}

		private static void _ShowPinsClicked(string buttonName)
		{
			MapModS.Instance.Settings.ShowAllPins = !MapModS.Instance.Settings.ShowAllPins;
			_SetShowPins();
		}

		// This one is independent of the ShowButtons function as the button may or may not exist
		private static void _SetShowPins()
		{
			if (MapModS.Instance.Settings.ShowAllPins)
			{
				_mapControlPanel.GetButton("Show Pins").UpdateText("Show Pins\nall areas");
			}
			else
			{
				_mapControlPanel.GetButton("Show Pins").UpdateText("Show Pins\nmap only");
			}
		}

		private static void _TogglePoolPanel()
		{
			_mapControlPanel.TogglePanel("Pools");
			_mapControlPanel.GetButton("Pools").UpdateText
				(
					_mapControlPanel.GetPanel("Pools").Active ? "Hide Pools" : "Show Pools"
				);
		}

		private static void _BossGeoClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.BossGeo);
		}

		private static void _CharmClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Charm);
		}

		private static void _CocoonClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Cocoon);
		}

		private static void _DreamerClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Dreamer);
		}

		private static void _EggClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Egg);
		}

		private static void _EssenceBossClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.EssenceBoss);
		}

		private static void _FlameClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Flame);
		}

		private static void _GeoClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Geo);
		}

		private static void _GrubClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Grub);
		}

		private static void _JournalClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Journal);
		}

		private static void _JunkClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Junk);
		}

		private static void _KeyClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Key);
		}

		private static void _LoreClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Lore);
		}

		private static void _MapClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Map);
		}

		private static void _MaskClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Mask);
		}

		private static void _MimicClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Mimic);
		}

		private static void _NotchClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Notch);
		}

		private static void _OreClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Ore);
		}

		private static void _PalaceJournalClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.PalaceJournal);
		}

		private static void _PalaceLoreClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.PalaceLore);
		}

		private static void _PalaceSoulClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.PalaceSoul);
		}

		private static void _RelicClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Relic);
		}

		private static void _RockClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Rock);
		}

		private static void _RootClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Root);
		}

		private static void _ShopClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Shop);
		}

		private static void _SkillClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Skill);
		}

		private static void _SoulClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Soul);
		}

		private static void _StagClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Stag);
		}

		private static void _VesselClicked(string buttonName)
		{
			MapModS.Instance.PinGroupInstance.ToggleGroup(PinGroup.GroupName.Vessel);
		}

		private static void _SetPoolButton(string buttonName, bool setting)
		{
			_mapControlPanel.GetPanel("Pools").GetButton(buttonName).SetTextColor
				(
					setting ? Color.green : Color.white
				);
		}
	}
}