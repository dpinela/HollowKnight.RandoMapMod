using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using UnityEngine;

namespace RandoMapMod {
	[DebugName(nameof(ResourceHelper))]
	static class ResourceHelper {
		#region Constants
		public enum Sprites {
			//old_prereq,

			//oldGeoRock,
			//oldGrub,
			//oldLifeblood,
			//oldTotem,

			//oldGeoRockInv,
			//oldGrubInv,
			//oldLifebloodInv,
			//oldTotemInv,

			Unknown,
			//Prereq,

			Charm,
			Cocoon,
			Dreamer,
			Egg,
			EssenceBoss,
			Flame,
			Geo,
			Grub,
			Key,
			Lore,
			Map,
			Mask,
			Notch,
			Ore,
			Relic,
			Rock,
			Root,
			Shop,
			Skill,
			Stag,
			Totem,
			Vessel,

			//reqRoot,
			//reqGrub,
			//reqEssenceBoss,
		}
		#endregion

		#region Constructors
		static ResourceHelper() {
			Assembly theDLL = typeof(MapModS).Assembly;
			_pSprites = new Dictionary<Sprites, Sprite>();
			foreach (string resource in theDLL.GetManifestResourceNames()) {
				if (resource.EndsWith(".png")) {
					//Load up all the one sprites!
					Stream img = theDLL.GetManifestResourceStream(resource);
					byte[] buff = new byte[img.Length];
					img.Read(buff, 0, buff.Length);
					img.Dispose();

					Texture2D texture = new Texture2D(1, 1);
					texture.LoadImage(buff, true);
					Sprites? key = resource switch {
						//"RandoMapMod.Resources.Map.old_prereqPin.png" => Sprites.old_prereq,

						//"RandoMapMod.Resources.Map.pinUnknown_GeoRock.png" => Sprites.oldGeoRock,
						//"RandoMapMod.Resources.Map.pinUnknown_Grub.png" => Sprites.oldGrub,
						//"RandoMapMod.Resources.Map.pinUnknown_Lifeblood.png" => Sprites.oldLifeblood,
						//"RandoMapMod.Resources.Map.pinUnknown_Totem.png" => Sprites.oldTotem,

						//"RandoMapMod.Resources.Map.pinUnknown_GeoRockInv.png" => Sprites.oldGeoRockInv,
						//"RandoMapMod.Resources.Map.pinUnknown_GrubInv.png" => Sprites.oldGrubInv,
						//"RandoMapMod.Resources.Map.pinUnknown_LifebloodInv.png" => Sprites.oldLifebloodInv,
						//"RandoMapMod.Resources.Map.pinUnknown_TotemInv.png" => Sprites.oldTotemInv,

						"RandoMapMod.Resources.Map.pinUnknown.png" => Sprites.Unknown,
						//"RandoMapMod.Resources.Map.modPrereq.png" => Sprites.Prereq,

						"RandoMapMod.Resources.Map.pinCharm.png" => Sprites.Charm,
						"RandoMapMod.Resources.Map.pinCocoon.png" => Sprites.Cocoon,
						"RandoMapMod.Resources.Map.pinDreamer.png" => Sprites.Dreamer,
						"RandoMapMod.Resources.Map.pinEgg.png" => Sprites.Egg,
						"RandoMapMod.Resources.Map.pinEssenceBoss.png" => Sprites.EssenceBoss,
						"RandoMapMod.Resources.Map.pinFlame.png" => Sprites.Flame,
						"RandoMapMod.Resources.Map.pinGeo.png" => Sprites.Geo,
						"RandoMapMod.Resources.Map.pinGrub.png" => Sprites.Grub,
						"RandoMapMod.Resources.Map.pinKey.png" => Sprites.Key,
						"RandoMapMod.Resources.Map.pinLore.png" => Sprites.Lore,
						"RandoMapMod.Resources.Map.pinMap.png" => Sprites.Map,
						"RandoMapMod.Resources.Map.pinMask.png" => Sprites.Mask,
						"RandoMapMod.Resources.Map.pinNotch.png" => Sprites.Notch,
						"RandoMapMod.Resources.Map.pinOre.png" => Sprites.Ore,
						"RandoMapMod.Resources.Map.pinRelic.png" => Sprites.Relic,
						"RandoMapMod.Resources.Map.pinRock.png" => Sprites.Rock,
						"RandoMapMod.Resources.Map.pinRoot.png" => Sprites.Root,
						"RandoMapMod.Resources.Map.pinShop.png" => Sprites.Shop,
						"RandoMapMod.Resources.Map.pinSkill.png" => Sprites.Skill,
						"RandoMapMod.Resources.Map.pinStag.png" => Sprites.Stag,
						"RandoMapMod.Resources.Map.pinTotem.png" => Sprites.Totem,
						"RandoMapMod.Resources.Map.pinVessel.png" => Sprites.Vessel,

						//"RandoMapMod.Resources.Map.reqEssenceBoss.png" => Sprites.reqEssenceBoss,
						//"RandoMapMod.Resources.Map.reqGrub.png" => Sprites.reqGrub,
						//"RandoMapMod.Resources.Map.reqRoot.png" => Sprites.reqRoot,
						_ => null
					};
					if (key == null) {
						DebugLog.Warn($"Found unrecognized sprite {resource}. Ignoring.");
					} else {
						_pSprites.Add(
							(Sprites) key,
							Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
					}
				} else if (resource.EndsWith("pindata.xml")) {
					//Load the pin-specific data; we'll follow up with the direct rando info later, so we don't duplicate defs...
					try {
						using (Stream stream = theDLL.GetManifestResourceStream(resource)) {
							PinDataDictionary = _LoadPinData(stream);
						}
					} catch (Exception e) {
						DebugLog.Error("pindata.xml Load Failed!");
						DebugLog.Error(e.ToString());
					}
				}
			}

			static void __ParseItems(XmlDocument xml) => _LoadItemData(xml.SelectNodes("randomizer/item"));

			//Assembly randoDLL = MapModS.VersionController.GetInfoAssembly();
			Assembly randoDLL = typeof(RandomizerMod.RandomizerMod).Assembly;
			Dictionary<String, Action<XmlDocument>> resourceProcessors = new Dictionary<String, Action<XmlDocument>>
			{
				{"items.xml", __ParseItems},
				{"rocks.xml", __ParseItems},
				{"soul_lore.xml", __ParseItems},
				//{"shops.xml", __ParseItems},    // There aren't any item defs in this one... Silly Rando3.
			};
			foreach (string resource in randoDLL.GetManifestResourceNames()) {
				foreach (string resourceEnding in resourceProcessors.Keys) {
					if (resource.EndsWith(resourceEnding)) {
						DebugLog.Log($"Loading data from {nameof(RandomizerMod)}'s {resource} file.");
						try {
							using (Stream stream = randoDLL.GetManifestResourceStream(resource)) {
								XmlDocument xml = new XmlDocument();
								xml.Load(stream);
								resourceProcessors[resourceEnding].Invoke(xml);
							}
						} catch (Exception e) {
							DebugLog.Error($"{resourceEnding} Load Failed!");
							DebugLog.Error(e.ToString());
						}
						break;
					}
				}
			}
		}
		#endregion

		#region Private Non-Methods
		private static readonly Dictionary<Sprites, Sprite> _pSprites;
		#endregion

		#region Non-Private Non-Methods
		public static Dictionary<string, PinData> PinDataDictionary { get; }
		#endregion

		#region Non-Private Methods
		public static Sprite FetchSprite(Sprites pSpriteName) {
			if (_pSprites.TryGetValue(pSpriteName, out Sprite sprite)) {
				sprite.name = pSpriteName.ToString();
				return sprite;
			}

			DebugLog.Error("Failed to load sprite named '" + pSpriteName + "'");
			return null;
		}

		internal static Sprite FetchSpriteByPool(string pool) {
			Sprites sid;

			sid = pool switch {
				"Dreamer" => Sprites.Dreamer,
				"Skill" => Sprites.Skill,
				"Charm" => Sprites.Charm,
				"Key" => Sprites.Key,
				"Mask" => Sprites.Mask,
				"Vessel" => Sprites.Vessel,
				"Notch" => Sprites.Notch,
				"Ore" => Sprites.Ore,
				"Geo" => Sprites.Geo,
				"Egg" => Sprites.Egg,
				"Relic" => Sprites.Relic,
				"Map" => Sprites.Map,
				"Stag" => Sprites.Stag,
				"Cocoon" => Sprites.Cocoon,
				"Flame" => Sprites.Flame,
				"Rock" => Sprites.Rock,
				"Soul" => Sprites.Totem,
				"PalaceSoul" => Sprites.Totem,
				"Lore" => Sprites.Lore,

				"Grub" => Sprites.Grub,
				"Root" => Sprites.Root,
				"Essence_Boss" => Sprites.EssenceBoss, //See above comment

				//"?Fake" => Sprites.Unknown,
				"Cursed" => Sprites.Skill,
				"Spell" => Sprites.Skill,
				"SplitCloak" => Sprites.Skill,
				"SplitClaw" => Sprites.Skill,
				"SplitCloakLocation" => Sprites.Skill,
				"CursedNail" => Sprites.Skill,
				"Boss_Geo" => Sprites.Geo,
				"PalaceLore" => Sprites.Lore,
				"Focus" => Sprites.Skill,

				"Shop" => Sprites.Shop,
				_ => Sprites.Unknown
			};

			if (sid == Sprites.Unknown) {
				DebugLog.Log($"{pool} => unknown sprite");
			}

			return FetchSprite(sid);
		}

		// This method finds the randomized item pool corresponding to each pin, using RandomizerMod's ItemPlacements array
		public static void FindRandoPools() {
			foreach (KeyValuePair<string, PinData> entry in PinDataDictionary) {
				string vanillaItem = entry.Key;
				string randoItem;
				PinData pinD = entry.Value;

				// First check if this is a shop pin
				if (pinD.IsShop) {
					pinD.VanillaPool = "Shop";
					pinD.RandoPool = "Shop";
					randoItem = vanillaItem; // These are actually the shop names ("Sly" etc.)
					//DebugLog.Log($"{vanillaItem} is a shop pin");

					// Then check if this item is randomized
				} else if (RandomizerMod.RandomizerMod.Instance.Settings.ItemPlacements.Any(pair => pair.Item2 == vanillaItem)) {
					(string, string) itemLocationPair = RandomizerMod.RandomizerMod.Instance.Settings.ItemPlacements.Single(pair => pair.Item2 == vanillaItem);
					randoItem = itemLocationPair.Item1;

					// If randoItem's in the PinDataDictionary, we already have the pool
					if (PinDataDictionary.ContainsKey(randoItem)) {
						pinD.RandoPool = PinDataDictionary[randoItem].VanillaPool;
						//DebugLog.Log($"In ItemPlacement and PinDataDictionary: {vanillaItem} -> {randoItem}, {pinD.VanillaPool} -> {pinD.RandoPool}");

						// For dupes and cursed items
					} else if (GameStatus.IsOtherMajorItem(randoItem)) {
						pinD.RandoPool = GameStatus.GetOtherMajorItemPool(randoItem);
						//DebugLog.Log($"Other major item: {vanillaItem} -> {randoItem}, {pinD.VanillaPool} -> {pinD.RandoPool}");

						// Shop items WITHOUT a pin in the vanilla pool
					} else if (GameStatus.IsShopItem(randoItem)) {
						pinD.RandoPool = GameStatus.GetShopItemPool(randoItem);
						//DebugLog.Log($"Shop item (no pin): {vanillaItem} -> {randoItem}, {pinD.VanillaPool} -> {pinD.RandoPool}");

						// New items in v3.12c(884), no vanilla pins for these
					} else if (GameStatus.IsNewItem(randoItem)) {
						pinD.RandoPool = GameStatus.GetNewItemPool(randoItem);
						//DebugLog.Log($"New item: {vanillaItem} -> {randoItem}, {pinD.VanillaPool} -> {pinD.RandoPool}");

						// If it is 1_Geo (cursed on)
					} else if (randoItem.StartsWith("1_Geo")) {
						pinD.RandoPool = "Geo";
						//DebugLog.Log($"1 Geo item: {vanillaItem} -> {randoItem}, {pinD.VanillaPool} -> {pinD.RandoPool}");

						// Nothing should end up here!
					} else {
						pinD.RandoPool = pinD.VanillaPool;
						DebugLog.Warn($"Item not found anywhere: {vanillaItem} -> {randoItem}, {pinD.VanillaPool}");
					}

				} else {
					//DebugLog.Log($"Not in ItemPlacement: {vanillaItem}, {pinD.VanillaPool}");
					randoItem = vanillaItem;
					pinD.RandoPool = pinD.VanillaPool;
				}

				// Need to convert pools for shop items here:
				// Sly -> Mask, Charm, Key, Charm, Vessel, Skill, Egg
				// Salubra -> Charm, Notch
				// Leg Eater -> Charm

				pinD.RandoPool = randoItem switch {
					"Gathering_Swarm" => "Charm",
					"Heavy_Blow" => "Charm",
					"Sprintmaster" => "Charm",
					"Stalwart_Shell" => "Charm",
					"Wayward_Compass" => "Charm",
					"Simple_Key-Sly" => "Key",
					"Elegant_Key" => "Key",
					"Mask_Shard-Sly1" => "Mask",
					"Mask_Shard-Sly2" => "Mask",
					"Mask_Shard-Sly3" => "Mask",
					"Mask_Shard-Sly4" => "Mask",
					"Vessel_Fragment-Sly1" => "Vessel",
					"Vessel_Fragment-Sly2" => "Vessel",
					"Lumafly_Lantern" => "Skill",
					"Rancid_Egg-Sly" => "Egg",

					"Longnail" => "Charm",
					"Quick_Focus" => "Charm",
					"Steady_Body" => "Charm",
					"Shaman_Stone" => "Charm",
					"Lifeblood_Heart" => "Charm",

					"Fragile_Greed" => "Charm",
					"Fragile_Heart" => "Charm",
					"Fragile_Strength" => "Charm",
					_ => pinD.RandoPool
				};
				DebugLog.Log($"{vanillaItem} -> {randoItem}, {pinD.VanillaPool} -> {pinD.RandoPool}");
			}

			return;
		}
		#endregion

		#region Private Methods
		private static void _LoadItemData(XmlNodeList nodes) {
			foreach (XmlNode node in nodes) {
				string itemName = node.Attributes["name"].Value;
				if (!PinDataDictionary.ContainsKey(itemName)) {
					//Skip warnings for:
					string[] skipPools = {
						"fake",					//These aren't real items
					};
					string[] types = {
						"shop",					//One pin per shop
					};
					if (skipPools.Contains(node.SelectSingleNode("pool").InnerText.ToLower()) ||
						    types.Contains(node.SelectSingleNode("type").InnerText.ToLower())
						) {
						continue;
					}

					// Suppress log for new beta items for now
					if (GameStatus.IsNewItem(itemName)) {
						continue;
					}

					DebugLog.Warn($"Unknown Rando Item `{itemName}`. Tell devs to check 'pindata.xml'");
					foreach (XmlNode chld in node.ChildNodes) {
						DebugLog.Warn($"    {chld.Name} : {chld.InnerText}");
					}
					continue;
				}

				PinData pinD = PinDataDictionary[itemName];
				//PinData pinD = new PinData();
				foreach (XmlNode chld in node.ChildNodes) {
					if (chld.Name == "sceneName") {
						pinD.SceneName = chld.InnerText;
						continue;
					}

					if (chld.Name == "objectName") {
						pinD.OriginalName = chld.InnerText;
						continue;
					}

					if (chld.Name == "itemLogic") {
						pinD.LogicRaw = chld.InnerText;
						continue;
					}

					if (chld.Name == "boolName") {
						pinD.ObtainedBool = chld.InnerText;
						continue;
					}

					if (chld.Name == "inChest") {
						pinD.InChest = true;
						continue;
					}

					if (chld.Name == "newShiny") {
						pinD.NewShiny = true;
						continue;
					}

					if (chld.Name == "x") {
						pinD.NewX = (int) XmlConvert.ToDouble(chld.InnerText);
						continue;
					}

					if (chld.Name == "y") {
						pinD.NewY = (int) XmlConvert.ToDouble(chld.InnerText);
						continue;
					}

					if (chld.Name == "pool") {
						pinD.VanillaPool = chld.InnerText;
						continue;
					}
				}
			}
		}

		private static Dictionary<string, PinData> _LoadPinData(Stream stream) {
			DebugLog.Log("LoadPinData running");
			Dictionary<string, PinData> retVal = new Dictionary<string, PinData>();

			XmlDocument xml = new XmlDocument();
			xml.Load(stream);
			foreach (XmlNode node in xml.SelectNodes("randomap/pin")) {
				PinData newPin = new PinData {
					ID = node.Attributes["name"].Value
				};
				foreach (XmlNode chld in node.ChildNodes) {
					if (chld.NodeType == XmlNodeType.Comment) {
						continue;
					}
					switch (chld.Name) {
						case "pinScene":
							newPin.PinScene = chld.InnerText;
							break;
						case "checkBool":
							newPin.CheckBool = chld.InnerText;
							break;
						case "offsetX":
							newPin.OffsetX = XmlConvert.ToSingle(chld.InnerText);
							break;
						case "offsetY":
							newPin.OffsetY = XmlConvert.ToSingle(chld.InnerText);
							break;
						case "offsetZ":
							newPin.OffsetZ = XmlConvert.ToSingle(chld.InnerText);
							break;
						case "hasPrereq":
							//newPin.HasPrereq = XmlConvert.ToBoolean(chld.InnerText);
							break;
						case "isShop":
							newPin.IsShop = XmlConvert.ToBoolean(chld.InnerText);
							break;
						default:
							DebugLog.Error($"Pin '{newPin.ID}' in XML had node '{chld.Name}' not parsable!");
							break;
					}
				}
				
				retVal.Add(newPin.ID, newPin);
				
			}
			return retVal;
		}
		#endregion
	}
}
