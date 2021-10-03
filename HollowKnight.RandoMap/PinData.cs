using UnityEngine;

namespace RandoMapMod
{
	public class PinData
	{
		public PinData()
		{
			this.NotPin = false;
			this.OffsetX = 0.0f;
			this.OffsetY = 0.0f;
			this.OffsetZ = 0.0f;
			this.ObjectName = "";
			this.SceneName = "";
			this.MapArea = "";
			this.VanillaPool = "";
		}

		//Assigned with pindata.xml
		public string ID
		{
			get;
			internal set;
		}

		public bool NotPin
		{
			get;
			internal set;
		}

		public string PinScene
		{
			get;
			internal set;
		}

		public float OffsetX
		{
			get;
			internal set;
		}

		public float OffsetY
		{
			get;
			internal set;
		}

		public float OffsetZ
		{
			get;
			internal set;
		}

		public bool IsShop
		{
			get;
			internal set;
		}

		//Assigned with Randomizer's items.xml:
		public string SceneName
		{
			get;
			internal set;
		}

		public string MapArea
		{
			get;
			internal set;
		}

		public string VanillaPool
		{
			get;
			internal set;
		}

		public string SpoilerPool
		{
			get;
			internal set;
		}

		// These are used to track unrandomized items. pindata.xml will overwrite when needed
		public string ObjectName
		{
			get;
			internal set;
		}

		public Vector3 Offset => new Vector3(this.OffsetX, this.OffsetY, this.OffsetZ);

		public override string ToString()
		{
			return "Pin_" + this.ID;
		}
	}
}