using UI.UIBase;
using UnityEngine.Localization.Settings;

namespace UI.Popup.Setting
{
    public class SettingView : PopupBase
    {
        public void UserLocalization(int index) 
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        }
    }
}