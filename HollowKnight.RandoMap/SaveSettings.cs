namespace MapModS
{
	public class SaveSettings
	{
		public static SaveSettings Instance;

		public SaveSettings()
		{
			Instance = this;
		}

		public bool GetBoolFromGroup(PinGroup.GroupName group)
		{
			return group switch
			{
				PinGroup.GroupName.Dreamer => DreamerOn,
				PinGroup.GroupName.Skill => SkillOn,
				PinGroup.GroupName.Charm => CharmOn,
				PinGroup.GroupName.Key => KeyOn,
				PinGroup.GroupName.Geo => GeoOn,
				PinGroup.GroupName.Junk => JunkOn,
				PinGroup.GroupName.Mask => MaskOn,
				PinGroup.GroupName.Vessel => VesselOn,
				PinGroup.GroupName.Notch => NotchOn,
				PinGroup.GroupName.Ore => OreOn,
				PinGroup.GroupName.Egg => EggOn,
				PinGroup.GroupName.Relic => RelicOn,
				PinGroup.GroupName.Map => MapOn,
				PinGroup.GroupName.Stag => StagOn,
				PinGroup.GroupName.Grub => GrubOn,
				PinGroup.GroupName.Mimic => MimicOn,
				PinGroup.GroupName.Root => RootOn,
				PinGroup.GroupName.Rock => RockOn,
				PinGroup.GroupName.BossGeo => BossGeoOn,
				PinGroup.GroupName.Soul => SoulOn,
				PinGroup.GroupName.Lore => LoreOn,
				PinGroup.GroupName.PalaceSoul => PalaceSoulOn,
				PinGroup.GroupName.PalaceLore => PalaceLoreOn,
				PinGroup.GroupName.PalaceJournal => PalaceJournalOn,
				PinGroup.GroupName.PoPSoul => PoPSoulOn,
				PinGroup.GroupName.PoPLore => PoPLoreOn,
				PinGroup.GroupName.PoPJournal => PoPJournalOn,
				PinGroup.GroupName.Cocoon => CocoonOn,
				PinGroup.GroupName.Flame => FlameOn,
				PinGroup.GroupName.EssenceBoss => EssenceBossOn,
				PinGroup.GroupName.Journal => JournalOn,
				PinGroup.GroupName.Shop => ShopOn,
				PinGroup.GroupName.CursedGeo => CursedGeoOn,
				_ => false,
			};
		}

		public void SetBoolFromGroup(PinGroup.GroupName group, bool value)
		{
			switch (group)
			{
				case PinGroup.GroupName.Dreamer:
					DreamerOn = value;
					break;

				case PinGroup.GroupName.Skill:
					SkillOn = value;
					break;

				case PinGroup.GroupName.Charm:
					CharmOn = value;
					break;

				case PinGroup.GroupName.Key:
					KeyOn = value;
					break;

				case PinGroup.GroupName.Geo:
					GeoOn = value;
					break;

				case PinGroup.GroupName.Junk:
					JunkOn = value;
					break;

				case PinGroup.GroupName.Mask:
					MaskOn = value;
					break;

				case PinGroup.GroupName.Vessel:
					VesselOn = value;
					break;

				case PinGroup.GroupName.Notch:
					NotchOn = value;
					break;

				case PinGroup.GroupName.Ore:
					OreOn = value;
					break;

				case PinGroup.GroupName.Egg:
					EggOn = value;
					break;

				case PinGroup.GroupName.Relic:
					RelicOn = value;
					break;

				case PinGroup.GroupName.Map:
					MapOn = value;
					break;

				case PinGroup.GroupName.Stag:
					StagOn = value;
					break;

				case PinGroup.GroupName.Grub:
					GrubOn = value;
					break;

				case PinGroup.GroupName.Mimic:
					MimicOn = value;
					break;

				case PinGroup.GroupName.Root:
					RootOn = value;
					break;

				case PinGroup.GroupName.Rock:
					RockOn = value;
					break;

				case PinGroup.GroupName.BossGeo:
					BossGeoOn = value;
					break;

				case PinGroup.GroupName.Soul:
					SoulOn = value;
					break;

				case PinGroup.GroupName.Lore:
					LoreOn = value;
					break;

				case PinGroup.GroupName.PalaceSoul:
					PalaceSoulOn = value;
					break;

				case PinGroup.GroupName.PalaceLore:
					PalaceLoreOn = value;
					break;

				case PinGroup.GroupName.PalaceJournal:
					PalaceJournalOn = value;
					break;

				case PinGroup.GroupName.PoPSoul:
					PoPSoulOn = value;
					break;

				case PinGroup.GroupName.PoPLore:
					PoPLoreOn = value;
					break;

				case PinGroup.GroupName.PoPJournal:
					PoPJournalOn = value;
					break;

				case PinGroup.GroupName.Cocoon:
					CocoonOn = value;
					break;

				case PinGroup.GroupName.Flame:
					FlameOn = value;
					break;

				case PinGroup.GroupName.EssenceBoss:
					EssenceBossOn = value;
					break;

				case PinGroup.GroupName.Journal:
					JournalOn = value;
					break;

				case PinGroup.GroupName.Shop:
					ShopOn = value;
					break;

				case PinGroup.GroupName.CursedGeo:
					CursedGeoOn = value;
					break;
			}
		}

		public PinGroup.PinStyles PinStyle = PinGroup.PinStyles.Normal;

		public bool MapsGiven;

		public bool RevealedMap;

		public bool ShowAllPins;

		public bool SpoilerOn;

		public bool RandomizedOn = true;

		public bool OthersOn = true;

		public bool DreamerOn = true;

		public bool SkillOn = true;

		public bool CharmOn = true;

		public bool KeyOn = true;

		public bool GeoOn = true;

		public bool JunkOn = true;

		public bool MaskOn = true;

		public bool VesselOn = true;

		public bool NotchOn = true;

		public bool OreOn = true;

		public bool EggOn = true;

		public bool RelicOn = true;

		public bool MapOn = true;

		public bool StagOn = true;

		public bool GrubOn = true;

		public bool MimicOn = true;

		public bool RootOn = true;

		public bool RockOn = true;

		public bool BossGeoOn = true;

		public bool SoulOn = true;

		public bool LoreOn = true;

		public bool PalaceSoulOn = true;

		public bool PalaceLoreOn = true;

		public bool PalaceJournalOn = true;

		public bool PoPSoulOn = true;

		public bool PoPLoreOn = true;

		public bool PoPJournalOn = true;

		public bool CocoonOn = true;

		public bool FlameOn = true;

		public bool EssenceBossOn = true;

		public bool JournalOn = true;

		public bool ShopOn = true;

		public bool CursedGeoOn = true;

	}
}