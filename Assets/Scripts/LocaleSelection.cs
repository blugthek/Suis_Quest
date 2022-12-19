using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleSelection : MonoBehaviour
{
    private void Start()
    {
        int ID = PlayerPrefs.GetInt("LocaleKey", 0);
        ChangeLocale(ID);
    }

    private bool active = false;

    public void ChangeLocale(int localID)
    {
        if (active == true)
            return;
        StartCoroutine(SetLocale(localID));
    }
    IEnumerator SetLocale(int _localID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localID];
        PlayerPrefs.SetInt("LocaleKey", _localID);
        active = false;
    }
}
