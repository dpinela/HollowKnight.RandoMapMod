using RandoMapMod;
//using RandoMapMod.BoringInternals;
using System;
using UnityEngine;
using UnityEngine.UI;

[DebugName(nameof(Pin))]
class Pin : MonoBehaviour {
	#region Private Non-Methods
	internal readonly Color InactiveColor = Color.gray;

	private bool? _isPossible = null;
	private bool _updateTrigger = false;

	internal Vector3 OrigScale;
	internal Color OrigColor;
	internal Sprite VanillaSprite;
	internal Sprite RandoSprite;
	internal Vector3 OrigPosition;

	private SpriteRenderer _SR => this.gameObject.GetComponent<SpriteRenderer>();
	#endregion

	#region Public Non-Methods
	public PinData PinData { get; private set; } = null;
	public void SetPinData(PinData pd) {
		this.PinData = pd;

		this.OrigScale = this.transform.localScale;
		this.OrigColor = this._SR.color;
		this.VanillaSprite = this._SR.sprite;
		this.OrigPosition = this.transform.localPosition;

		// Store the sprite for toggling to randomized items
		this.RandoSprite = ResourceHelper.FetchSpriteByPool(pd.RandoPool);

		this._UpdateState();
	} // As a setter, this totally counts as a non-method >_>
	#endregion

	#region Public Methods
	public void SetVanillaRandoSprite(bool RandoOn) {
		try {
			if (RandoOn) {
				this._SR.sprite = this.RandoSprite;
			} else {
				this._SR.sprite = this.VanillaSprite;
			}
		} catch (Exception e) {
			DebugLog.Error($"Failed to toggle pin sprite! ID: {this.PinData.ID}", e);
		}
	}
	public void SetUnknownSprite() {
		this._SR.sprite = ResourceHelper.FetchSprite(ResourceHelper.Sprites.Unknown);
	}
	#endregion

	#region MonoBehaviour "Overrides"
	protected void OnEnable() {
		_updateTrigger = true;
	}
	protected void Update() {
		if (_updateTrigger) {
			this._UpdateState();
		}
	}
	#endregion

	#region Private Methods
	private void _UpdateState() {
		try {
			if (this.PinData == null) {
				throw new Exception("Cannot enable pin with null pindata. Ensure game object is disabled before adding as component, then call SetPinData(<pd>) before enabling.");
			}

			this._UpdateReachableState();

			//Disable Pin if we've already obtained / checked this location.
			if (GameStatus.ItemIsChecked(this.PinData.ID)) {
				if (this.name == "Stag_Nest_Stag") {
					DebugLog.Log("Hi");
				}
				this._DisableSelf();
			} else {
				if (this.name == "Stag_Nest_Stag") {
					DebugLog.Log("Bye");
				}
			}
		} catch (Exception e) {
			DebugLog.Error($"Failed to enable pin! ID: {this.PinData.ID}", e);
		}
	}

	private void _DisableSelf() {
		this.gameObject.SetActive(false);
	}

	private void _UpdateReachableState() {
		bool newValue = GameStatus.ItemIsReachable(this.PinData.ID);

		if (newValue == this._isPossible) {
			return;
		}

		if (newValue == true) {
			// We can reach this item now!
			this.transform.localScale = this.OrigScale;
			this._SR.color = this.OrigColor;
			this._isPossible = true;
		} else {
			// We can't reach this item.
			this.transform.localScale = this.OrigScale * 0.5f;
			this._SR.color = this.InactiveColor;
			this._isPossible = false;
		}
	}
	#endregion
}