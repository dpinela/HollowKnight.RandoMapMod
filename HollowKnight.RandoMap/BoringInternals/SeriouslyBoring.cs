using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using RND = UnityEngine.Random;

namespace RandoMapMod.BoringInternals {

	class Anchors {
		public static Anchors Instance { get; private set; } = new Anchors();
		private static Dictionary<BoringPinThing, Vector3> _anchors = new Dictionary<BoringPinThing, Vector3>();
		private static float _seconds = -20f;

		public Vector3 this[BoringPinThing pin] {
			get {
				if (_anchors.TryGetValue(pin, out Vector3 vec)) {
					return vec;
				}

				return Vector3.zero;
			}
			set {
				if (_anchors.ContainsKey(pin)) {
					_anchors[pin] = value;
					return;
				}

				_anchors.Add(pin, value);
			}
		}

		private static void _Shuffle() {
			BoringPinThing[] pins = _anchors.Keys.ToArray();
			List<Vector3> oldVecs = _anchors.Values.ToList();
			List<Vector3> newVecs = new List<Vector3>();

			Random rnd = new Random();
			while (oldVecs.Count > 0) {
				int next = rnd.Next(oldVecs.Count);
				newVecs.Add(oldVecs[next]);
				oldVecs.RemoveAt(next);
			}

			Dictionary<BoringPinThing, Vector3> dict = new Dictionary<BoringPinThing, Vector3>();
			for (int i = 0; i < pins.Length; i++) {
				dict.Add(pins[i], newVecs[i]);
			}

			_anchors = dict;
		}

		private static int _logtrack = 3000;

		internal static void Update(BoringPinThing pin, float sumval) {
			if (pin != _anchors.Keys.First()) {
				return;
			}

			if (_seconds < 0f) {
				_Shuffle();
				_logtrack = 3000;
				_seconds = -20f;
			}

			if (_seconds < -5f) {
				_seconds = RND.Range(15f, 30f);
			}

			_seconds -= Time.deltaTime;

			if (_seconds*3 < _logtrack) {
				DebugLog.Log($"Anchor Update : {_seconds}, {sumval}");
				_logtrack = (int)_seconds*3;
			}
		}
	}

	class BoringPinThing : MonoBehaviour {

		private SpriteRenderer _SR => this.gameObject.GetComponent<SpriteRenderer>();
		private Pin _Pin => this.gameObject.GetComponent<Pin>();

		protected void Start() {
			//DebugLog.Log("BoringPin Start");
		}
		protected void Awake() {
			//DebugLog.Log("BoringPin Awake");
		}

		protected void Update() {
			if (this._SR.color != this._Pin.OrigColor && this._SR.color != this._Pin.InactiveColor) {
				if (GameStatus.ItemIsReachable(this._Pin.PinData.ID)) {
					this._SR.color = this._Pin.OrigColor;
				} else {
					this._SR.color = this._Pin.InactiveColor;
				}
			}
		}
	}
}