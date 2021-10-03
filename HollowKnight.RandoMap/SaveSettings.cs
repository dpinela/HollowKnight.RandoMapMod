using SereCore;

namespace RandoMapMod
{
	public class SaveSettings : BaseSettings
	{
		public static SaveSettings Instance;

		public SaveSettings()
		{
			AfterDeserialize += () =>
			{

			};
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
				PinGroup.GroupName.Cocoon => CocoonOn,
				PinGroup.GroupName.Flame => FlameOn,
				PinGroup.GroupName.EssenceBoss => EssenceBossOn,
				PinGroup.GroupName.Journal => JournalOn,
				PinGroup.GroupName.Shop => ShopOn,
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
			}
		}

		public PinGroup.PinStyles PinStyle = PinGroup.PinStyles.Normal;

		public bool MapsGiven
		{
			get => GetBool(false);
			set => SetBool(value);
		}

		public bool RevealedMap
		{
			get => GetBool(false);
			set => SetBool(value);
		}

		public bool ShowAllPins
		{
			get => GetBool(false);
			set => SetBool(value);
		}

		public bool SpoilerOn
		{
			get => GetBool(false);
			set => SetBool(value);
		}

		public bool RandomizedOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool OthersOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool DreamerOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool SkillOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool CharmOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool KeyOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool GeoOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool JunkOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool MaskOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool VesselOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool NotchOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool OreOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool EggOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool RelicOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool MapOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool StagOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool GrubOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool MimicOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool RootOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool RockOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool BossGeoOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool SoulOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool LoreOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool PalaceSoulOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool PalaceLoreOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool PalaceJournalOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool CocoonOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool FlameOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool EssenceBossOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool JournalOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}

		public bool ShopOn
		{
			get => GetBool(true);
			set => SetBool(value);
		}
	}
}