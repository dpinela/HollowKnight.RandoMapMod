﻿using UnityEngine;
//using System.Diagnostics.Eventing.Reader;
using System.Collections;
using System;
using System.Collections.Generic;
//using RandoMapMod.BoringInternals;

namespace RandoMapMod.UnityComponents {
	class InputListener : MonoBehaviour {
		#region Statics
		private static GameObject _instance_GO = null;

		public static InputListener Instance {
			get {
				InstantiateSingleton();
				return _instance_GO.GetComponent<InputListener>();
			}
		}

		public static void InstantiateSingleton() {
			if (_instance_GO == null) {
				_instance_GO = GameObject.Find("RandoMapInputListener");
				if (_instance_GO == null) {
					DebugLog.Log("Adding Input Listener.");
					_instance_GO = new GameObject("RandoMapInputListener");
					_instance_GO.AddComponent<InputListener>();
					DontDestroyOnLoad(_instance_GO);
				}
			}
		}
		#endregion

		#region Private Non-Methods
		#endregion

		#region MonoBehaviour "Overrides"
		protected void Update() {
			if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
				if (Input.GetKeyDown(KeyCode.Alpha1)) {
					DebugLog.Log("Ctrl+1 : Toggle Group 1");
					MapModS.ToggleGroup1();
				}

				if (Input.GetKeyDown(KeyCode.Alpha2)) {
					DebugLog.Log("Ctrl+2 : Toggle Group 2");
					MapModS.ToggleGroup2();
				}

				if (Input.GetKeyDown(KeyCode.Alpha3)) {
					DebugLog.Log("Ctrl+3 : Toggle Group 3");
					MapModS.ToggleGroup3();
				}

				if (Input.GetKeyDown(KeyCode.Alpha4)) {
					DebugLog.Log("Ctrl+4 : Toggle Group 4");
					MapModS.ToggleGroup4();
				}

				if (Input.GetKeyDown(KeyCode.Alpha5)) {
					DebugLog.Log("Ctrl+5 : Toggle Group 5");
					MapModS.ToggleGroup5();
				}

				if (Input.GetKeyDown(KeyCode.Alpha6)) {
					DebugLog.Log("Ctrl+6 : Toggle Group 6");
					MapModS.ToggleGroup6();
				}

				if (Input.GetKeyDown(KeyCode.M)) {
					DebugLog.Log("Ctrl+M : Give All Maps");
					MapModS.GiveAllMaps("Hotkey");
				}

				if (Input.GetKeyDown(KeyCode.P)) {
					DebugLog.Log("Ctrl+P : Toggle Pins");
					MapModS.ToggleAllPins();
				}

				if (Input.GetKeyDown(KeyCode.R)) {
					DebugLog.Log("Ctrl+R : Toggle Unknown Pins");
					MapModS.ToggleUnknownPins();
				}

				if (Input.GetKeyDown(KeyCode.T)) {
					DebugLog.Log("Ctrl+T : Toggle Vanilla/Spoiler Pins");
					MapModS.ToggleRandoPins();
				}

#if DEBUG
				// Used for various debugging tasks
				if (Input.GetKeyDown(KeyCode.O)) {
					MapModS.ReloadGameMapPins();
				}
#endif
			}
		}
#endregion

#region Non-Private Methods
#endregion
	}
}
