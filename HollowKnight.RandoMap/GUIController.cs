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

		private GameObject _canvas;

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
			Destroy(_instance._canvas);
			Destroy(_instance.gameObject);
		}
		public void BuildMenus() {
			_LoadResources();

			_canvas = new GameObject();
			_canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
			CanvasScaler scaler = _canvas.AddComponent<CanvasScaler>();
			scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			scaler.referenceResolution = new Vector2(1920f, 1080f);
			_canvas.AddComponent<GraphicRaycaster>();

			PauseGUI.BuildMenu(_canvas);

			DontDestroyOnLoad(_canvas);
		}

		public void Update() {
			try {
				PauseGUI.Update();
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