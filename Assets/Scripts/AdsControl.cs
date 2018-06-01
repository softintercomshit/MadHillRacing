using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System;
using System.Runtime.InteropServices;

public class AdsControl : MonoBehaviour
{


	protected AdsControl ()
	{
	}

	private static AdsControl _instance;

	public static AdsControl Instance { get { return _instance; } }

	void Awake ()
	{
		if (FindObjectsOfType (typeof(AdsControl)).Length > 1) {
			Destroy (gameObject);
			return;
		}

		_instance = this;

		DontDestroyOnLoad (gameObject);
	}

	[DllImport ("__Internal")]
	private static extern void showSICAds();

	public void showAds ()
	{
		try{
			showSICAds ();
		}catch{}
	}

	// reward on buy coins
	private void buyCoinsSucces(string indexProduct){
		// need to add coins
		AddCoin (int.Parse(indexProduct));
	}

	void AddCoin(int _index)
	{
		switch (_index) {

		case 0:
			HomeManager._homeManager.ChangeCoin(-5000);
			break;
		case 1:
			HomeManager._homeManager.ChangeCoin(-20000);
			break;
		case 2:
			HomeManager._homeManager.ChangeCoin(-50000);
			break;
		}
	}

	[DllImport ("__Internal")]
	private static extern void showRewardedVideo();

	public void ShowRewardVideo ()
	{
//		Advertisement.Show (UnityZoneID, options);
		showRewardedVideo();
	}

	private void rewardedVideoWatched(string result){
		// need to add 1000 coins
		HomeManager._homeManager.ChangeCoin (-1000);
	}
}

