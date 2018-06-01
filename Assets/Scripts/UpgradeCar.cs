using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeCar : MonoBehaviour
{
	private ULocalization.Localization m_localization;
	private string m_fileName = "strings";
	private string m_xmlString = "";

	public Sprite _select, _unSelect;

	public UpgradeItem[] _item;

	public Text priceText, contentText, headText, upgradeCarText;

	public int _type;

	public GameObject loading;

	int id;
	[HideInInspector]public int Engine, Fuel, Suspension, Speed;

	void Awake () {
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
	}

	void SetLanguage (string language)
	{
		m_localization.SetLanguageString (m_xmlString, language);
	}

	// Use this for initialization
	void Start ()
	{
//		HomeManager._homeManager.ChangeCoin(-5000000);

		SetChooseItem (0);
		LoadUpgrade ();

		upgradeCarText.text = m_localization["UPGRADE CAR"];
	}

	void OnEnable(){
		SetChooseItem (0);
		LoadUpgrade ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void SetChooseItem (int _index)
	{
		string coinsText = m_localization["coins"];
		string upgradeCostText = m_localization["Upgrade cost"];

		contentText.text = m_localization[_item [_index]._content];
		_type = _index;
		if (_index == 0) {
			_item [0]._border.sprite = _select;
			_item [1]._border.sprite = _unSelect;
			_item [2]._border.sprite = _unSelect;
			_item [3]._border.sprite = _unSelect;

//			priceText.text = "Upgrade cost : " + _item [0].price [PlayerPrefs.GetInt ("Engine" + id.ToString ())].ToString () + " coins";
			try{
				priceText.text = upgradeCostText + " : " + _item [0].price [PlayerPrefs.GetInt ("Engine" + id.ToString ())].ToString () + " " + coinsText;	
			}catch{
//				priceText.text = "Upgrade cost : " + _item [0].price [PlayerPrefs.GetInt ("Engine" + id.ToString ())-1].ToString () + " coins";
				priceText.text = m_localization["Completed"];
			}
			headText.text = m_localization["Upgrade Engine"].ToUpper();
		} else if (_index == 1) {

			_item [0]._border.sprite = _unSelect;
			_item [1]._border.sprite = _select;
			_item [2]._border.sprite = _unSelect;
			_item [3]._border.sprite = _unSelect;
//			priceText.text = "Upgrade cost : " + _item [1].price [PlayerPrefs.GetInt ("Suspension" + id.ToString ())].ToString () + " coins";
			try{
				priceText.text = upgradeCostText + " : " + _item [1].price [PlayerPrefs.GetInt ("Suspension" + id.ToString ())].ToString () + " " + coinsText;
			}catch{
//				priceText.text = "Upgrade cost : " + _item [1].price [PlayerPrefs.GetInt ("Suspension" + id.ToString ())-1].ToString () + " coins";
				priceText.text = m_localization["Completed"];
			}
			headText.text = m_localization["Upgrade Suspension"].ToUpper();
		} else if (_index == 2) {

			_item [0]._border.sprite = _unSelect;
			_item [1]._border.sprite = _unSelect;
			_item [2]._border.sprite = _select;
			_item [3]._border.sprite = _unSelect;
//			priceText.text = "Upgrade cost : " + _item [2].price [PlayerPrefs.GetInt ("Speed" + id.ToString ())].ToString () + " coins";
			try{
				priceText.text = upgradeCostText + " : " + _item [2].price [PlayerPrefs.GetInt ("Speed" + id.ToString ())].ToString () + " " + coinsText;
			}catch{
//				priceText.text = "Upgrade cost : " + _item [2].price [PlayerPrefs.GetInt ("Speed" + id.ToString ())-1].ToString () + " coins";
				priceText.text = m_localization["Completed"];
			}
			headText.text = m_localization["Upgrade Tire"].ToUpper();
		} else if (_index == 3) {

			_item [0]._border.sprite = _unSelect;
			_item [1]._border.sprite = _unSelect;
			_item [2]._border.sprite = _unSelect;
			_item [3]._border.sprite = _select;
//			priceText.text = "Upgrade cost : " + _item [3].price [PlayerPrefs.GetInt ("Fuel" + id.ToString ())].ToString () + " coins";
			try{
				priceText.text = upgradeCostText + " : " + _item [3].price [PlayerPrefs.GetInt ("Fuel" + id.ToString ())].ToString () + " " + coinsText;
			}catch{
//				priceText.text = "Upgrade cost : " + _item [3].price [PlayerPrefs.GetInt ("Fuel" + id.ToString ())].ToString () + " coins";
				priceText.text = m_localization["Completed"];
			}
			headText.text = m_localization["Upgrade Fuel"].ToUpper();
		}
	}

	public void Upgrade ()
	{
		if (_type == 0)
			EngineUpgrade ();
		else if (_type == 1)
			SuspensionUpgrade ();
		else if (_type == 2)
			SpeedUpgrade ();
		else if (_type == 3)
			FuelUpgrade ();
	}

	public void LoadUpgrade ()
	{
		id = PlayerPrefs.GetInt ("SelectedCar");

		Engine = PlayerPrefs.GetInt ("Engine" + id.ToString ());
		Fuel = PlayerPrefs.GetInt ("Fuel" + id.ToString ());
		Suspension = PlayerPrefs.GetInt ("Suspension" + id.ToString ());
		Speed = PlayerPrefs.GetInt ("Speed" + id.ToString ());

		string leveltext = m_localization["Level"];

		_item [0].levelInfo.text = leveltext + ": " + PlayerPrefs.GetInt ("Engine" + id.ToString ()).ToString () + " / " + _item [0].price.Length.ToString ();
		_item [1].levelInfo.text = leveltext + ": " + PlayerPrefs.GetInt ("Suspension" + id.ToString ()).ToString () + " / " + _item [1].price.Length.ToString ();
		_item [2].levelInfo.text = leveltext + ": " + PlayerPrefs.GetInt ("Speed" + id.ToString ()).ToString () + " / " + _item [2].price.Length.ToString ();
		_item [3].levelInfo.text = leveltext + ": " + PlayerPrefs.GetInt ("Fuel" + id.ToString ()).ToString () + " / " + _item [3].price.Length.ToString ();
	}


	public void EngineUpgrade ()
	{
		SetChooseItem (0);
		if (PlayerPrefs.GetInt ("Engine" + id.ToString ()) < _item [0].price.Length) {

			if (PlayerPrefs.GetInt ("Coins") >= _item [0].price [PlayerPrefs.GetInt ("Engine" + id.ToString ())]) {
				HomeManager._homeManager.ChangeCoin (_item [0].price [PlayerPrefs.GetInt ("Engine" + id.ToString ())]);
				PlayerPrefs.SetInt ("Engine" + id.ToString (), PlayerPrefs.GetInt ("Engine" + id.ToString ()) + 1);
				_item [0].levelInfo.text = m_localization["Level"] + ": " + PlayerPrefs.GetInt ("Engine" + id.ToString ()).ToString () + " / " + _item [0].price.Length.ToString ();

				if (PlayerPrefs.GetInt ("Engine" + id.ToString ()) < _item [0].price.Length)
					priceText.text = m_localization["Upgrade cost"] + " : " + _item [0].price [PlayerPrefs.GetInt ("Engine" + id.ToString ())].ToString () + " " + m_localization["coins"];
				else
					priceText.text = m_localization["Completed"];
			} else {
				HomeManager._homeManager.OpenPurchase ();


			}

		}
	}

	public void SuspensionUpgrade ()
	{
		SetChooseItem (1);
		if (PlayerPrefs.GetInt ("Suspension" + id.ToString ()) < _item [1].price.Length) {

			if (PlayerPrefs.GetInt ("Coins") >= _item [1].price [PlayerPrefs.GetInt ("Suspension" + id.ToString ())]) {
				
				HomeManager._homeManager.ChangeCoin (_item [1].price [PlayerPrefs.GetInt ("Suspension" + id.ToString ())]);
				PlayerPrefs.SetInt ("Suspension" + id.ToString (), PlayerPrefs.GetInt ("Suspension" + id.ToString ()) + 1);
				_item [1].levelInfo.text = m_localization["Level"] + ": " + PlayerPrefs.GetInt ("Suspension" + id.ToString ()).ToString () + " / " + _item [1].price.Length.ToString ();

				if (PlayerPrefs.GetInt ("Suspension" + id.ToString ()) < _item [1].price.Length)
					priceText.text = m_localization["Upgrade cost"] + " : " + _item [1].price [PlayerPrefs.GetInt ("Suspension" + id.ToString ())].ToString () + " " + m_localization["coins"];
				else
					priceText.text = m_localization["Completed"];
			} else {
				HomeManager._homeManager.OpenPurchase ();
			}
		}
	}

	public void FuelUpgrade ()
	{
		SetChooseItem (3);
		if (PlayerPrefs.GetInt ("Fuel" + id.ToString ()) < _item [3].price.Length) {

			if (PlayerPrefs.GetInt ("Coins") >= _item [3].price [PlayerPrefs.GetInt ("Fuel" + id.ToString ())]) {
				
				HomeManager._homeManager.ChangeCoin (_item [3].price [PlayerPrefs.GetInt ("Fuel" + id.ToString ())]);
				PlayerPrefs.SetInt ("Fuel" + id.ToString (), PlayerPrefs.GetInt ("Fuel" + id.ToString ()) + 1);
				_item [3].levelInfo.text = m_localization["Level"] + ": " + PlayerPrefs.GetInt ("Fuel" + id.ToString ()).ToString () + " / " + _item [3].price.Length.ToString ();

				if (PlayerPrefs.GetInt ("Fuel" + id.ToString ()) < _item [3].price.Length)
					priceText.text = m_localization["Upgrade cost"] + " : " + _item [3].price [PlayerPrefs.GetInt ("Fuel" + id.ToString ())].ToString () + " " + m_localization["coins"];
				else
					priceText.text = m_localization["Completed"];
			} else {
				HomeManager._homeManager.OpenPurchase ();

			}
		}
	}

	public void SpeedUpgrade ()
	{
		SetChooseItem (2);
		if (PlayerPrefs.GetInt ("Speed" + id.ToString ()) < _item [2].price.Length) {

			if (PlayerPrefs.GetInt ("Coins") >= _item [2].price [PlayerPrefs.GetInt ("Speed" + id.ToString ())]) {
				
				HomeManager._homeManager.ChangeCoin (_item [2].price [PlayerPrefs.GetInt ("Speed" + id.ToString ())]);
				PlayerPrefs.SetInt ("Speed" + id.ToString (), PlayerPrefs.GetInt ("Speed" + id.ToString ()) + 1);
				_item [2].levelInfo.text = m_localization["Level"] + ": " + PlayerPrefs.GetInt ("Speed" + id.ToString ()).ToString () + " / " + _item [0].price.Length.ToString ();

				if (PlayerPrefs.GetInt ("Speed" + id.ToString ()) < _item [2].price.Length)
					priceText.text = m_localization["Upgrade cost"] + " : " + _item [2].price [PlayerPrefs.GetInt ("Speed" + id.ToString ())].ToString () + " " + m_localization["coins"];
				else
					priceText.text = m_localization["Completed"];
			} else {
				
				HomeManager._homeManager.OpenPurchase ();

			}
		}
	}


	public void StartGame ()
	{

		//Loading.SetActive (true);
		//PlayerPrefs.SetInt ("AllScoreTemp", PlayerPrefs.GetInt ("Coins"));
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("Level"+PlayerPrefs.GetInt ("SelectedLevel").ToString());

		//sceneLoading.ActivateNextScene();
		loading.SetActive(true);
		//gameObject.SetActive (false);
	}

	[System.Serializable]
	public class UpgradeItem
	{
		public Text levelInfo;

		public Image _border;

		public int[] price;

		public string _content;
	}
}
