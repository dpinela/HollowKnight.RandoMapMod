using HutongGames.PlayMaker;
using Modding;
using SereCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace RandoMapMod {
	public static class Unrandomized {
		public static void Hook() {
			On.PlayMakerFSM.OnEnable += _UnrandomizedFsmItems;
			On.HealthCocoon.SetBroken += _LifebloodCocoon;
		}

		private static void _UnrandomizedFsmItems(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
			orig(self);

			if (self.FsmName == "Geo Rock") {
				FsmState state = self.GetState("Destroy");
				SereCore.PlayMakerExtensions.AddFirstAction(state, new TrackObject(self));
			} else if (self.FsmName == "Inspection") {
				FsmState state = self.GetState("Take Control");
				SereCore.PlayMakerExtensions.AddFirstAction(state, new TrackObject(self));
			} else {
				if (self.gameObject.name == "Health Cocoon") {
					foreach (FsmState state in self.FsmStates) {
						MapModS.Instance.Log(state.Name);
					}
				}
				//FsmState state = self.GetState("Destroy");
				//if (state != null) {
				//	//MapModS.Instance.Log(self.FsmName);
				//	//MapModS.Instance.Log(self.gameObject.name);
				//}
			}
		}

		private class TrackObject : FsmStateAction {
			private readonly PlayMakerFSM _thisFsm;
			object mapSettings;
			Type mapSettingsType;
			MethodInfo AddObtainedLocation;

			public TrackObject(PlayMakerFSM self) {
				_thisFsm = self;

				mapSettings = MapModS.Instance.Settings;
				mapSettingsType = mapSettings.GetType();
				this.AddObtainedLocation = mapSettingsType.GetMethod("AddObtainedLocation");
			}

			public override void OnEnter() {
				string scene = GameManager.instance.sceneName;
				string objectName = _thisFsm.gameObject.name;

				try { // Resolve duplicate geo rock object name
					if (AddObtainedLocation != null) {
						if (scene == "Crossroads_ShamanTemple" && objectName == "Geo Rock 2") {
							// Check if the hero is closer to lower rock or higher rock
							if (Math.Abs(HeroController.instance.transform.position.y - 46.5f)
								< Math.Abs(HeroController.instance.transform.position.y - 60.8f)) {
								//_dictionary[scene + "Geo Rock 2 (Dupe)"] = true;
								AddObtainedLocation.Invoke(mapSettings, new object[] { scene + "Geo Rock 2 (Dupe)" });
							} else {
								//_dictionary[scene + "Geo Rock 2 (Tree)"] = true;
								AddObtainedLocation.Invoke(mapSettings, new object[] { scene + "Geo Rock 2 (Tree)" });
							}

							// Every other case goes here
						} else {
							//MapModS.Instance.Log(scene);
							//MapModS.Instance.Log(objectName);
							//_dictionary[scene + objectName] = true;
							AddObtainedLocation.Invoke(mapSettings, new object[] { scene + objectName });
						}
					}
				} catch (Exception e) {
					MapModS.Instance.LogError($"Something wrong happened with unrandomized item tracking: {e}");
				}


				Finish();
			}

		}

		// Conveniently, when cocoons are randomized there are no HealthCocoons in the game
		private static void _LifebloodCocoon(On.HealthCocoon.orig_SetBroken orig, HealthCocoon self) {
			orig(self);
			string scene = GameManager.instance.sceneName;

			object mapSettings = MapModS.Instance.Settings;
			Type mapSettingsType = mapSettings.GetType();
			MethodInfo AddObtainedLocation = mapSettingsType.GetMethod("AddObtainedLocation");

			if (AddObtainedLocation != null) {
				AddObtainedLocation.Invoke(mapSettings, new object[] { scene + "Health Cocoon" });
			}

		}
	}
}
