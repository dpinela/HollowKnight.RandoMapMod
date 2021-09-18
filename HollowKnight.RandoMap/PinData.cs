//using RandoMapMod.VersionDiffs;
using System.Security.AccessControl;
using UnityEngine;

namespace RandoMapMod {

	[DebugName(nameof(PinData))]
	public class PinData {
		#region Constructors
		public PinData() {
			//Some of these things don't appear in the items.xml file, so I'll just set some defaults...
			this.HidePin = false;
			this.SceneName = "";
			this.OriginalName = "";
			this.LogicRaw = "";
			this.ObtainedBool = "";
			this.NewShiny = false;
			this.OffsetX = 0.0f;
			this.OffsetY = 0.0f;
			this.OffsetZ = 0.0f;
			this.VanillaPool = "";
		}
		#endregion

		#region Private Non-Methods
		//Assigned with pindata.xml
		public string ID {
			get;
			internal set;
		}
		public bool HidePin {
			get;
			internal set;
		}
		public string PinScene {
			get;
			internal set;
		}

		public string CheckBool {
			get;
			internal set;
		}
		public float OffsetX {
			get;
			internal set;
		}
		public float OffsetY {
			get;
			internal set;
		}
		public float OffsetZ {
			get;
			internal set;
		}

		//Assigned with Randomizer's items.xml:
		public string SceneName {
			get;
			internal set;
		}
		public string OriginalName {
			get;
			internal set;
		}
		public string LogicRaw {
			get;
			internal set;
		}
		public string ObtainedBool {
			get;
			internal set;
		}
		public bool InChest {
			get;
			internal set;
		}
		public bool NewShiny {
			get;
			internal set;
		}
		public int NewX {
			get;
			internal set;
		}
		public int NewY {
			get;
			internal set;
		}
		public string VanillaPool {
			get;
			internal set;
		}
		public string SpoilerPool {
			get;
			internal set;
		}

		public bool IsShop {
			get;
			internal set;
		}

		public Vector3 Offset => new Vector3(this.OffsetX, this.OffsetY, this.OffsetZ);
		#endregion

		#region <> Overrides
		public override string ToString() {
			return "Pin_" + this.ID;
		}
		#endregion
	}
}