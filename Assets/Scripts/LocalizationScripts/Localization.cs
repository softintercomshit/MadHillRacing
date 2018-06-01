using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using UnityEngine;

namespace ULocalization
{
	public class Localization : ScriptableObject
	{
		#region [Public properties]

		public string Language
		{
			get
			{
				return m_Language;
			}
			private set
			{
				m_Language = value;
			}
		}

		#endregion
	
		#region [Private members]

		[SerializeField]
		private Dictionary<string, string> m_Strings = new Dictionary<string, string>();
		
		[SerializeField]
		private string m_Language = null;

		#endregion
		
		#region [Public methods]	

		/// <summary>
		/// Sets the localization file string and the language to load.
		/// </summary>
		/// <param name="aXMLString">A XML string.</param>
		/// <param name="aLanguage">A language.</param>
		public void SetLanguageString(string aXMLString, string aLanguage)
		{
			var lXml = new XmlDocument();
			lXml.Load(new StringReader(aXMLString));
			
			InitLanguageFromXML(lXml, aLanguage);
		}

		/// <summary>
		/// Gets the localized string.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="aKey">A key.</param>
		public string GetString(string aKey)
		{
			if (!m_Strings.ContainsKey(aKey))
			{
				Debug.LogError("The specified key does not exist: " + aKey);
				
				return ""; // Default value (could be null);
			}
			
			return (string) m_Strings [aKey];
		}
		
		/// <summary>
		/// Gets the localized string and insert values. 
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="aKey">A key.</param>
		/// <param name="aValuesToInsertArray">A array of values to insert. "%x%" will be replaced by the value at x (index).</param>
		public string GetString(string aKey, params string[] aValuesToInsertArray)
		{
			string lString = GetString(aKey); // Get the "raw" string
			string lItem = "";
			for(int i = 0; i < aValuesToInsertArray.Length ; i++)
			{
				lItem = "%" + i + "%";
				lString = lString.Replace(lItem, aValuesToInsertArray[i]); // Insert "dynamic" values in the string
			}
			
			return lString;
        }
		
		public string this [string aKey]
		{
			get
			{
				return GetString(aKey);
			}
		}
		
		public string this [string aKey, params string[] aValuesToInsertArray]
		{
			get
			{
				return GetString(aKey, aValuesToInsertArray);
            }
        }

		#endregion
		
		#region [Private methods]

		void InitLanguageFromXML(XmlDocument aXMLDocument, string aLanguage)
		{
			var lDocument = aXMLDocument.DocumentElement[aLanguage];
			if (lDocument != null)
			{
				m_Strings = new Dictionary<string, string>();
				m_Language = aLanguage;
						
				var lElementEnumerator = lDocument.GetEnumerator();
				while (lElementEnumerator.MoveNext())
				{
					var lXmlItem = lElementEnumerator.Current as XmlElement;
					m_Strings.Add(lXmlItem.GetAttribute("name"), lXmlItem.InnerText);
				}
			} else
			{
				m_Language = null;
				Debug.LogError("Language does not exist: " + aLanguage);
			}
        }
        #endregion
    }
}