using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using UnityEngine;
using RandomizerMod.RandomizerData;

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
			LoadRandoModAssets();
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
						"Mask_Shard" => Sprites.Mask,
						"CursedMask" => Sprites.Mask,
						"Vessel_Fragment" => Sprites.Vessel,
						"Charm_Notch" => Sprites.Notch,
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
						"DreamWarrior" => Sprites.EssenceBoss,
						"DreamBoss" => Sprites.EssenceBoss,
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
						"PoPJournal" => Sprites.Lore,
						"Rock" => Sprites.Rock,
						"Soul" => Sprites.Totem,
						"PalaceSoul" => Sprites.Totem,
						"PoPSoul" => Sprites.Totem,
						"Lore" => Sprites.Lore,
						"PalaceLore" => Sprites.Lore,
						"PoPLore" => Sprites.Lore,
						"Shop" => Sprites.Shop,
						"CursedGeo" => Sprites.Geo,
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

		private static void LoadRandoModAssets()
		{
			foreach (var loc in Data.GetLocationArray())
			{
				if (PinDataDictionary.TryGetValue(loc.Name, out var pin))
				{
					pin.SceneName = loc.SceneName;
					pin.MapArea = loc.MapArea;
				}
			}
			foreach (var item in Data.GetItemArray())
			{
				if (PinDataDictionary.TryGetValue(item.Name, out var pin) && string.IsNullOrEmpty(pin.VanillaPool))
				{
					pin.VanillaPool = item.Pool;
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
						"MapModS.Resources.Map.pinUnknown_GeoRock.png" => Sprites.oldGeoRock,
						"MapModS.Resources.Map.pinUnknown_Grub.png" => Sprites.oldGrub,
						"MapModS.Resources.Map.pinUnknown_Lifeblood.png" => Sprites.oldLifeblood,
						"MapModS.Resources.Map.pinUnknown_Totem.png" => Sprites.oldTotem,

						"MapModS.Resources.Map.pinUnknown_GeoRockInv.png" => Sprites.oldGeoRockInv,
						"MapModS.Resources.Map.pinUnknown_GrubInv.png" => Sprites.oldGrubInv,
						"MapModS.Resources.Map.pinUnknown_LifebloodInv.png" => Sprites.oldLifebloodInv,
						"MapModS.Resources.Map.pinUnknown_TotemInv.png" => Sprites.oldTotemInv,

						"MapModS.Resources.Map.pinUnknown.png" => Sprites.Unknown,
						"MapModS.Resources.Map.pinCharm.png" => Sprites.Charm,
						"MapModS.Resources.Map.pinCocoon.png" => Sprites.Cocoon,
						"MapModS.Resources.Map.pinDreamer.png" => Sprites.Dreamer,
						"MapModS.Resources.Map.pinEgg.png" => Sprites.Egg,
						"MapModS.Resources.Map.pinEssenceBoss.png" => Sprites.EssenceBoss,
						"MapModS.Resources.Map.pinFlame.png" => Sprites.Flame,
						"MapModS.Resources.Map.pinGeo.png" => Sprites.Geo,
						"MapModS.Resources.Map.pinGrub.png" => Sprites.Grub,
						"MapModS.Resources.Map.pinKey.png" => Sprites.Key,
						"MapModS.Resources.Map.pinLore.png" => Sprites.Lore,
						"MapModS.Resources.Map.pinMap.png" => Sprites.Map,
						"MapModS.Resources.Map.pinMask.png" => Sprites.Mask,
						"MapModS.Resources.Map.pinNotch.png" => Sprites.Notch,
						"MapModS.Resources.Map.pinOre.png" => Sprites.Ore,
						"MapModS.Resources.Map.pinRelic.png" => Sprites.Relic,
						"MapModS.Resources.Map.pinRock.png" => Sprites.Rock,
						"MapModS.Resources.Map.pinRoot.png" => Sprites.Root,
						"MapModS.Resources.Map.pinShop.png" => Sprites.Shop,
						"MapModS.Resources.Map.pinSkill.png" => Sprites.Skill,
						"MapModS.Resources.Map.pinStag.png" => Sprites.Stag,
						"MapModS.Resources.Map.pinTotem.png" => Sprites.Totem,
						"MapModS.Resources.Map.pinVessel.png" => Sprites.Vessel,
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

				// Unlike rando 3, rando 4 has a single grub/ore/etc. item that it reuses everywhere.
				// As a consequence, there is no item named like Grub-X_Location or similar, and thus
				// we can't determine its pool directly from rando data.
				if (newPin.ID.StartsWith("Grub-"))
				{
					newPin.VanillaPool = "Grub";
				}
				else if (newPin.ID.StartsWith("Mask_Shard-"))
				{
					newPin.VanillaPool = "Mask_Shard";
				}
				else if (newPin.ID.StartsWith("Vessel_Fragment-"))
				{
					newPin.VanillaPool = "Vessel_Fragment";
				}
				else if (newPin.ID.StartsWith("Pale_Ore-"))
				{
					newPin.VanillaPool = "Ore";
				}
				else if (newPin.ID.StartsWith("Charm_Notch-"))
				{
					newPin.VanillaPool = "Charm_Notch";
				}
				else if (newPin.ID.StartsWith("Rancid_Egg-"))
				{
					newPin.VanillaPool = "Egg";
				}
				else if (newPin.ID.StartsWith("Simple_Key-"))
				{
					newPin.VanillaPool = "Key";
				}
				else if (newPin.ID.StartsWith("Geo_Rock-"))
				{
					newPin.VanillaPool = "Rock";
				}
				else if (newPin.ID.StartsWith("Soul_Totem-White_Palace_"))
				{
					newPin.VanillaPool = "PalaceSoul";
				}
				else if (newPin.ID.StartsWith("Soul_Totem-Path_of_Pain_"))
				{
					newPin.VanillaPool = "PoPSoul";
				}
				else if (newPin.ID.StartsWith("Soul_Totem-"))
				{
					newPin.VanillaPool = "Soul";
				}
				// This lore tablet is in pool "PalaceLore" in rando's item list
				else if (newPin.ID.StartsWith("Lore_Tablet-Path_of_Pain_"))
				{
					newPin.VanillaPool = "PoPLore";
				}
				else if (newPin.ID == "Journal_Entry-Seal_of_Binding")
				{
					newPin.VanillaPool = "PoPJournal";
				}
				else if (newPin.ID.StartsWith("Lifeblood_Cocoon-"))
				{
					newPin.VanillaPool = "Cocoon";
				}
				else if (newPin.ID.StartsWith("Grimmkin_Flame-"))
				{
					newPin.VanillaPool = "Flame";
				}
				else if (newPin.ID.StartsWith("Geo_Chest-Junk_Pit") || newPin.ID == "Lumafly_Escape-Junk_Pit_Chest_4")
				{
					newPin.VanillaPool = "JunkPitChest";
				}
				else if (newPin.ID.StartsWith("Wanderer's_Journal-") || newPin.ID.StartsWith("Hallownest_Seal-") || newPin.ID.StartsWith("King's_Idol-") || newPin.ID.StartsWith("Arcane_Egg"))
				{
					newPin.VanillaPool = "Relic";
				}
				// There is only a single Deepnest_Map item.
				else if (newPin.ID.StartsWith("Deepnest_Map-"))
				{
					newPin.VanillaPool = "Map";
				}
				// There is not a single item named "Grimmchild"; instead there's Grimmchild1
				// and Grimmchild2.
				else if (newPin.ID == "Grimmchild")
				{
					newPin.VanillaPool = "Charm";
				}

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

				if (pinD.NotPin)
				{
					continue;
				}

				// First check if this is a shop pin
				if (pinD.IsShop)
				{
					pinD.VanillaPool = "Shop";
					pinD.SpoilerPool = "Shop";
					spoilerItem = vanillaItem;

					// Then check if this item is randomized
				}
				else if (RandomizerMod.RandomizerMod.RS.Context.itemPlacements.Any(pair => pair.location.Name == vanillaItem))
				{
					var ilp = RandomizerMod.RandomizerMod.RS.Context.itemPlacements.First(pair => pair.location.Name == vanillaItem);
					spoilerItem = ilp.item.Name.Replace("Placeholder-", "");

					// If spoilerItem's in the PinDataDictionary, use that Value
					if (PinDataDictionary.ContainsKey(spoilerItem))
					{
						pinD.SpoilerPool = PinDataDictionary[spoilerItem].VanillaPool;

						// Items that are not in RandomizerMod's xml files but are created during randomization
					}
					else if (Dictionaries.IsClonedItem(spoilerItem))
					{
						pinD.SpoilerPool = Dictionaries.GetPoolFromClonedItem(spoilerItem);

					}
					// For cursed mode
					else if (spoilerItem.StartsWith("1_Geo") || spoilerItem.StartsWith("Lumafly_Escape"))
					{
						pinD.SpoilerPool = "CursedGeo";

						// Nothing should end up here!
					}
					else if (spoilerItem == "Grub" || spoilerItem == "Mask_Shard" || spoilerItem == "Vessel_Fragment" || spoilerItem == "Rancid_Egg" || spoilerItem == "Charm_Notch")
					{
						pinD.SpoilerPool = spoilerItem;
					}
					else if (spoilerItem == "Wanderer's_Journal" || spoilerItem == "Hallownest_Seal" || spoilerItem == "King's_Idol" || spoilerItem == "Arcane_Egg")
					{
						pinD.SpoilerPool = "Relic";
					}
					else if (spoilerItem == "Simple_Key")
					{
						pinD.SpoilerPool = "Key";
					}
					else if (spoilerItem == "Pale_Ore")
					{
						pinD.SpoilerPool = "Ore";
					}
					else if (spoilerItem.StartsWith("Grimmchild"))
					{
						pinD.SpoilerPool = "Charm";
					}
					else if (spoilerItem.StartsWith("Lifeblood_Cocoon_"))
					{
						pinD.SpoilerPool = "Cocoon";
					}
					else if (spoilerItem == "Grimmkin_Flame")
					{
						pinD.SpoilerPool = "Flame";
					}
					else if (spoilerItem.StartsWith("Soul_Totem-"))
					{
						pinD.SpoilerPool = "Soul";
					}
					else if (spoilerItem.StartsWith("Geo_Rock-"))
					{
						pinD.SpoilerPool = "Rock";
					}
					else if (spoilerItem == "Dreamer")
					{
						pinD.SpoilerPool = "Dreamer";
					}
					else if (spoilerItem == "One_Geo")
					{
						pinD.SpoilerPool = "CursedGeo";
					}
					else if (spoilerItem == "Quill" || spoilerItem == "Deepnest_Map")
					{
						pinD.SpoilerPool = "Map";
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