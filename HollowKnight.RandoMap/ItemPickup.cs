using System;
using JetBrains.Annotations;
using MonoMod.RuntimeDetour;
using GiveItemActions = RandomizerMod.GiveItemActions;

namespace RandoMapMod
{
	// This class triggers updating the Pins based on if items are checked, using a hook to RandomizerMod's item pickup event
	// Unfortunately there is no easy way to do any of this for non-randomized items...
	// On game load, we can look at RandoLogger.obtainedItems for previously checked items
	public class ItemPickup
	{
		public delegate void OnItemPickupHandler(string itemName);

		public static OnItemPickupHandler OnItemPickup;
		public ItemPickup() => HookRando();

		private Hook _updatePinHook;

		public void HookRando()
		{
			Type giveItemActions = Type.GetType("RandomizerMod.GiveItemActions, RandomizerMod3.0");

			if (giveItemActions == null)
				return;

			DebugLog.Log("Hooking GiveItemActions");

			_updatePinHook = new Hook
			(
				giveItemActions.GetMethod("GiveItem"),
				typeof(ItemPickup).GetMethod(nameof(HidePin))
			);

		}

		[UsedImplicitly]
		public static void HidePin
		(
			Action<GiveItemActions.GiveAction, string, string, int> orig,
			GiveItemActions.GiveAction action,
			string item,
			string location,
			int geo
		)
		{
			orig(action, item, location, geo);
			// item -> spoilerItem
			// location -> vanillaItem
			MapModS.Mm.PinGroupInstance.UpdatePins();
		}
	}
}
