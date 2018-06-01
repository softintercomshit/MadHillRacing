using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ULocalization
{
	[RequireComponent(typeof(Text))]
	/// <summary>
	/// UnityEngine.UI Text localizer. Updade text value with localized string automatically.
	/// </summary>
	public class UIComponentLocalizer : MonoBehaviour 
	{
		#region [Public members]

		public LocalizationManager Localizer {get{return m_localizationManager;}set{m_localizationManager = value;}}
		public string Key {get{return m_key;}set{m_key = value;}}

		#endregion
		
		#region [Private members]

		[SerializeField]
		LocalizationManager m_localizationManager;

		[SerializeField]
		string m_key;

		Text m_text;

		#endregion
		
		#region [MonoBehaviour]

		// Use this for initialization
		void OnEnable() 
		{
			m_text = GetComponent<Text>();
			if(m_localizationManager != null)
				m_localizationManager.OnLanguageChanged += UpdateText;
				
			UpdateText();
		}
		
		void OnDisable()
		{
			if(m_localizationManager != null)
				m_localizationManager.OnLanguageChanged -= UpdateText;
		}

		#endregion
		
		#region [Public methods]

		public void UpdateText()
		{
			if(m_localizationManager != null)
						m_text.text = m_localizationManager.GetString(m_key);
		}

		#endregion
	}	
}

