using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using UnityEngine;

namespace MapModS
{
	// This class handles loading:
	// - This Mod's xml file
	// - The Pin sprites
	// - Data from the RandomizerMod xml files
	internal static class ResourceLoader
	{
		private static readonly Dictionary<Sprites, Sprite> _pSprites;

		static ResourceLoader()
		{
			_pSprites = new Dictionary<Sprites, Sprite>();
			_LoadMapModAssets();
			_LoadRandoModAssets();
		}

		public enum Sprites
		{
			oldGeoRock,
			oldGrub,
			oldLifeblood,
			oldTotem,

			oldGeoRockInv,
			oldGrubInv,
			oldLifebloodInv,
			oldTotemInv,

			Unknown,
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
		}

		public static Dictionary<string, PinData> PinDataDictionary { get; set; }

		internal static Sprite GetSprite(Sprites pSpriteName)
		{
			if (_pSprites.TryGetValue(pSpriteName, out Sprite sprite))
			{
				sprite.name = pSpriteName.ToString();
				return sprite;
			}

			MapModS.Instance.Log("Failed to load sprite named '" + pSpriteName + "'");
			return null;
		}

		internal static Sprite GetSpriteFromPool(string pool)
		{
			Sprites sid;

			switch (MapModS.Instance.Settings.PinStyle)
			{
				case PinGroup.PinStyles.Normal:
					sid = pool switch
					{
						"Dreamer" => Sprites.Dreamer,
						"Skill" => Sprites.Skill,
						"Focus" => Sprites.Skill,
						"Swim" => Sprites.Skill,
						"SplitCloak" => Sprites.Skill,
						"SplitClaw" => Sprites.Skill,
						"SplitCloakLocation" => Sprites.Skill,
						"CursedNail" => Sprites.Skill,
						"Charm" => Sprites.Charm,
						"Key" => Sprites.Key,
						"ElevatorPass" => Sprites.Key,
						"Mask" => Sprites.Mask,
						"CursedMask" => Sprites.Mask,
						"Vessel" => Sprites.Vessel,
						"Notch" => Sprites.Notch,
						"CursedNotch" => Sprites.Notch,
						"Ore" => Sprites.Ore,
						"Geo" => Sprites.Geo,
						"JunkPitChest" => Sprites.Geo,
						"EggShopLocation" => Sprites.Geo,
						"Boss_Geo" => Sprites.Geo,
						"Egg" => Sprites.Egg,
						"EggShopItem" => Sprites.Egg,
						"Relic" => Sprites.Relic,
						"Root" => Sprites.Root,
						"Essence_Boss" => Sprites.EssenceBoss,
						"Grub" => Sprites.Grub,
						"GrubItem" => Sprites.Grub,
						"Mimic" => Sprites.Grub,
						"MimicItem" => Sprites.Grub,
						"Map" => Sprites.Map,
						"Stag" => Sprites.Stag,
						"Cocoon" => Sprites.Cocoon,
						"Flame" => Sprites.Flame,
						"Journal" => Sprites.Lore,
						"PalaceJournal" => Sprites.Lore,
						"Rock" => Sprites.Rock,
						"Soul" => Sprites.Totem,
						"PalaceSoul" => Sprites.Totem,
						"Lore" => Sprites.Lore,
						"PalaceLore" => Sprites.Lore,
						"Shop" => Sprites.Shop,
						_ => Sprites.Unknown
					};
					break;

				case PinGroup.PinStyles.Q_Marks_1:
					sid = pool switch
					{
						"Shop" => Sprites.Shop,
						_ => Sprites.Unknown
					};
					break;

				case PinGroup.PinStyles.Q_Marks_2:
					sid = pool switch
					{
						"Rock" => Sprites.oldGeoRockInv,
						"Grub" => Sprites.oldGrubInv,
						"Mimic" => Sprites.oldGrubInv,
						"Cocoon" => Sprites.oldLifebloodInv,
						"Soul" => Sprites.oldTotemInv,
						"Shop" => Sprites.Shop,
						_ => Sprites.Unknown
					};
					break;

				case PinGroup.PinStyles.Q_Marks_3:
					sid = pool switch
					{
						"Rock" => Sprites.oldGeoRock,
						"Grub" => Sprites.oldGrub,
						"Mimic" => Sprites.oldGrub,
						"Cocoon" => Sprites.oldLifeblood,
						"Soul" => Sprites.oldTotem,
						"Shop" => Sprites.Shop,
						_ => Sprites.Unknown
					};
					break;

				default:
					sid = pool switch
					{
						"Shop" => Sprites.Shop,
						_ => Sprites.Unknown
					};
					break;
			}

			return GetSprite(sid);
		}

		private static void _LoadRandoModAssets()
		{
			static void __ParseItems(XmlDocument xml) => _LoadItemData(xml.SelectNodes("randomizer/item"));

			Assembly randoDLL = typeof(RandomizerMod.RandomizerMod).Assembly;
			Dictionary<string, Action<XmlDocument>> resourceProcessors = new Dictionary<string, Action<XmlDocument>>
			{
				{"items.xml", __ParseItems},
				{"rocks.xml", __ParseItems},
				{"soul_lore.xml", __ParseItems},
			};
			foreach (string resource in randoDLL.GetManifestResourceNames())
			{
				foreach (string resourceEnding in resourceProcessors.Keys)
				{
					if (resource.EndsWith(resourceEnding))
					{
						MapModS.Instance.Log($"Loading data from {nameof(RandomizerMod)}'s {resource} file.");
						try
						{
							using (Stream stream = randoDLL.GetManifestResourceStream(resource))
							{
								XmlDocument xml = new XmlDocument();
								xml.Load(stream);
								resourceProcessors[resourceEnding].Invoke(xml);
							}
						}
						catch (Exception e)
						{
							MapModS.Instance.LogError($"{resourceEnding} Load Failed!\n{e}");
						}
						break;
					}
				}
			}
		}

		private static void _LoadItemData(XmlNodeList nodes)
		{
			foreach (XmlNode node in nodes)
			{
				string itemName = node.Attributes["name"].Value;
				PinData pinD = PinDataDictionary[itemName];

				foreach (XmlNode chld in node.ChildNodes)
				{
					if (chld.Name == "sceneName")
					{
						pinD.SceneName = chld.InnerText;
						continue;
					}
					if (chld.Name == "objectName")
					{
						// Let pindata.xml keep its overwrite
						if (pinD.ObjectName == "")
						{
							pinD.ObjectName = chld.InnerText;
						}
						continue;
					}
					if (chld.Name == "areaName")
					{
						if (Dictionaries.IsArea(chld.InnerText))
						{
							pinD.MapArea = Dictionaries.GetMapAreaFromArea(chld.InnerText);
						}
						continue;
					}
					if (chld.Name == "pool")
					{
						pinD.VanillaPool = chld.InnerText;
						continue;
					}
				}
			}
		}

		private static void _LoadMapModAssets()
		{
			Assembly theDLL = typeof(MapModS).Assembly;
			foreach (string resource in theDLL.GetManifestResourceNames())
			{
				if (resource.EndsWith(".png"))
				{
					Stream img = theDLL.GetManifestResourceStream(resource);
					byte[] buff = new byte[img.Length];
					img.Read(buff, 0, buff.Length);
					img.Dispose();

					Texture2D texture = new Texture2D(1, 1);
					texture.LoadImage(buff, true);
					Sprites? key = resource switch
					{
						"RandoMapMod.Resources.Map.pinUnknown_GeoRock.png" => Sprites.oldGeoRock,
						"RandoMapMod.Resources.Map.pinUnknown_Grub.png" => Sprites.oldGrub,
						"RandoMapMod.Resources.Map.pinUnknown_Lifeblood.png" => Sprites.oldLifeblood,
						"RandoMapMod.Resources.Map.pinUnknown_Totem.png" => Sprites.oldTotem,

						"RandoMapMod.Resources.Map.pinUnknown_GeoRockInv.png" => Sprites.oldGeoRockInv,
						"RandoMapMod.Resources.Map.pinUnknown_GrubInv.png" => Sprites.oldGrubInv,
						"RandoMapMod.Resources.Map.pinUnknown_LifebloodInv.png" => Sprites.oldLifebloodInv,
						"RandoMapMod.Resources.Map.pinUnknown_TotemInv.png" => Sprites.oldTotemInv,

						"RandoMapMod.Resources.Map.pinUnknown.png" => Sprites.Unknown,
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
						_ => null
					};

					if (key == null)
					{
						if (resource.Contains("GUI"))
						{
							continue;
						}
						else
						{
							MapModS.Instance.LogWarn($"Found unrecognized sprite {resource}. Ignoring.");
						}
					}
					else
					{
						_pSprites.Add(
							(Sprites) key,
							Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
					}
				}
				else if (resource.EndsWith("pindata.xml"))
				{
					try
					{
						using (Stream stream = theDLL.GetManifestResourceStream(resource))
						{
							PinDataDictionary = _LoadPinData(stream);
						}
					}
					catch (Exception e)
					{
						MapModS.Instance.LogWarn($"pindata.xml Load Failed!\n{e}");
					}
				}
			}
		}

		private static Dictionary<string, PinData> _LoadPinData(Stream stream)
		{
			Dictionary<string, PinData> retVal = new Dictionary<string, PinData>();

			XmlDocument xml = new XmlDocument();
			xml.Load(stream);
			foreach (XmlNode node in xml.SelectNodes("randomap/pin"))
			{
				PinData newPin = new PinData
				{
					ID = node.Attributes["name"].Value
				};
				foreach (XmlNode chld in node.ChildNodes)
				{
					if (chld.NodeType == XmlNodeType.Comment)
					{
						continue;
					}
					switch (chld.Name)
					{
						case "notPin":
							newPin.NotPin = XmlConvert.ToBoolean(chld.InnerText);
							break;

						case "pinScene":
							newPin.PinScene = chld.InnerText;
							break;
						// Only used for getting Shop Pins in the right place
						case "mapArea":
							newPin.MapArea = chld.InnerText;
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

						case "isShop":
							newPin.IsShop = XmlConvert.ToBoolean(chld.InnerText);
							break;

						// Only used for fixing duplicate object names in the same scene
						case "objectName":
							newPin.ObjectName = chld.InnerText;
							break;

						default:
							MapModS.Instance.LogError($"Pin '{newPin.ID}' in XML had node '{chld.Name}' not parsable!");
							break;
					}
				}

				retVal.Add(newPin.ID, newPin);
			}

			return retVal;
		}

		// This method finds the spoiler item pools corresponding to each Pin, using RandomizerMod's ItemPlacements array
		public static void FindSpoilerPools()
		{
			foreach (KeyValuePair<string, PinData> entry in PinDataDictionary)
			{
				string vanillaItem = entry.Key;
				string spoilerItem;
				PinData pinD = entry.Value;

				// First check if this is a shop pin
				if (pinD.IsShop)
				{
					pinD.VanillaPool = "Shop";
					pinD.SpoilerPool = "Shop";
					spoilerItem = vanillaItem;

					// Then check if this item is randomized
				}
				else if (RandomizerMod.RandomizerMod.Instance.Settings.ItemPlacements.Any(pair => pair.Item2 == vanillaItem))
				{
					(string, string) itemLocationPair = RandomizerMod.RandomizerMod.Instance.Settings.ItemPlacements.Single(pair => pair.Item2 == vanillaItem);
					spoilerItem = itemLocationPair.Item1;

					// If spoilerItem's in the PinDataDictionary, use that Value
					if (PinDataDictionary.ContainsKey(spoilerItem))
					{
						pinD.SpoilerPool = PinDataDictionary[spoilerItem].VanillaPool;

						// Items that are not in RandomizerMod's xml files but are created during randomization
					}
					else if (Dictionaries.IsClonedItem(spoilerItem))
					{
						pinD.SpoilerPool = Dictionaries.GetPoolFromClonedItem(spoilerItem);

						// For cursed mode
					}
					else if (spoilerItem.StartsWith("1_Geo") || spoilerItem.StartsWith("Lumafly_Escape"))
					{
						pinD.SpoilerPool = "Rock";

						// Nothing should end up here!
					}
					else
					{
						pinD.SpoilerPool = pinD.VanillaPool;
						MapModS.Instance.LogWarn($"Item in RandomizerMod not recognized: {vanillaItem} -> {spoilerItem}, {pinD.VanillaPool}");
					}

					// Don't create the Pin if it is not recognized by RandomizerMod
					// ElevatorPass Pin should not be created if ElevatorPass is false
				}
				else if (pinD.VanillaPool == "" || pinD.VanillaPool == "ElevatorPass")
				{
					spoilerItem = vanillaItem;
					pinD.SpoilerPool = pinD.VanillaPool;
					pinD.NotPin = true;

					// These items are recognized by RandomizerMod, but not randomized
				}
				else
				{
					spoilerItem = vanillaItem;
					pinD.SpoilerPool = pinD.VanillaPool;
				}

				//MapModS.Instance.Log($"{vanillaItem} -> {spoilerItem}, {pinD.VanillaPool} -> {pinD.SpoilerPool}");
			}

			return;
		}

		// This stuff was used to update pin positions during run time
		private static string _pinPath = "";

		private static string _PinPath
		{
			get
			{
				if (_pinPath == "")
				{
					string codeBase = Assembly.GetExecutingAssembly().CodeBase;
					UriBuilder uri = new UriBuilder(codeBase);
					_pinPath = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
				}
				return _pinPath;
			}
		}

		private static string _PinData => _PinPath + @"/pindebug.xml";

		public static void ReloadPinData()
		{
			try
			{
				using (Stream stream = File.Open(_PinData, FileMode.Open))
				{
					XmlDocument xml = new XmlDocument();
					xml.Load(stream);
					foreach (XmlNode node in xml.SelectNodes("randomap/pin"))
					{
						string ID = node.Attributes["name"].Value;
						foreach (XmlNode chld in node.ChildNodes)
						{
							if (chld.NodeType == XmlNodeType.Comment)
							{
								continue;
							}
							if (PinDataDictionary.ContainsKey(ID))
							{
								switch (chld.Name)
								{
									case "notPin":
										PinDataDictionary[ID].NotPin = XmlConvert.ToBoolean(chld.InnerText);
										break;

									case "pinScene":
										PinDataDictionary[ID].PinScene = chld.InnerText;
										break;
									// Only used for getting Shop Pins in the right place
									case "mapArea":
										PinDataDictionary[ID].MapArea = chld.InnerText;
										break;

									case "vanillaPool":
										PinDataDictionary[ID].VanillaPool = chld.InnerText;
										break;

									case "offsetX":
										PinDataDictionary[ID].OffsetX = XmlConvert.ToSingle(chld.InnerText);
										break;

									case "offsetY":
										PinDataDictionary[ID].OffsetY = XmlConvert.ToSingle(chld.InnerText);
										break;

									case "offsetZ":
										PinDataDictionary[ID].OffsetZ = XmlConvert.ToSingle(chld.InnerText);
										break;

									default:
										break;
								}
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				MapModS.Instance.LogWarn($"pindebug.xml Load Failed!\n{e}");
			}
		}
	}
}