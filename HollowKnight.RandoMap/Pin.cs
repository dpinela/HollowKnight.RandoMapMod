using MapModS;
using System;
using UnityEngine;

internal class Pin : MonoBehaviour
{
	private readonly Color _inactiveColor = Color.gray;
	private Color _origColor;
	private Vector3 _origScale;
	public PinData PinData { get; private set; } = null;
	private SpriteRenderer _SR => this.gameObject.GetComponent<SpriteRenderer>();

	public void SetPinData(PinData pd)
	{
		this.PinData = pd;

		this._origScale = this.transform.localScale;
		this._origColor = this._SR.color;
	}

	public void SetSprite(Sprite SpriteName)
	{
		this._SR.sprite = SpriteName;
	}

	public void SetPinState(string mapAreaName)
	{
		try
		{
			if (this.PinData == null)
			{
				throw new Exception("Cannot enable pin with null pindata. Ensure game object is disabled before adding as component, then call SetPinData(<pd>) before enabling.");
			}

			this._ShowIfCorrectMap(mapAreaName);
			this._HideIfFound();
			this._SetBigIfReachable();
		}
		catch (Exception e)
		{
			MapModS.MapModS.Instance.LogError($"Failed to update pin! ID: {this.PinData.ID}\n{e}");
		}
	}

	// This method hides or shows the pin depending on which map was opened
	// and whether the player has the relevant map
	private void _ShowIfCorrectMap(string mapAreaName)
	{
		if (this.gameObject == null) {
			return;
		}
		this.gameObject.SetActive(
			(mapAreaName == this.PinData.MapArea || mapAreaName == "WorldMap") &&
			(MapModS.MapModS.Instance.Settings.ShowAllPins ||
				Dictionaries.GetPlayerDataMapSetting(this.PinData.MapArea))
		);
	}

	private void _HideIfFound()
	{
		if (RandomizerMod.RandomizerMod.RS.TrackerData.clearedLocations.Contains(this.PinData.ID))
		{
			this.gameObject.SetActive(false);
		}
	}

	private void _SetBigIfReachable()
	{
		// for visibility when debugging
		//this.transform.localScale = this.OrigScale * 0.7f;
		//return;

		if (RandomizerMod.RandomizerMod.RS.TrackerData.uncheckedReachableLocations.Contains(this.PinData.ID))
		{
			// We can reach this item now!
			this.transform.localScale = this._origScale * 0.7f;
			this._SR.color = this._origColor;
		}
		else
		{
			// We can't reach this item.
			this.transform.localScale = this._origScale * 0.5f;
			this._SR.color = this._inactiveColor;
		}
	}
}