using UnityEngine;

namespace RandoMapMod.UnityComponents
{
	// This class handles hotkey behaviour
	internal class InputListener : MonoBehaviour
	{
		private static GameObject _instance_GO = null;

		public static void InstantiateSingleton()
		{
			if (_instance_GO == null)
			{
				_instance_GO = GameObject.Find("RandoMapInputListener");
				if (_instance_GO == null)
				{
					MapModS.Instance.Log("Adding Input Listener.");
					_instance_GO = new GameObject("RandoMapInputListener");
					_instance_GO.AddComponent<InputListener>();
					DontDestroyOnLoad(_instance_GO);
				}
			}
		}

		public static void DestroySingleton()
		{
			if (_instance_GO != null)
			{
				Destroy(_instance_GO);
			}
		}

		protected void Update()
		{
			if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			{
				if (Input.GetKeyDown(KeyCode.M))
				{
					MapModS.EnableMapMod("Hotkey");
				}

				if (MapModS.Instance.Settings.MapsGiven)
				{
					if (Input.GetKeyDown(KeyCode.Alpha1))
					{
						MapModS.Instance.PinGroupInstance.ToggleSpoilers();
					}

					if (Input.GetKeyDown(KeyCode.Alpha2))
					{
						MapModS.Instance.PinGroupInstance.TogglePinStyle();
					}

					if (Input.GetKeyDown(KeyCode.Alpha3))
					{
						MapModS.Instance.PinGroupInstance.ToggleRandomized();
					}

					if (Input.GetKeyDown(KeyCode.Alpha4))
					{
						MapModS.Instance.PinGroupInstance.ToggleOthers();
					}
				}

				//Used for various debugging tasks
				//if (Input.GetKeyDown(KeyCode.O)) {
				//	//MapModS.ReloadGameMapPins();
				//	MapModS.GetAllActiveObjects();
				//}
			}
		}
	}
}