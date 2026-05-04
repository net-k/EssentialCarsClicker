// #define LOCALIZE_ENGLISH
// #define LOCALIZE_JAPANESE

using UnityEngine;

namespace Quiz.Framework.Localize
{
	public class Localize : MonoBehaviour
	{
		[SerializeField]
		private LocalizeDataMaster localizeDataMaster;

		public static string DeviceLanguageJapanese = "Japanese";
		public static string DeviceLanguageEnglish = "English";
	
		public static string GetDeviceLanguage()
		{
			string deviceLanguage = UnityEngine.Application.systemLanguage.ToString();
#if LOCALIZE_JAPANESE
return "Japanese";
#endif

#if LOCALIZE_ENGLISH
return "English";
#endif
			if (deviceLanguage == DeviceLanguageJapanese)
			{
			}
			else if (deviceLanguage == DeviceLanguageEnglish)
			{
			}
			else
			{
				return DeviceLanguageEnglish;
			}

			return deviceLanguage;
		}

		public string GetText(string key)
		{
			// Debug.LogFormat("Localize.GetText key={0}", key);
			int index = localizeDataMaster.LocalizeDataList.FindIndex(n => n.key == key);
			if (index == -1)
			{
				return "";
			}

			switch (GetDeviceLanguage())
			{
				case "Japanese":
					return localizeDataMaster.LocalizeDataList[index].Japanese;
				case "English":
				default:
					return localizeDataMaster.LocalizeDataList[index].English;
				
			}
		}
	}
}
