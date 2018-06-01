using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

//[System.Serializable]

public class IAPManager : MonoBehaviour
{
	private ULocalization.Localization m_localization;
	private string m_fileName = "strings";
	private string m_xmlString = "";

	public Text watchVideotext, coins5000, coin20000, coins50000;

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

	void Start ()
	{
		watchVideotext.text = m_localization["View reward video to earn 1000 coins"];
		coins5000.text = "5000 " + m_localization ["coins"];
		coin20000.text = "20000 " + m_localization ["coins"];
		coins50000.text = "50000 " + m_localization ["coins"];
	}

	[DllImport ("__Internal")]
	private static extern void buyCoins(int indexProduct);

	public void BuyProductConsume(int indexProduct)
	{
		// send message to Xcode
		buyCoins(indexProduct);
		print ("buy coins !!!!!! " + indexProduct);
	}
}
