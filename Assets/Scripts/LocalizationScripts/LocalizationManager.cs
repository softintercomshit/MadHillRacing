using UnityEngine;
using System.Collections;
using System.IO;
//
using UnityEngine.UI;

namespace ULocalization
{
	public delegate void EventHandler();

	/// <summary>
	/// Localization manager loads localization file and handles language change.
	/// </summary>
	public class LocalizationManager : MonoBehaviour 
	{
		#region [Public properties]

		/// <summary>
		/// Gets current language.
		/// </summary>
		/// <value>The language.</value>
		public string Language
		{
			get 
			{
				if(m_Localization == null)
					return null;
				
				return m_Localization.Language;
			}
		}

		/// <summary>
		/// Gets or sets the default language.
		/// </summary>
		/// <value>The default language.</value>
		public string DefaultLanguage
		{
			get
			{
				return m_defaultLanguage;
			}
			set
			{
				m_defaultLanguage = value;
			}
		}

		/// <summary>
		/// Gets or sets the path to the localization file.
		/// </summary>
		/// <value>The localization file.</value>
		public string LocalizationFile
		{
			get
			{
				return m_localizationFile;
			}
			set
			{
				m_localizationFile = value;
			}
		}

		/// <summary>
		/// Gets or sets the localization item.
		/// </summary>
		/// <value>The localization.</value>
		public Localization Localization
		{
			get
			{
				return m_Localization;
			}
			set
			{
				m_Localization = value;
			}
		}

		/// <summary>
		/// Raised on language changed to update components.
		/// </summary>
		public EventHandler OnLanguageChanged;

		#endregion
		
		#region [Private members]
		
		[SerializeField]
		Localization m_Localization;
		
		[SerializeField]
		string m_defaultLanguage = "en";
		
		[SerializeField]
		string m_localizationFile;

		#endregion
		
		#region [MonoBehaviour]
		
		void Awake () 
		{
			//ULocalization.Localization contains localized strings. 
			m_Localization = ScriptableObject.CreateInstance<Localization>();

			// Load system language (or default is unavailable) and apply.
			LoadLanguage();
		}

		#endregion
		
		#region [Public methods]

		/// <summary>
		/// Loads the language.
		/// </summary>
		/// <param name="aLanguage">A language to load. If null, then system language is loaded.</param>
		public void LoadLanguage(string aLanguage = null)
		{
			StartCoroutine("LoadResourceLanguageCoroutine", aLanguage);
		}

		/// <summary>
		/// Gets the localized string for the given key.
		/// </summary>
		/// <returns>The localized string.</returns>
		/// <param name="aKey">A key.</param>
		public string GetString(string aKey)
		{
			return m_Localization[aKey];
		}

		/// <summary>
		/// Gets the localized string for the given key with inserted values.
		/// </summary>
		/// <returns>The localized string.</returns>
		/// <param name="aKey">A key.</param>
		/// <param name="aValueToInsertArray">Values to insert in the string.</param>
		public string GetString(string aKey, params string[] aValueToInsertArray)
		{
			return m_Localization[aKey, aValueToInsertArray];
        }

		#endregion
		
		#region [PRIVATE METHODS]

		/// <summary>
		/// Gets the language key associated to the given system language.
		/// Write your own key codes here.
		/// </summary>
		/// <returns>The language key.</returns>
		string GetSystemLanguageKey()
		{
			switch(Application.systemLanguage)
			{
				case SystemLanguage.Chinese:
				{
					return "cn";
				}
				case SystemLanguage.ChineseSimplified:
				{
					return "cn-simple";
				}	
				case SystemLanguage.ChineseTraditional:
				{
					return "cn-trad";
				}		
				case SystemLanguage.English:
				{
					return "en";
				}
				case SystemLanguage.Dutch:
				{
					return "du";
				}	
				case SystemLanguage.French:
				{
					return "fr";
				}
				case SystemLanguage.German:
				{
					return "de";
				}
				case SystemLanguage.Italian:
				{
					return "it";
				}	
				case SystemLanguage.Russian:
				{
					return "ru";
				}
				case SystemLanguage.Spanish:
				{
					return "sp";
				}	
				//TODO add other languages
				case SystemLanguage.Unknown:
				default:
				{
					return DefaultLanguage;
				}
			}
		}

		/// <summary>
		/// Notifies language change.
		/// </summary>
		void NotifyLanguageChanged()
		{
			if(OnLanguageChanged != null)
				OnLanguageChanged.Invoke();
		}

		/// <summary>
		/// Asynchronously loads the resource localization file and apply given language.
		/// </summary>
		/// <returns>The language file coroutine.</returns>
		/// <param name="aLanguage">A language.</param>
		IEnumerator LoadResourceLanguageCoroutine(string aLanguage)
	 	{
	 		// Find default language
			if(aLanguage == null)
			{
				aLanguage = GetSystemLanguageKey();
				Debug.Log("[LOCALIZATION] System language: " + aLanguage);
			}

			// Load file from Resources
			var request = Resources.LoadAsync<TextAsset>(m_localizationFile);
			yield return request.isDone;

			if(request.asset != null)
			{
				string localizationText = (request.asset as TextAsset).text;

				m_Localization.SetLanguageString(localizationText, aLanguage);

				if(m_Localization.Language == null)
				{
					Debug.LogWarning("[LOCALIZATION] Language not found, use default: " + DefaultLanguage);
					m_Localization.SetLanguageString(localizationText, DefaultLanguage);
				}

				if(m_Localization.Language != null)
				{
					NotifyLanguageChanged();
				}
			}
			else
			{
				Debug.LogError("[LOCALIZATION] Resource not found: " + m_localizationFile);
			}
	 	}

		#endregion
	}
}

