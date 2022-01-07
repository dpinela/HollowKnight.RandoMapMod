using System.Collections.Generic;
using System.Reflection;

namespace MapModS
{
	// This class has some useful Get methods / value lookups
	public static class Dictionaries
	{

		// These items are not in RandomizerMod's items.xml but are created during randomization. Cursed Geo is handled by string check instead
		private static readonly Dictionary<string, string> _clonedItemToPool = new Dictionary<string, string>()
		{
			{"Dreamer_(1)"              , "Dreamer"     },
			{"Mothwing_Cloak_(1)"       , "Skill"       },
			{"Mantis_Claw_(1)"          , "Skill"       },
			{"Crystal_Heart_(1)"        , "Skill"       },
			{"Monarch_Wings_(1)"        , "Skill"       },
			{"Shade_Cloak_(1)"          , "Skill"       },
			{"Isma's_Tear_(1)"          , "Skill"       },
			{"Dream_Nail_(1)"           , "Skill"       },
			{"Vengeful_Spirit_(1)"      , "Skill"       },
			{"Desolate_Dive_(1)"        , "Skill"       },
			{"Howling_Wraiths_(1)"      , "Skill"       },
			{"Void_Heart_(1)"           , "Charm"       },
			{"Left_Mothwing_Cloak_(1)"  , "SplitCloak"  },
			{"Right_Mothwing_Cloak_(1)" , "SplitCloak"  },
			{"Left_Shade_Cloak_(1)"     , "SplitCloak"  },
			{"Right_Shade_Cloak_(1)"    , "SplitCloak"  },

			{"Rancid_Egg_(0)"           , "Egg"         },
			{"Rancid_Egg_(1)"           , "Egg"         },
			{"Rancid_Egg_(2)"           , "Egg"         },
			{"Rancid_Egg_(3)"           , "Egg"         },
			{"Rancid_Egg_(4)"           , "Egg"         },
			{"Rancid_Egg_(5)"           , "Egg"         },
			{"Rancid_Egg_(6)"           , "Egg"         },
			{"Rancid_Egg_(7)"           , "Egg"         },
			{"Rancid_Egg_(8)"           , "Egg"         },
			{"Rancid_Egg_(9)"           , "Egg"         },
			{"Rancid_Egg_(10)"          , "Egg"         },
			{"Rancid_Egg_(11)"          , "Egg"         },
			{"Rancid_Egg_(12)"          , "Egg"         },
			{"Rancid_Egg_(13)"          , "Egg"         },
			{"Rancid_Egg_(14)"          , "Egg"         },
			{"Rancid_Egg_(15)"          , "Egg"         },
			{"Rancid_Egg_(16)"          , "Egg"         },
			{"Rancid_Egg_(17)"          , "Egg"         },
			{"Rancid_Egg_(18)"          , "Egg"         },
			{"Rancid_Egg_(19)"          , "Egg"         },

			{"Grub_(0)"                 , "Grub"        },
			{"Grub_(1)"                 , "Grub"        },
			{"Grub_(2)"                 , "Grub"        },
			{"Grub_(3)"                 , "Grub"        },
			{"Grub_(4)"                 , "Grub"        },
			{"Grub_(5)"                 , "Grub"        },
			{"Grub_(6)"                 , "Grub"        },
			{"Grub_(7)"                 , "Grub"        },
			{"Grub_(8)"                 , "Grub"        },
			{"Grub_(9)"                 , "Grub"        },
			{"Grub_(10)"                , "Grub"        },
			{"Grub_(11)"                , "Grub"        },
			{"Grub_(12)"                , "Grub"        },
			{"Grub_(13)"                , "Grub"        },
			{"Grub_(14)"                , "Grub"        },
			{"Grub_(15)"                , "Grub"        },
			{"Grub_(16)"                , "Grub"        },
			{"Grub_(17)"                , "Grub"        },
			{"Grub_(18)"                , "Grub"        },
			{"Grub_(19)"                , "Grub"        },
			{"Grub_(20)"                , "Grub"        },
			{"Grub_(21)"                , "Grub"        },
			{"Grub_(22)"                , "Grub"        },
			{"Grub_(23)"                , "Grub"        },
			{"Grub_(24)"                , "Grub"        },
			{"Grub_(25)"                , "Grub"        },
			{"Grub_(26)"                , "Grub"        },
			{"Grub_(27)"                , "Grub"        },
			{"Grub_(28)"                , "Grub"        },
			{"Grub_(29)"                , "Grub"        },
			{"Grub_(30)"                , "Grub"        },
			{"Grub_(31)"                , "Grub"        },
			{"Grub_(32)"                , "Grub"        },
			{"Grub_(33)"                , "Grub"        },
			{"Grub_(34)"                , "Grub"        },
			{"Grub_(35)"                , "Grub"        },
			{"Grub_(36)"                , "Grub"        },
			{"Grub_(37)"                , "Grub"        },
			{"Grub_(38)"                , "Grub"        },
			{"Grub_(39)"                , "Grub"        },
			{"Grub_(40)"                , "Grub"        },
			{"Grub_(41)"                , "Grub"        },
			{"Grub_(42)"                , "Grub"        },
			{"Grub_(43)"                , "Grub"        },
			{"Grub_(44)"                , "Grub"        },
			{"Grub_(45)"                , "Grub"        },

			{ "Mimic_Grub_(0)"          , "MimicItem"   },
			{ "Mimic_Grub_(1)"          , "MimicItem"   },
			{ "Mimic_Grub_(2)"          , "MimicItem"   },
			{ "Mimic_Grub_(3)"          , "MimicItem"   },
		};

		public static bool GetPlayerDataMapSetting(string mapArea)
		{
			return mapArea switch
			{
				"Ancient Basin" => PlayerData.instance.mapAbyss,
				"City of Tears" => PlayerData.instance.mapCity,
				"Howling Cliffs" => PlayerData.instance.mapCliffs,
				"Forgotten Crossroads" => PlayerData.instance.mapCrossroads,
				"Crystal Peak" => PlayerData.instance.mapMines,
				"Deepnest" => PlayerData.instance.mapDeepnest,
				"Dirtmouth" => PlayerData.instance.mapDirtmouth,
				"Fog Canyon" => PlayerData.instance.mapFogCanyon,
				"Fungal Wastes" => PlayerData.instance.mapFungalWastes,
				"Greenpath" => PlayerData.instance.mapGreenpath,
				"Kingdom's Edge" => PlayerData.instance.mapOutskirts,
				"Queen's Gardens" => PlayerData.instance.mapRoyalGardens,
				"Resting Grounds" => PlayerData.instance.mapRestingGrounds,
				"Royal Waterways" => PlayerData.instance.mapWaterways,
				_ => false,
			};
		}

		public static bool GetRandomizerSetting(PinGroup.GroupName group)
		{
			var randoSettings = RandomizerMod.RandomizerMod.RS.GenerationSettings;
			var popAllowed = randoSettings.LongLocationSettings.RandomizationInWhitePalace ==
				RandomizerMod.Settings.LongLocationSettings.WPSetting.Allowed;
			var palaceAllowed = randoSettings.LongLocationSettings.RandomizationInWhitePalace !=
				RandomizerMod.Settings.LongLocationSettings.WPSetting.ExcludeWhitePalace;
			System.Type randoSettingsType = randoSettings.GetType();
			switch (group)
			{
				case PinGroup.GroupName.Dreamer:
					return randoSettings.PoolSettings.Dreamers;

				case PinGroup.GroupName.Skill:
					return randoSettings.PoolSettings.Skills;

				case PinGroup.GroupName.Charm:
					return randoSettings.PoolSettings.Charms;

				case PinGroup.GroupName.Key:
					return randoSettings.PoolSettings.Keys;

				case PinGroup.GroupName.Geo:
					return randoSettings.PoolSettings.GeoChests;

				case PinGroup.GroupName.Junk:
					return randoSettings.PoolSettings.JunkPitChests;
				case PinGroup.GroupName.Mask:
					return randoSettings.PoolSettings.MaskShards;

				case PinGroup.GroupName.Vessel:
					return randoSettings.PoolSettings.VesselFragments;

				case PinGroup.GroupName.Notch:
					return randoSettings.PoolSettings.CharmNotches;

				case PinGroup.GroupName.Ore:
					return randoSettings.PoolSettings.PaleOre;

				case PinGroup.GroupName.Egg:
					return randoSettings.PoolSettings.RancidEggs;

				case PinGroup.GroupName.Relic:
					return randoSettings.PoolSettings.Relics;

				case PinGroup.GroupName.Map:
					return randoSettings.PoolSettings.Maps;

				case PinGroup.GroupName.Stag:
					return randoSettings.PoolSettings.Stags;

				case PinGroup.GroupName.Grub:
					return randoSettings.PoolSettings.Grubs;

				case PinGroup.GroupName.Mimic:
					return randoSettings.CursedSettings.RandomizeMimics;
				case PinGroup.GroupName.Root:
					return randoSettings.PoolSettings.WhisperingRoots;

				case PinGroup.GroupName.Rock:
					return randoSettings.PoolSettings.GeoRocks;

				case PinGroup.GroupName.BossGeo:
					return randoSettings.PoolSettings.BossGeo;

				case PinGroup.GroupName.Soul:
					return randoSettings.PoolSettings.SoulTotems;

				case PinGroup.GroupName.Lore:
					return randoSettings.PoolSettings.LoreTablets;

				case PinGroup.GroupName.PalaceSoul:
					return randoSettings.PoolSettings.SoulTotems && palaceAllowed;

				case PinGroup.GroupName.PalaceLore:
					return randoSettings.PoolSettings.LoreTablets && palaceAllowed;

				case PinGroup.GroupName.PalaceJournal:
					return randoSettings.PoolSettings.JournalEntries && palaceAllowed;
				case PinGroup.GroupName.PoPSoul:
					return randoSettings.PoolSettings.SoulTotems && popAllowed;

				case PinGroup.GroupName.PoPLore:
					return randoSettings.PoolSettings.LoreTablets && popAllowed;

				case PinGroup.GroupName.PoPJournal:
					return randoSettings.PoolSettings.JournalEntries && popAllowed;
				case PinGroup.GroupName.Cocoon:
					return randoSettings.PoolSettings.LifebloodCocoons;

				case PinGroup.GroupName.Flame:
					return randoSettings.PoolSettings.GrimmkinFlames;

				case PinGroup.GroupName.EssenceBoss:
					return randoSettings.PoolSettings.BossEssence;

				case PinGroup.GroupName.Journal:
					return randoSettings.PoolSettings.JournalEntries;

				case PinGroup.GroupName.CursedGeo:
					return randoSettings.CursedSettings.ReplaceJunkWithOneGeo;

				case PinGroup.GroupName.Shop:
					return false;
			}
			MapModS.Instance.LogWarn($"Unhandled GroupName: {group}");
			return false;
		}

		internal static string GetPoolFromClonedItem(string itemName)
		{
			return _clonedItemToPool[itemName];
		}

		internal static bool IsClonedItem(string itemName)
		{
			return _clonedItemToPool.ContainsKey(itemName);
		}
	}
}