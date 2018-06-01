using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HomeManager : MonoBehaviour
{
	private ULocalization.Localization m_localization;
	private string m_fileName = "strings";
	private string m_xmlString = "";

	public GameObject mapPanel, vehiclePanel, upgradeCarPanel;

	public Text coinText, cancelText, okText, notEnoughText, cancelText2, okText2, viewRewardVideoText;

	public GameObject purchasePanel, IAPPanel, settingPanel;

	public Button soundButton, musicButton;

	public Sprite soundOn, soundOnPressed, soundOff, soundOffPressed, musicOn, musicOnPressed, musicOff, musicOffPressed;

	public static HomeManager _homeManager;

	public AudioSource mainMusic;

	// Use this for initialization

	void Awake ()
	{
		m_localization = ScriptableObject.CreateInstance<ULocalization.Localization> ();
		TextAsset textAsset = Resources.Load<TextAsset> (m_fileName);
		if (textAsset != null) {
			m_xmlString = textAsset.text;
		} else {
			Debug.LogError ("Localizations not found : " + m_fileName);
			m_xmlString = "";
		}

		string deviceLanguage = XcodeManager.GetDeviceLanguageFromXcode ();
		// CHECK IF LANGUAGE EXISTS IN strings.xml
		if (!deviceLanguage.Equals ("en") && !deviceLanguage.Equals ("fr") && !deviceLanguage.Equals ("de") && !deviceLanguage.Equals ("ru") && !deviceLanguage.Equals ("es") && !deviceLanguage.Equals ("it") && !deviceLanguage.Equals ("ja") && !deviceLanguage.Equals ("ko") && !deviceLanguage.Equals ("nl") && !deviceLanguage.Equals ("pt") && !deviceLanguage.Equals ("sv") && !deviceLanguage.Equals ("zh") && !deviceLanguage.Equals ("th") && !deviceLanguage.Equals ("tr")) {
			deviceLanguage = "en";
		}
		SetLanguage (deviceLanguage);

		//PlayerPrefs.DeleteAll ();
		//ChangeCoin(500000);
		_homeManager = this;
	}

	void SetLanguage (string language)
	{
		m_localization.SetLanguageString (m_xmlString, language);
	}

	void Start ()
	{
		cancelText.text = m_localization ["CANCEL"];
		okText.text = m_localization ["OK"];
		cancelText2.text = m_localization ["CANCEL"];
		okText2.text = m_localization ["OK"];
		notEnoughText.text = m_localization ["Not enough coins! Do you want to buy some coins?"];
		viewRewardVideoText.text = m_localization ["View reward video to earn more coins?"];

		UpdateCoin ();
		LoadMusicAndSound ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void OpenMapPanel ()
	{
		mapPanel.SetActive (true);
		vehiclePanel.SetActive (false);
		upgradeCarPanel.SetActive (false);
	}

	public void OpenVehiclePanel ()
	{
		mapPanel.SetActive (false);
		vehiclePanel.SetActive (true);
		upgradeCarPanel.SetActive (false);
	}

	public void OpenUpgradeVehiclePanel ()
	{
		mapPanel.SetActive (false);
		vehiclePanel.SetActive (false);
		upgradeCarPanel.SetActive (true);
	}

	public void UpdateCoin ()
	{
		int _coin = PlayerPrefs.GetInt ("Coins");
		coinText.text = _coin.ToString ();
	}

	public void ChangeCoin (int _value)
	{
		int _coin = PlayerPrefs.GetInt ("Coins");
		_coin -= _value;
		PlayerPrefs.SetInt ("Coins", _coin);
		coinText.text = _coin.ToString ();
	}

	public void ClosePurchase ()
	{
		purchasePanel.SetActive (false);
	}

	public void  OpenPurchase ()
	{
		purchasePanel.SetActive (true);
	}

	public void CloseIAP ()
	{
		IAPPanel.SetActive (false);
	}

	public void  OpenIAP ()
	{
		ClosePurchase ();
		CloseSetting ();
		IAPPanel.SetActive (true);
	}

	public void CloseSetting ()
	{
		settingPanel.SetActive (false);
	}

	public void SettingPanel ()
	{
		ClosePurchase ();
		CloseIAP ();
		settingPanel.SetActive (true);
	}

	public void PlayReward ()
	{
		CloseIAP ();
		AdsControl.Instance.ShowRewardVideo ();
	}

	public void ToggleMusic ()
	{
		if (PlayerPrefs.HasKey ("MusicVolume")) {
			if (PlayerPrefs.GetFloat ("MusicVolume") >= 1.0f) {
				PlayerPrefs.SetFloat ("MusicVolume", 0.0f);
				SetButtonSprites (musicButton, musicOff, musicOffPressed);
				mainMusic.mute = true;
			} else {
				PlayerPrefs.SetFloat ("MusicVolume", 1.0f);	
				SetButtonSprites (musicButton, musicOn, musicOnPressed);
				mainMusic.mute = false;
			}
		} else {

			PlayerPrefs.SetFloat ("MusicVolume", 1.0f);
			SetButtonSprites (musicButton, musicOn, musicOnPressed);
			mainMusic.mute = false;
		}
	}

	public void ToggleSound ()
	{
		if (PlayerPrefs.HasKey ("EngineVolume")) {
			if (PlayerPrefs.GetFloat ("EngineVolume") >= 1.0f) {
				PlayerPrefs.SetFloat ("EngineVolume", 0.0f);
				SetButtonSprites (soundButton, soundOff, soundOffPressed);
			} else {
				PlayerPrefs.SetFloat ("EngineVolume", 1.0f);
				SetButtonSprites (soundButton, soundOn, soundOnPressed);
			}
		} else {
			PlayerPrefs.SetFloat ("EngineVolume", 1.0f);
			SetButtonSprites (soundButton, soundOn, soundOnPressed);
		}
			
	}

	public void LoadMusicAndSound ()
	{
		if (PlayerPrefs.HasKey ("MusicVolume")) {
			if (PlayerPrefs.GetFloat ("MusicVolume") >= 1.0f) {
				SetButtonSprites (musicButton, musicOn, musicOnPressed);
				mainMusic.mute = false;
			} else {
				SetButtonSprites (musicButton, musicOff, musicOffPressed);
				mainMusic.mute = true;
			}
		} else {
			PlayerPrefs.SetFloat ("MusicVolume", 1.0f);
			mainMusic.mute = false;
			SetButtonSprites (musicButton, musicOn, musicOnPressed);
		}

		if (PlayerPrefs.HasKey ("EngineVolume")) {
			if (PlayerPrefs.GetFloat ("EngineVolume") >= 1.0f) {
				SetButtonSprites (soundButton, soundOn, soundOnPressed);
			} else {
				SetButtonSprites (soundButton, soundOff, soundOffPressed);
			}
		} else {
			PlayerPrefs.SetFloat ("EngineVolume", 1.0f);
			SetButtonSprites (soundButton, soundOn, soundOnPressed);
		}
	}

	private void SetButtonSprites (Button button, Sprite normal, Sprite highlighted)
	{
		button.image.sprite = normal;
		SpriteState spriteState = new SpriteState ();
		spriteState = button.spriteState;
		spriteState.pressedSprite = highlighted;
		button.spriteState = spriteState;
	}
}
