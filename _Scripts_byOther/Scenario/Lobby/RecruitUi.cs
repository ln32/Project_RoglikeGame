using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EnumCollection;

public class RecruitUi : LobbyUiBase
{
    public TextStatus hpStatus;
    public TextStatus abilityStatus;
    public TextStatus speedStatus;
    public TextStatus resistStatus;
    private Dictionary<TMP_Text, Dictionary<Language, string>> texts = new();
    private void Awake()
    {
    }
    public void InitStatusText(float _hp, float _ability, float _textSpeed, float textResist)
    {
    }
    private void OnLanguageChange(Language _language)
    {
        foreach (KeyValuePair<TMP_Text, Dictionary<Language, string>> keyValue in texts)
        {
            
        }
    }
}
