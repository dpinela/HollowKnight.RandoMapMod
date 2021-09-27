using System.Collections.Generic;
using System.Reflection;

namespace RandoMapMod {

	// This class has some useful Get methods / value lookups
	public static class Dictionaries {

		// Used for figuring out which MapArea a Pin belongs to (for Quick Map)
		private static readonly Dictionary<string, string> _areatoMapArea = new Dictionary<string, string>
		{
			{"Abyss"                , "Ancient_Basin"       },
			{"Ancient_Basin"        , "Ancient_Basin"       },
			{"Palace_Grounds"       , "Ancient_Basin"       },
			{"City_of_Tears"        , "City_of_Tears"       },
			{"Kings_Station"        , "City_of_Tears"       },
			{"Pleasure_House"       , "City_of_Tears"       },
			{"Soul_Sanctum"         , "City_of_Tears"       },
			{"Tower_of_Love"        , "City_of_Tears"       },
			{"Crystallized_Mound"   , "Crystal_Peak"        },
			{"Crystal_Peak"         , "Crystal_Peak"        },
			{"Hallownests_Crown"    , "Crystal_Peak"        },
			{"Beasts_Den"           , "Deepnest"            },
			{"Deepnest"             , "Deepnest"            },
			{"Distant_Village"      , "Deepnest"            },
			{"Failed_Tramway"       , "Deepnest"            },
			{"Weavers_Den"          , "Deepnest"            },
			{"Dirtmouth"            , "Dirtmouth"           },
			{"Kings_Pass"           , "Dirtmouth"           },
			{"Fog_Canyon"           , "Fog_Canyon"          },
			{"Overgrown_Mound"      , "Fog_Canyon"          },
			{"Teachers_Archives"    , "Fog_Canyon"          },
			{"Fungal_Core"          , "Fungal_Wastes"       },
			{"Fungal_Wastes"        , "Fungal_Wastes"       },
			{"Mantis_Village"       , "Fungal_Wastes"       },
			{"Queens_Station"       , "Fungal_Wastes"       },
			{"Ancestral_Mound"      , "Forgotten_Crossroads"},
			{"Black_Egg_Temple"     , "Forgotten_Crossroads"},
			{"Forgotten_Crossroads" , "Forgotten_Crossroads"},
			{"Greenpath"            , "Greenpath"           },
			{"Lake_of_Unn"          , "Greenpath"           },
			{"Stone_Sanctuary"      , "Greenpath"           },
			{"Howling_Cliffs"       , "Howling_Cliffs"      },
			{"Stag_Nest"            , "Howling_Cliffs"      },
			{"Cast_Off_Shell"       , "Kingdoms_Edge"       },
			{"Colosseum"            , "Kingdoms_Edge"       },
			{"Hive"                 , "Kingdoms_Edge"       },
			{"Kingdoms_Edge"        , "Kingdoms_Edge"       },
			{"Queens_Gardens"       , "Queens_Gardens"      },
			{"Blue_Lake"            , "Resting_Grounds"     },
			{"Resting_Grounds"      , "Resting_Grounds"     },
			{"Spirits_Glade"        , "Resting_Grounds"     },
			{"Ismas_Grove"          , "Royal_Waterways"     },
			{"Junk_Pit"             , "Royal_Waterways"     },
			{"Royal_Waterways"      , "Royal_Waterways"     },
			{"Inventory"            , ""                    },
			{""                     , ""                    },
		};

		// These items are not in RandomizerMod's items.xml but are created during randomization. Cursed Geo is handled by string check instead
		private static readonly Dictionary<string, string> _clonedItemToPool = new Dictionary<string, string>() {
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

		public static bool GetPlayerDataMapSetting(string mapArea) {
			return mapArea switch {
				"Ancient_Basin" => PlayerData.instance.mapAbyss,
				"City_of_Tears" => PlayerData.instance.mapCity,
				"Howling_Cliffs" => PlayerData.instance.mapCliffs,
				"Forgotten_Crossroads" => PlayerData.instance.mapCrossroads,
				"Crystal_Peak" => PlayerData.instance.mapMines,
				"Deepnest" => PlayerData.instance.mapDeepnest,
				"Dirtmouth" => PlayerData.instance.mapDirtmouth,
				"Fog_Canyon" => PlayerData.instance.mapFogCanyon,
				"Fungal_Wastes" => PlayerData.instance.mapFungalWastes,
				"Greenpath" => PlayerData.instance.mapGreenpath,
				"Kingdom's_Edge" => PlayerData.instance.mapOutskirts,
				"Queens_Gardens" => PlayerData.instance.mapRoyalGardens,
				"Resting_Grounds" => PlayerData.instance.mapRestingGrounds,
				"Royal_Waterways" => PlayerData.instance.mapWaterways,
				_ => false,
			};
		}

		public static bool GetRandomizerSetting(PinGroup.GroupName group) {
			object randoSettings = RandomizerMod.RandomizerMod.Instance.Settings;
			System.Type randoSettingsType = randoSettings.GetType();
			switch (group) {
				case PinGroup.GroupName.Dreamer:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeDreamers;

				case PinGroup.GroupName.Skill:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeSkills;

				case PinGroup.GroupName.Charm:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeCharms;

				case PinGroup.GroupName.Key:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeKeys;

				case PinGroup.GroupName.Geo:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeGeoChests;

				case PinGroup.GroupName.Junk:
					// For compatibility with RandomizerMod v3.12(573)
					PropertyInfo junkSetting = randoSettingsType.GetProperty("RandomizeJunkPitChests");
					if (junkSetting != null) {
						return (bool) junkSetting.GetValue(randoSettings, null);
					} else {
						return false;
					}
				case PinGroup.GroupName.Mask:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeMaskShards;

				case PinGroup.GroupName.Vessel:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeVesselFragments;

				case PinGroup.GroupName.Notch:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeCharmNotches;

				case PinGroup.GroupName.Ore:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizePaleOre;

				case PinGroup.GroupName.Egg:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeRancidEggs;

				case PinGroup.GroupName.Relic:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeRelics;

				case PinGroup.GroupName.Map:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeMaps;

				case PinGroup.GroupName.Stag:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeStags;

				case PinGroup.GroupName.Grub:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeGrubs;

				case PinGroup.GroupName.Mimic:
					// For compatibility with RandomizerMod v3.12(573)
					PropertyInfo mimicSetting = randoSettingsType.GetProperty("RandomizeMimics");
					if (mimicSetting != null) {
						return (bool) mimicSetting.GetValue(randoSettings, null);
					} else {
						return false;
					}
				case PinGroup.GroupName.Root:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeWhisperingRoots;

				case PinGroup.GroupName.Rock:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeRocks;

				case PinGroup.GroupName.BossGeo:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeBossGeo;

				case PinGroup.GroupName.Soul:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeSoulTotems;

				case PinGroup.GroupName.Lore:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeLoreTablets;

				case PinGroup.GroupName.PalaceSoul:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizePalaceTotems;

				case PinGroup.GroupName.PalaceLore:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizePalaceTablets;

				case PinGroup.GroupName.PalaceJournal:
					// For compatibility with RandomizerMod v3.12(573)
					PropertyInfo pJournalSetting = randoSettingsType.GetProperty("RandomizePalaceEntries");
					if (pJournalSetting != null) {
						return (bool) pJournalSetting.GetValue(randoSettings, null);
					} else {
						return false;
					}
				case PinGroup.GroupName.Cocoon:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeLifebloodCocoons;

				case PinGroup.GroupName.Flame:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeGrimmkinFlames;

				case PinGroup.GroupName.EssenceBoss:
					return RandomizerMod.RandomizerMod.Instance.Settings.RandomizeBossEssence;

				case PinGroup.GroupName.Journal:
					// For compatibility with RandomizerMod v3.12(573)
					PropertyInfo journalSetting = randoSettingsType.GetProperty("RandomizeJournalEntries");
					if (journalSetting != null) {
						return (bool) journalSetting.GetValue(randoSettings, null);
					} else {
						return false;
					}
				case PinGroup.GroupName.Shop:
					return false;
			}
			MapModS.Instance.LogWarn($"Unhandled GroupName: {group}");
			return false;
		}

		internal static string GetMapAreaFromArea(string areaName) {
			return _areatoMapArea[areaName];
		}

		internal static string GetPoolFromClonedItem(string itemName) {
			return _clonedItemToPool[itemName];
		}

		internal static bool IsArea(string areaName) {
			if (_areatoMapArea.ContainsKey(areaName)) {
				return true;
			}
			return false;
		}

		internal static bool IsClonedItem(string itemName) {
			return _clonedItemToPool.ContainsKey(itemName);
		}

		internal static bool IsMapArea(string mapAreaName) {
			if (_areatoMapArea.ContainsValue(mapAreaName)) {
				return true;
			}
			return false;
		}
	}
}