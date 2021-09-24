using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RandoMapMod {

	// All the following was modified from the GUI implementation of BenchwarpMod by homothetyhk
	public class GUIController : MonoBehaviour {
		public Dictionary<string, Texture2D> Images = new Dictionary<string, Texture2D>();

		private static GUIController _instance;

		private GameObject _pauseCanvas;
		private GameObject _mapCanvas;

		public static GUIController Instance {
			get {
				if (_instance != null) return _instance;

				_instance = FindObjectOfType<GUIController>();

				if (_instance != null) return _instance;

				MapModS.Instance.LogWarn("Couldn't find GUIController");

				GameObject GUIObj = new GameObject();
				_instance = GUIObj.AddComponent<GUIController>();
				DontDestroyOnLoad(GUIObj);

				return _instance;
			}
		}

		public Font TrajanBold { get; private set; }

		public Font TrajanNormal { get; private set; }

		private Font _Arial { get; set; }

		public static void Setup() {
			GameObject GUIObj = new GameObject("RandoMapMod GUI");
			_instance = GUIObj.AddComponent<GUIController>();
			DontDestroyOnLoad(GUIObj);
		}

		public static void Unload() {
			Destroy(_instance._pauseCanvas);
			Destroy(_instance._mapCanvas);
			Destroy(_instance.gameObject);
		}

		public void BuildMenus() {
			_LoadResources();

			_pauseCanvas = new GameObject();
			_pauseCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
			CanvasScaler pauseScaler = _pauseCanvas.AddComponent<CanvasScaler>();
			pauseScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			pauseScaler.referenceResolution = new Vector2(1920f, 1080f);
			_pauseCanvas.AddComponent<GraphicRaycaster>();

			PauseGUI.BuildMenu(_pauseCanvas);

			DontDestroyOnLoad(_pauseCanvas);

			_mapCanvas = new GameObject();
			_mapCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
			CanvasScaler mapScaler = _mapCanvas.AddComponent<CanvasScaler>();
			mapScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			mapScaler.referenceResolution = new Vector2(1920f, 1080f);
			_pauseCanvas.AddComponent<GraphicRaycaster>();

			MapText.BuildText(_mapCanvas);

			DontDestroyOnLoad(_mapCanvas);

			_mapCanvas.transform.SetParent(MapModS.Instance.PinGroupInstance.transform);
		}

		public void Update() {
			try {
				PauseGUI.Update();
				MapText.Update();
			} catch (Exception e) {
				MapModS.Instance.LogError(e);
			}
		}

		private void _LoadResources() {
			TrajanBold = Modding.CanvasUtil.TrajanBold;
			TrajanNormal = Modding.CanvasUtil.TrajanNormal;

			try {
				_Arial = Font.CreateDynamicFontFromOSFont
				(
					Font.GetOSInstalledFontNames().First(x => x.ToLower().Contains("arial")),
					13
				);
			} catch {
				MapModS.Instance.LogWarn("Unable to find Arial! Using Perpetua.");
				_Arial = Modding.CanvasUtil.GetFont("Perpetua");
			}

			if (TrajanBold == null || TrajanNormal == null || _Arial == null) {
				MapModS.Instance.LogError("Could not find game fonts");
			}

			Assembly asm = Assembly.GetExecutingAssembly();

			foreach (string res in asm.GetManifestResourceNames()) {
				if (!res.StartsWith("RandoMapMod.Resources.GUI.")) continue;

				try {
					using (Stream imageStream = asm.GetManifestResourceStream(res)) {
						byte[] buffer = new byte[imageStream.Length];
						imageStream.Read(buffer, 0, buffer.Length);

						Texture2D tex = new Texture2D(1, 1);
						tex.LoadImage(buffer.ToArray());

						string[] split = res.Split('.');
						string internalName = split[split.Length - 2];

						Images.Add(internalName, tex);
					}
				} catch (Exception e) {
					MapModS.Instance.LogError("Failed to load image: " + res + "\n" + e);
				}
			}
		}
	}
}