using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapSelector : MonoBehaviour
{
	private ULocalization.Localization m_localization;
	private string m_fileName = "strings";
	private string m_xmlString = "";

	public Item[] _item;

	public GameObject purchasePanel;

	public Text chooseMapText;

	public enum ITEM_TYPE
	{
		LEVEL,
		VEHICLE}

	;

	public ITEM_TYPE _type;

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
		print ("START !!!!!");
		//PlayerPrefs.DeleteAll ();
		if (_type == ITEM_TYPE.LEVEL)
			chooseMapText.text = m_localization ["CHOOSE MAP"];
		else
			chooseMapText.text = m_localization ["CHOOSE CAR"];

		PlayerPrefs.SetInt ("Level0", 1);
		PlayerPrefs.SetInt ("Car0", 1);
		LoadItemInfor ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void ChooseItem (int _index)
	{
		string prefix = "";
		if (_type == ITEM_TYPE.LEVEL)
			prefix = "Level";
		else
			prefix = "Car";
		int _lock = PlayerPrefs.GetInt (prefix + _index.ToString ());

		if (_lock != 0) {
			//buy
			//Debug.Log ("opened");
			if (_type == ITEM_TYPE.LEVEL) {
				PlayerPrefs.SetInt ("SelectedLevel", _index);
				HomeManager._homeManager.OpenVehiclePanel ();
			} else if (_type == ITEM_TYPE.VEHICLE) {
				PlayerPrefs.SetInt ("SelectedCar", _index);
				HomeManager._homeManager.OpenUpgradeVehiclePanel ();
			}

		} else {
			//Debug.Log ("map is locked : ");

			if (PlayerPrefs.GetInt ("Coins") >= _item [_index].price) {

				HomeManager._homeManager.ChangeCoin (_item [_index].price);

				PlayerPrefs.SetInt (prefix + _index.ToString (), 1);
				LoadItemInfor ();

			} else
				purchasePanel.SetActive (true);

		}
	}


	public void LoadItemInfor ()
	{
		Debug.Log ("map is locked : " + gameObject.name);
		string prefix = "";
		if (_type == ITEM_TYPE.LEVEL)
			prefix = "Level";
		else
			prefix = "Car";

		print ("TEST !!!!!! ");


		for (int i = 0; i < _item.Length; i++) {

			print (prefix + i.ToString ());

			int _lock = PlayerPrefs.GetInt (prefix + i.ToString ());
			print (_lock);

			if (_lock == 0) {

				_item [i].lockMask.enabled = true;
				_item [i].mapPrice.enabled = true;
				_item [i].mapPrice.text = m_localization ["Cost"] + " : " + _item [i].price.ToString () + " " + m_localization ["coins"];
				_item [i].mapName.text = m_localization[_item [i].mapName.text];

				if (_type == ITEM_TYPE.LEVEL)
					_item [i].bestScore.enabled = false;
			} else {

				_item [i].lockMask.enabled = false;
				_item [i].mapPrice.enabled = false;
				//_item [i].mapPrice.text = _item [i].price.ToString ();
				if (_type == ITEM_TYPE.LEVEL) {
					_item [i].bestScore.enabled = true;
					int highDistance = PlayerPrefs.GetInt ("BestDistance" + i.ToString ());
					_item [i].bestScore.text = m_localization["Best"] + " : " + highDistance.ToString () + " m";
				}
				_item [i].mapName.text = m_localization[_item [i].mapName.text];
			}
		}
	}

	[System.Serializable]
	public class Item
	{
		public Text bestScore;

		public Text mapPrice;

		public Image lockMask;

		public Text mapName;

		public int price;
	}
}
