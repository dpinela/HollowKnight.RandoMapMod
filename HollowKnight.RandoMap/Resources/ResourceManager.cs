using System.Collections.Generic;

namespace RandoMapMod {
	[DebugName(nameof(ResourceManager))]

	// This class handles loading:
	// - Data from the RandomizerMod xml files
	// - This Mod's xml file
	// - The Pin sprites
	static class ResourceManager {
		private static readonly Dictionary<string, string> _areatoMapArea = new Dictionary<string, string>
		{
			{"Abyss"				, "Ancient_Basin"		},
			{"Ancient_Basin"		, "Ancient_Basin"		},
			{"Palace_Grounds"       , "Ancient_Basin"       },
			{"City_of_Tears"		, "City_of_Tears"		},
			{"Kings_Station"        , "City_of_Tears"       },
			{"Pleasure_House"       , "City_of_Tears"       },
			{"Soul_Sanctum"			, "City_of_Tears"		},
			{"Tower_of_Love"        , "City_of_Tears"       },
			{"Crystallized_Mound"	, "Crystal_Peak"		},
			{"Crystal_Peak"			, "Crystal_Peak"		},
			{"Beasts_Den"			, "Deepnest"			},
			{"Deepnest"				, "Deepnest"            },
			{"Distant_Village"      , "Deepnest"            },
			{"Failed_Tramway"       , "Deepnest"            },
			{"Weavers_Den"          , "Deepnest"            },
			{"Dirtmouth"			, "Dirtmouth"           },
			{"Kings_Pass"			, "Dirtmouth"			},
			{"Fog_Canyon"           , "Fog_Canyon"          },
			{"Overgrown_Mound"		, "Fog_Canyon"			},
			{"Teachers_Archives"	, "Fog_Canyon"			},
			{"Fungal_Core"          , "Fungal_Wastes"       },
			{"Fungal_Wastes"		, "Fungal_Wastes"       },
			{"Mantis_Village"		, "Fungal_Wastes"		},
			{"Queens_Station"       , "Fungal_Wastes"       },
			{"Ancestral_Mound"		, "Forgotten_Crossroads"},
			{"Black_Egg_Temple"		, "Forgotten_Crossroads"},
			{"Forgotten_Crossroads"	, "Forgotten_Crossroads"},
			{"Greenpath"			, "Greenpath"			},
			{"Lake_of_Unn"			, "Greenpath"           },
			{"Stone_Sanctuary"		, "Greenpath"			},
			{"Howling_Cliffs"		, "Howling_Cliffs"		},
			{"Stag_Nest"            , "Howling_Cliffs"		},
			{"Cast_Off_Shell"       , "Kingdoms_Edge"       },
			{"Colosseum"            , "Kingdoms_Edge"       },
			{"Hallownests_Crown"    , "Kingdoms_Edge"       },
			{"Hive"                 , "Kingdoms_Edge"       },
			{"Kingdoms_Edge"		, "Kingdoms_Edge"		},
			{"Queens_Gardens"       , "Queens_Gardens"      },
			{"Blue_Lake"            , "Resting_Grounds"     },
			{"Resting_Grounds"		, "Resting_Grounds"		},
			{"Spirits_Glade"        , "Resting_Grounds"     },
			{"Ismas_Grove"			, "Royal_Waterways"		},
			{"Junk_Pit"             , "Royal_Waterways"     },
			{"Royal_Waterways"		, "Royal_Waterways"     },
			{"Inventory"			, ""					},
			{""                     , ""					},
		};
		public static bool IsAnArea (string areaName) {
			if (_areatoMapArea.ContainsKey(areaName)) {
				return true;
			}
			return false;
		}
		public static bool IsAMapArea (string mapAreaName) {
			if (_areatoMapArea.ContainsValue(mapAreaName)) {
				return true;
			}
			return false;
		}
		public static string GetMapAreaFromArea (string areaName) {
			return _areatoMapArea[areaName];
		}
	}
}
