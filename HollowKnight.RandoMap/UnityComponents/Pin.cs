using RandoMapMod;
using System;
using UnityEngine;

[DebugName(nameof(Pin))]
class Pin : MonoBehaviour {
	#region Private Non-Methods
	internal readonly Color InactiveColor = Color.gray;

	internal Vector3 OrigScale;
	internal Color OrigColor;
	internal Sprite VanillaSprite;
	internal Sprite SpoilerSprite;
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

		// Store the sprite for toggling to spoiler items
		this.SpoilerSprite = ResourceHelper.FetchSpriteByPool(pd.SpoilerPool);

		this.UpdateState();
	}
	#endregion

	#region Public Methods
	public void SetSprite(string SpriteName) {
		this._SR.sprite = SpriteName switch {
			"Vanilla" => this.VanillaSprite,
			"Spoiler" => this.SpoilerSprite,
			_ => ResourceHelper.FetchSprite(ResourceHelper.Sprites.Unknown),
		};
	}
	#endregion

	#region Private Methods
	public void UpdateState() {
		try {
			if (this.PinData == null) {
				throw new Exception("Cannot enable pin with null pindata. Ensure game object is disabled before adding as component, then call SetPinData(<pd>) before enabling.");
			}

			// Check if item is in RandoLogger's obtainedLocations
			this._UpdateChecked();
			// Check if item is in RandoLogger's uncheckedLocations
			this._UpdateReachable();

		} catch (Exception e) {
			DebugLog.Error($"Failed to update pin! ID: {this.PinData.ID}", e);
		}
	}

	public void DisableSelf() {
		this.gameObject.SetActive(false);
	}

	private void _UpdateChecked() {
		if (RandomizerMod.RandoLogger.obtainedLocations.Contains(this.PinData.ID)) {
			this.gameObject.SetActive(false);
		}
	}
	private void _UpdateReachable() {
#if DEBUG
		// for visibility
		//this.transform.localScale = this.OrigScale * 0.7f;
		//return;
#endif
		if (RandomizerMod.RandoLogger.uncheckedLocations.Contains(this.PinData.ID)) {
			// We can reach this item now!
			this.transform.localScale = this.OrigScale * 0.7f;
			this._SR.color = this.OrigColor;
		} else {
			// We can't reach this item.
			this.transform.localScale = this.OrigScale * 0.5f;
			this._SR.color = this.InactiveColor;
		}
	}
	#endregion
}