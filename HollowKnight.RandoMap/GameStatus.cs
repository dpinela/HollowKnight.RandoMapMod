using System.Collections.Generic;
using System.Linq;

namespace RandoMapMod {
	[DebugName(nameof(GameStatus))]
	public static class GameStatus {
		#region Statics
		private static HelperLog.DataStore _HData => HelperLog.Data;

		private static readonly Dictionary<string, string> _shopItems = new Dictionary<string, string>() {
			//{"Gathering Swarm", "Sly"},
			//{"Stalwart Shell", "Sly"},
			//{"Lumafly Lantern", "Sly"},
			//{"Simple Key-Sly", "Sly"},
			//{"Mask Shard-Sly1", "Sly"},
			//{"Mask Shard-Sly2", "Sly"},
			//{"Vessel Fragment-Sly1", "Sly"},
			//{"Rancid Egg-Sly", "Sly"},

			//{"Heavy Blow", "Sly (Key)"},
			//{"Sprintmaster", "Sly (Key)"},
			//{"Elegant Key", "Sly (Key)"},

			//{"Wayward Compass", "Iselda"},

			//{"Quick Focus", "Salubra"},
			//{"Lifeblood Heart", "Salubra"},
			//{"Steady Body", "Salubra"},
			//{"Long Nail", "Salubra"},
			//{"Shaman Stone", "Salubra"},

			//{"Fragile Heart", "Leg Eater"},
			//{"Fragile Greed", "Leg Eater"},
			//{"Fragile Strength", "Leg Eater"},

			//{"Mask Shard-5 Grubs", "Grubfather"},
			//{"Pale Ore-Grubs", "Grubfather"},
			//{"Rancid Egg-Grubs", "Grubfather"},
			//{"Hallownest Seal-Grubs", "Grubfather"},
			//{"King's Idol-Grubs", "Grubfather"},
			//{"Grubsong", "Grubfather"},
			//{"Grubberfly's Elegy", "Grubfather"},

			//{"Arcane Egg-Seer", "Seer"},
			//{"Vessel Fragment-Seer", "Seer"},
			//{"Pale Ore-Seer", "Seer"},
			//{"Hallownest Seal-Seer", "Seer"},
			//{"Mask Shard-Seer", "Seer"},
			//{"Dream Gate", "Seer"},
			//{"Awoken Dream Nail", "Seer"},
			//{"Dream Wielder", "Seer"},

			{"Gathering_Swarm", "Sly"},
			{"Stalwart_Shell", "Sly"},
			{"Lumafly_Lantern", "Sly"},
			{"Simple_Key-Sly", "Sly"},
			{"Mask_Shard-Sly1", "Sly"},
			{"Mask_Shard-Sly2", "Sly"},
			{"Vessel_Fragment-Sly1", "Sly"},
			{"Rancid_Egg-Sly", "Sly"},

			{"Heavy_Blow", "Sly (Key)"},
			{"Sprintmaster", "Sly (Key)"},
			{"Elegant_Key", "Sly (Key)"},

			{"Wayward_Compass", "Iselda"},

			{"Quick_Focus", "Salubra"},
			{"Lifeblood_Heart", "Salubra"},
			{"Steady_Body", "Salubra"},
			{"Longnail", "Salubra"},
			{"Shaman_Stone", "Salubra"},

			{"Fragile_Heart", "Leg_Eater"},
			{"Fragile_Greed", "Leg_Eater"},
			{"Fragile_Strength", "Leg_Eater"},

			{"Mask_Shard-5_Grubs", "Grubfather"},
			{"Pale_Ore-Grubs", "Grubfather"},
			{"Rancid_Egg-Grubs", "Grubfather"},
			{"Hallownest_Seal-Grubs", "Grubfather"},
			{"King's_Idol-Grubs", "Grubfather"},
			{"Grubsong", "Grubfather"},
			{"Grubberfly's_Elegy", "Grubfather"},

			{"Arcane_Egg-Seer", "Seer"},
			{"Vessel_Fragment-Seer", "Seer"},
			{"Pale_Ore-Seer", "Seer"},
			{"Hallownest_Seal-Seer", "Seer"},
			{"Mask_Shard-Seer", "Seer"},
			{"Dream_Gate", "Seer"},
			{"Awoken_Dream_Nail", "Seer"},
			{"Dream_Wielder", "Seer"},

			// Added these
			{"Mask_Shard-Sly3", "Sly (Key)"},
			{"Mask_Shard-Sly4", "Sly (Key)"},
			{"Vessel_Fragment-Sly2", "Sly (Key)"},
		};

		private static readonly Dictionary<string, string> _otherMajorItems = new Dictionary<string, string>() {
			{"Dreamer_(1)", "Dreamer"},

			{"Mothwing_Cloak_(1)", "Skill"},
			{"Mantis_Claw_(1)", "Skill"},
			{"Crystal_Heart_(1)", "Skill"},
			{"Monarch_Wings_(1)", "Skill"},
			{"Shade_Cloak_(1)", "Skill"},
			{"Isma's_Tear_(1)", "Skill"},
			{"Dream_Nail_(1)", "Skill"},

			{"Vengeful_Spirit_(1)", "Spell"},
			{"Desolate_Dive_(1)", "Spell"},
			{"Howling_Wraiths_(1)", "Spell"},

			{"Void_Heart_(1)", "Charm"},

			// Cursed or split
			{"Focus", "Skill"},
			{"Left_Mothwing_Cloak", "Skill"},
			{"Left_Mothwing_Cloak_(1)", "Skill"},
			{"Right_Mothwing_Cloak", "Skill" },
			{"Right_Mothwing_Cloak_(1)", "Skill" },
			{"Left_Shade_Cloak", "Skill" },
			{"Left_Shade_Cloak_(1)", "Skill" },
			{"Right_Shade_Cloak", "Skill" },
			{"Right_Shade_Cloak_(1)", "Skill" },
			{"Split_Mothwing_Cloak", "Skill" },
			{"Left_Mantis_Claw", "Skill" },
			{"Right_Mantis_Claw", "Skill" },
			{"Leftslash", "Skill" },
			{"Rightslash", "Skill" },
			{"Upslash", "Skill" }
		};

		// These items were added in the current beta of RandomizerMod (v3.12c(884))
		private static readonly Dictionary<string, string> _newItems = new Dictionary<string, string>() {
			{"Swim", "Skill"},
			{"Mask_Shard-Start", "Mask"},
			{"Mask_Shard-Start_2", "Mask"},
			{"Mask_Shard-Start_3", "Mask"},
			{"Mask_Shard-Start_4", "Mask"},
			{"Mask_Shard-Start_5", "Mask"},
			{"Mask_Shard-Start_6", "Mask"},
			{"Mask_Shard-Start_7", "Mask"},
			{"Mask_Shard-Start_8", "Mask"},
			{"Mask_Shard-Start_9", "Mask"},
			{"Mask_Shard-Start_10", "Mask"},
			{"Mask_Shard-Start_11", "Mask"},
			{"Mask_Shard-Start_12", "Mask"},
			{"Mask_Shard-Start_13", "Mask"},
			{"Mask_Shard-Start_14", "Mask"},
			{"Mask_Shard-Start_15", "Mask"},
			{"Mask_Shard-Start_16", "Mask"},
			{"Charm_Notch-Start", "Notch" },
			{"Charm_Notch-Start_2", "Notch" },
			{"8_Geo-Junk_Pit_Chest_1", "Geo" },
			{"8_Geo-Junk_Pit_Chest_2", "Geo" },
			{"25_Geo-Junk_Pit_Chest_3", "Geo" },
			{"Lumafly_Escape-Junk_Pit_Chest_4", "Geo" }, // Not sure about this... ?
			{"10_Geo-Junk_Pit_Chest_5", "Geo" },
			{"Mimic_Grub-Deepnest_1", "Grub" },
			{"Mimic_Grub-Deepnest_2", "Grub" },
			{"Mimic_Grub-Deepnest_3", "Grub" },
			{"Mimic_Grub-Crystal_Peak", "Grub" },
			{"Grub", "Grub" },
			{"Mimic_Grub", "Mimic_Grub" },
			{"Hunter's_Journal", "Lore" }, // Not sure about these... maybe should have a Journal pin type?
			{"Journal_Entry-Void_Tendrils", "Lore" },
			{"Journal_Entry-Charged_Lumafly", "Lore" },
			{"Journal_Entry-Goam", "Lore" },
			{"Journal_Entry-Garpede", "Lore" },
			{"Journal_Entry-Seal_of_Binding", "Lore" },
			{"Elevator_Pass", "Key" },
			{"450_Geo-Egg_Shop_1", "Geo" },
			{"450_Geo-Egg_Shop_2", "Geo" },
			{"450_Geo-Egg_Shop_3", "Geo" },
			{"450_Geo-Egg_Shop_4", "Geo" },
			{"450_Geo-Egg_Shop_5", "Geo" },
			{"Rancid_Egg", "Egg" },
			{"Rancid_Egg_(0)", "Egg" },
			{"Rancid_Egg_(1)", "Egg" },
			{"Rancid_Egg_(2)", "Egg" },
			{"Rancid_Egg_(3)", "Egg" },
			{"Rancid_Egg_(4)", "Egg" },
			{"Rancid_Egg_(5)", "Egg" },
			{"Rancid_Egg_(6)", "Egg" },
			{"Rancid_Egg_(7)", "Egg" },
			{"Rancid_Egg_(8)", "Egg" },
			{"Rancid_Egg_(9)", "Egg" },
			{"Rancid_Egg_(10)", "Egg" },
			{"Rancid_Egg_(11)", "Egg" },
			{"Rancid_Egg_(12)", "Egg" },
			{"Rancid_Egg_(13)", "Egg" },
			{"Rancid_Egg_(14)", "Egg" },
			{"Rancid_Egg_(15)", "Egg" },
			{"Rancid_Egg_(16)", "Egg" },
			{"Rancid_Egg_(17)", "Egg" },
			{"Rancid_Egg_(18)", "Egg" },
			{"Rancid_Egg_(19)", "Egg" },
		};
		public static bool ItemIsChecked(string itemName) {
			if (_HData == null) {
				return false;
			}
			return _HData.HasChecked(itemName);
		}

		public static bool ItemIsReachable(string itemName) {
			//return MapMod.VersionController.CanGet(itemName);

			//return false;
			string cleanName = itemName.Replace('_', ' ');
			if (_HData == null) {
				return false;
			}

			if (_HData.CanReach(cleanName)) {
				return true;
			}

			if (_shopItems.ContainsKey(cleanName)) {
				/*
				 * If this is a shop item, we need to say it's reachable whether the item
				 * is in HelperData's "checked" or "reachable", or else after the player
				 * checks the shop once, the pins will all shrink.
				 */
				string shopName = _shopItems[cleanName];
				if (_HData.CheckedShopItems != null && _HData.CheckedShopItems.Contains(shopName)) {
					return true;
				}
				if (_HData.ReachableShopItems != null && _HData.ReachableShopItems.Contains(shopName)) {
					return true;
				}
			}
			return false;
		}

		//public static bool ItemPrereqsAreMet(string itemName) {
		//	string cleanName = itemName.Replace('_', ' ');

		//	//DebugLog.Log($"Checking if {this.ID} has its prereqs met...");
		//	int cost = 0;
		//	(string, int)[] costs = RandomizerMod.RandomizerMod.Instance.Settings.VariableCosts;
		//	for (int i = 0; i < costs.Length; i++) {
		//		if (costs[i].Item1 == itemName) {
		//			cost = costs[i].Item2;
		//			break;
		//		}
		//	}
		//	if (cost == 0) {
		//		DebugLog.Log($"Cost for {itemName} was zero, so marking as prereqs met.");
		//		return true;
		//	}
		//	if (GameStatus.IsGrubFatherItem(cleanName)) {
		//		bool retVal = PlayerData.instance.grubsCollected > cost;
		//		//DebugLog.Log($"{this.ID} is a grubfather item, and  {PlayerData.instance.grubsCollected} > {cost} == {retVal}.");
		//		return retVal;
		//	}
		//	if (GameStatus.IsSeerItem(cleanName)) {
		//		bool retVal = PlayerData.instance.dreamOrbs > cost;
		//		//DebugLog.Log($"{this.ID} is a Seer item, and  {PlayerData.instance.dreamOrbs} > {cost} == {retVal}.");
		//		return retVal;
		//	}
		//	DebugLog.Log($"{itemName} returning false by default.");
		//	return false;
		//}

		//public static bool IsGrubFatherItem(string itemName) {
		//	return "Grubfather".Equals(_shopItems[itemName]);
		//}

		//public static bool IsSeerItem(string itemName) {
		//	return "Seer".Equals(_shopItems[itemName]);
		//}
		#endregion

		#region Public Methods
		public static bool IsShopItem(string itemName) {
			return _shopItems.ContainsKey(itemName);
		}
		public static bool IsOtherMajorItem(string itemName) {
			return _otherMajorItems.ContainsKey(itemName);
		}
		public static bool IsNewItem(string itemName) {
			return _newItems.ContainsKey(itemName);
		}

		public static string GetShopItemPool(string itemName) {
			return _shopItems[itemName];
		}

		public static string GetOtherMajorItemPool(string itemName) {
			return _otherMajorItems[itemName];
		}

		public static string GetNewItemPool(string itemName) {
			return _newItems[itemName];
		}
		#endregion
	}
}